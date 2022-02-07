using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Coodesh.Back.End.Challenge2021.CSharp.Entities.Notifications
{
    public class XNotifies
    {
        public XNotifies()
        {
            Notifies = new List<XNotifies>();
        }

        public XNotifies(string pMessage, string pPropertyName)
        {
            Message = pMessage;
            PropertyName = pPropertyName;
            Notifies = new List<XNotifies>();
        }

        [NotMapped]
        public string PropertyName
        {
            get; set;
        }

        [NotMapped]
        public string Message
        {
            get; set;
        }

        [NotMapped]
        public List<XNotifies> Notifies;

        public bool CheckString(string pValue, string pPropertyName)
        {
            if (!string.IsNullOrWhiteSpace(pValue) && !string.IsNullOrWhiteSpace(pPropertyName))
                return true;
            Notifies.Add(new XNotifies("Campo Obrigatório", pPropertyName));
            return false;
        }

        public bool CheckInt(int pValue, string pPropertyName)
        {
            if (pValue >= 1 && !string.IsNullOrWhiteSpace(pPropertyName))
                return true;
            Notifies.Add(new XNotifies("Valor deve ser maior que 0", pPropertyName));
            return false;
        }

        public bool CheckDecimal(decimal pValue, string pPropertyName)
        {
            if (pValue >= 1 && !string.IsNullOrWhiteSpace(pPropertyName))
                return true;
            Notifies.Add(new XNotifies("Valor deve ser maior que 0", pPropertyName));
            return false;
        }
    }
}