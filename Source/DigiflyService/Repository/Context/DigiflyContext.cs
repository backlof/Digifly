using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.IO;
using DigiflyService.Repository.Table;
using Microsoft.Data.Sqlite;

namespace DigiflyService.Repository.Context
{
	public class DigiflyContext : DbContext, IDisposable
	{
		protected readonly string _location;

		internal DbSet<Image> Images { get; set; }
		internal DbSet<Rating> Ratings { get; set; }

		public DigiflyContext(string folder, string filename)
		{
			_location = Path.Combine(folder, filename);

			Database.EnsureCreated();
			Database.Migrate();
		}

		public void DetachAll()
		{
			foreach (var item in ChangeTracker.Entries())
			{
				item.State = EntityState.Detached;
			}
		}

		private SqliteConnection CreateConnection()
		{
			return new SqliteConnection
			{
				ConnectionString = new SqliteConnectionStringBuilder { DataSource = _location }.ToString()
			};
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{

			optionsBuilder.UseSqlite(CreateConnection());
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			#region Tables

			modelBuilder.Entity<Image>()
				.ToTable("Images");

			modelBuilder.Entity<Rating>()
				.ToTable("Ratings");

			#endregion

			#region Primary keys

			modelBuilder.Entity<Image>()
				.HasKey(x => x.Id);

			modelBuilder.Entity<Rating>()
				.HasKey(x => x.Id);

			#endregion

			#region Auto generation

			modelBuilder.Entity<Image>()
				.Property(x => x.Id)
				.ValueGeneratedOnAdd();

			modelBuilder.Entity<Rating>()
				.Property(x => x.Id)
				.ValueGeneratedOnAdd();

			#endregion

			#region Default values

			modelBuilder.Entity<Image>()
				.Property(x => x.Id)
				.HasDefaultValue(1);

			modelBuilder.Entity<Rating>()
				.Property(x => x.Id)
				.HasDefaultValue(1);

			#endregion

			#region Foreign keys

			modelBuilder.Entity<Image>()
				.HasMany(x => x.Wins)
				.WithOne(x => x.Winner)
				.HasForeignKey(x => x.WinnerId);

			modelBuilder.Entity<Image>()
				.HasMany(x => x.Losses)
				.WithOne(x => x.Loser)
				.HasForeignKey(x => x.LoserId);

			#endregion

			#region Rules

			modelBuilder.Entity<Image>()
				.Property(x => x.Added)
				.IsRequired();

			modelBuilder.Entity<Image>()
				.HasIndex(x => x.FileName)
				.IsUnique();

			modelBuilder.Entity<Rating>()
				.Property(x => x.Time)
				.IsRequired();

			#endregion
		}
	}
}
