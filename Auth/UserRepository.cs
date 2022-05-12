using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly CarContext _context;
    private bool _disposed = false;

    public UserRepository(CarContext context)
    {
        _context = context;
    }

    // create
    public async Task AddUserAsync(User user)
    {
        if (await _context.Users.FindAsync(user.UserName) != null)
        {
            Results.Problem("already exsists");
        }
        else
        {
            await _context.Users.AddAsync(user);
        }
    }

    // read
    public async Task<User> GetUserByUsername(string username)
    {
        return await _context.Users.FindAsync(username);
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    // update
    public async Task ChangeUserAsync(User user)
    {
        var dbUser = await _context.Users.FindAsync(user.UserName);
        if (dbUser == null)
        {
            return;
        }
        dbUser.UserName = user.UserName;
        dbUser.Password = user.Password;
    }

    // delete
    public async Task DeleteUserAsync(string username)
    {
        var dbUser = await _context.Users.FindAsync(username);
        if(dbUser == null)
        {
            return;
        }
        _context.Users.Remove(dbUser);
    }

    // service
    public async Task<User> ConfirmUser(User user)
    {
        var dbUser = await _context.Users.Where(t => t.UserName == user.UserName && t.Password == user.Password).FirstAsync();
        return dbUser;
    }
    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
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
