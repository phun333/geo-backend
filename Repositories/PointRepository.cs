using geoproject.Models;
using geoproject.Interfaces;

namespace geoproject.Repositories
{
    public class PointRepository : IPointRepository
    {
        // singleton pattern - all instances share the same list
        private static readonly List<Point> _points = new List<Point>();
        private static int _nextId = 1; // To generate unique IDs for new points

        public async Task<List<Point>> GetAllAsync()
        {
            return await Task.FromResult(_points.ToList());
        }

        public async Task<Point?> GetByIdAsync(int id)
        {
            var point = _points.FirstOrDefault(p => p.Id == id);
            return await Task.FromResult(point);
        }

        public async Task<Point> AddAsync(Point point)
        {
            //* assign a unique ID to the new point, first value will be always 1
            point.Id = _nextId; 
            _nextId++;

            _points.Add(point);
            return await Task.FromResult(point);
        }

        public async Task<Point?> UpdateAsync(int id, double pointX, double pointY, string name)
        {
            var existingPoint = _points.FirstOrDefault(p => p.Id == id);
            if (existingPoint != null)
            {
                existingPoint.PointX = pointX;
                existingPoint.PointY = pointY;
                existingPoint.Name = name;
                return await Task.FromResult(existingPoint);
            }
            return await Task.FromResult<Point?>(null);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var point = _points.FirstOrDefault(p => p.Id == id);
            if (point != null)
            {
                _points.Remove(point);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            var exists = _points.Any(p => p.Id == id);
            return await Task.FromResult(exists);
        }
    }
}
