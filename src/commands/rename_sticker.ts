import {
    CommandInteraction,
    Client,
    Permissions,
} from 'discord.js';
import Sticker from '../db/datatype/sticker';

export const renameSticker = async (interaction: CommandInteraction, client: Client) => {
    if (!(interaction.member?.permissions as Permissions).has(Permissions.STAGE_MODERATOR)) {
        await interaction.reply('You don\'t have permission to access this command');
        return;
    }

    const stickerName = interaction.options.get('sticker_name')?.value as string;
    const newStickerName = interaction.options.get('new_sticker_name')?.value as string;
    const targetSticker = await Sticker.findOne({ where: { keyword: stickerName } });

    if (targetSticker === null) {
        await interaction.reply(`Sticker "${stickerName}" not found`);
        return;
    }

    (targetSticker as any).keyword = newStickerName;
    targetSticker.save();
    await interaction.reply(`Sticker ${stickerName}'s name changed to ${newStickerName}`);
}