using geoproject.Models;
using geoproject.Interfaces;

namespace geoproject.Services
{
    public class PointGetAllService : IPointGetAllService
    {
        private readonly IPointRepository _pointRepository;

        public PointGetAllService(IPointRepository pointRepository)
        {
            _pointRepository = pointRepository;
        }

        public async Task<ApiResponse<List<Point>>> GetAllPointsAsync()
        {
            var points = await _pointRepository.GetAllAsync();
            
            var response = new ApiResponse<List<Point>>
            {
                IsSuccess = true,
                Message = "All points successfully returned",
                Data = points
            };
            
            return await Task.FromResult(response);
        }
    }
}