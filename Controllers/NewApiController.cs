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
            IPointDeleteService deleteService
            )
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
            try
            {
                var response = await _getAllService.GetAllPointsAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving points: {ex.Message}");
                //* i write to console error because it is not a good practice to return detailed error messages to the user
                return StatusCode(500, new ApiResponse<List<Point>>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving points"
                });
            }
        }

        //* Add a new point [POST]

        [HttpPost]
        public async Task<ActionResult<ApiResponse<Point>>> AddPoint(double pointX, double pointY, string name, CoordinateType coordinateType)
        {
            try
            {
                //! Validation: coordinate range check
                if (pointX < -180 || pointX > 180)
                {
                    return BadRequest(new ApiResponse<Point>
                    {
                        IsSuccess = false,
                        Message = "Longitude must be between -180 and 180"
                    });
                }

                if(pointY < -90 || pointY > 90)
                {
                    return BadRequest(new ApiResponse<Point>
                    {
                        IsSuccess = false,
                        Message = "Latitude must be between -90 and 90"
                    });
                }

                var newPoint = new Point
                {
                    PointX = pointX,
                    PointY = pointY,
                    Name = name,
                    CoordinateType = CoordinateType.Point
                };

                var response = await _addService.AddPointAsync(newPoint);

                if (!response.IsSuccess)
                {
                    return BadRequest(response);
                }

                return CreatedAtAction(nameof(GetPointById), new { id = response.Data!.Id }, response);
            }
            catch (ArgumentNullException ex)
            {
                //NOTE : validation errors is makes sense if we return to user
                return BadRequest(new ApiResponse<Point>
                {
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
            catch (ArgumentException ex)
            {
                //NOTE : same thing here
                return BadRequest(new ApiResponse<Point>
                {
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                // NOTE : but if this error type is Systems errors its better to log it and return a generic error message to the user for security reasons

                Console.WriteLine($"Error adding point: {ex.Message}");
                return StatusCode(500, new ApiResponse<Point>
                {
                    IsSuccess = false,
                    Message = "An error occurred while adding the point"
                });
            }
        }

        //* Get point by ID [GET]

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Point>>> GetPointById(int id)
        {
            try
            {
                var response = await _getByIdService.GetPointByIdAsync(id);
                
                if (!response.IsSuccess)
                {
                    return response.Message!.Contains("greater than 0") 
                        ? BadRequest(response) 
                        : NotFound(response);
                }

                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<Point>
                {
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving point: {ex.Message}");
                return StatusCode(500, new ApiResponse<Point>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving the point :"
                });
            }
        }

        //* Update point by ID [PUT]

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<Point>>> UpdatePoint(int id, double pointX, double pointY, string name, CoordinateType coordinateType)
        {
            try
            {
                var response = await _updateService.UpdatePointAsync(id, pointX, pointY, name, coordinateType);
                
                if (!response.IsSuccess)
                {
                    return NotFound(response);
                }

                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<Point>
                {
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating point: {ex.Message}");
                return StatusCode(500, new ApiResponse<Point>
                {
                    IsSuccess = false,
                    Message = "An error occurred while updating the point"
                });
            }
        }

        //* Delete point by ID [DELETE]

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<Point>>> DeletePoint(int id)
        {
            try
            {
                var response = await _deleteService.DeletePointAsync(id);
                
                if (!response.IsSuccess)
                {
                    return NotFound(response);
                }

                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<Point>
                {
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting point: {ex.Message}");
                return StatusCode(500, new ApiResponse<Point>
                {
                    IsSuccess = false,
                    Message = "An error occurred while deleting the point"
                });
            }
        }
    }
}