#include <iostream>
#include <dpp/dpp.h>

void onBotReady(dpp::cluster &client, const dpp::ready_t &ev) {
    if (dpp::run_once<struct register_bot_commands>()) {
        client.global_command_create(dpp::slashcommand { "ping", "Ping Pong!", client.me.id });
    }
}

int main(int argc, char** argv) {
    dpp::cluster client { "" };
    client.on_log(dpp::utility::cout_logger());
    client.on_ready([&client](const dpp::ready_t &ev) { onBotReady(client, ev); });
    return 0;
}