using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
RegiSterServices(builder.Services);

var app = builder.Build();
Configure(app);
new CarApi().Register(app);
new UserApi().Register(app);
app.Run();

void RegiSterServices(IServiceCollection services)
{
    var configuration = builder.Configuration;
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "Input example: Bearer token",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
              new OpenApiSecurityScheme
              {
                Reference = new OpenApiReference
                  {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                  },
                  Scheme = "oauth2",
                  Name = "Bearer",
                  In = ParameterLocation.Header,
                },
                new List<string>()
              }
        });
    });
    var connection = new ConnectionConfig(configuration);
    Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAA");
    Console.WriteLine(connection.OnGet().Content);
    services.AddDbContext<CarContext>(options =>
    {
        options.UseSqlServer(connection.OnGet().Content);
    });
    services.AddScoped<ICarRepository, CarRepository>();
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddSingleton<ITokenService>(new TokenService());
    services.AddAuthorization();
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
        });
}

void Configure(WebApplication app)
{
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
    PrepDB.PrepPopulation(app);
}
