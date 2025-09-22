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

        group.MapGet("/", (AppDbContext db) => ApiResponseBuilder.CreateAsync(
            () => db.Teams.Include(t => t.Attachments).ToListAsync()
        ));

        group.MapGet("/{id}", (AppDbContext db, int id) => ApiResponseBuilder.CreateAsync(
            () => db.Teams.Include(t => t.Attachments).FirstOrDefaultAsync(t => t.Id == id)
        ));

        group.MapPost("/", (AppDbContext db, Team team) => ApiResponseBuilder.CreateAsync(async () =>
        {
            db.Teams.Add(team);
            await db.SaveChangesAsync();
            return team;
        }, "Team created successfully.", 201));

        group.MapPut("/{id}", (AppDbContext db, int id, Team input) => ApiResponseBuilder.CreateAsync(async () =>
        {
            var team = await db.Teams.FindAsync(id);
            if (team is null) return null;

            team.Name = input.Name;

            await db.SaveChangesAsync();
            return team;
        }, "Team updated successfully."));

        group.MapDelete("/{id}", (AppDbContext db, int id) => ApiResponseBuilder.CreateAsync(async () =>
        {
            var team = await db.Teams.FindAsync(id);
            if (team is null) return null;

            db.Teams.Remove(team);
            await db.SaveChangesAsync();
            return new { DeletedId = id };
        }, "Team deleted successfully."));
    }
}
