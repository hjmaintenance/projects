using JinRestApi.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using JinRestApi.Data;

namespace JinRestApi.Endpoints;

public static class TeamEndpoints
{
    public static void MapTeamEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/teams");

        group.MapGet("/", async (AppDbContext db) =>
            await db.Teams.Include(t => t.Attachments).ToListAsync());

        group.MapGet("/{id}", async (AppDbContext db, int id) =>
            await db.Teams.Include(t => t.Attachments).FirstOrDefaultAsync(t => t.Id == id));

        group.MapPost("/", async (AppDbContext db, Team team) =>
        {
            db.Teams.Add(team);
            await db.SaveChangesAsync();
            return Results.Created($"/api/teams/{team.Id}", team);
        });

        group.MapPut("/{id}", async (AppDbContext db, int id, Team input) =>
        {
            var team = await db.Teams.FindAsync(id);
            if (team is null) return Results.NotFound();

            team.Name = input.Name;
            team.ModifiedBy = input.ModifiedBy;
            team.MenuContext = input.MenuContext;
            team.ModifiedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();
            return Results.Ok(team);
        });

        group.MapDelete("/{id}", async (AppDbContext db, int id) =>
        {
            var team = await db.Teams.FindAsync(id);
            if (team is null) return Results.NotFound();

            db.Teams.Remove(team);
            await db.SaveChangesAsync();
            return Results.Ok();
        });
    }
}
