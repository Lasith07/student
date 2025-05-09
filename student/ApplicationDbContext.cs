using DemoAPI.Helpers.Utils;
using DemoAPI.Helpers.Utils.GlobalAttributes;
using DemoAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoAPI
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public virtual DbSet<StudentModel> Students { get; set; }
        public virtual DbSet<SubjectModel> Subjects { get; set; }
        public virtual DbSet<UserModel> User { get; set; }
        public virtual DbSet<LoginDetailModel> LoginDetails { get; set; }
        public virtual DbSet<StoryModel> Story { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(
                    GlobalAttributes.MySqlConfiguration.connectionString,
                    ServerVersion.AutoDetect(GlobalAttributes.MySqlConfiguration.connectionString));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<StoryModel>().HasKey(s => s.storyid);

            base.OnModelCreating(modelBuilder);
        }
    }
}
