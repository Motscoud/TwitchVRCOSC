using AsyncAwaitBestPractices;
using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;
using SharpOSC;

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
            string[] commands = conf.getCommands();
            string commlist = "!help,";

            foreach (var item in commands.Skip(1))
            {
                commlist = commlist + $" {item},";
            }

            var oscsender = new SharpOSC.UDPSender("127.0.0.1",9000);
            var oscsleep = new SharpOSC.OscMessage("/avatar/parameters/twitchindex", 0);

            var bot = new TwitchBot(uname, oauth, streamer);
            bot.Start().SafeFireAndForget();
            await bot.JoinStreamer(streamer);
            await bot.SendMessage(streamer, "Beep boop! Hello I am TwitchVRCOSC, I'll be able to send commands to avatars in VRChat!");

            bot.OnMessage += async (Sender, TwitchChatMessage) =>
            {
                Console.WriteLine($"{TwitchChatMessage.Sender}: {TwitchChatMessage.Message}");
                if (commands.Any(TwitchChatMessage.Message.Contains))
                {
                    Console.WriteLine("Valid command recieved");
                    int commandposition = Array.IndexOf(commands,TwitchChatMessage.Message);
                    var oscmessage = new SharpOSC.OscMessage("/avatar/parameters/twitchindex", commandposition);
                    oscsender.Send(oscmessage);
                    Thread.Sleep(100);
                    oscsender.Send(oscsleep);

                }
                else if (TwitchChatMessage.Message == "!help")
                {
                    await bot.SendMessage(streamer, $"Here are the available commands: {commlist}");
                }
            };
            await Task.Delay(-1);
            //Console.WriteLine($"{uname}+{oauth}+{streamer}");


        }
    }
}
