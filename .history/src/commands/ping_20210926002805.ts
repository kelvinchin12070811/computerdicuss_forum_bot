/***********************************************************************************************************************
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 **********************************************************************************************************************/
import {
    CommandInteraction,
    Client,
    
} from 'discord.js';

import { registerCommand } from './CommandFactory';

/**
 * A command that simply reply pong,
 * @param interaction Interaction that triggered this command.
 * @param client Client that execute this command.
 */
const ping = async (interaction: CommandInteraction, client: Client) => {
    const latency = Date.now() - interaction.createdTimestamp;
    const embed = new MessageEmbed()
        .setColor('#00ff00')
        .setTitle('Ping')
        .addFields(
            { name: 'Bot Latency', value: `${latency}ms` },
            { name: 'API Latency', value: `${client.ws.ping}ms`, inline: true },
        )
    
    await interaction.reply({ embeds: [embed] });
};

registerCommand('ping', ping, {
    description: 'reply with pong',
})