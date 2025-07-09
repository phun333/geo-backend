using geoproject.Models;
using geoproject.Interfaces;
using geoproject.Resources;

namespace geoproject.Services
{
    public class PointUpdateService : IPointUpdateService
    {
        private readonly IPointRepository _pointRepository;

        public PointUpdateService(IPointRepository pointRepository)
        {
            _pointRepository = pointRepository;
        }

        public async Task<ApiResponse<Point>> UpdatePointAsync(int id, string geometry, string name, CoordinateType coordinateType)
        {
            var updatedPoint = await _pointRepository.UpdateAsync(id, geometry, name, coordinateType);
            
            if (updatedPoint == null)
            {
                return new ApiResponse<Point>
                {
                    IsSuccess = false,
                    Message = string.Format(Messages.Errors.PointNotFound, id)
                };
            }

            return new ApiResponse<Point>
            {
                IsSuccess = true,
                Message = string.Format(Messages.Success.PointUpdated, id),
                Data = updatedPoint
            };
        }
    }
}