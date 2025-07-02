using Microsoft.AspNetCore.Mvc;
using geoproject.Models;
using geoproject.Interfaces;

namespace geoproject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewApiController : ControllerBase
    {
        private readonly IPointGetAllService _getAllService;
        private readonly IPointAddService _addService;
        private readonly IPointGetByIdService _getByIdService;
        private readonly IPointUpdateService _updateService;
        private readonly IPointDeleteService _deleteService;

        public NewApiController(
            IPointGetAllService getAllService,
            IPointAddService addService,
            IPointGetByIdService getByIdService,
            IPointUpdateService updateService,
            IPointDeleteService deleteService)
        {
            _getAllService = getAllService;
            _addService = addService;
            _getByIdService = getByIdService;
            _updateService = updateService;
            _deleteService = deleteService;
        }

        //* Get all points [GET]

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<Point>>>> GetAllPointsAsync()
        {
            var response = await _getAllService.GetAllPointsAsync();
            return Ok(response);
        }

        //* Add a new point [POST]

        [HttpPost]
        public async Task<ActionResult<ApiResponse<Point>>> AddPoint(double pointX, double pointY, string name)
        {
            var newPoint = new Point
            {
                PointX = pointX,
                PointY = pointY,
                Name = name
            };

            var response = await _addService.AddPointAsync(newPoint);

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return CreatedAtAction(nameof(GetPointById), new { id = response.Data!.Id }, response);
            //! i use Null-forgiving operator here because the response.Data.Id will never be null
            //! imo its not a good practice to use but this is a simple example, if application grows larger, i should differentiate method
        }

        //* Get point by ID [GET]

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Point>>> GetPointById(int id)
        {
            var response = await _getByIdService.GetPointByIdAsync(id);
            
            if (!response.IsSuccess)
            {
                return response.Message.Contains("greater than 0") 
                    ? BadRequest(response) 
                    : NotFound(response);
            }

            return Ok(response);
        }

        //* Update point by ID [PUT]

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<Point>>> UpdatePoint(int id, double pointX, double pointY, string name)
        {
            var response = await _updateService.UpdatePointAsync(id, pointX, pointY, name);
            
            if (!response.IsSuccess)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        //* Delete point by ID [DELETE]

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<Point>>> DeletePoint(int id)
        {
            var response = await _deleteService.DeletePointAsync(id);
            
            if (!response.IsSuccess)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}