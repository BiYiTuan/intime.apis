using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yintai.Architecture.Common.Models;

namespace Yintai.Hangzhou.Model
{
    public class ErrorResult
    {
        public ErrorResult(string message)
        {
            Code = StatusCode.InternalServerError;
            Message = message;
        }
        public StatusCode Code { get; set; }
        public string Message { get; set; }
    }
}
