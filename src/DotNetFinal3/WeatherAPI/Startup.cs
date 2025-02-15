using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Services.Queries;
using Services.Commands;
using Infrastructure.Utils;
using WeatherAPI.BackgroundTask;

namespace WeatherAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WeatherAPI", Version = "v1" });
            });


            services.AddCors(options => options.AddPolicy("CorsPolicy",
            builder =>
            {
                builder.AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(_ => true) // core5 ����ʹ��AllowAnyOrigin() 
                    .AllowCredentials();
            }));

            services.AddSingleton<IHostedService, WeatherAlert>();
            services.AddScoped<WeatherRepository>();
            services.AddScoped<CityRepository>();
            services.AddScoped<CityCommand>();
            services.AddScoped<WeatherQuery>();
            services.AddScoped<CityQuery>();
            services.AddScoped<WeatherCommand>();
            services.AddScoped<BaseUtils>();
            services.AddScoped<BaseWeatherService>();

            #region MySQL

            services.AddDbContext<weatherforecastdbContext>(options =>
            {
                options.UseMySql("server=localhost;port=3306;database=weatherforecastdb;user id=root;password=zhuzhu,00.;charset=utf8", ServerVersion.Parse("8.0.26-mysql"));
            });

            #endregion
            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WeatherAPI v1"));
            }

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireCors("CorsPolicy");
            });
        }
    }
}
