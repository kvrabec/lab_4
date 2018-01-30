using System;
using System.Collections.Generic;
using System.Text;

namespace LAB4.MODEL.Entities.ActuallyUsefullClasses
{
    public class TokenRequestModel
    {
        public string FirstName { get; set; }
        public string AccountNumber { get; set; }
        public string Rowguid { get; set; }
        public Customer Customer { get; set; }

        public TokenRequestModel()
        { }
    }
}
