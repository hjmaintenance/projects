using JinRestApi.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using JinRestApi.Data;
using JinRestApi.Dtos;

namespace JinRestApi.Endpoints;

public static class DashboardEndpoints
{
    public static void MapDashboardEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/dashboard");

        group.MapGet("/company-stats", (AppDbContext db) => ApiResponseBuilder.CreateAsync(async () => 
        {
            var stats = await db.Requests
                .Where(r => r.Customer != null && r.Customer.Company != null)
                .GroupBy(r => r.Customer.Company)
                .Select(g => new
                {
                    CompanyName = g.Key.Name,
                    LastPendingDate = g.Where(r => r.Status == ImprovementStatus.Pending)
                                    .OrderByDescending(r => r.CreatedAt)
                                    .Select(r => (DateTime?)r.CreatedAt)
                                    .FirstOrDefault(),
                    PendingCount = g.Count(r => r.Status == ImprovementStatus.Pending),
                    InProgressCount = g.Count(r => r.Status == ImprovementStatus.InProgress),
                    CompletedCount = g.Count(r => r.Status == ImprovementStatus.Completed),
                    RejectedCount = g.Count(r => r.Status == ImprovementStatus.Rejected),
                    TotalCount = g.Count()
                })
                .ToListAsync();

            return stats.Select(s =>
            {
                double completionRate = (s.TotalCount - s.RejectedCount) > 0
                    ? Math.Round((double)s.CompletedCount / (s.TotalCount - s.RejectedCount) * 100, 1)
                    : 0;

                return new CompanyStatsDto
                {
                    CompanyName = s.CompanyName,
                    LastPendingDate = s.LastPendingDate,
                    PendingCount = s.PendingCount,
                    InProgressCount = s.InProgressCount,
                    CompletedCount = s.CompletedCount,
                    RejectedCount = s.RejectedCount,
                    CompletionRate = completionRate
                };
            }).ToList();
        }));

        group.MapGet("/admin-stats", async (HttpContext http, AppDbContext db) =>
        {
            var uid = http.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            if (string.IsNullOrEmpty(uid) || !int.TryParse(uid, out var adminId))
            {
                return Results.Unauthorized();
            }

            var totalRequests = await db.Requests.CountAsync();
            var inProgressCount = await db.Requests.CountAsync(r => r.AdminId == adminId && r.Status == ImprovementStatus.InProgress);
            var completedCount = await db.Requests.CountAsync(r => r.AdminId == adminId && r.Status == ImprovementStatus.Completed);
            var rejectedCount = await db.Requests.CountAsync(r => r.AdminId == adminId && r.Status == ImprovementStatus.Rejected);

            var stats = new AdminStatsDto
            {
                InProgressCount = inProgressCount,
                CompletedCount = completedCount,
                RejectedCount = rejectedCount,
                TotalRequests = totalRequests
            };

            return Results.Ok(new { success = true, data = stats });

        }).RequireAuthorization();

        group.MapGet("/all-admin-stats", async (AppDbContext db) =>
        {
            var totalRequests = await db.Requests.CountAsync();

            var adminStats = await db.Admins
                .Select(admin => new AllAdminStatsDto
                {
                    AdminId = admin.Id,
                    AdminName = admin.UserName,
                    AdminPhoto = admin.Photo,
                    InProgressCount = db.Requests.Count(r => r.AdminId == admin.Id && r.Status == ImprovementStatus.InProgress),
                    CompletedCount = db.Requests.Count(r => r.AdminId == admin.Id && r.Status == ImprovementStatus.Completed),
                    RejectedCount = db.Requests.Count(r => r.AdminId == admin.Id && r.Status == ImprovementStatus.Rejected),
                    TotalHandled = db.Requests.Count(r => r.AdminId == admin.Id && (r.Status == ImprovementStatus.InProgress || r.Status == ImprovementStatus.Completed || r.Status == ImprovementStatus.Rejected))
                })
                .ToListAsync();

            foreach (var stat in adminStats)
            {
                stat.AcceptanceRate = totalRequests > 0 ? Math.Round((double)stat.TotalHandled / totalRequests * 100, 1) : 0;
                stat.CompletionRate = stat.TotalHandled > 0 ? Math.Round((double)stat.CompletedCount / stat.TotalHandled * 100, 1) : 0;
            }

            var sortedStats = adminStats
                .OrderByDescending(s => s.AcceptanceRate)
                .ThenByDescending(s => s.CompletionRate)
                .ToList();

            return Results.Ok(new { success = true, data = sortedStats });
        }).RequireAuthorization();

        group.MapGet("/my-company-stats", async (HttpContext http, AppDbContext db) => 
        {
            var uid = http.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            if (string.IsNullOrEmpty(uid) || !int.TryParse(uid, out var customerId))
            {
                return Results.Unauthorized();
            }

            var customer = await db.Customers.FindAsync(customerId);
            if (customer == null || customer.CompanyId == null)
            {
                return Results.NotFound("Customer or company not found.");
            }

            var companyId = customer.CompanyId;

            var stats = await db.Requests
                .Where(r => r.Customer.CompanyId == companyId)
                .GroupBy(r => r.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            var result = new MyCompanyStatsDto
            {
                PendingCount = stats.FirstOrDefault(s => s.Status == ImprovementStatus.Pending)?.Count ?? 0,
                InProgressCount = stats.FirstOrDefault(s => s.Status == ImprovementStatus.InProgress)?.Count ?? 0,
                CompletedCount = stats.FirstOrDefault(s => s.Status == ImprovementStatus.Completed)?.Count ?? 0,
                RejectedCount = stats.FirstOrDefault(s => s.Status == ImprovementStatus.Rejected)?.Count ?? 0
            };
            result.TotalCount = result.PendingCount + result.InProgressCount + result.CompletedCount + result.RejectedCount;

            return Results.Ok(new { success = true, data = result });
        }).RequireAuthorization();

        group.MapGet("/requests/status-count", (AppDbContext db) => ApiResponseBuilder.CreateAsync(
            () => db.Requests.GroupBy(r => r.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync()));

        group.MapGet("/companies/requests", (AppDbContext db) => ApiResponseBuilder.CreateAsync(
            () => db.Companies
                .Select(c => new {
                    c.Id,
                    c.Name,
                    RequestCount = db.Requests.Count(r => r.Customer.CompanyId == c.Id)
                }).ToListAsync()));

        group.MapGet("/teams/workload", (AppDbContext db) => ApiResponseBuilder.CreateAsync(
            () => db.Teams
                .Select(t => new {
                    t.Id,
                    t.Name,
                    AssignedRequests = db.Requests.Count(r => r.Admin != null && r.Admin.TeamId == t.Id)
                }).ToListAsync()));

        group.MapGet("/admins/workload", (AppDbContext db) => ApiResponseBuilder.CreateAsync(
            () => db.Admins
                .Select(a => new {
                    a.Id,
                    a.UserName,
                    AssignedRequests = db.Requests.Count(r => r.AdminId == a.Id),
                    CompletedRequests = db.Requests.Count(r => r.AdminId == a.Id && r.Status == ImprovementStatus.Completed)
                }).ToListAsync()));

        group.MapGet("/requests/comments", (AppDbContext db) => ApiResponseBuilder.CreateAsync(
            () => db.Requests
                .Select(r => new {
                    r.Id,
                    r.Title,
                    CommentCount = db.Comments.Count(c => c.RequestId == r.Id)
                }).ToListAsync()));

        group.MapGet("/requests/recent", (AppDbContext db, int topN) => ApiResponseBuilder.CreateAsync(
            () => db.Requests.OrderByDescending(r => r.CreatedAt).Take(topN).ToListAsync()));

        group.MapGet("/attachments/entity", (AppDbContext db) => ApiResponseBuilder.CreateAsync(
            () => db.Attachments.GroupBy(a => a.EntityType)
                .Select(g => new { EntityType = g.Key, Count = g.Count() })
                .ToListAsync()));
    }
}
