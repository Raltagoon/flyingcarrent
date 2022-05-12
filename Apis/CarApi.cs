using Microsoft.AspNetCore.Authorization;

public class CarApi
{
    public void Register(WebApplication app)
    {
        // create
        app.MapPost("/rentcars", [Authorize] async (ICarRepository repository, FlyingCar car) =>
        {
            await repository.AddCarAsync(car);
            await repository.SaveAsync();
            return Results.NoContent();
        })
        .WithName("Create car")
        .WithTags("Creators");

        // read
        app.MapGet("/rentcars/{id}", [Authorize] async (ICarRepository repository, int id) =>
        {
            var tmp = await repository.GetCarByIdAsync(id);
            if (tmp != null)
            {
                return Results.Ok(tmp);
            }
            else
            {
                return Results.NotFound();
            }
        })
        .WithName("Get Car")
        .WithTags("Getters");

        app.MapGet("/rentcars", [Authorize] async (ICarRepository repository) =>
        {
            return Results.Ok(await repository.GetAllCarsAsync());
        })
        .WithName("Get All Cars")
        .WithTags("Getters");

        app.MapGet("/rentcars/search/name/{query}", [Authorize] async (ICarRepository repository, string query) =>
        {
            if (await repository.GetAllCarsAsync(query) is IEnumerable<FlyingCar> cars)
            {
                return Results.Ok(cars);
            }
            else
            {
                return Results.NotFound();
            }
        })
        .WithName("Get Cars By Search")
        .WithTags("Getters");

        app.MapGet("/rentcars/free", [Authorize] async (ICarRepository repository) =>
        {
            return Results.Ok(await repository.GetAllFreeCars());
        })
        .WithName("Get All Free Cars")
        .WithTags("Getters");

        // update
        app.MapPut("/rentcars/{id}", [Authorize] async (ICarRepository repository, FlyingCar car) =>
        {
            await repository.ChangeCarAsync(car);
            await repository.SaveAsync();
            return Results.NoContent();
        })
        .WithName("Update car")
        .WithTags("Updaters");

        // delete
        app.MapDelete("/rentcars/{id}", [Authorize] async (ICarRepository repository, int id) =>
        {
            await repository.DeleteCarByIdAsync(id);
            await repository.SaveAsync();
            return Results.NoContent();
        })
        .WithName("Delete car")
        .WithTags("Deleters");
    }
}
