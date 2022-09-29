package cc.kelvinchin.ListenerAdapters;

import cc.kelvinchin.Bot;
import net.dv8tion.jda.api.EmbedBuilder;
import net.dv8tion.jda.api.events.interaction.command.SlashCommandInteractionEvent;
import net.dv8tion.jda.api.hooks.ListenerAdapter;

public class SlashCommandListenerAdapter extends ListenerAdapter {
    @Override
    public void onSlashCommandInteraction(SlashCommandInteractionEvent ev) {
        if (ev.getName().equals("ping")) {
            long time = System.currentTimeMillis();
            ev.reply("Pong!").setEphemeral(true)
                    .flatMap(v -> ev.getHook().editOriginalFormat("Pong: %dms", System.currentTimeMillis() - time))
                    .queue();
        } else if (ev.getName().equals("issues-test")) {
            var msg = new EmbedBuilder().setTitle("Github Issue Tracker", "https://github.com/kelvinchin12070811" +
                            "/computerdicuss_forum_bot/issues")
                    .setDescription("Any issues? Please report it here!")
                    .setThumbnail("https://icons.duckduckgo.com/ip3/github.com.png")
                    .setAuthor(ev.getGuild().getMemberById(Bot.INSTANCE.getClient().getSelfUser().getId())
                            .getNickname(), "https://github.com/kelvinchin12070811/computerdicuss_forum_bot");

            ev.replyEmbeds(msg.build()).queue();
        }
    }
}
