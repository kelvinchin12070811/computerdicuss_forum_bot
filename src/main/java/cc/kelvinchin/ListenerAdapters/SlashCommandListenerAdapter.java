package cc.kelvinchin.ListenerAdapters;

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
        }
    }
}
