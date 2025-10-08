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
