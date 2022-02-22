using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace TwitchVRCOSCAdvanced
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var conf = new ConfigReader();
            var confResult = conf.GetConfig();
            string uname = confResult.Item1;
            string oauth = confResult.Item2;
            string streamer = confResult.Item3;

            var bot = new TwitchBot(uname, oauth, streamer);
            await bot.Start();
            //Console.WriteLine($"{uname}+{oauth}+{streamer}");
        }
    }
}
