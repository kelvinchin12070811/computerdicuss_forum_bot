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
    await interaction.reply('Pong!');
};

registerCommand('ping', ping, {
    description: 'reply with pong',
})