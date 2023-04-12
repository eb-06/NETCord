using Discord.Interactions;
using Discord.WebSocket;
using Discord;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace NETCord.Bot
{
    internal class Interaction
    {
        private readonly DiscordSocketClient Client;
        private readonly InteractionService Service;
        private readonly IServiceProvider Provider;

        private Task ComponentCommandExecuted(ComponentCommandInfo component, IInteractionContext interaction, IResult result) { return Task.CompletedTask; }
        private Task ContextCommandExecuted(ContextCommandInfo context, IInteractionContext interaction, IResult result) { return Task.CompletedTask; }
        private Task SlashCommandExecuted(SlashCommandInfo info, IInteractionContext interaction, IResult result) { return Task.CompletedTask; }

        public Interaction(DiscordSocketClient client, InteractionService service, IServiceProvider provider)
        {
            Client = client;
            Service = service;
            Provider = provider;
        }

        public async Task InitializeAsync()
        {
            await Service.AddModulesAsync(Assembly.GetEntryAssembly(), Provider);
            Client.InteractionCreated += HandleInteraction;
            Service.SlashCommandExecuted += SlashCommandExecuted;
            Service.ContextCommandExecuted += ContextCommandExecuted;
            Service.ComponentCommandExecuted += ComponentCommandExecuted;
        }

        private async Task HandleInteraction(SocketInteraction socket)
        {
            try { await Service.ExecuteCommandAsync(new SocketInteractionContext(Client, socket), Provider); }
            catch
            {
                if (socket.Type == InteractionType.ApplicationCommand)
                    await socket.GetOriginalResponseAsync().ContinueWith(async (Response) => await Response.Result.DeleteAsync());
            }
        }
    }
}