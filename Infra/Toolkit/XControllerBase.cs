using System;
using Coodesh.Back.End.Challenge2021.CSharp.Infra.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Coodesh.Back.End.Challenge2021.CSharp.Infra.Toolkit
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