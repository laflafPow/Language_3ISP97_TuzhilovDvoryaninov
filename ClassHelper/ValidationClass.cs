using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Language_3ISP97_TuzhilovDvoryaninov.ClassHelper
{
    public class ValidationClass
    {
        public static bool ValidationFIO(string name)
        {
            if (name.Length < 2 || name.Length > 50)
                return false;
            if (name.Any(Char.IsDigit))
                return false;
            if (!name.Contains("-") && !name.Contains(" ") && name.Any(Char.IsPunctuation))
                return false;

            return true;
        }

        public static bool ValidationEmail(string email)
        {
            try
            {
                MailAddress m = new MailAddress(email);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        //плюс, минус, открывающая и
        //закрывающая круглые скобки, знак пробела
        public static bool ValidationPhone(string phone)
        {
            if (phone.Any(Char.IsLetter))
                return false;
            if (!phone.Contains("-") && !phone.Contains("+") && 
                !phone.Contains("(") && !phone.Contains(")") && 
                !phone.Contains(" ") && phone.Any(Char.IsPunctuation))
                return false;

            return true;
        }
    }
}
