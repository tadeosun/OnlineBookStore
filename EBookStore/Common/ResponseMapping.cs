using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookStore.Common
{
    public class ResponseMapping
    {
        public const string ResponseCode00 = "00";
        public const string ResponseCode00Message = "Success";

        public const string ResponseCode01 = "01";
        public const string ResponseCode01Message = "Authentication failed, kindly try again.";

        public const string ResponseCode02 = "02";
        public const string ResponseCode02Message = "Parameter request incomplete";

        public const string ResponseCode03 = "03";
        public const string ResponseCode03Message = "Invalid genre supplied";

        public const string ResponseCode04 = "04";
        public const string ResponseCode04Message = "Invalid title supplied";

        public const string ResponseCode05 = "05";
        public const string ResponseCode05Message = "Invalid ISBN code supplied";

        public const string ResponseCode06 = "06";
        public const string ResponseCode06Message = "{0}} already exists.";

        public const string ResponseCode07 = "07";
        public const string ResponseCode07Message = "{0} not found.";

        public const string ResponseCode08 = "08";
        public const string ResponseCode08Message = "One or more items in the cart are no longer available or their prices have changed.";

        public const string ResponseCode09 = "09";
        public const string ResponseCode09Message = "Unable to save {0} record.";

        public const string ResponseCode10 = "10";
        public const string ResponseCode10Message = "Unable to update {0} record.";

        public const string ResponseCode11 = "11";
        public const string ResponseCode11Message = "Invalid password supplied, please check password complexity";

        public const string ResponseCode12 = "12";
        public const string ResponseCode12Message = "Invalid username supplied, please ensure that username is not more than 10 characters";

        public const string ResponseCode13 = "13";
        public const string ResponseCode13Message = "Invalid email supplied.";

        public const string ResponseCode14 = "14";
        public const string ResponseCode14Message = "Invalid ISBN code supplied";

        public const string ResponseCode99 = "99";
        public const string ResponseCode99Message = "Request processing error";
    }
}
