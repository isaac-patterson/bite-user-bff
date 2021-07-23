using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;
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
            object p = services.AddControllers();

            //var connectionString = "Server=146.71.76.234;Database=swiftfooddb;Uid=dbUser;Pwd=Pass@2021";
            //var connectionString = "Server=localhost;Database=bite;Uid=root;Pwd=XXXXXXXX";
            //var connectionString = Configuration.GetValue<String>("Config:AuroraConnectionString");
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 21));

            services.AddDbContext<DBContext>(
                x => x.UseMySql(connectionString, serverVersion, options => options.EnableRetryOnFailure())
            );

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "User-BFF API", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<ICouponService, CouponService>();
            services.AddTransient<IStripeService, StripeService>();
            services.AddTransient<IPaymentService, PaymentService>();
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
            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "User-BFF API V1");
            });

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
