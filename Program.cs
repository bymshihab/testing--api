using HotelListing.Api.Contracts;
using HotelListing.Api.Data;
using HotelListing.Api.Mapping;
using HotelListing.Api.Services;
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
builder.Services.AddScoped<ICountriesService>(provider => provider.GetRequiredService<CountriesService>());
builder.Services.AddScoped<IHotelsService, HotelsService>();
builder.Services.AddTransient<CountryNameResolver>();

builder.Services.AddDataProtection();

builder.Services.AddIdentityCore<ApplicationUser>(options =>{})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<HotelListingDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
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
