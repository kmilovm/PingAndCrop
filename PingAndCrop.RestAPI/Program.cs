using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text.Json;
using PingAndCrop.Domain.Extensions;

namespace PingAndCrop.RestAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddInjectedDependencies();
            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.All;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpClient();
            builder.Services.AddSignalR().AddAzureSignalR();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(corsPolicyBuilder =>
                {
                    corsPolicyBuilder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            builder.Services.AddSingleton(builder.Configuration);
            
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseCors();
            app.MapControllers();
            app.Run();
        }
    }
}
