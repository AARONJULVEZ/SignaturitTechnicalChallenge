using Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class ApiBaseController : ControllerBase
    {
        protected ActionResult ResolveResult<T>(Result<T> result)
        {
            if (result.IsCorrect)
            {
                return Ok(result.Value);
            }
            else
            {
                if (result.Error.Code == 400)
                {
                    return BadRequest(result.Error.Message);
                }
                else
                {
                    return StatusCode((int)result.Error.Code, result.Error);
                }
            }
        }
    }
}
