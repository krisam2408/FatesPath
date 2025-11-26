using AppConfigLib;
using DiscordBot;
using DiscordBot.Configuration;

AppConfig appConfig = await AppConfigBuilder.ReadSettings<AppConfig>();

DiscordBotApp app = DiscordBotApp.Build(appConfig);

await app.Run();