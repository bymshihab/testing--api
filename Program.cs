using HotelListing.Api.Contracts;
using HotelListing.Api.Data;
using HotelListing.Api.Mapping;
using HotelListing.Api.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the IOC container.
builder.Services.AddControllers();

builder.Services.AddDbContext<HotelListingDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("HotelListingDb"))
);

builder
    .Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System
            .Text
            .Json
            .Serialization
            .ReferenceHandler
            .IgnoreCycles;
    });

builder.Services.AddScoped<CountriesService>();
builder.Services.AddScoped<ICountriesService>(provider =>
    provider.GetRequiredService<CountriesService>()
);
builder.Services.AddScoped<IHotelsService, HotelsService>();
builder.Services.AddScoped<IUserService, UsersService>();
builder.Services.AddTransient<CountryNameResolver>();

builder.Services.AddDataProtection();

builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["jwtSettings:Issuer"],
            ValidAudience = builder.Configuration["jwtSettings:Audience"],
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration["jwtSettings:Key"]!)),
            ClockSkew = TimeSpan.Zero // Disable clock skew to ensure tokens expire exactly at token expiration time (instead of 5 minutes later)
        };
    })
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(
        AuthenticationDefaults.BasicScheme,
        _ => { }
    );
;

builder
    .Services.AddIdentityCore<ApplicationUser>(options => { })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<HotelListingDbContext>()
    .AddDefaultTokenProviders();

builder
    .Services.AddIdentityApiEndpoints<ApplicationUser>()
    .AddEntityFrameworkStores<HotelListingDbContext>();
builder.Services.AddAuthorization();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<CountryMappingProfile>();
    cfg.AddProfile<HotelMappingProfile>();
});

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapGroup("api/defaultauth").MapIdentityApi<ApplicationUser>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
