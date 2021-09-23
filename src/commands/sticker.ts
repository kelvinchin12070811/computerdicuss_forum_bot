import {
    Client,
    CommandInteraction,
} from 'discord.js';

import Sticker from '../db/datatype/sticker';
import { registerCommand } from './CommandFactory';

/**
 * Command that reply the user with sticker with name that match the name supplied by a user.
 * @param interaction Command interaction triggered this command.
 * @param client Bot client of current bot instance.
 */
const returnSticker = async (interaction: CommandInteraction, client: Client) => {
    const stickerName = (interaction.options.get('sticker_name')?.value as string).toLowerCase();
    let message = '';

    const sticker = await Sticker.findOne({ where: { keyword: stickerName } });

    if (sticker == null) message = `Sticker "${stickerName}" does not exist.`;
    else message = (sticker as any).uri as string;

    await interaction.reply(message);
}

registerCommand('sticker', returnSticker, {
    description: 'Send a sticker to command invoked channel',
    options: [
        {
            name: 'sticker_name',
            description: 'Unique id or name of the sticker',
            type: 3,
            required: true,
        },
    ],
});