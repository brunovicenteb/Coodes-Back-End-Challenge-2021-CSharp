using System;

namespace Coodesh.Back.End.Challenge2021.CSharp.Toolkit.Exceptions
{
    public sealed class XUnauthorizedException : XException
    {
        public XUnauthorizedException(string pMessage)
            : base(pMessage)
        {
        }
    }
}