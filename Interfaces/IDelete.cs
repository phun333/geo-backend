using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using geoproject.Models;

namespace geoproject.Interfaces
{
    public interface IPointDeleteService
    {
        Task<ApiResponse<Point>> DeletePointAsync(int id);
    }
}