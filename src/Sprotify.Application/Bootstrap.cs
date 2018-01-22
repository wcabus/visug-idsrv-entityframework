using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sprotify.Application.Services;
using Sprotify.DAL;
using Sprotify.DAL.Repositories;
using Sprotify.Domain.Repositories;
using Sprotify.Domain.Services;

namespace Sprotify.Application
{
    public static class Bootstrap
    {
        public static void Initialize(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<SprotifyDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddScoped<UnitOfWork>();
            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISongRepository, SongRepository>();
            services.AddScoped<IBandRepository, BandRepository>();
            services.AddScoped<IAlbumRepository, AlbumRepository>();
            services.AddScoped<IPlaylistRepository, PlaylistRepository>();

            services.AddScoped<ISubscriptionService, SubscriptionService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISongService, SongService>();
            services.AddScoped<IBandService, BandService>();
            services.AddScoped<IAlbumService, AlbumService>();
            services.AddScoped<IPlaylistService, PlaylistService>();
        }
    }
}
