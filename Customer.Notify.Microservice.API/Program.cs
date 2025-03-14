using Customer.Notify.Microservice.API.Services;
using Customer.Notify.Microservice.APP;
using Customer.Notify.Microservice.Domain;
using Customer.Notify.Microservice.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Customer.Notify.Microservice.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            var configuration = builder.Configuration;

            builder.Services.AddControllers();
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<NotifyDBContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("Value"), b => b.MigrationsAssembly("Customer.Notify.Microservice.API")));


            builder.Services.AddScoped<INotifyRepository, NotifyRepository>();
            builder.Services.AddScoped<INotifyServices, NotifyService>();
            builder.Services.AddScoped<RabbitMQConsumer>();
            builder.Services.AddScoped<RabbitMQProducer>();

            builder.Services.AddCors(options =>
            {

                options.AddPolicy("nuevaPolitica", app =>
                {

                    app.AllowAnyOrigin();
                    app.AllowAnyHeader();
                    app.AllowAnyMethod();
                });



            });


            var app = builder.Build();

            
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }



            app.UseCors("nuevaPolitica");
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}