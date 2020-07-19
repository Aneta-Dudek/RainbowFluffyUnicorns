using Microsoft.AspNetCore.Mvc.Filters;

namespace Questore.ModelState
{
    public class ModelStateTransfer : ActionFilterAttribute
    {
        protected const string Key = nameof(ModelStateTransfer);
    }
}
