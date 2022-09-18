#include <iostream>
#include <dpp/dpp.h>

void onBotReady(dpp::cluster &client) {

}

int main(int argc, char** argv) {
    dpp::cluster client { "" };
    client.on_ready([&client](const dpp::ready_t &event) { onBotReady(client); });
    return 0;
}