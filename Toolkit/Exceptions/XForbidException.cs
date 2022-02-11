using System;

namespace Coodesh.Back.End.Challenge2021.CSharp.Toolkit.Exceptions
{
    public sealed class XForbidException : XException
    {
        public XForbidException(string pMessage)
            : base(pMessage)
        {
        }
    }
}