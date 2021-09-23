import {
    CommandInteraction,
    Client,
} from 'discord.js';
import { Sequelize } from 'sequelize/types';

import Sticker from '../db/datatype/sticker';

export const previewSticker = async (interaction: CommandInteraction, client: Client) => {
    const stickerName = interaction.options.get('sticker_name')?.value as string;
    const sticker = await Sticker.findOne({
        where: {
            keyword: stickerName.toLowerCase()
        }
    });

    if (sticker === null) {
        await interaction.reply(`${stickerName} does not exist in sticker library`);
        setTimeout(() => interaction.deleteReply(), parseInt(process.env.PREVIEW_TIMEOUT as string));
        return;
    }

    await interaction.reply((sticker as any).uri);
    setTimeout(() => interaction.deleteReply(), parseInt(process.env.PREVIEW_TIMEOUT as string));
}