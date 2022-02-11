using System;

namespace Coodesh.Back.End.Challenge2021.CSharp.Toolkit.Exceptions
{
    public sealed class XNotFoundException : XException
    {
        public XNotFoundException(string pMessage)
            : base(pMessage)
        {
        }
    }
}