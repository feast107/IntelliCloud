using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repositories;
using Repositories.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyOpenCv.API;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Drawing;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace IntelliCloud
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
            services.AddAuthentication(o =>
            {
                o.DefaultScheme =
                o.DefaultAuthenticateScheme = 
                o.DefaultChallengeScheme = 
                o.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                   o =>
                   {
                        //��¼·�����û���ͼ������Դ����δ����֤��ʱ�����ת������·��
                        o.LoginPath = new PathString("");
                        //��ֹ����·�������û���ͼ������Դ��δ���κ���Ȩ���ԣ�����ᱻ�ض�������·��
                        o.AccessDeniedPath = new PathString("");
                   });
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddDistributedMemoryCache();//�ֲ�ʽ����
            services.AddSession(options =>
            {
                options.Cookie.Name = "IntelliCloud";
                options.IdleTimeout = TimeSpan.FromHours(3);//����session�Ĺ���ʱ��
                options.Cookie.HttpOnly = true;//���������������ͨ��js��ø�cookie��ֵ
            });
  
            services.AddSwaggerGen();
            services.AddControllersWithViews();

            services.AddSingleton<IImageFilter, CvImageFilter>();
            services.AddSingleton<I2ImageFilter, Cv2ImageFilter>();
            services.AddSingleton<IFactory, Factory>();
            services.AddSingleton<IService, Service>();
            services.AddSingleton<IAnalyzer, Analyzer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSession();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.ShowExtensions();
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI V1");
                    options.DocExpansion(DocExpansion.None);
                    options.DocumentTitle = "IntelliCloud Apis";
                    options.HeadContent = "<link rel=\"icon\" type=\"image/png\" href=\"/rowss.png\" />\n" +
                    "<link rel=\"shortcut icon\" type=\"image/png\" href=\"/rowss.png\" />\n";
                });

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
            app.UseAuthentication();
            app.UseAuthorization();
          
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Document}/{action=Swagger}/{id?}");
            });
        }

      
    }
}
