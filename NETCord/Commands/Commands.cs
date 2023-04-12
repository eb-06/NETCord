using Discord;
using Discord.Commands;
using Discord.Interactions;
using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NETCord.Commands
{
    public class Commands : InteractionModuleBase<SocketInteractionContext>
    {
        EmbedBuilder embedBuilder = new EmbedBuilder();
        ComponentBuilder componentBuilder = new ComponentBuilder();

        [SlashCommand("echo", "Echo whatever you had inputted.")]
        public async Task Echo([Remainder] string message) => await RespondAsync(message);

        [SlashCommand("choice", "Choose between true or false.")]
        public async Task Choice(bool choice) => await RespondAsync($"You picked: {choice}");

        [SlashCommand("messagebox", "Create and send a message box with a customized message.")]
        public async Task NewMessageBox([Remainder] string message)
        {
            await RespondAsync("Created message box!");
            MessageBox.Show(message, "NETCord", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        [SlashCommand("readfile", "Read contents of a file.")]
        public async Task ReadFile(IAttachment attachment)
        {
            using (WebClient WebClient = new WebClient())
            {
                embedBuilder.WithTitle("NETCord");
                embedBuilder.WithDescription("File:");
                if (Encoding.Unicode.GetByteCount(WebClient.DownloadString(attachment.Url)) >= 1024)
                    embedBuilder.AddField(attachment.Filename, "```\nThe file is to big to be displayed.\n```");
                else embedBuilder.AddField(attachment.Filename, $"```\n{WebClient.DownloadString(attachment.Url)}\n```");
                embedBuilder.WithThumbnailUrl("https://raw.githubusercontent.com/eb-06/NETCord/main/Resources/GitHub.png");
                embedBuilder.WithColor(Color.Green);
                embedBuilder.WithFooter("NETCord", "https://avatars.githubusercontent.com/u/73481203?v=4").WithTimestamp(DateTimeOffset.Now);
                await RespondAsync(embed: embedBuilder.Build());
            }
        }

        [SlashCommand("embed", "Display an embed.")]
        public async Task Embed()
        {
            embedBuilder.WithTitle("NETCord");
            embedBuilder.WithDescription("This is an embed, how cool!");
            embedBuilder.WithThumbnailUrl("https://raw.githubusercontent.com/eb-06/NETCord/main/Resources/GitHub.png");
            embedBuilder.WithColor(Color.Default);
            embedBuilder.WithFooter("NETCord", "https://avatars.githubusercontent.com/u/73481203?v=4").WithTimestamp(DateTimeOffset.Now);
            componentBuilder.WithButton(label: "GitHub Repository", style: ButtonStyle.Link, url: "https://github.com/eb-06/NETCord");
            await RespondAsync(embed: embedBuilder.Build(), components: componentBuilder.Build());
            Process.Start("https://github.com/eb-06/NETCord");
        }
    }
}