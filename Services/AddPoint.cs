using geoproject.Models;
using geoproject.Interfaces;

namespace geoproject.Services
{
    public class PointAddService : IPointAddService
    {
        private readonly IPointRepository _pointRepository;

        public PointAddService(IPointRepository pointRepository)
        {
            _pointRepository = pointRepository;
        }

        public async Task<ApiResponse<Point>> AddPointAsync(Point point)
        {
            // Add the new point
            var addedPoint = await _pointRepository.AddAsync(point);

            return new ApiResponse<Point>
            {
                IsSuccess = true,
                Message = $"Point added successfully with ID #{addedPoint.Id}",
                Data = addedPoint
            };
        }
    }
}