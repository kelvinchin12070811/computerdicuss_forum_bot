package cc.kelvinchin.Events;

import cc.kelvinchin.Bot;
import net.dv8tion.jda.api.events.GenericEvent;
import net.dv8tion.jda.api.events.ReadyEvent;
import net.dv8tion.jda.api.hooks.EventListener;

public class ReadyEventListener implements EventListener {
    @Override
    public void onEvent(GenericEvent ev) {
        if (!(ev instanceof ReadyEvent)) return;

        System.out.println("Bot is Ready");
        System.out.printf("Hi! I'm %s!\n", Bot.INSTANCE.getClient().getSelfUser().getName());
    }
}
