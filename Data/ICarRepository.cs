public interface ICarRepository : IDisposable
{
    // create
    Task AddCarAsync(FlyingCar car);
    // read
    Task<FlyingCar> GetCarByIdAsync(int carId);
    Task<List<FlyingCar>> GetAllCarsAsync();
    Task<List<FlyingCar>> GetAllCarsAsync(string model);
    Task<List<FlyingCar>> GetAllFreeCars();
    // update
    Task ChangeCarAsync(FlyingCar car);
    // delete
    Task DeleteCarByIdAsync(int carId);
    // service
    Task SaveAsync();
}
