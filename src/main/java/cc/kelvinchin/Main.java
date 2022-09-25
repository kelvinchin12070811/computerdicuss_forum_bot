package cc.kelvinchin;

import org.apache.commons.cli.*;

public class Main {
    public static void main(String[] args) {
        Options options = new Options();
        Option optVersion = Option.builder("v").longOpt("version").desc("Print the version number").build();
        Option optHelp = Option.builder("h").longOpt("help").desc("Print this message").build();
        Option optConfigFile = Option.builder("config").hasArg().argName("configFile")
                .desc("Config file of the bot to apply, and switch to production mode").build();

        options.addOption(optHelp);
        options.addOption(optVersion);
        options.addOption(optConfigFile);

        CommandLine cmdLine;
        CommandLineParser parser = new DefaultParser();
        HelpFormatter helpMsg = new HelpFormatter();

        try {
            cmdLine = parser.parse(options, args);

            if (cmdLine.hasOption("version")) printVersion();
            if (cmdLine.hasOption("help")) printHelp(options, helpMsg);

            if (cmdLine.hasOption("config")) Config.parseConfig(cmdLine.getOptionValue("configFile"));
            else Config.parseConfig();
        } catch (Exception e) {
            System.err.println(e.getMessage());
            System.exit(0);
        }

        try {
            Bot.INSTANCE.init();
        } catch (Exception e) {
            System.err.println(e.getMessage());
            e.printStackTrace();
        }
    }

    public static void printVersion() {
        System.out.println(Config.VERSION);
    }

    public static void printHelp(Options options, HelpFormatter formatter) {
        formatter.printHelp("Usage: ", options);
    }
}