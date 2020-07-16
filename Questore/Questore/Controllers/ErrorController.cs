using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Questore.Data.Logger;


namespace Questore.Controllers
{

    public class ErrorController : Controller
    {
        private ILog _iLog;

        public ErrorController()
        {
            _iLog = Data.Logger.Logger.GetInstance;
        }

        public IActionResult Index()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            _iLog.LogException(exceptionDetails.Path, exceptionDetails.Error.Message, exceptionDetails.Error.StackTrace);
            ViewBag.ExceptionPath = exceptionDetails.Path;
            ViewBag.ExceptionMessage = exceptionDetails.Error.Message;
            ViewBag.Stacktrace = exceptionDetails.Error.StackTrace;

            return View("Error");

        }


        public IActionResult Error(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry, the resource you requested could not be found.";
                    ViewBag.Path = statusCodeResult.OriginalPath;
                    ViewBag.QS = statusCodeResult.OriginalQueryString;
                    break;
            }

            return View("NotFound");

        }



    }
}