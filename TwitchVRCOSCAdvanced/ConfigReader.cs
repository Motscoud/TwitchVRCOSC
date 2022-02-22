using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace TwitchVRCOSCAdvanced
{
    public class ConfigReader
    {
        //Test
        private String uname;
        private String oauth;
        private String streamer;
        private String[] commands;
        private String CurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public (String, String, String) GetConfig()
        {
            //get uname
            uname = GetLine((CurrentDir+"\\config.conf"), 2);
            oauth = GetLine((CurrentDir + "\\config.conf"), 4);
            streamer = GetLine((CurrentDir + "\\config.conf"), 6);

            return (uname, oauth, streamer);
        }

        public String[] getCommands()
        {
            commands = File.ReadAllLines(CurrentDir + "\\commands.conf");
            return commands;
        }

        private string GetLine(string fileName, int line)
        {
            using (var sr = new StreamReader(fileName))
            {
                for (int i = 1; i < line; i++)
                    sr.ReadLine();
                return sr.ReadLine();
            }
        }
    }
}
