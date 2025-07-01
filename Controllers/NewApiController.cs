using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using geoproject.Models; // Ensure you have the correct namespace for Point model

namespace geoproject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewApiController : ControllerBase
    {
        private static readonly List<Point> _pointList = new List<Point>();

        [HttpGet]
        public List<Point> GetAllPoints()
        {
            // Return the list of points
            return _pointList;
        }

        [HttpPost]

        public Point AddPoint(Point p)
        {
            // Add the new point to the list
            _pointList.Add(p);
            return p; // Return the added point
        }


    }
}