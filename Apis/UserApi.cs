using Microsoft.AspNetCore.Authorization;

public class UserApi
{
    public void Register(WebApplication app)
    {
        // create
        app.MapPost("/rentcars/register", [AllowAnonymous] async (IUserRepository repository, User user) =>
        {
            if (repository.GetUserByUsername(user.UserName) == null)
            {
                return Results.Problem("already exists");
            }
            else
            {
                await repository.AddUserAsync(user);
                await repository.SaveAsync();
                return Results.NoContent();
            }
        })
        .WithName("Add User")
        .WithTags("Auth");

        // read
        app.MapGet("/rentcars/users/{id}", [Authorize] async (IUserRepository repository, string username) =>
        {
            var tmp = await repository.GetUserByUsername(username);
            if (tmp != null)
            {
                return Results.Ok(tmp);
            }
            else
            {
                return Results.NotFound();
            }    
        })
        .WithName("Get User By Username")
        .WithTags("Auth");

        app.MapGet("/rentcars/users", [Authorize] async (IUserRepository repository) =>
        {
            return Results.Ok(await repository.GetAllUsersAsync());
        })
        .WithName("Get All Users")
        .WithTags("Auth");

        // update
        app.MapPut("/rentcars/users/{id}", [Authorize] async (IUserRepository repository, User user) =>
        {
            await repository.ChangeUserAsync(user);
            await repository.SaveAsync();
            return Results.NoContent();
        })
        .WithName("Update user")
        .WithTags("Auth");

        // delete
        app.MapDelete("/rentcars/users/{id}", [Authorize] async (IUserRepository repository, string username) =>
        {
            await repository.DeleteUserAsync(username);
            await repository.SaveAsync();
            return Results.NoContent();
        })
        .WithName("Delete user")
        .WithTags("Auth");

        // service
        app.MapPut("/rentcars/login", [AllowAnonymous] async (IUserRepository repository, ITokenService tokenService, User user) =>
        {
            var userDto = await repository.ConfirmUser(user);
            if (userDto == null)
            {
                return Results.Unauthorized();
            }
            var token = tokenService.BuildToken(app.Configuration["Jwt:Key"],
                app.Configuration["Jwt:Issuer"], userDto);
            return Results.Ok(token);
        })
        .WithName("Get Token")
        .WithTags("Auth");
    }
}
