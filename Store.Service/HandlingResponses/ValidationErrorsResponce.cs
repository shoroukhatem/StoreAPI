using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.HandlingResponses
{
    public class ValidationErrorsResponce : CustomException
    {
        public ValidationErrorsResponce() : base(400)
        {
        }
        public IEnumerable<string> Errors { get; set; }
    }
}
