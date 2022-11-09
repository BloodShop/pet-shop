using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetShopProj.Models;
using System.Xml.Linq;

namespace PetShopProj.Data
{
    public class PetDbContext : DbContext
    {
        public PetDbContext(DbContextOptions<PetDbContext> options) : base(options) { }

        public virtual DbSet<Animal> Animals { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Category>().HasData(
        //        new { CategoryId = 1, Name = "Mammal" },
        //        new { CategoryId = 2, Name = "Birds" },
        //        new { CategoryId = 3, Name = "Amphibians" },
        //        new { CategoryId = 4, Name = "Fish" },
        //        new { CategoryId = 5, Name = "Reptiles" },
        //        new { CategoryId = 6, Name = "Invertebrates" }
        //    );
        //    modelBuilder.Entity<Animal>().HasData(
        //        new { AnimalId = 1,  Name = "Bear",     Age = 1, Description = @"\Images\bear.png", CategoryId = 1 },
        //        new { AnimalId = 2,  Name = "Tiger",    Age = 12,Description = "", CategoryId = 1 },
        //        new { AnimalId = 3,  Name = "Whale",    Age = 8, Description = "", CategoryId = 1 },
        //        new { AnimalId = 4,  Name = "Ostrich",  Age = 2, Description = "", CategoryId = 2 },
        //        new { AnimalId = 5,  Name = "Peacock",  Age = 4, Description = "", CategoryId = 2 },
        //        new { AnimalId = 6,  Name = "Eagle",    Age = 18,Description = "", CategoryId = 2 },
        //        new { AnimalId = 7,  Name = "Salamon",  Age = 5, Description = "", CategoryId = 4 },
        //        new { AnimalId = 8,  Name = "Goldfish", Age = 3, Description = "", CategoryId = 4 },
        //        new { AnimalId = 9,  Name = "Guppy",    Age = 4, Description = "", CategoryId = 4 },
        //        new { AnimalId = 10, Name = "Crocodile",Age = 4, Description = "", CategoryId = 5 },
        //        new { AnimalId = 11, Name = "Snake",    Age = 7, Description = "", CategoryId = 5 },
        //        new { AnimalId = 12, Name = "Turtle",   Age = 11,Description = "", CategoryId = 5 },
        //        new { AnimalId = 13, Name = "Frog",     Age = 10,Description = "", CategoryId = 3 },
        //        new { AnimalId = 14, Name = "Toad",     Age = 19,Description = "", CategoryId = 3 },
        //        new { AnimalId = 15, Name = "Newt",     Age = 11,Description = "", CategoryId = 3 },
        //        new { AnimalId = 15, Name = "Ant",      Age = 3, Description = "", CategoryId = 6 },
        //        new { AnimalId = 15, Name = "Cockroach",Age = 32,Description = "", CategoryId = 6 },
        //        new { AnimalId = 15, Name = "Scorpion", Age = 11,Description = "", CategoryId = 6 }
        //    );

        //    modelBuilder.Entity<Comment>().HasData(
        //        new { CommentId = 1, AnimalId = 1, Text = "Awesome animal" }
        //        //new {  },
        //        //new {  }
        //    );
        //}

    }
}
