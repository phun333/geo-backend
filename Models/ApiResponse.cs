using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace geoproject.Models
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty; // nullable reference type
        public T? Data { get; set; } // T -> generic type
    }
}