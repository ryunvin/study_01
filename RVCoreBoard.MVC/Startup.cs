using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.CodeAnalysis.Options;
using Microsoft.AspNetCore.SignalR;
using RVCoreBoard.MVC.DataContext;
using RVCoreBoard.MVC.Services;
using RVCoreBoard.MVC.Hubs;
using Microsoft.Extensions.WebEncoders;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace RVCoreBoard.MVC
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
            services.AddControllersWithViews();

            // SignalR 서비스 등록
            services.AddSignalR();

            // DBContext 서비스 등록 [DI]
            services.AddDbContext<RVCoreBoardDBContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("localDB"));
            });

            services.AddAuthentication(options => {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Error/AccessDenied";
            });

            // NoteService 서비스 컨테이너 등록
            services.AddTransient<IBoardService, BoardService>();
            // AccountService 서비스 컨테이너 등록 - 20.09.09
            services.AddTransient<IUserService, UserService>();

            // ForwardedHeaders Remote IPAddress 가져오는부분 처리
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.RequireHeaderSymmetry = false;
                options.ForwardLimit = null;
                if (options.KnownNetworks != null)
                {
                    options.KnownNetworks.Add(new IPNetwork(IPAddress.Parse("::ffff:172.17.0.1"), 104));
                }
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDistributedMemoryCache();
            services.AddSession(so =>
            {
                so.IdleTimeout = TimeSpan.FromMinutes(60);
            });

            services.Configure<WebEncoderOptions>(option =>
            {
                // 한글 인코딩 처리    [2020. 11. 16 엄태영]
                option.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
            });
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
                app.UseHsts();
            }
            app.UseRouting();

            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                // SignalR Hub 라우트 처리
                endpoints.MapHub<CommentHub>("/commentHub");
            });

            // 404 Page Setting
            app.Use(async (ctx, next) =>
            {
                await next();

                if (ctx.Response.StatusCode == 404 && !ctx.Response.HasStarted)
                {
                    ctx.Request.Path = "/Error/404";
                    await next();
                }
                else if (ctx.Response.StatusCode == 500 && !ctx.Response.HasStarted)
                {
                    ctx.Request.Path = "/Error/500";
                    await next();
                }
            });
        }
    }
}
