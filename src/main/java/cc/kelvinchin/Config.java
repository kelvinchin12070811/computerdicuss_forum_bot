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
        }

        var config = (Map<String, Object>) load.loadFromInputStream(configFile);

        token = (String) config.get("token");
        clientID = Long.toString((long) config.get("client id"));
        autoDisposeDuration = (Integer) config.get("auto dispose duration");

        System.out.printf("token: %s\nclient id: %s\nauto dispose duration: %s\n", token, clientID,
                autoDisposeDuration);
    }
}
