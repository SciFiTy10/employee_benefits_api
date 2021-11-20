using System;
using Microsoft.AspNetCore.Mvc;

namespace employee_benefits_api.Classes
{
    public class Result : ActionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
