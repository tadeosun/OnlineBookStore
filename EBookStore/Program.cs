using EBookStore.Common;
using EBookStore.Data;
using EBookStore.Implementations;
using EBookStore.Interfaces;
using EBookStore.RepositoryImplementation;
using EBookStore.RepositoryInterfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICheckoutService,CheckoutService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IConfigurationAccessor , ConfigurationAccessor>();
builder.Services.AddScoped<ICheckoutRepository, CheckoutRepository>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMemoryCache();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IDataContext, DataContext>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddSwaggerGen(c =>
            c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                In = ParameterLocation.Header,
                Description = "Basic Authorization header using the Bearer scheme"
            })
            );
builder.Services.AddSwaggerGen(c =>
c.AddSecurityRequirement(new OpenApiSecurityRequirement
{
                {
                    new  OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type =  ReferenceType.SecurityScheme,
                            Id = "bearer"
                        }
                    },
                    new string[] {}
                }
}));

var app = builder.Build();
app.UseMiddleware<LoggingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();


app.UseHttpsRedirection();
app.MapControllers();

app.Run();
