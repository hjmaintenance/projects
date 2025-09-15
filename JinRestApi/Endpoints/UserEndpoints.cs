using JinRestApi.Models;

namespace JinRestApi.Endpoints;

public static class UserEndpoints {
    // 메모리 DB 대체용 (테스트 용도)
    private static readonly List<User> Users = new() {
        new User { Id = 1, Name = "Alice" },
        new User { Id = 2, Name = "Bob" }
    };

    public static void MapUserEndpoints(this IEndpointRouteBuilder routes) {
        var group = routes.MapGroup("/users");

        // 1. 전체 조회 (GET /users)
        group.MapGet("/", () => Users);

        // 2. 단일 조회 (GET /users/{id})
        group.MapGet("/{id:int}", (int id) => {
            var user = Users.FirstOrDefault(u => u.Id == id);
            return user is not null ? Results.Ok(user) : Results.NotFound();
        });

        // 3. 저장 (POST /users)
        group.MapPost("/", (User user) => {
            user.Id = Users.Any() ? Users.Max(u => u.Id) + 1 : 1;
            Users.Add(user);
            return Results.Created($"/users/{user.Id}", user);
        });

        // 4. 삭제 (DELETE /users/{id})
        group.MapDelete("/{id:int}", (int id) => {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user is null)
                return Results.NotFound();

            Users.Remove(user);
            return Results.NoContent();
        });
    }
}
