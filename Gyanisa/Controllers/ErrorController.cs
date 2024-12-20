using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gyanisa.Controllers
{
    public class ErrorController : Controller
    {
        //[Route("Error/{statusCode}")]
        //public IActionResult HttpStatusCodeHandler( int statusCode)
        //{
        //    switch(statusCode)
        //    {
        //        case 404:
        //            ViewBag.ErroMessage = "Sorry, the resource you requested could not be found ";
        //            break;
        //    }
        //    return View("NotFound");
        //}
        [AllowAnonymous]
        [Route("Error")]       
        public IActionResult Error()
        {
            //retrive the exception Details
            var exceptionHandlerPathFeature =
                HttpContext.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
            ViewBag.ExceptionPath = exceptionHandlerPathFeature.Path;
            ViewBag.ExceptionMessage = exceptionHandlerPathFeature.Error.Message;
            ViewBag.StackTrace = exceptionHandlerPathFeature.Error.StackTrace;

            return View("NotFound");
        }
    }
}
