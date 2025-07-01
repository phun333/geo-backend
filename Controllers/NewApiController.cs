using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using geoproject.Models; // we import the all models from Models folder

namespace geoproject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewApiController : ControllerBase
    {
        private static readonly List<Point> _pointList = new List<Point>();

        //* GET method to retrieve all points

        [HttpGet]
        public ActionResult<ApiResponse<List<Point>>> GetAllPoints()
        {
            var response = new ApiResponse<List<Point>>
            {
            IsSuccess = true,
            Message = "All points successfully returned",
            Data = _pointList
            };
            return Ok(response);
        }

        //* POST method to add a new point

        [HttpPost]
        public ActionResult<ApiResponse<Point>> AddPoint(Point p)
        {
            // Validation for minus ids
            if (p.Id <= 0)
            {
                var errorResponse = new ApiResponse<Point> 
                { 
                    IsSuccess = false, 
                    Message = "ID must be greater than 0" 
                };
                return BadRequest(errorResponse);
            }
            
            // if its duplicate, return conflict
            var existingPoint = _pointList.FirstOrDefault(x => x.Id == p.Id);
            if (existingPoint != null)
            {
                var errorResponse = new ApiResponse<Point> 
                { 
                    IsSuccess = false, 
                    Message = $"Point with ID #{p.Id} already exists" 
                };
                return Conflict(errorResponse);
            }
            
            // Add the new point to the list
            _pointList.Add(p);
            
            var successResponse = new ApiResponse<Point>
            {
                IsSuccess = true,
                Message = "Point added successfully.",
                Data = p
            };
            
            return Created($"/api/NewApi/{p.Id}", successResponse); // 201 Created + Location header
        }

        //* GET method to return a point by ID

         [HttpGet("{id}")]
        public ActionResult<ApiResponse<Point>> GetPointById(int id)
        {
            var point = _pointList.FirstOrDefault(p => p.Id == id);

             if (id <= 0)
            {
                var errorResponse = new ApiResponse<Point> 
                { 
                    IsSuccess = false, 
                    Message = "ID must be greater than 0" 
                };
                return BadRequest(errorResponse);
            }
            
            if (point == null) // for type safety, we check if point is null (like ts)
            {
                var errorResponse = new ApiResponse<Point>
                {
                    IsSuccess = false,
                    Message = $"No point with ID #{id} found in the database."
                };
                return NotFound(errorResponse);
            }

            var successResponse = new ApiResponse<Point>
            {
                IsSuccess = true,
                Message = $"Point with ID #{id} returned successfully.",
                Data = point
            };
            return Ok(successResponse);
        }

        //* PUT method to update an existing point by ID

        [HttpPut("{id}")]
        public ActionResult<ApiResponse<Point>> UpdatePoint(int id, double pointX, double pointY, string name)
        
        {
            // Find the point with the given ID
            var existingPoint = _pointList.FirstOrDefault(p => p.Id == id);
            if (existingPoint == null)
            {
                var errorResponse = new ApiResponse<Point>
                {
                    IsSuccess = false,
                    Message = $"No point with ID #{id} found in the database."
                };
                return NotFound(errorResponse);
            }

            // Update the existing point with new values
            existingPoint.PointX = pointX;
            existingPoint.PointY = pointY;
            existingPoint.Name = name;

            var successResponse = new ApiResponse<Point>
            {
                IsSuccess = true,
                Message = $"Point with ID #{id} has been successfully updated.",
                Data = existingPoint // Return the updated point
            };

            return Ok(successResponse); // Return 200 OK with the updated point
        }
        //* DELETE method to remove a point by ID

       [HttpDelete("{id}")]
        public ActionResult<ApiResponse<Point>> DeletePoint(int id)
        {
            // Find the point with the given ID
            var point = _pointList.FirstOrDefault(p => p.Id == id);
            if (point == null)
            {
                var errorResponse = new ApiResponse<Point>
                {
                    IsSuccess = false,
                    Message = $"No point with ID #{id} found in the database."
                };
                return NotFound(errorResponse);
            }

            // Remove the point from the list
            _pointList.Remove(point);

            var successResponse = new ApiResponse<Point>
            {
                IsSuccess = true,
                Message = $"Point with ID #{id} has been successfully deleted.",
                Data = point // deleted point data
            };
            
            return Ok(successResponse); // Return 200 OK with the deleted point data
        }
        
    }
}