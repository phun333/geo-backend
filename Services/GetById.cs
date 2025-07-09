using geoproject.Models;
using geoproject.Interfaces;
using geoproject.Resources;

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
                    Message = Messages.Errors.InvalidId
                };
            }

            var point = await _pointRepository.GetByIdAsync(id);
            if (point == null)
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
                Message = string.Format(Messages.Success.PointRetrieved, id),
                Data = point
            };
        }
    }
}