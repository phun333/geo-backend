using System.ComponentModel.DataAnnotations;

namespace geoproject.Models
{
    public class Point
    {        
        public int Id { get; set; }
        
        [Required]
        [StringLength(500)]
        public string Geometry { get; set; } = string.Empty; // Coordinate values: "28.9784 41.0082"
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public CoordinateType CoordinateType { get; set; } = CoordinateType.Point;
    }
}