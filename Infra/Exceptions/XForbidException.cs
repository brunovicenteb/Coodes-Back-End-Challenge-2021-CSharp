using System;

namespace Coodesh.Back.End.Challenge2021.CSharp.Infra.Exceptions
{
    public sealed class XForbidException : XException
    {
        public XForbidException(string pMessage)
            : base(pMessage)
        {
        }
    }
}