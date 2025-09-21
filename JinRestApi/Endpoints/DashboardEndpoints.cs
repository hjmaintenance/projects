using JinRestApi.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using JinRestApi.Data;

namespace JinRestApi.Endpoints;

public static class DashboardEndpoints
{
    public static void MapDashboardEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/dashboard");

        group.MapGet("/requests/status-count", async (AppDbContext db) =>
            await db.Requests.GroupBy(r => r.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync());

        group.MapGet("/companies/requests", async (AppDbContext db) =>
            await db.Companies
                .Select(c => new {
                    c.Id,
                    c.Name,
                    RequestCount = db.Requests.Count(r => r.Customer.CompanyId == c.Id)
                }).ToListAsync());

        group.MapGet("/teams/workload", async (AppDbContext db) =>
            await db.Teams
                .Select(t => new {
                    t.Id,
                    t.Name,
                    AssignedRequests = db.Requests.Count(r => r.Admin != null && r.Admin.TeamId == t.Id)
                }).ToListAsync());

        group.MapGet("/admins/workload", async (AppDbContext db) =>
            await db.Admins
                .Select(a => new {
                    a.Id,
                    a.UserName,
                    AssignedRequests = db.Requests.Count(r => r.AdminId == a.Id),
                    CompletedRequests = db.Requests.Count(r => r.AdminId == a.Id && r.Status == ImprovementStatus.Completed)
                }).ToListAsync());

        group.MapGet("/requests/comments", async (AppDbContext db) =>
            await db.Requests
                .Select(r => new {
                    r.Id,
                    r.Title,
                    CommentCount = db.Comments.Count(c => c.RequestId == r.Id)
                }).ToListAsync());

        group.MapGet("/requests/recent", async (AppDbContext db, int topN) =>
            await db.Requests.OrderByDescending(r => r.CreatedAt).Take(topN).ToListAsync());

        group.MapGet("/attachments/entity", async (AppDbContext db) =>
            await db.Attachments.GroupBy(a => a.EntityType)
                .Select(g => new { EntityType = g.Key, Count = g.Count() })
                .ToListAsync());
    }
}
