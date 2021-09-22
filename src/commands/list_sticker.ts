import {
    CommandInteraction,
    Client,
} from 'discord.js';
import { Op } from 'sequelize';

import Sticker from '../db/datatype/sticker';

export const listSticker = async (interaction: CommandInteraction, client: Client) => {
    const stickers = (await Sticker.findAll({
        where: {
            keyword: {
                [Op.like]: '%',
            },
        },
    })).map((sticker: Sticker) => (sticker as any).keyword);

    const stickerNames = stickers.join(', ');
    await interaction.reply(`Here are all available stickers:\n${stickerNames}`);
}