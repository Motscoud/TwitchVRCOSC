using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

namespace TwitchVRCOSCAdvanced
{
    public class TwitchBot
    {
        const string ip = "irc.chat.twitch.tv";
        const int port = 6667;
        private TaskCompletionSource<int> connect = new TaskCompletionSource<int>();

        private string uname;
        private string oauth;
        private string streamer;
        private StreamReader streamReader;
        private StreamWriter streamWriter;

        public TwitchBot(string uname, string oauth, string streamer)
        {
            this.uname = uname;
            this.oauth = oauth;
            this.streamer = streamer;
        }

        public async Task Start()
        {
            var tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(ip, port);
            //StreamRead/Write to pull and push data to Twitch IRC
            streamReader = new StreamReader(tcpClient.GetStream());
            // NewLine = \r\n adds \r\n on the end of a line to mark a new message
            // AutoFlush will call streamWriter.Flush() automagically
            streamWriter = new StreamWriter(tcpClient.GetStream()) { NewLine = "\r\n", AutoFlush = true };
            await streamWriter.WriteLineAsync($"PASS {oauth}");
            await streamWriter.WriteLineAsync($"NICK {uname}");
            connect.SetResult(0);

            while (true)
            {
                string line = await streamReader.ReadLineAsync();
                //Console.WriteLine(line);
                string[] split = line.Split(" ");
                //Anti-Disconnect measure
                if (line.StartsWith("PING"))
                {
                    Console.WriteLine("Recieved Ping Check from Twitch");
                    await streamWriter.WriteLineAsync($"PONG {split[1]}");
                }

                if (split.Length > 1 && split[1] == "PRIVMSG")
                {
                    int exclamationPointPosition = split[0].IndexOf("!");
                    string user = split[0].Substring(1, exclamationPointPosition - 1);
                    int secondcolon = line.IndexOf(':', 1);
                    string message = line.Substring(secondcolon + 1);
                    Console.WriteLine($"{user}: {message}");
                }

            }

        }

        public async Task SendMessage(String streamer, String message)
        {
            await connect.Task;
            await streamWriter.WriteLineAsync($"PRIVMSG #{streamer} :{message}");
        }

        public async Task JoinStreamer(String streamer)
        {
            await connect.Task;
            await streamWriter.WriteLineAsync($"JOIN #{streamer}");
        }
    }
}
