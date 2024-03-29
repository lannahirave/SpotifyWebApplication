﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SpotifyWebApplication;

public partial class spotifyContext : DbContext
{
    public spotifyContext()
    {
    }

    public spotifyContext(DbContextOptions<spotifyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Album> Albums { get; set; }
    public virtual DbSet<Artist> Artists { get; set; }
    public virtual DbSet<ArtistsSong> ArtistsSongs { get; set; }
    public virtual DbSet<Playlist> Playlists { get; set; }
    public virtual DbSet<PlaylistsSong> PlaylistsSongs { get; set; }
    public virtual DbSet<Publisher> Publishers { get; set; }
    public virtual DbSet<Song> Songs { get; set; }
    
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Album>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.PhotoLink)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasOne(d => d.Artist)
                .WithMany(p => p.Albums)
                .HasForeignKey(d => d.ArtistId)
                .OnDelete(DeleteBehavior.ClientCascade)
                .HasConstraintName("albums_artistid_foreign");

            entity.HasOne(d => d.Publisher)
                .WithMany(p => p.Albums)
                .HasForeignKey(d => d.PublisherId)
                .OnDelete(DeleteBehavior.ClientCascade)
                .HasConstraintName("albums_publisherid_foreign");
        });

        modelBuilder.Entity<Artist>(entity =>
        {
            entity.HasIndex(e => e.RankOnSpotify, "artists_rankonspotify_unique")
                .IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.PhotoLink)
                .IsRequired()
                .HasMaxLength(255);
        });

        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.PhotoLink)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasMany(c => c.Songs)
                .WithMany(a => a.Playlists)
                .UsingEntity<PlaylistsSong>(
                    configureRight => configureRight
                        .HasOne(d => d.Song)
                        .WithMany()
                        .HasForeignKey(d => d.SongId)
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .HasConstraintName("playlists_songs_songid_foreign"),
                    configureLeft => configureLeft
                        .HasOne(d => d.Playlist)
                        .WithMany()
                        .HasForeignKey(d => d.PlaylistId)
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .HasConstraintName("playlists_songs_playlistid_foreign"),
                    builder => builder
                        .ToTable("Playlists_songs")
                        .Property(x => x.Id)
                );
        });
        
        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);
        });

        modelBuilder.Entity<Song>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasOne(d => d.Album)
                .WithMany(p => p.Songs)
                .HasForeignKey(d => d.AlbumId)
                .OnDelete(DeleteBehavior.ClientCascade)
                .HasConstraintName("songs_albumid_foreign");

            entity.HasMany(c => c.Artists)
                .WithMany(a => a.Songs)
                .UsingEntity<ArtistsSong>(
                    configureLeft => configureLeft
                        .HasOne(d => d.Artist)
                        .WithMany()
                        .HasForeignKey(d => d.ArtistId)
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .HasConstraintName("artists_songs_artistid_foreign"),
                    configureRight => configureRight
                        .HasOne(d => d.Song)
                        .WithMany()
                        .HasForeignKey(d => d.SongId)
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .HasConstraintName("artists_songs_songid_foreign"),
                    builder => builder
                        .ToTable("Artists_songs")
                        .Property(x => x.Id)
                );
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}