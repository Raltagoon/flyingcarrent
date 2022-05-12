public interface IUserRepository : IDisposable
{
    // create
    Task AddUserAsync(User user);
    // read
    Task<User> GetUserByUsername(string username);
    Task<List<User>> GetAllUsersAsync();
    // update
    Task ChangeUserAsync(User user);
    // delete
    Task DeleteUserAsync(string username);
    // service
    Task<User> ConfirmUser(User user);
    Task SaveAsync();
}
