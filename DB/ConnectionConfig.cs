using Microsoft.AspNetCore.Mvc;

public class ConnectionConfig
{
    private readonly IConfiguration Configuration;

    public ConnectionConfig(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public ContentResult OnGet()
    {
        var dbOptions = new DBOptions();
        Configuration.Bind(dbOptions);

        ContentResult result = new ContentResult();
        result.Content = ($"Server={dbOptions.Server},{dbOptions.Port};Database={dbOptions.Name};User Id={dbOptions.Username};Password={dbOptions.Password}");
        return result;
    }
}
