using geoproject.Models;
using geoproject.Interfaces;

namespace geoproject.Services
{
    public class PointDeleteService : IPointDeleteService
    {
        private readonly IPointRepository _pointRepository;

        public PointDeleteService(IPointRepository pointRepository)
        {
            _pointRepository = pointRepository;
        }

        public async Task<ApiResponse<Point>> DeletePointAsync(int id)
        {
            var point = await _pointRepository.GetByIdAsync(id);
            if (point == null)
            {
                return new ApiResponse<Point>
                {
                    IsSuccess = false,
                    Message = $"No point with ID #{id} found in the database."
                };
            }

            await _pointRepository.DeleteAsync(id);

            return new ApiResponse<Point>
            {
                IsSuccess = true,
                Message = $"Point with ID #{id} has been successfully deleted.",
                Data = point
            };
        }
    }
}