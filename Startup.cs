using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using user_bff.Services;

namespace user_bff
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
            services.AddCors();
            services.AddControllers();

            //var connectionString = "Server=localhost;Database=bite;Uid=root;Pwd=ipat2421G#";
            var connectionString = Configuration.GetValue<String>("Config:AuroraConnectionString");
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 21));

            services.AddDbContext<DBContext>(
                x => x.UseMySql(connectionString, serverVersion)
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

             //Accept All HTTP Request Methods from all origins
            app.UseCors(builder =>builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

            app.UsePathBase(new PathString("/user"));
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); //Routes for my API controllers
            });

            app.ApplicationServices.GetRequiredService<DBContext>().Database.EnsureCreated();
        }
    }
}
