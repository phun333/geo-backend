using geoproject.Models;
using geoproject.Interfaces;
using geoproject.Data;
using geoproject.Resources;
using Microsoft.EntityFrameworkCore;

namespace geoproject.Repositories
{
    public class PointRepository : IPointRepository
    {
        private readonly ApplicationDbContext _context;

        public PointRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Point> AddAsync(Point point)
        {
            if (point == null)
                throw new ArgumentNullException(nameof(point), Messages.Errors.PointNull);

            if (string.IsNullOrWhiteSpace(point.Name))
                throw new ArgumentException(Messages.Errors.PointNameEmpty, nameof(point));

            //* Set default values
            point.CoordinateType = CoordinateType.Point;
            
            _context.Points.Add(point); //* change tracker
            await _context.SaveChangesAsync(); //* commit changes to the database
            return point;
        }

        public async Task<Point?> GetByIdAsync(int id)
        {
            return await _context.Points
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Point>> GetAllAsync()
        {
            return await _context.Points
            .ToListAsync();
        }

        public async Task<Point?> UpdateAsync(int id, string geometry, string name, CoordinateType coordinateType)
        {
            if (id <= 0)
                throw new ArgumentException(Messages.Errors.InvalidId, nameof(id));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(Messages.Errors.PointNameEmpty, nameof(name));
                
            if (string.IsNullOrWhiteSpace(geometry))
                throw new ArgumentException(Messages.Errors.GeometryEmpty, nameof(geometry));

            var existingPoint = await _context.Points.FirstOrDefaultAsync(p => p.Id == id);
            if (existingPoint != null)
            {
                existingPoint.Geometry = geometry;
                existingPoint.Name = name;
                existingPoint.CoordinateType = coordinateType;
                
                await _context.SaveChangesAsync();
                return existingPoint;
            }
            return null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException(Messages.Errors.InvalidId, nameof(id));

            var point = await _context.Points.FirstOrDefaultAsync(p => p.Id == id);
            if (point != null)
            {
                _context.Points.Remove(point);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException(Messages.Errors.InvalidId, nameof(id));

            return await _context.Points.AnyAsync(p => p.Id == id);
        }

        //* Additional methods for coordinate type filtering
        public async Task<List<Point>> GetByCoordinateTypeAsync(CoordinateType coordinateType)
        {
            return await _context.Points
                .Where(p => p.CoordinateType == coordinateType)
                .ToListAsync();
        }
    }
}
