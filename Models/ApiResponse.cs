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
        public T? Data { get; set; } //* generic type because i will use this class for different types of data list and single data
                                      
    }
}