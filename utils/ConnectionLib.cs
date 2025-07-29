using PrimS.Telnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace HuaweiONTRestart.utils
{
    internal class ConnectionLib
    {
        public bool CheckONTAvailable(string ip)
        {
            Ping pingSender = new Ping();
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 120;

            PingReply reply = pingSender.Send(ip, timeout, buffer);

            if (reply.Status == IPStatus.TimedOut)
            {
                return false;
            }

            return true;
        }
        public async Task<string?> GetIPAddress()
        {
            HttpClient client = new HttpClient();
            try
            {
                using HttpResponseMessage response = await client.GetAsync("https://4.ident.me/");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                return responseBody;
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro, código: " + e.ToString());
                return null;
            }
        }

        [Obsolete]
        public async Task RebootONT(string IP)
        {
            using var client = new Client(IP, 23, new CancellationToken());
            Thread.Sleep(1500);

            await client.WriteLineAsync("root");
            Thread.Sleep(1500);
            await client.WriteLineAsync("admin");

            Thread.Sleep(1500);
            await client.WriteLineAsync("reset");

        }
    }
}
