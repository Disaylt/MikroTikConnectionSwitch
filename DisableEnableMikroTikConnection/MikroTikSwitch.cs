using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tik4net;

namespace MikroTikConnectionSwitch
{
    public class MikroTikSwitch
    {
        private readonly ConnectionData _connectionData;

        public MikroTikSwitch(ConnectionData connectionData)
        {
            _connectionData = connectionData;
        }

        private string GetSecretId(string name)
        {
            using ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api);
            connection.Open(_connectionData.Host, _connectionData.User, _connectionData.Password);

            var cmd = connection.CreateCommand("ppp/secret/print");
            var id = cmd.ExecuteList().Single(x => x.Words["name"] == name).GetId();
            return id;
        }
        private string GetConnectionId(string name)
        {
            using ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api);
            connection.Open(_connectionData.Host, _connectionData.User, _connectionData.Password);

            var cmd = connection.CreateCommand("ppp/active/print");
            var id = cmd.ExecuteList().Single(x => x.Words["name"] == name).GetId();
            return id;
        }
        public SwitchOffData GetSwitchOffData(string name)
        {
            var d = new SwitchOffData()
            {
                SecretId = GetSecretId(name),
                ConnectionId = GetConnectionId(name)
            };
            return d;
        }
        public void SwithOffConnection(string secretId, string connectionId)
        {
            DisableSecret(secretId);
            RemoveActiveConnection(connectionId);
        }

        public void SwithOnConnection(string secretId)
        {
            EnableSecret(secretId);
        }

        public void PrintSecrets()
        {
            using ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api);
            connection.Open(_connectionData.Host, _connectionData.User, _connectionData.Password);

            var cmd = connection.CreateCommand("ppp/secret/print");
            var r = cmd.ExecuteList();
            foreach (var item in r)
            {
                Console.WriteLine("\t" + item);
            }
        }

        public void PrintActiveConnections()
        {
            using ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api);
            connection.Open(_connectionData.Host, _connectionData.User, _connectionData.Password);

            var cmd = connection.CreateCommand("ppp/active/print");
            var r = cmd.ExecuteList();
            foreach (var item in r)
            {
                Console.WriteLine("\t" + item);
            }
        }

        private void DisableSecret(string secretId)
        {
            using ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api);
            connection.Open(_connectionData.Host, _connectionData.User, _connectionData.Password);

            var cmd = connection.CreateCommand("ppp/secret/disable", connection.CreateParameter(".id", secretId));
            cmd.ExecuteNonQuery();
        }

        private void EnableSecret(string secretId)
        {
            using ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api);
            connection.Open(_connectionData.Host, _connectionData.User, _connectionData.Password);

            var cmd = connection.CreateCommand("ppp/secret/enable", connection.CreateParameter(".id", secretId));
            cmd.ExecuteNonQuery();
        }

        private void RemoveActiveConnection(string connectionId)
        {
            using ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api);
            connection.Open(_connectionData.Host, _connectionData.User, _connectionData.Password);

            var cmd = connection.CreateCommand("ppp/active/remove", connection.CreateParameter(".id", connectionId));
            cmd.ExecuteNonQuery();
        }
    }
}
