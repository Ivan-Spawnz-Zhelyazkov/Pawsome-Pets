using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pawsome_Pets.Models;

namespace Pawsome_Pets.Data
{
	public class PawsomeDbContext : IdentityDbContext<ApplicationUser>
	{
		public PawsomeDbContext(DbContextOptions<PawsomeDbContext> options)
			: base(options)
		{
		}

		public DbSet<Animal> Animal { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<AdoptionRequest> AdoptionRequests { get; set; }
		public DbSet<CaretakingRequest> CaretakingRequests { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<Category>().HasData
				(
				new Category { Id = 1, Name = "Dogs" },
				new Category { Id = 2, Name = "Cats" },
				new Category { Id = 3, Name = "Birds" },
				new Category { Id = 4, Name = "Reptiles" },
				new Category { Id = 5, Name = "Fish" }
			);

			// Animal -> Giver
			builder.Entity<Animal>()
				.HasOne(a => a.Giver)
				.WithMany(u => u.GivenAnimals)
				.HasForeignKey(a => a.GiverId)
				.OnDelete(DeleteBehavior.Restrict);

			// Animal -> Adopter
			builder.Entity<Animal>()
				.HasOne(a => a.Adopter)
				.WithMany(u => u.AdoptedAnimals)
				.HasForeignKey(a => a.AdopterId)
				.OnDelete(DeleteBehavior.SetNull);

			// AdoptionRequest -> Animal
			builder.Entity<AdoptionRequest>()
				.HasOne(ar => ar.Animal)
				.WithMany(a => a.AdoptionRequests)
				.HasForeignKey(ar => ar.AnimalId)
				.OnDelete(DeleteBehavior.Restrict);

			// Adoption Request -> Adopter
			builder.Entity<AdoptionRequest>()
				.HasOne(ar => ar.Adopter)
				.WithMany(u => u.AdoptionRequests)
				.HasForeignKey(ar => ar.AdopterId)
				.OnDelete(DeleteBehavior.Restrict);

			// Caretaking -> Animal
			builder.Entity<CaretakingRequest>()
				.HasOne(cr => cr.Animal)
				.WithMany(a => a.CaretakingRequests)
				.HasForeignKey(cr => cr.AnimalId)
				.OnDelete(DeleteBehavior.Cascade);

			// Caretaking -> Caretaker
			builder.Entity<CaretakingRequest>()
				.HasOne(cr => cr.Caretaker)
				.WithMany(u => u.CaretakingRequests)
				.HasForeignKey(cr => cr.CaretakerId)
				.OnDelete(DeleteBehavior.Cascade);


		}

	}
}
