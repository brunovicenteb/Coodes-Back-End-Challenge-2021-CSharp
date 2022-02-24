using System;
using Microsoft.AspNetCore.Mvc;
using Coodesh.Back.End.Challenge2021.CSharp.Toolkit.Exceptions;

namespace Coodesh.Back.End.Challenge2021.CSharp.Toolkit.Web
{
    public abstract class XControllerBase : ControllerBase
    {
        protected IActionResult TryExecuteOK(Func<object> pExecute)
        {
            Func<object, IActionResult> action = delegate (object result)
            {
                return Ok(result);
            };
            return TryExecute(action, pExecute);
        }

        protected IActionResult TryExecuteOKCreated(string pActionName, Func<object> pExecute)
        {
            Func<object, IActionResult> action = delegate (object result)
            {
                return CreatedAtAction(pActionName, result);
            };
            return TryExecute(action, pExecute);
        }

        protected IActionResult TryExecute(Func<object, IActionResult> pResultFunc, Func<object> pExecute)
        {
            try
            {
                object result = pExecute();
                return pResultFunc(result);
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