using CrudSample.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudSample
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
            services.AddSingleton(Configuration.GetSection("CosmosDb"));
            services.AddSingleton(InitializeCosmosClientInstance(Configuration.GetSection("CosmosDb")));
            services.AddSingleton<IStudentDbService, StudentDbService>();
            services.AddSingleton<ITeacherDbService, TeacherDbService>();
            services.AddSingleton<ICourseDbService, CourseDbService>();
            services.AddControllersWithViews();
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        /// <summary>
        /// Creates a Cosmos DB client instance
        /// </summary>
        /// <returns></returns>
        private static CosmosClient InitializeCosmosClientInstance(IConfigurationSection configurationSection)
        {
            string account = configurationSection.GetSection("Account").Value;
            string key = configurationSection.GetSection("Key").Value;
            //var options = new CosmosClientOptions()
            //{
            //    SerializerOptions = new CosmosSerializationOptions()
            //    {
            //        PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
            //    }
            //};
            CosmosClient client = new CosmosClient(account, key);
            
            return client;
        }
    }
}
