using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetShopProj.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace PetShopProj.Data
{
    public partial class PetDbContext : IdentityDbContext
    {
        public PetDbContext(DbContextOptions<PetDbContext> options) : base(options) { }

        public virtual DbSet<Animal> Animals { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //var Mammals = new Category { Id = 1, Name = "Mammals" };
            //var Birds = new Category { Id = 2, Name = "Birds" };
            //var Amphibians = new Category { Id = 3, Name = "Amphibians" };
            //var Fish = new Category { Id = 4, Name = "Fish" };
            //var Reptiles = new Category { Id = 5, Name = "Reptiles" };
            //var Invertebrates = new Category { Id = 6, Name = "Invertebrates" };

            //var Cow = new Animal { Id = 1, PicturePath = "d2326411-92ec-4698-a1b2-0cbad507ae08_cow.jpg", Name = "Cow", Age = 1, Description = "", Category = Mammals };
            //var Tiger = new Animal { Id = 2, PicturePath = "715fcda7-488b-4fce-831f-322a4bc5e624_tiger.jpg", Name = "Tiger", Age = 12, Description = "", Category = Mammals };
            //var Lion = new Animal { Id = 3, PicturePath = "adbbb496-9b17-4c1f-8cbc-5943dd7c964e_lion.jpg", Name = "Lion", Age = 8, Description = "", Category = Mammals };
            //var Finch = new Animal { Id = 5, PicturePath = "f94ca42a-e0a3-4d3e-b0ef-81c0e31588f2_finch.jpg", Name = "Finch", Age = 4, Description = "", Category = Birds };
            //var Cocktail = new Animal { Id = 6, PicturePath = "a6b31533-7662-4b95-beec-8c605145f6cb_cocktail.jpg", Name = "Cocktail", Age = 18, Description = "", Category = Birds };
            //var Fighting_Fish = new Animal { Id = 7, PicturePath = "63bb0350-8337-46a8-b7a6-3bcc4227618e_Fighting-Fish.jpg", Name = "Fighting Fish", Age = 5, Description = "", Category = Fish };
            //var Goldfish = new Animal { Id = 8, PicturePath = "12017f59-9e39-4a6c-9153-66d83400e5ea_goldfish.jpg", Name = "Goldfish", Age = 3, Description = "", Category = Fish };

            //modelBuilder.Entity<Category>().HasData(
            ////Mammals, Birds, Amphibians, Fish, Reptiles, Invertebrates
            //new { Id = 1, Name = "Mammal" },
            //new { Id = 2, Name = "Birds" },
            //new { Id = 3, Name = "Amphibians" },
            //new { Id = 4, Name = "Fish" },
            //new { Id = 5, Name = "Reptiles" },
            //new { Id = 6, Name = "Invertebrates" }
            //);

            //modelBuilder.Entity<Animal>().HasData(
            ////Cow, Tiger, Lion, Finch, Cocktail, Fighting_Fish, Goldfish
            //new { Id = 1, PicturePath = "d2326411-92ec-4698-a1b2-0cbad507ae08_cow.jpg", Name = "Cow", Age = 1, Description = "", CategoryId = 1 },
            //new { Id = 2, PicturePath = "715fcda7-488b-4fce-831f-322a4bc5e624_tiger.jpg", Name = "Tiger", Age = 12, Description = "", CategoryId = 1 },
            //new { Id = 3, PicturePath = "adbbb496-9b17-4c1f-8cbc-5943dd7c964e_lion.jpg", Name = "Lion", Age = 8, Description = "", CategoryId = 1 },
            //new { Id = 5, PicturePath = "f94ca42a-e0a3-4d3e-b0ef-81c0e31588f2_finch.jpg", Name = "Finch", Age = 4, Description = "", CategoryId = 2 },
            //new { Id = 6, PicturePath = "a6b31533-7662-4b95-beec-8c605145f6cb_cocktail.jpg", Name = "Cocktail", Age = 18, Description = "", CategoryId = 2 },
            //new { Id = 7, PicturePath = "63bb0350-8337-46a8-b7a6-3bcc4227618e_Fighting-Fish.jpg", Name = "Fighting Fish", Age = 5, Description = "", CategoryId = 4 },
            //new { Id = 8, PicturePath = "12017f59-9e39-4a6c-9153-66d83400e5ea_goldfish.jpg", Name = "Goldfish", Age = 3, Description = "", CategoryId = 4 }
            ////new { Id = 4,  PicturePath = "", Name = "Ostrich", Age = 2, Description = "", CategoryId = 2 },
            ////new { Id = 9,  PicturePath = "", Name = "Guppy", Age = 4, Description = "", CategoryId = 4 },
            ////new { Id = 10, PicturePath = "", Name = "Crocodile", Age = 4, Description = "", CategoryId = 5 },
            ////new { Id = 11, PicturePath = "", Name = "Snake", Age = 7, Description = "", CategoryId = 5 },
            ////new { Id = 12, PicturePath = "", Name = "Turtle", Age = 11, Description = "", CategoryId = 5 },
            ////new { Id = 13, PicturePath = "", Name = "Frog", Age = 10, Description = "", CategoryId = 3 },
            ////new { Id = 14, PicturePath = "", Name = "Toad", Age = 19, Description = "", CategoryId = 3 },
            ////new { Id = 15, PicturePath = "", Name = "Newt", Age = 11, Description = "", CategoryId = 3 },
            ////new { Id = 15, PicturePath = "", Name = "Ant", Age = 3, Description = "", CategoryId = 6 },
            ////new { Id = 15, PicturePath = "", Name = "Cockroach", Age = 32, Description = "", CategoryId = 6 },
            ////new { Id = 15, PicturePath = "", Name = "Scorpion", Age = 11, Description = "", CategoryId = 6 }
            //);

            //modelBuilder.Entity<Comment>().HasData(

            //    );


            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(i => i.UserId);
            modelBuilder.Entity<IdentityUserRole<string>>().HasKey(role => new { role.RoleId, role.UserId });
            modelBuilder.Entity<IdentityUserClaim<string>>().HasKey(c => c.UserId);
            modelBuilder.Entity<IdentityUserToken<string>>().HasKey(t => t.UserId);
            modelBuilder.Entity<IdentityUser<string>>().HasKey(u => u.Id);
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

    internal class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<object>
    {
        public void Configure(EntityTypeBuilder<object> builder) { }
    }
}
