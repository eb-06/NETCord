using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace NETCord.Bot
{
    class Backend
    {
        private DiscordSocketClient client;

        public async Task Initialize(string token)
        {
            IHost IHost = Host.CreateDefaultBuilder().ConfigureServices((Singleton) => Singleton
            .AddSingleton<Interaction>()
            .AddSingleton(new CommandService())
            .AddSingleton(new DiscordSocketClient())
            .AddSingleton(InteractionService => new InteractionService(InteractionService.GetRequiredService<DiscordSocketClient>()))).Build();
            await Load(token, IHost);
        }

        private async Task Load(string token, IHost ihost)
        {
            IServiceScope service = ihost.Services.CreateScope();
            IServiceProvider provider = service.ServiceProvider;

            var Commands = provider.GetRequiredService<InteractionService>();
            client = provider.GetRequiredService<DiscordSocketClient>();
            await provider.GetRequiredService<Interaction>().InitializeAsync();
            client.Ready += async () => { await Commands.RegisterCommandsGloballyAsync(false); };
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
            await client.SetGameAsync($"NETCord - https://github.com/eb-06/NETCord");
            await Task.Delay(-1);
        }
    }
}