using AsyncAwaitBestPractices;
using System;
using System.IO;
using System.Linq;
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
            string[] commands = conf.getCommands();
            string commlist = "!help,";

            foreach (var item in commands.Skip(1))
            {
                commlist = commlist + $" {item},";
            }

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
