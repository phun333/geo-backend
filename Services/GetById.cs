using geoproject.Models;
using geoproject.Interfaces;

namespace geoproject.Services
{
    public class PointGetByIdService : IPointGetByIdService
    {
        private readonly IPointRepository _pointRepository;

        public PointGetByIdService(IPointRepository pointRepository)
        {
            _pointRepository = pointRepository;
        }

        public async Task<ApiResponse<Point>> GetPointByIdAsync(int id)
        {
            if (id <= 0)
            {
                return new ApiResponse<Point>
                {
                    IsSuccess = false,
                    Message = "ID must be greater than 0"
                };
            }

            var point = await _pointRepository.GetByIdAsync(id);
            if (point == null)
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
                Message = $"Point with ID #{id} returned successfully.",
                Data = point
            };
        }
    }
}