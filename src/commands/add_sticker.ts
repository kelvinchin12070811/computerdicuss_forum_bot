import {
    CommandInteraction,
    Client,
    Permissions,
} from 'discord.js';

import Sticker from '../db/datatype/sticker';

export const addSticker = async (interaction: CommandInteraction, client: Client) => {
    if (!(interaction.member?.permissions as Permissions).has(Permissions.STAGE_MODERATOR))
        return;

    const stickerName = interaction.options.get('sticker_name')?.value as string;
    const stickerURI = interaction.options.get('uri')?.value as string;
    await Sticker.create({ keyword: stickerName, uri: stickerURI });
    await interaction.reply('Sticker added to library');
}