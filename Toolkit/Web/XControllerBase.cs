using System;
using Microsoft.AspNetCore.Mvc;
using Coodesh.Back.End.Challenge2021.CSharp.Toolkit.Exceptions;

namespace Coodesh.Back.End.Challenge2021.CSharp.Toolkit.Web
{
    public abstract class XControllerBase : ControllerBase
    {
        protected IActionResult Execute(Func<object> pExecute)
        {
            try
            {
                var result = pExecute();
                return Ok(result);
            }
            catch (XNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (XForbidException ex)
            {
                return Forbid(ex.Message);
            }
            catch (XUnauthorizedException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}