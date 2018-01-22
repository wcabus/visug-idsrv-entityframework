using System;
using System.Linq;
using System.Reflection;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sprotify.IDP.Data;
using Sprotify.IDP.Models;

namespace Sprotify.IDP
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                x => x.UseSqlServer(Configuration.GetConnectionString("IdentityDB"))
            );

            services.AddIdentity<ApplicationUser, IdentityRole>(
                    x =>
                    {
                        x.User.RequireUniqueEmail = true;
                        x.Password.RequireDigit = false;
                        x.Password.RequireNonAlphanumeric = false;
                        x.Password.RequireUppercase = false;
                        x.Password.RequiredLength = 6;
                    }
            )
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                // Clients, resources, ...
                .AddConfigurationStore(x =>
                {
                    x.ConfigureDbContext = dbco => dbco.UseSqlServer(Configuration.GetConnectionString("IDPDB"),
                        options => options.MigrationsAssembly(migrationsAssembly));
                })
                // Tokens, codes, consents, ...
                .AddOperationalStore(x =>
                {
                    x.ConfigureDbContext = dbco => dbco.UseSqlServer(Configuration.GetConnectionString("IDPDB"),
                        options => options.MigrationsAssembly(migrationsAssembly));
                })
                .AddAspNetIdentity<ApplicationUser>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            InitializeDatabase(app);

            loggerFactory.AddConsole(LogLevel.Debug);
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseIdentityServer();

            app.UseMvcWithDefaultRoute();
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>()
                    .Database.Migrate();

                using (var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>())
                {
                    context.Database.Migrate();

                    if (!context.Clients.Any())
                    {
                        foreach (var client in Config.GetClients())
                        {
                            context.Clients.Add(client.ToEntity());
                        }
                        context.SaveChanges();
                    }

                    if (!context.IdentityResources.Any())
                    {
                        foreach (var resource in Config.GetIdentityResources())
                        {
                            context.IdentityResources.Add(resource.ToEntity());
                        }
                        context.SaveChanges();
                    }

                    if (!context.ApiResources.Any())
                    {
                        foreach (var resource in Config.GetApiResources())
                        {
                            context.ApiResources.Add(resource.ToEntity());
                        }
                        context.SaveChanges();
                    }
                }

                using (var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    context.Database.EnsureCreated();

                    IdentityRole adminRole;
                    if (!context.Roles.Any())
                    {
                        adminRole = new IdentityRole("admin") { NormalizedName = "admin" };
                        context.Roles.Add(adminRole);
                        context.SaveChanges();
                    }
                    else
                    {
                        adminRole = context.Roles.First(x => x.Name == "admin");
                    }

                    if (!context.Users.Any())
                    {
                        var user = new ApplicationUser
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserName = "admin@sprotify.com",
                            NormalizedUserName = "ADMIN@SPROTIFY.COM",
                            Email = "admin@sprotify.com",
                            NormalizedEmail = "ADMIN@SPROTIFY.COM",
                            EmailConfirmed = true,
                            SecurityStamp = Guid.NewGuid().ToString()
                        };

                        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<ApplicationUser>>();
                        user.PasswordHash = passwordHasher.HashPassword(user, "password");

                        context.Users.Add(user);

                        // User claims
                        context.UserClaims.AddRange(
                            new IdentityUserClaim<string> { UserId = user.Id, ClaimType = "given_name", ClaimValue = "Sprotify" },
                            new IdentityUserClaim<string> { UserId = user.Id, ClaimType = "family_name", ClaimValue = "Admin" },
                            new IdentityUserClaim<string> { UserId = user.Id, ClaimType = "email", ClaimValue = "admin@sprotify.com" }
                        );

                        // User roles
                        context.UserRoles.Add(new IdentityUserRole<string> { UserId = user.Id, RoleId = adminRole.Id });

                        context.SaveChanges();
                    }
                }
            }
        }
    }
}
