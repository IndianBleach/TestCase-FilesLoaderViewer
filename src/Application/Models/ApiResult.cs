using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class ApiResult<T>
    {
        public bool Succeeded { get; set; }

        public T? Content { get; set; }

        public ICollection<string> Errors { get; set; }

        public ApiResult(bool successed,
            T? content,
            ICollection<string> errors)
        {
            Content = content;
            Succeeded = successed;
            Errors = errors;
        }

        public static ApiResult<T> Failed(T? content, ICollection<string> errors)
            => new(false, content, errors);

        public static ApiResult<T> SuccessOk(T content)
            => new (true, content, new List<string>());

    }
}
