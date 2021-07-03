using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kavenegar;
namespace RShop.Authentication
{
    public class SMS
    {
        public void Send(string to,string body)
        {
            var sender = "1000596446";
            var receptor = to;
            var message = body;
            var api = new KavenegarApi("35503362386879645A4B7A5354524F76516F797A50625044494A3757706E373231792F4B5237626A75726B3D");
            api.Send(sender, receptor, message);
        }
    }
}
