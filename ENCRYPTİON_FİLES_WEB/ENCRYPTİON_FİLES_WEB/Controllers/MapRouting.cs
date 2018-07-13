using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ENCRYPTİON_FİLES_WEB.Controllers
{
    public class MapRouting:Attribute
    {
        public void ConfigureServices(
               IServiceCollection services)
        {
            services.AddMvc();
        }

        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "goto_one",
                    template: "one",
                    defaults: new { controller = "Home", action = "PageOne" });

                routes.MapRoute(
                    name: "goto_two",
                    template: "two/{id?}",
                    defaults: new { controller = "Home", action = "PageTwo" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
