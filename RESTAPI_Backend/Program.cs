using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using RESTAPI_Backend.Data;
using Microsoft.Extensions.Options;
using RESTAPI_Backend.Services;

namespace RESTAPI_Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IToDoService, ToDoService>();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI
                (options =>
                    {
                        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
                        options.RoutePrefix = string.Empty;
                    }
                );
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            app.Run();
        }
    }
}
