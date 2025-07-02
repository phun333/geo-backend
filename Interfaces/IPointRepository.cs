using geoproject.Models;

namespace geoproject.Interfaces
{
    public interface IPointRepository
    {
        Task<List<Point>> GetAllAsync();
        Task<Point?> GetByIdAsync(int id);
        Task<Point> AddAsync(Point point);
        Task<Point?> UpdateAsync(int id, double pointX, double pointY, string name);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
