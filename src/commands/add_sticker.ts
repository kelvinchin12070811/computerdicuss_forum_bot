import {
    CommandInteraction,
    Client,
    Permissions,
} from 'discord.js';

import Sticker from '../db/datatype/sticker';

export const addSticker = async (interaction: CommandInteraction, client: Client) => {
    if (!(interaction.member?.permissions as Permissions).has(Permissions.STAGE_MODERATOR)) {
        await interaction.reply('You do not have permission to access this command');
        return;
    }

    const stickerName = interaction.options.get('sticker_name')?.value as string;
    const stickerURI = interaction.options.get('uri')?.value as string;

    if ((await Sticker.findOne({ where: { keyword: stickerName } })) !== null) {
        await interaction.reply(`There's already has a sticker call ${stickerName}`);
        return;
    }

    await Sticker.create({ keyword: stickerName, uri: stickerURI });
    await interaction.reply(`Sticker ${stickerName} added to library`);
}