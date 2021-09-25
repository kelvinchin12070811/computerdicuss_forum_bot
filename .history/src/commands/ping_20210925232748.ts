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
    message.channel.send("Pinging...").then(m => {
        // The math thingy to calculate the user's ping
        var ping = m.createdTimestamp - message.createdTimestamp;

        // Basic embed
        var embed = new Discord.MessageEmbed()
            .setAuthor(`Your ping is ${ping}`)
            .setColor("Your Color")
}

registerCommand('ping', ping, {
    description: 'reply with pong',
})