import {
    Client,
    CommandInteraction,
} from 'discord.js';

import Sticker from '../db/datatype/sticker';

export const returnSticker = async (interaction: CommandInteraction, client: Client) => {
    const stickerName = (interaction.options.get('sticker_name')?.value as string).toLowerCase();
    let message = '';

    const sticker = await Sticker.findOne({ where: { keyword: stickerName } });

    if (sticker == null) message = `Sticker "${stickerName}" does not exist.`;
    else message = (sticker as any).uri as string;

    await interaction.reply(message);
}