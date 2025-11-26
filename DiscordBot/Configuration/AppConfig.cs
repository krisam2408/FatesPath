using FatesPathLib;
using FatesPathLib.Configuration;

namespace DiscordBot.Configuration;

public class AppConfig
{
    public string DiscordToken { get; set; }
    public FateConfig FateConfig { get; set; }
}