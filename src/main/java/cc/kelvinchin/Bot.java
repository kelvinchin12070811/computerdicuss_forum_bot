package cc.kelvinchin;

import cc.kelvinchin.Events.ReadyEventListener;
import net.dv8tion.jda.api.JDA;
import net.dv8tion.jda.api.JDABuilder;
import net.dv8tion.jda.api.requests.GatewayIntent;

import java.util.EnumSet;

public enum Bot {
    INSTANCE;

    private JDA client = null;

    public void init() {
        client = JDABuilder.createDefault(Config.token).enableIntents(EnumSet.allOf(GatewayIntent.class))
                .addEventListeners(new ReadyEventListener()).build();
    }

    public JDA getClient() {
        return client;
    }
}
