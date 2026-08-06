
using Admin_Portal.Application.Admin.Interfaces;
using Admin_Portal.Application.Admin.Services;
using Admin_Portal.Application.Bookings.Interfaces;
using Admin_Portal.Application.Bookings.Services;
using Admin_Portal.Core.Admin.Interfaces;
using Admin_Portal.Core.Bookings.Interfaces;
using Admin_Portal.Infrastructure.Admin.Data;
using Admin_Portal.Infrastructure.Admin.Repositories;
using Admin_Portal.Infrastructure.Bookings.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Admin_Web_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Configure Database Context
            var connectionString = builder.Configuration.GetConnectionString("AdminPortalConnection");
            builder.Services.AddDbContext<AdminPortalDbContext>(options =>
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly("Admin-Portal.Infrastructure"))
            );

            // Register repositories
            builder.Services.AddScoped<IAdminUserRepository, AdminUserRepository>();
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();

            // Register services
            builder.Services.AddScoped<IAdminAuthService, AdminAuthService>();
            builder.Services.AddScoped<IBookingService, BookingService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseAuthorization();

            app.MapControllers();

            // Run database migrations automatically on startup
            //using (var scope = app.Services.CreateScope())
            //{
            //    var dbContext = scope.ServiceProvider.GetRequiredService<AdminPortalDbContext>();
            //    dbContext.Database.Migrate();
            //}

            app.Run();
        }
    }
}
