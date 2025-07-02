using geoproject.Models;
using geoproject.Interfaces;

namespace geoproject.Services
{
    public class PointUpdateService : IPointUpdateService
    {
        private readonly IPointRepository _pointRepository;

        public PointUpdateService(IPointRepository pointRepository)
        {
            _pointRepository = pointRepository;
        }

        public async Task<ApiResponse<Point>> UpdatePointAsync(int id, double pointX, double pointY, string name)
        {
            var updatedPoint = await _pointRepository.UpdateAsync(id, pointX, pointY, name);
            
            if (updatedPoint == null)
            {
                return new ApiResponse<Point>
                {
                    IsSuccess = false,
                    Message = $"No point with ID #{id} found in the database."
                };
            }

            return new ApiResponse<Point>
            {
                IsSuccess = true,
                Message = $"Point with ID #{id} has been successfully updated.",
                Data = updatedPoint
            };
        }
    }
}