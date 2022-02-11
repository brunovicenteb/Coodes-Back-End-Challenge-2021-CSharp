using System;

namespace Coodesh.Back.End.Challenge2021.CSharp.Infra.Exceptions
{
    public sealed class XBadRequestException : XException
    {
        public XBadRequestException(string pMessage)
            : base(pMessage)
        {
        }
    }
}