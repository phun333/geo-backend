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
                
                entity.Property(e => e.Geometry)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasComment("Coordinate values based on CoordinateType (Point: 'x y', Line: 'x1 y1, x2 y2', Polygon: 'x1 y1, x2 y2, ...')");
                
                entity.Property(e => e.CoordinateType)
                    .IsRequired()
                    .HasConversion<int>() //* Enum to int conversion
                    .HasDefaultValue(CoordinateType.Point);

                //* Index for better query performance
                entity.HasIndex(e => e.CoordinateType);
                entity.HasIndex(e => e.Geometry); //* Spatial queries için
            });

            //* Seed data - default points with coordinate values only
            modelBuilder.Entity<Point>().HasData(
                new Point
                {
                    Id = 1,
                    Geometry = "28.9784 41.0082", // Istanbul coordinates (Longitude Latitude)
                    Name = "Istanbul",
                    CoordinateType = CoordinateType.Point
                },
                new Point
                {
                    Id = 2,
                    Geometry = "32.8597 39.9334", // Ankara coordinates (Longitude Latitude)
                    Name = "Ankara",
                    CoordinateType = CoordinateType.Point
                },
                new Point
                {
                    Id = 3,
                    Geometry = "28.9784 41.0082, 32.8597 39.9334", // Istanbul to Ankara route points
                    Name = "Istanbul-Ankara Route",
                    CoordinateType = CoordinateType.Line
                },
                new Point
                {
                    Id = 4,
                    Geometry = "28.5 40.5, 29.5 40.5, 29.5 41.5, 28.5 41.5, 28.5 40.5", // Istanbul region polygon points
                    Name = "Istanbul Region",
                    CoordinateType = CoordinateType.Polygon
                }
            );
        }
    }
}
