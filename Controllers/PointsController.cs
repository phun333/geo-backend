using Microsoft.AspNetCore.Mvc;
using geoproject.Models;
using geoproject.Interfaces;

namespace geoproject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PointsController : ControllerBase
    {
        private readonly IPointGetAllService _getAllService;
        private readonly IPointAddService _addService;
        private readonly IPointGetByIdService _getByIdService;
        private readonly IPointUpdateService _updateService;
        private readonly IPointDeleteService _deleteService;

        public PointsController(
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
        public async Task<ActionResult<ApiResponse<Point>>> AddPoint(string geometry, string name, CoordinateType coordinateType)
        {
            try
            {
                //! Validation: Geometry format check
                if (string.IsNullOrWhiteSpace(geometry))
                {
                    return BadRequest(new ApiResponse<Point>
                    {
                        IsSuccess = false,
                        Message = "Geometry cannot be empty"
                    });
                }

                // Basic coordinate validation based on coordinate type
                if (!IsValidGeometryFormat(geometry, coordinateType))
                {
                    return BadRequest(new ApiResponse<Point>
                    {
                        IsSuccess = false,
                        Message = GetGeometryFormatHelp(coordinateType)
                    });
                }

                var newPoint = new Point
                {
                    Geometry = geometry,
                    Name = name,
                    CoordinateType = coordinateType
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
        public async Task<ActionResult<ApiResponse<Point>>> UpdatePoint(int id, string geometry, string name, CoordinateType coordinateType)
        {
            try
            {
                var response = await _updateService.UpdatePointAsync(id, geometry, name, coordinateType);
                
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

        //* Helper methods for geometry validation
        private bool IsValidGeometryFormat(string geometry, CoordinateType coordinateType)
        {
            try
            {
                var trimmed = geometry.Trim();
                var parts = trimmed.Split(',').Select(p => p.Trim()).ToArray();

                switch (coordinateType)
                {
                    case CoordinateType.Point:
                        // Point: "x y" - should have one coordinate pair
                        return IsValidCoordinatePair(trimmed);

                    case CoordinateType.Line:
                        // Line: "x1 y1, x2 y2" - should have at least 2 coordinate pairs
                        return parts.Length >= 2 && parts.All(IsValidCoordinatePair);

                    case CoordinateType.Polygon:
                        // Polygon: "x1 y1, x2 y2, x3 y3, x1 y1" - should have at least 4 coordinate pairs (closed)
                        return parts.Length >= 4 && parts.All(IsValidCoordinatePair);

                    default:
                        return false;
                }
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidCoordinatePair(string coordinatePair)
        {
            var coords = coordinatePair.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return coords.Length == 2 && 
                   double.TryParse(coords[0], out var x) && 
                   double.TryParse(coords[1], out var y) &&
                   x >= -180 && x <= 180 && // Longitude validation
                   y >= -90 && y <= 90;     // Latitude validation
        }

        private string GetGeometryFormatHelp(CoordinateType coordinateType)
        {
            return coordinateType switch
            {
                CoordinateType.Point => "Point format: 'longitude latitude' (e.g., '28.9784 41.0082')",
                CoordinateType.Line => "Line format: 'lon1 lat1, lon2 lat2' (e.g., '28.9784 41.0082, 32.8597 39.9334')",
                CoordinateType.Polygon => "Polygon format: 'lon1 lat1, lon2 lat2, lon3 lat3, lon1 lat1' (closed polygon)",
                _ => "Invalid coordinate type"
            };
        }
    }
}