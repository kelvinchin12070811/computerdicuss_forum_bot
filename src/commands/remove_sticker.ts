import {
    CommandInteraction,
    Client,
    Permissions,
} from 'discord.js';

import Sticker from '../db/datatype/sticker';
import { registerCommand } from './CommandFactory';

/**
 * Allow admin to remove a sticker from the library.
 * @param interaction Interaction that triggered this command.
 * @param client Client that called this command.
 */
const removeSticker = async (interaction: CommandInteraction, client: Client) => {
    if (!(interaction.member?.permissions as Permissions).has(Permissions.FLAGS.ADMINISTRATOR)) {
        await interaction.reply('You do not have permission to access this command');
        return;
    }

    const stickerName = (interaction.options.get('sticker_name')?.value as string).toLowerCase();
    const sticker = await Sticker.findOne({ where: { keyword: stickerName } });

    if (sticker === null) {
        await interaction.reply(`Sticker "${stickerName}" does not exist`);
        return;
    }

    await sticker.destroy();
    await interaction.reply(`Sticker ${stickerName} has been removed`);
}

registerCommand('remove_sticker', removeSticker, {
    description: 'Remove a sticker from library, only admin can access this command',
    options: [
        {
            name: 'sticker_name',
            description: 'Unique id or name used to define the sticker',
            type: 3,
            required: true,
        }
    ],
});