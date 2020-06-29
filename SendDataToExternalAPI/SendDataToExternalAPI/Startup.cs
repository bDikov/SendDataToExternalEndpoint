using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NToastNotify;
using Polly;
using Polly.Extensions.Http;
using SendDataToExternalAPI.Services.Services;
using SendDataToExternalAPI.Services.Services.Contracts;
using System.Net.Http;

namespace SendDataToExternalAPI
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
            services.AddMvc().AddNToastNotifyToastr(new ToastrOptions()
            {
                ProgressBar = false,
                PositionClass = ToastPositions.TopFullWidth
            });
            services.AddControllersWithViews();
            services.AddScoped<IFormService, FormService>();

            services.AddHttpClient("HttpClient").AddPolicyHandler(GetRetryPolicy());

             static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
            {
                return HttpPolicyExtensions
                  // Handle HttpRequestExceptions, 408 and 5xx status codes
                  .HandleTransientHttpError()
                  // Handle 404 not found
                  .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                  // Handle 401 Unauthorized
                  .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                  .OrResult(msg=>msg.StatusCode==System.Net.HttpStatusCode.GatewayTimeout)
                  // What to do if any of the above erros occur:
                  // Retry 3 times, each time wait 1,2 and 4 seconds before retrying.
                  //Hopping it wont have more than 3 attempts
                  .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));                
            }

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
            app.UseNToastNotify();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
