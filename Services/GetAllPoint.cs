using geoproject.Models;
using geoproject.Interfaces;
using geoproject.Resources;

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
                Message = Messages.Success.AllPointsRetrieved,
                Data = points
            };
            
            return await Task.FromResult(response);
        }
    }
}