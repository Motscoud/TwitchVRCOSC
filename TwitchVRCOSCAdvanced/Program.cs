using AsyncAwaitBestPractices;
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
            bot.Start().SafeFireAndForget();
            await bot.JoinStreamer(streamer);
            await bot.SendMessage(streamer, "Beep boop! Hello I am TwitchVRCOSC, I'll be able to send commands to avatars in VRChat!");
            await Task.Delay(-1);
            //Console.WriteLine($"{uname}+{oauth}+{streamer}");
        }
    }
}
