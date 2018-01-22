using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sprotify.Domain.Models;

namespace Sprotify.DAL
{
    public class SprotifyDbContext : DbContext
    {
        public SprotifyDbContext(DbContextOptions<SprotifyDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Our different subscriptions
            var subscriptionEntity = modelBuilder.Entity<Subscription>();
            subscriptionEntity.SetupKey(x => x.Id);
            subscriptionEntity.SetupUnicodeString(x => x.Title, 50);
            subscriptionEntity.SetupUnicodeString(x => x.Description, 1000).IsRequired(false);

            // People who subscribe and listen to music
            var userEntity = modelBuilder.Entity<User>();
            userEntity.SetupKey(x => x.Id);
            userEntity.SetupUnicodeString(x => x.Name, 300);
            userEntity.Property(x => x.Registered).HasColumnType("datetimeoffset(7)");

            // Keeping track of people who bought a subscription
            var userSubscriptionEntity = modelBuilder.Entity<UserSubscription>();
            userSubscriptionEntity.SetupKey(x => x.Id);
            userSubscriptionEntity.HasOne(x => x.User)
                .WithMany(x => x.Subscriptions)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            userSubscriptionEntity.HasOne(x => x.Subscription)
                .WithMany()
                .HasForeignKey(x => x.SubscriptionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Time to manage our content
            // First, the music
            var songEntity = modelBuilder.Entity<Song>();
            songEntity.SetupKey(x => x.Id);
            songEntity.SetupUnicodeString(x => x.Title, 500);
            songEntity.Property(x => x.ReleaseDate).HasColumnType("date");

            // Then, the artist of band who performed the songs
            var bandEntity = modelBuilder.Entity<Band>();
            bandEntity.SetupKey(x => x.Id);
            bandEntity.SetupUnicodeString(x => x.Name, 100);
            bandEntity.HasMany(x => x.Songs)
                .WithOne(x => x.Band)
                .HasForeignKey(x => x.BandId)
                .OnDelete(DeleteBehavior.Restrict);

            // Artists/bands usually release albums
            var albumEntity = modelBuilder.Entity<Album>();
            albumEntity.SetupKey(x => x.Id);
            albumEntity.SetupUnicodeString(x => x.Title, 100);
            albumEntity.SetupUnicodeString(x => x.Art, 1000).IsRequired(false);
            albumEntity.Property(x => x.ReleaseDate).HasColumnType("date");
            albumEntity.HasOne(x => x.Band)
                .WithMany(x => x.Albums)
                .HasForeignKey(x => x.BandId)
                .OnDelete(DeleteBehavior.Cascade);

            // And these albums need songs as well
            var albumSongEntity = modelBuilder.Entity<AlbumSong>();
            albumSongEntity.HasKey(x => new {x.AlbumId, x.SongId});
            albumSongEntity.HasOne(x => x.Album)
                .WithMany(x => x.Songs)
                .HasForeignKey(x => x.AlbumId)
                .OnDelete(DeleteBehavior.Cascade);

                // Songs can appear on multiple albums! This explains the one-to-many relationship.
                // Note: for remixes (having a different title and duration), we create another song entity.
            albumSongEntity.HasOne(x => x.Song)
                .WithMany()
                .HasForeignKey(x => x.SongId)
                .OnDelete(DeleteBehavior.Restrict);

            // Users create and can subscribe to playlists
            var playlistEntity = modelBuilder.Entity<Playlist>();
            playlistEntity.SetupKey(x => x.Id);
            playlistEntity.SetupUnicodeString(x => x.Title, 100);
            playlistEntity.HasOne(x => x.Creator)
                .WithMany()
                .HasForeignKey(x => x.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Keep track of songs being added to playlists
            var playlistSongEntity = modelBuilder.Entity<PlaylistSong>();
                // Decided to give this entity its own ID in order to easily add the same song multiple times.
                // Another approach was to make the primary key span over playlist id, song id and index but
                // that might make reordering songs in a playlist more difficult.
            playlistSongEntity.SetupKey(x => x.Id);
            playlistSongEntity.HasOne(x => x.Song)
                .WithMany()
                .HasForeignKey(x => x.SongId)
                .OnDelete(DeleteBehavior.Restrict);
            playlistSongEntity.HasOne(x => x.Playlist)
                .WithMany(x => x.Songs)
                .HasForeignKey(x => x.PlaylistId)
                .OnDelete(DeleteBehavior.Cascade);
            playlistSongEntity.HasOne(x => x.AddedBy)
                .WithMany()
                .HasForeignKey(x => x.AddedById)
                .OnDelete(DeleteBehavior.Restrict);

            // Finally, we have to know which users have subscribed to which playlists
            var playlistSubscriptionEntity = modelBuilder.Entity<PlaylistSubscription>();
            playlistSubscriptionEntity.HasKey(x => new {x.UserId, x.PlaylistId});
            playlistSubscriptionEntity.HasOne(x => x.Playlist)
                .WithMany()
                .HasForeignKey(x => x.PlaylistId)
                .OnDelete(DeleteBehavior.Restrict);
            playlistSubscriptionEntity.HasOne(x => x.User)
                .WithMany(x => x.Playlists)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public async Task Seed()
        {
            if (await Set<Song>().AnyAsync())
            {
                // Don't seed if there already is data
                return;
            }

            var moby = new Band("Moby");
            moby.Albums.Add(new Album
                {
                    Title = "Play",
                    ReleaseDate = new DateTime(2008, 6, 2),
                    Art = "https://img.discogs.com/mhK1IDkRl1L_IRmhOOLkP43h8kg=/fit-in/600x588/filters:strip_icc():format(jpeg):mode_rgb():quality(90)/discogs-images/R-55313-1345254572-7258.jpeg.jpg",
                    Songs = new List<AlbumSong>
                    {
                        new AlbumSong
                        {
                            Position = 1,
                            Song = new Song
                            {
                                Title = "Honey",
                                Duration = new TimeSpan(0, 3, 29),
                                Band = moby
                            }
                        },
                        new AlbumSong
                        {
                            Position = 2,
                            Song = new Song
                            {
                                Title = "Find My Baby",
                                Duration = new TimeSpan(0, 3, 59),
                                Band = moby
                            }
                        },
                        new AlbumSong
                        {
                            Position = 3,
                            Song = new Song
                            {
                                Title = "Porcelain",
                                Duration = new TimeSpan(0, 4, 1),
                                Band = moby
                            }
                        },
                        new AlbumSong
                        {
                            Position = 4,
                            Song = new Song
                            {
                                Title = "Why Does My Heart Feel So Bad?",
                                Duration = new TimeSpan(0, 4, 25),
                                Band = moby
                            }
                        },
                        new AlbumSong
                        {
                            Position = 5,
                            Song = new Song
                            {
                                Title = "South Side",
                                Duration = new TimeSpan(0, 3, 50),
                                Band = moby
                            }
                        },
                        new AlbumSong
                        {
                            Position = 6,
                            Song = new Song
                            {
                                Title = "Rushing",
                                Duration = new TimeSpan(0, 3, 0),
                                Band = moby
                            }
                        },
                        new AlbumSong
                        {
                            Position = 7,
                            Song = new Song
                            {
                                Title = "Bodyrock",
                                Duration = new TimeSpan(0, 3, 36),
                                Band = moby
                            }
                        },
                        new AlbumSong
                        {
                            Position = 8,
                            Song = new Song
                            {
                                Title = "Natural Blues",
                                Duration = new TimeSpan(0, 4, 14),
                                Band = moby
                            }
                        },
                        new AlbumSong
                        {
                            Position = 9,
                            Song = new Song
                            {
                                Title = "Machete",
                                Duration = new TimeSpan(0, 3, 38),
                                Band = moby
                            }
                        },
                        new AlbumSong
                        {
                            Position = 10,
                            Song = new Song
                            {
                                Title = "7",
                                Duration = new TimeSpan(0, 1, 2),
                                Band = moby
                            }
                        },
                        new AlbumSong
                        {
                            Position = 11,
                            Song = new Song
                            {
                                Title = "Run On",
                                Duration = new TimeSpan(0, 3, 45),
                                Band = moby
                            }
                        },
                        new AlbumSong
                        {
                            Position = 12,
                            Song = new Song
                            {
                                Title = "Down Slow",
                                Duration = new TimeSpan(0, 1, 36),
                                Band = moby
                            }
                        }
                    }
                });

            var daftPunk = new Band("Daft Punk");
            daftPunk.Albums.Add(new Album
            {
                Title = "Homework",
                ReleaseDate = new DateTime(1997, 1, 16),
                Art = "https://img.discogs.com/xRCJGKvOZz5saV2mXXzw_-_JZU8=/fit-in/600x600/filters:strip_icc():format(jpeg):mode_rgb():quality(90)/discogs-images/R-3235-1200238868.jpeg.jpg",
                Songs = new List<AlbumSong>
                {
                    new AlbumSong
                    {
                        Position = 1,
                        Song = new Song
                        {
                            Title = "Daftendirekt",
                            Duration = new TimeSpan(0, 2, 45),
                            Band = daftPunk
                        }
                    },
                    new AlbumSong
                    {
                        Position = 2,
                        Song = new Song
                        {
                            Title = "Wdpk 837 Fm",
                            Duration = new TimeSpan(0, 0, 28),
                            Band = daftPunk
                        }
                    },
                    new AlbumSong
                    {
                        Position = 3,
                        Song = new Song
                        {
                            Title = "Revolution 909",
                            Duration = new TimeSpan(0, 5, 35),
                            Band = daftPunk
                        }
                    },
                    new AlbumSong
                    {
                        Position = 4,
                        Song = new Song
                        {
                            Title = "Da Funk",
                            Duration = new TimeSpan(0, 5, 29),
                            Band = daftPunk
                        }
                    },
                    new AlbumSong
                    {
                        Position = 5,
                        Song = new Song
                        {
                            Title = "Phoenix",
                            Duration = new TimeSpan(0, 4, 57),
                            Band = daftPunk
                        }
                    },
                    new AlbumSong
                    {
                        Position = 6,
                        Song = new Song
                        {
                            Title = "Fresh",
                            Duration = new TimeSpan(0, 4, 4),
                            Band = daftPunk
                        }
                    },
                    new AlbumSong
                    {
                        Position = 7,
                        Song = new Song
                        {
                            Title = "Around The World",
                            Duration = new TimeSpan(0, 7, 10),
                            Band = daftPunk
                        }
                    },
                    new AlbumSong
                    {
                        Position = 8,
                        Song = new Song
                        {
                            Title = "Rollin' & Scratchin'",
                            Duration = new TimeSpan(0, 7, 29),
                            Band = daftPunk
                        }
                    },
                    new AlbumSong
                    {
                        Position = 9,
                        Song = new Song
                        {
                            Title = "Teachers",
                            Duration = new TimeSpan(0, 2, 53),
                            Band = daftPunk
                        }
                    },
                    new AlbumSong
                    {
                        Position = 10,
                        Song = new Song
                        {
                            Title = "High Fidelity",
                            Duration = new TimeSpan(0, 6, 2),
                            Band = daftPunk
                        }
                    },
                    new AlbumSong
                    {
                        Position = 11,
                        Song = new Song
                        {
                            Title = "Rock'n Roll",
                            Duration = new TimeSpan(0, 7, 34),
                            Band = daftPunk
                        }
                    },
                    new AlbumSong
                    {
                        Position = 12,
                        Song = new Song
                        {
                            Title = "Oh Yeah",
                            Duration = new TimeSpan(0, 2, 01),
                            Band = daftPunk
                        }
                    },
                    new AlbumSong
                    {
                        Position = 13,
                        Song = new Song
                        {
                            Title = "Burnin'",
                            Duration = new TimeSpan(0, 6, 54),
                            Band = daftPunk
                        }
                    },
                    new AlbumSong
                    {
                        Position = 14,
                        Song = new Song
                        {
                            Title = "Indo Silver Club",
                            Duration = new TimeSpan(0, 4, 35),
                            Band = daftPunk
                        }
                    },
                    new AlbumSong
                    {
                        Position = 15,
                        Song = new Song
                        {
                            Title = "Alive",
                            Duration = new TimeSpan(0, 5, 16),
                            Band = daftPunk
                        }
                    },
                    new AlbumSong
                    {
                        Position = 16,
                        Song = new Song
                        {
                            Title = "Funk Ad",
                            Duration = new TimeSpan(0, 0, 51),
                            Band = daftPunk
                        }
                    }
                }
            });
            daftPunk.Albums.Add(new Album
            {
                Title = "Discovery",
                ReleaseDate = new DateTime(2001, 3, 7),
                Art = "https://img.discogs.com/IfNxVjRatDT0GOfgNAfdtNy9nDQ=/fit-in/600x600/filters:strip_icc():format(jpeg):mode_rgb():quality(90)/discogs-images/R-136430-1481404839-1193.png.jpg",
                Songs = new List<AlbumSong>
                {
                    new AlbumSong
                    {
                        Position = 1,
                        Song = new Song
                        {
                            Title = "One More Time",
                            Duration = new TimeSpan(0, 5, 20),
                            Band = daftPunk
                        }
                    },
                    new AlbumSong
                    {
                        Position = 2,
                        Song = new Song
                        {
                            Title = "Aerodynamic",
                            Duration = new TimeSpan(0, 3, 33),
                            Band = daftPunk
                        }
                    },
                    new AlbumSong
                    {
                        Position = 3,
                        Song = new Song
                        {
                            Title = "Digital Love",
                            Duration = new TimeSpan(0, 5, 1),
                            Band = daftPunk
                        }
                    },
                    new AlbumSong
                    {
                        Position = 4,
                        Song = new Song
                        {
                            Title = "Harder Better Faster Stronger",
                            Duration = new TimeSpan(0, 3, 45),
                            Band = daftPunk
                        }
                    },
                    new AlbumSong
                    {
                        Position = 5,
                        Song = new Song
                        {
                            Title = "Crescendolls",
                            Duration = new TimeSpan(0, 3, 32),
                            Band = daftPunk
                        }
                    },
                    new AlbumSong
                    {
                        Position = 6,
                        Song = new Song
                        {
                            Title = "Nightvision",
                            Duration = new TimeSpan(0, 1, 44),
                            Band = daftPunk
                        }
                    },
                    new AlbumSong
                    {
                        Position = 7,
                        Song = new Song
                        {
                            Title = "Superheroes",
                            Duration = new TimeSpan(0, 3, 58),
                            Band = daftPunk
                        }
                    },
                    new AlbumSong
                    {
                        Position = 8,
                        Song = new Song
                        {
                            Title = "High Life",
                            Duration = new TimeSpan(0, 3, 22),
                            Band = daftPunk
                        }
                    },
                    new AlbumSong
                    {
                        Position = 9,
                        Song = new Song
                        {
                            Title = "Something About Us",
                            Duration = new TimeSpan(0, 3, 53),
                            Band = daftPunk
                        }
                    },
                    new AlbumSong
                    {
                        Position = 10,
                        Song = new Song
                        {
                            Title = "Voyager",
                            Duration = new TimeSpan(0, 3, 48),
                            Band = daftPunk
                        }
                    },
                    new AlbumSong
                    {
                        Position = 11,
                        Song = new Song
                        {
                            Title = "Veridis Quo",
                            Duration = new TimeSpan(0, 5, 45),
                            Band = daftPunk
                        }
                    },
                    new AlbumSong
                    {
                        Position = 12,
                        Song = new Song
                        {
                            Title = "Short Circuit",
                            Duration = new TimeSpan(0, 3, 27),
                            Band = daftPunk
                        }
                    },
                    new AlbumSong
                    {
                        Position = 13,
                        Song = new Song
                        {
                            Title = "Face To Face",
                            Duration = new TimeSpan(0, 4, 0),
                            Band = daftPunk
                        }
                    },
                    new AlbumSong
                    {
                        Position = 14,
                        Song = new Song
                        {
                            Title = "Too Long",
                            Duration = new TimeSpan(0, 10, 0),
                            Band = daftPunk
                        }
                    },
                }
            });

            Set<Band>().AddRange(moby, daftPunk);

            Set<Subscription>().AddRange(
                new Subscription
                (
                    "Sprotify Free",
                    "Listen to your favourite tracks for free!",
                    0,
                    hasAdvertisements: true,
                    canOnlyShuffle: true,
                    canPlayOffline: false,
                    hasHighQualityStreams: false
                ),
                new Subscription
                (
                    "Sprotify Standard",
                    "Listen to your favourite tracks and sync playlists offline to your devices!",
                    9.99m,
                    hasAdvertisements: false,
                    canOnlyShuffle: false,
                    canPlayOffline: true,
                    hasHighQualityStreams: false
                ),
                new Subscription
                (
                    "Sprotify Ultimate",
                    "Listen to your favourite tracks, sync playlists offline to your devices and listen in ultra-high quality!",
                    12.99m,
                    hasAdvertisements: false,
                    canOnlyShuffle: false,
                    canPlayOffline: true,
                    hasHighQualityStreams: true
                )
            );

            await SaveChangesAsync();
        }
    }
}