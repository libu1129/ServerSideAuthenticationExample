using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerSideAuthenticationExample.Data
{
    public class SessionRequest
    {
        public string session_id { get; set; }
        public bool keep_login { get; set; }
    }

    public class SessionModel
    {
        public bool success { get; set; }
        public string message { get; set; }
    }
}
