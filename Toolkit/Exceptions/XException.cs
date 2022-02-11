using System;

namespace Coodesh.Back.End.Challenge2021.CSharp.Toolkit.Exceptions
{
    public abstract class XException : Exception
    {
        public XException(string pMessage)
            : base(pMessage)
        {
        }
    }
}