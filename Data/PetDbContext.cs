using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetShopProj.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace PetShopProj.Data
{
    public partial class PetDbContext : IdentityDbContext, ICallCenterContext
    {
        public PetDbContext(DbContextOptions<PetDbContext> options) : base(options) { }

        public virtual DbSet<Animal> Animals { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<Call> Calls { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
            //Mammals, Birds, Amphibians, Fish, Reptiles, Invertebrates
                new { Id = 1, Name = "Mammal" },
                new { Id = 2, Name = "Birds" },
                new { Id = 3, Name = "Amphibians" },
                new { Id = 4, Name = "Fish" },
                new { Id = 5, Name = "Reptiles" },
                new { Id = 6, Name = "Invertebrates" }
            );

            modelBuilder.Entity<Animal>().HasData(
            //Cow, Tiger, Lion, Finch, Cocktail, Fighting_Fish, Goldfish
                new { Id = 1, PicturePath = "d2326411-92ec-4698-a1b2-0cbad507ae08_cow.jpg", Name = "Cow", Age = 1, Description = "The cow, in common parlance, a domestic bovine, regardless of sex and age, usually of the species Bos taurus. In precise usage, the name is given to mature females of several large mammals, including cattle (bovines), moose, elephants, sea lions, and whales.", CategoryId = 1 },
                new { Id = 2, PicturePath = "715fcda7-488b-4fce-831f-322a4bc5e624_tiger.jpg", Name = "Tiger", Age = 12, Description = "The tiger (Panthera tigris) is the largest living cat species and a member of the genus Panthera. It is most recognisable for its dark vertical stripes on orange fur with a white underside. An apex predator, it primarily preys on ungulates, such as deer and wild boar. It is territorial and generally a solitary but social predator, requiring large contiguous areas of habitat to support its requirements for prey and rearing of its offspring.", CategoryId = 1 },
                new { Id = 3, PicturePath = "adbbb496-9b17-4c1f-8cbc-5943dd7c964e_lion.jpg", Name = "Lion", Age = 8, Description = "The lion (Panthera leo) is a large cat of the genus Panthera native to Africa and India. It has a muscular, broad-chested body, short, rounded head, round ears, and a hairy tuft at the end of its tail. It is sexually dimorphic; adult male lions are larger than females and have a prominent mane. It is a social species, forming groups called prides. A lion's pride consists of a few adult males, related females, and cubs.", CategoryId = 1 },
                new { Id = 4, PicturePath = "f94ca42a-e0a3-4d3e-b0ef-81c0e31588f2_finch.jpg", Name = "Finch", Age = 4, Description = "The true finches are small to medium-sized birds in the family Fringillidae. Finches have stout conical bills adapted for eating seeds and nuts and often have colourful plumage. They occupy a great range of habitats where they are usually resident and do not migrate. They have a worldwide distribution except for Australia and the polar regions. The family Fringillidae contains more than two hundred species divided into fifty genera.", CategoryId = 2 },
                new { Id = 5, PicturePath = "a6b31533-7662-4b95-beec-8c605145f6cb_cocktail.jpg", Name = "Cocktail", Age = 18, Description = "The cockatiel (kɒkəˈtiːl) Nymphicus hollandicus), also known as weiro, or quarrion, is a medium-sized parrot that is a member of its own branch of the cockatoo family endemic to Australia. They are prized as household pets and companion parrots throughout the world and are relatively easy to breed. As a caged bird, cockatiels are second in popularity only to the budgerigar.", CategoryId = 2 },
                new { Id = 6, PicturePath = "63bb0350-8337-46a8-b7a6-3bcc4227618e_Fighting-Fish.jpg", Name = "Fighting Fish", Age = 5, Description = "The Siamese fighting fish (Betta splendens), commonly known as the betta, is a freshwater fish native to Southeast Asia, namely Cambodia, Laos, Myanmar, Malaysia, Indonesia, Thailand, and Vietnam. It is one of 73 species of the genus Betta, but the only one eponymously called \"betta\", owing to its global popularity as a pet; Betta splendens are among the most popular aquarium fish in the world, due to their diverse and colorful morphology and relatively low maintenance.", CategoryId = 4 },
                new { Id = 7, PicturePath = "12017f59-9e39-4a6c-9153-66d83400e5ea_goldfish.jpg", Name = "Goldfish", Age = 3, Description = "The goldfish (Carassius auratus) is a freshwater fish in the family Cyprinidae of order Cypriniformes. It is commonly kept as a pet in indoor aquariums, and is one of the most popular aquarium fish. Goldfish released into the wild have become an invasive pest in parts of North America.", CategoryId = 4 }
            );

            modelBuilder.Entity<Comment>().HasData(
                new { Id = 1, AnimalId = 1, Content = "woooow AS well" },
                new { Id = 2, AnimalId = 2, Content = "woooow" },
                new { Id = 3, AnimalId = 3, Content = "So cute" },
                new { Id = 4, AnimalId = 3, Content = "HAHAHA" },
                new { Id = 5, AnimalId = 4, Content = "LOL xD" }
                );

            modelBuilder.Entity<Call>().HasData(
                new { Id = 1, Name = "Alon", Email = "koliakovcr7@gmail.com", Problem = "Site too awesome!!", CallTime = DateTime.UtcNow, Answered = false, AnswerTime = DateTime.MinValue }
                );


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
