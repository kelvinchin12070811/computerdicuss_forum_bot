package cc.kelvinchin;

import org.snakeyaml.engine.v2.api.Load;
import org.snakeyaml.engine.v2.api.LoadSettings;

import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.InputStream;
import java.util.Map;

public final class Config {
    public static final String VERSION = "2.2.0-indev";

    public static String token = "";
    public static String clientID = "";
    public static int autoDisposeDuration = 0;
    public static String serverID = "";
    public static boolean isProduction = false;

    public static void parseConfig() throws FileNotFoundException {
        parseConfig("");
    }

    public static void parseConfig(String path) throws FileNotFoundException {
        InputStream configFile;
        var settings = LoadSettings.builder().build();
        var load = new Load(settings);

        if (path.equals("")) {
            configFile = Config.class.getClassLoader().getResourceAsStream("config.yaml");
            if (configFile == null) throw new FileNotFoundException("Failed to load config.yaml from resources");
        } else {
            configFile = new FileInputStream(path);
            isProduction = true;
        }

        var config = (Map<String, Object>) load.loadFromInputStream(configFile);

        token = config.get("token").toString();
        clientID = config.get("client id").toString();
        autoDisposeDuration = Integer.parseInt(config.get("auto dispose duration").toString());
        serverID = config.get("server id").toString();
    }
}
