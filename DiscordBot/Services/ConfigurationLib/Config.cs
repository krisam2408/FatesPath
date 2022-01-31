using FatesPathLib;
using FatesPathLib.Configuration;
using Newtonsoft.Json;
using System.IO;

namespace DiscordBot.Services.ConfigurationLib
{
    public class Config
    {
        private const string cfgPath = "config.json";

        public string Token { get; set; }
        public FateConfig FateConfig { get; set; }
        public DefaultCast DefaultCast { get; set; }

        private static Config _instance;
        public static Config Instance
        {
            get
            {
                if (_instance == null)
                {
                    string path = Path.GetFullPath(cfgPath);
                    string content = File.ReadAllText(path);
                    _instance = JsonConvert.DeserializeObject<Config>(content);
                }
                return _instance;
            }
        }

    }

    public class DefaultCast
    {
        public DiceType DiceType { get; set; }
        public int Difficulty { get; set; }
        public int Threshold { get; set; }
        public bool ThrowAgain { get; set; }
        public int ThrowAgainMinValue { get; set; }
    }
}
