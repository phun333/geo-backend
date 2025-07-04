//NOTE : object-relational mapping (ORM) using Entity Framework Core

using Microsoft.EntityFrameworkCore;
using geoproject.Models;

namespace geoproject.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Point> Points { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //* Point entity configuration
            modelBuilder.Entity<Point>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                
                entity.Property(e => e.PointX)
                    .IsRequired()
                    .HasColumnType("decimal(18,6)");
                
                entity.Property(e => e.PointY)
                    .IsRequired()
                    .HasColumnType("decimal(18,6)");
                
                entity.Property(e => e.CoordinateType)
                    .IsRequired()
                    .HasConversion<int>() //* Enum to int conversion
                    .HasDefaultValue(CoordinateType.Point);

                //* Index for better query performance
                entity.HasIndex(e => e.CoordinateType);
            });

            //* Seed data - default points
            modelBuilder.Entity<Point>().HasData(
                new Point
                {
                    Id = 1,
                    PointX = 41.0082,
                    PointY = 28.9784,
                    Name = "Istanbul",
                    CoordinateType = CoordinateType.Point
                },
                new Point
                {
                    Id = 2,
                    PointX = 39.9334,
                    PointY = 32.8597,
                    Name = "Ankara",
                    CoordinateType = CoordinateType.Point
                }
            );
        }
    }
}
