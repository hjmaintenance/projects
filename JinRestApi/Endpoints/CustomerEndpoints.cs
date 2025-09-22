using JinRestApi.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using JinRestApi.Data;
using System.ComponentModel.DataAnnotations;
using JinRestApi.Services;

namespace JinRestApi.Endpoints;

public static class CustomerEndpoints
{
    public record CustomerCreateDto([Required] string LoginId, [Required] string UserName, [Required] string Email, [Required] string Password, int CompanyId, string? Sex, string? Photo, string? CreatedBy, string? MenuContext);

    public static void MapCustomerEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/customers");

        group.MapGet("/", (AppDbContext db) => ApiResponseBuilder.CreateAsync(
            () => db.Customers.Include(c => c.Attachments).Select(u => new { u.Id, u.UserName, u.LoginId, u.Sex, u.Photo, u.Email }).ToListAsync()
        ));

        group.MapGet("/{id}", (AppDbContext db, int id) => ApiResponseBuilder.CreateAsync(
            () => db.Customers
                .Where(c => c.Id == id)
                .Select(u => new { u.Id, u.UserName, u.LoginId, u.Sex, u.Photo, u.Email })
                .FirstOrDefaultAsync()
        ));

        group.MapPost("/", (AppDbContext db, CustomerCreateDto customerDto) => ApiResponseBuilder.CreateAsync(async () =>
        {
            var passwordService = new PasswordService();
            var customer = new Customer
            {
                LoginId = customerDto.LoginId,
                UserName = customerDto.UserName,
                Email = customerDto.Email,
                CompanyId = customerDto.CompanyId,
                Sex = customerDto.Sex ?? "M",
                Photo = customerDto.Photo ?? "",
                CreatedBy = customerDto.CreatedBy ?? "system",
                MenuContext = customerDto.MenuContext
            };
            customer.PasswordHash = passwordService.HashPassword(customer, customerDto.Password);
            db.Customers.Add(customer);
            await db.SaveChangesAsync();
            return customer;
        }, "Customer created successfully.", 201));

        group.MapPut("/{id}", (AppDbContext db, int id, Customer input) => ApiResponseBuilder.CreateAsync(async () =>
        {
            var customer = await db.Customers.FindAsync(id);
            if (customer is null) return null;

            customer.UserName = input.UserName;
            customer.Email = input.Email;

            await db.SaveChangesAsync();
            return customer;
        }, "Customer updated successfully."));

        group.MapDelete("/{id}", (AppDbContext db, int id) => ApiResponseBuilder.CreateAsync(async () =>
        {
            var customer = await db.Customers.FindAsync(id);
            if (customer is null) return null;

            db.Customers.Remove(customer);
            await db.SaveChangesAsync();
            return new { DeletedId = id };
        }, "Customer deleted successfully."));
    }
}
