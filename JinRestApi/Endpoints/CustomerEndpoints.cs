using JinRestApi.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using JinRestApi.Data;

namespace JinRestApi.Endpoints;

public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/customers");

        group.MapGet("/", async (AppDbContext db) =>
            await db.Customers.Include(c => c.Attachments).ToListAsync());

        group.MapGet("/{id}", async (AppDbContext db, int id) =>
            await db.Customers.Include(c => c.Attachments).FirstOrDefaultAsync(c => c.Id == id));

        group.MapPost("/", async (AppDbContext db, Customer customer) =>
        {
            db.Customers.Add(customer);
            await db.SaveChangesAsync();
            return Results.Created($"/api/customers/{customer.Id}", customer);
        });

        group.MapPut("/{id}", async (AppDbContext db, int id, Customer input) =>
        {
            var customer = await db.Customers.FindAsync(id);
            if (customer is null) return Results.NotFound();

            customer.UserName = input.UserName;
            customer.Email = input.Email;
            customer.ModifiedBy = input.ModifiedBy;
            customer.MenuContext = input.MenuContext;
            customer.ModifiedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();
            return Results.Ok(customer);
        });

        group.MapDelete("/{id}", async (AppDbContext db, int id) =>
        {
            var customer = await db.Customers.FindAsync(id);
            if (customer is null) return Results.NotFound();

            db.Customers.Remove(customer);
            await db.SaveChangesAsync();
            return Results.Ok();
        });
    }
}
