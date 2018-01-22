using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sprotify.Web.Services;
using Sprotify.Web.Services.Core;
using Microsoft.AspNetCore.Mvc;

namespace Sprotify.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Add services
            services.AddScoped<SprotifyHttpClient>();
            services.AddScoped<SubscriptionService>();
            services.AddScoped<UserService>();
            services.AddScoped<SongService>();
            services.AddScoped<BandService>();
            services.AddScoped<AlbumService>();
            services.AddScoped<PlaylistService>();

            services.AddMvc(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            // Authentication
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie()
                .AddOpenIdConnect(options =>
                {
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.RequireHttpsMetadata = true;
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.SaveTokens = true;

                    options.Authority = Configuration.GetValue<string>("Authority");
                    options.ClientId = Configuration.GetValue<string>("ClientId");
                    options.ClientSecret = Configuration.GetValue<string>("ClientSecret");

                    options.Scope.Add("openid");
                    options.Scope.Add("email");
                    options.Scope.Add("profile");
                    options.Scope.Add("roles");
                    options.Scope.Add("offline_access");
                    options.Scope.Add(Configuration.GetValue<string>("ApiName"));

                    options.ResponseType = "code id_token";

                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        NameClaimType = "given_name",
                        RoleClaimType = "role"
                    };

                    options.Events = new OpenIdConnectEvents
                    {
                        OnTokenValidated = ctx =>
                        {
                            var identity = ctx.Principal.Identity as ClaimsIdentity;

                            var subjectClaim = identity.Claims.FirstOrDefault(x => x.Type == "sub");
                            var newIdentity = new ClaimsIdentity(identity.AuthenticationType, "given_name", "role");
                            newIdentity.AddClaim(subjectClaim);

                            ctx.Principal = new ClaimsPrincipal(newIdentity);

                            return Task.CompletedTask;
                        },
                        OnUserInformationReceived = ctx =>
                        {
                            var identity = ctx.Principal.Identity as ClaimsIdentity;

                            var newIdentity = new ClaimsIdentity(identity.AuthenticationType, "given_name", "role");
                            newIdentity.AddClaims(ctx.User.Properties().Select(x => new Claim(x.Name, x.Value.ToString())));

                            ctx.Principal = new ClaimsPrincipal(newIdentity);

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();

            app.UseSession();

            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                  );

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
