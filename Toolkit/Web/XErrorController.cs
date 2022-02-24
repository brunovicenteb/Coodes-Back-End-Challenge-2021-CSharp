using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Coodesh.Back.End.Challenge2021.CSharp.Toolkit.Web
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class XErrorController : ControllerBase
    {
        [Route("error")]
        public XErrorResponse Error()
        {
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            var id = Activity.Current?.Id ?? HttpContext?.TraceIdentifier;
            return new XErrorResponse(id);
        }
    }
}