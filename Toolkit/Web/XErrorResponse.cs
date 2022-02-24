using System;

namespace Coodesh.Back.End.Challenge2021.CSharp.Toolkit.Web
{
    public class XErrorResponse
    {
        public XErrorResponse(string pID)
        {
            ID = pID;
            Moment = DateTime.Now;
            Message = "Um erro inesperado aconteceu no servidor. Por favor entre em contato com o suporte.";
        }

        public string ID { get; set; }
        public DateTime Moment { get; set; }
        public string Message { get; set; }
    }
}