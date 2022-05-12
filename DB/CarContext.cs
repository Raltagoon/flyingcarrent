using Microsoft.EntityFrameworkCore;
public class CarContext : DbContext
{
    public CarContext(DbContextOptions<CarContext> options) : base(options)
    {
    }

    public DbSet<FlyingCar> FlyingCars => Set<FlyingCar>();

    public DbSet<User> Users => Set<User>();
}
