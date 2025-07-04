using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using geoproject.Models;

namespace geoproject.Interfaces
{
    public interface IPointUpdateService
    {
        Task<ApiResponse<Point>> UpdatePointAsync(int id, double pointX, double pointY, string name, CoordinateType coordinateType);
    }
}