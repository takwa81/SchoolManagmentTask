using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Responses
{
    public class ApiResponse<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public int Code { get; set; }

        public static ApiResponse<T> Success(T data, string message = "Success", int code = 200)
        {
            return new ApiResponse<T> { Status = true, Message = message, Data = data, Code = code };
        }

        public static ApiResponse<T> Fail(string message = "Error", int code = 500)
        {
            return new ApiResponse<T> { Status = false, Message = message, Data = default, Code = code };
        }
    }
}
