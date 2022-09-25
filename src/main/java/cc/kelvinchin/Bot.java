package cc.kelvinchin;

import cc.kelvinchin.Events.ReadyEventListener;
import cc.kelvinchin.ListenerAdapters.SlashCommandListenerAdapter;
import net.dv8tion.jda.api.JDA;
import net.dv8tion.jda.api.JDABuilder;
import net.dv8tion.jda.api.requests.GatewayIntent;

import java.util.EnumSet;
import java.util.Map;
import java.util.TreeMap;

public enum Bot {
    INSTANCE;

    private JDA client = null;

    public void init() throws InterruptedException {
        client = JDABuilder.createDefault(Config.token).enableIntents(EnumSet.allOf(GatewayIntent.class))
                .addEventListeners(new ReadyEventListener()).addEventListeners(new SlashCommandListenerAdapter())
                .build();
        client.awaitReady();
        registerCommands();
    }

    public void registerCommands() {
        Map<String, String> commands = new TreeMap();
        commands.put("ping", "Send ping response");

        if (Config.isProduction) {
            for (var commandName : commands.keySet()) {
                client.upsertCommand(commandName, commands.get(commandName)).queue();
            }
        } else {
            var serverID = Config.serverID.trim();
            if (serverID.isEmpty() || serverID.equals("~")) {
                throw new RuntimeException("Error, no server id provided in config file.");
            }

            var server = client.getGuildById(serverID);

            if (server == null) throw new RuntimeException(String.format("Cannot get targeted server (%s).", serverID));

            for (var commandName : commands.keySet()) {
                server.upsertCommand(String.format("%s-test", commandName), commands.get(commandName)).queue();
            }
        }
    }

    public JDA getClient() {
        return client;
    }
}
