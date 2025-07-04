using System.ComponentModel.DataAnnotations;

namespace geoproject.Models
{
    public class Point
    {        
        public int Id { get; set; }
        
        [Required]
        public double PointX { get; set; }
        
        [Required]
        public double PointY { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public CoordinateType CoordinateType { get; set; } = CoordinateType.Point;
    }
}