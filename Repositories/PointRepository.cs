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
            try
            {
                return await Task.FromResult(_points.ToList());
            }
            catch (Exception ex)
            {
                //* better error handling
                //* log the exception or handle it as needed
                throw new Exception("An error occurred while retrieving points.", ex);
            }
        }

        public async Task<Point?> GetByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("ID must be greater than 0", nameof(id));

                var point = _points.FirstOrDefault(p => p.Id == id);
                return await Task.FromResult(point);
            }
            catch (ArgumentException)
            {
                throw; //* re-throw argument exceptions as they are
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error occurred while retrieving point with ID {id}", ex);
            }
        }
        

         public async Task<Point> AddAsync(Point point)
        {
            try
            {
                if (point == null)
                    throw new ArgumentNullException(nameof(point), "Point cannot be null");

                if (string.IsNullOrWhiteSpace(point.Name))
                    throw new ArgumentException("Point name cannot be empty", nameof(point));

                //* assign a unique ID to the new point, 
                //* first value will be always 1
                point.Id = _nextId; 
                _nextId++;

                _points.Add(point);
                return await Task.FromResult(point);
            }
            catch (ArgumentException)
            {
                throw; //* re-throw argument exceptions as they are
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error occurred while adding new point", ex);
            }
        }

        public async Task<Point?> UpdateAsync(int id, double pointX, double pointY, string name)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("ID must be greater than 0", nameof(id));

                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Point name cannot be empty", nameof(name));

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
            catch (ArgumentException)
            {
                throw; //* re-throw argument exceptions as they are
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error occurred while updating point with ID {id}", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("ID must be greater than 0", nameof(id));

                var point = _points.FirstOrDefault(p => p.Id == id);
                if (point != null)
                {
                    _points.Remove(point);
                    return await Task.FromResult(true);
                }
                return await Task.FromResult(false);
            }
            catch (ArgumentException)
            {
                throw; //* re-throw argument exceptions as they are
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error occurred while deleting point with ID {id}", ex);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("ID must be greater than 0", nameof(id));

                var exists = _points.Any(p => p.Id == id);
                return await Task.FromResult(exists);
            }
            catch (ArgumentException)
            {
                throw; //* re-throw argument exceptions as they are
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error occurred while checking if point exists with ID {id}", ex);
            }
        }
    }
}