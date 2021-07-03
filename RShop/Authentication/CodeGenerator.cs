using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RShop.Authentication
{
    public class CodeGenerator
    {
        public static string ActiveCode()
        {
            return Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);
        }
    }
}
