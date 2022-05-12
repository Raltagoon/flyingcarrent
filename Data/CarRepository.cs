using Microsoft.EntityFrameworkCore;

public class CarRepository : ICarRepository
{
    private readonly CarContext _context;
    private bool _disposed = false;

    public CarRepository(CarContext context)
    {
        _context = context;
    }

    // create
    public async Task AddCarAsync(FlyingCar car)
    {
        await _context.FlyingCars.AddAsync(car);
    }

    // read
    public async Task<FlyingCar> GetCarByIdAsync(int carId)
    {
        return await _context.FlyingCars.FindAsync(carId);
    }

    public async Task<List<FlyingCar>> GetAllCarsAsync()
    {
        return await _context.FlyingCars.ToListAsync();
    }

    public async Task<List<FlyingCar>> GetAllCarsAsync(string model)
    {
        return await _context.FlyingCars.Where(t => t.ModelName.Contains(model)).ToListAsync();
    }

    public async Task<List<FlyingCar>> GetAllFreeCars()
    {
        return await _context.FlyingCars.Where(t => t.IsBusy == false).ToListAsync();
    }

    // update
    public async Task ChangeCarAsync(FlyingCar car)
    {
        var dbCar = await _context.FlyingCars.FindAsync(car.Id);
        if (dbCar == null)
        {
            return;
        }    
        dbCar.ModelName = car.ModelName;
        dbCar.Colour = car.Colour;
        dbCar.IsBusy = car.IsBusy;
    }

    // delete
    public async Task DeleteCarByIdAsync(int carId)
    {
        var dbCar = await _context.FlyingCars.FindAsync(carId);
        if (dbCar == null)
        {
            return;
        }
        _context.FlyingCars.Remove(dbCar);
    }
    
    // service
    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if(!_disposed)
        {
            if(disposing)
            {
                _context.Dispose();
            }
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
