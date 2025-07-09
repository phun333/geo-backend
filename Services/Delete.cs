using geoproject.Models;
using geoproject.Interfaces;
using geoproject.Resources;

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
                    Message = string.Format(Messages.Errors.PointNotFound, id)
                };
            }

            await _pointRepository.DeleteAsync(id);

            return new ApiResponse<Point>
            {
                IsSuccess = true,
                Message = string.Format(Messages.Success.PointDeleted, id),
                Data = point
            };
        }
    }
}