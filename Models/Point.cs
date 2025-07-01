namespace geoproject.Models
{
    public class Point
    {
        // id , pointx pointy,name
        
        public int Id { get; set; }
        public double PointX { get; set; }
        public double PointY { get; set; }
        public string? Name { get; set; }
    }
}