using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikroTikConnectionSwitch
{
    public class ConnectionData
    {
        public string Host { get; }
        public string User { get; }
        public string Password { get; }

        public ConnectionData(string host, string user, string password)
        {
            Host = host;
            User = user;
            Password = password;
        }
    }
}
