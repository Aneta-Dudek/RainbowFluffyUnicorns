﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Questore.ModelState
{
    public class ExportModelStateAttribute : ModelStateTransfer
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //Only export when ModelState is not valid
            if (!filterContext.ModelState.IsValid)
            {
                if (filterContext.Result is RedirectResult
                    || filterContext.Result is RedirectToRouteResult
                    || filterContext.Result is RedirectToActionResult)
                {
                    var controller = filterContext.Controller as Controller;
                    if (controller != null && filterContext.ModelState != null)
                    {
                        var modelState = ModelStatePreserver.SerialiseModelState(filterContext.ModelState);
                        controller.TempData[Key] = modelState;
                    }
                }
            }
            base.OnActionExecuted(filterContext);
        }
    }
}
