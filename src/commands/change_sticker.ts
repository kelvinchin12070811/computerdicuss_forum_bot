import {
    CommandInteraction,
    Client,
    Permissions,
} from 'discord.js';
import Sticker from '../db/datatype/sticker';

export const changeSticer = async (interaction: CommandInteraction, client: Client) => {
    if (!(interaction.member?.permissions as Permissions).has(Permissions.FLAGS.ADMINISTRATOR)) {
        await interaction.reply('You do not have permission to access this command');
        return;
    }

    const stickerName = interaction.options.get('sticker_name')?.value as string;
    const stickerURI = interaction.options.get('uri')?.value as string;
    const sticker = await Sticker.findOne({ where: { keyword: stickerName } });

    if (sticker === null) {
        await interaction.reply(`Sticker "${stickerName}" does not exist`);
        return;
    }

    (sticker as any).uri = stickerURI;
    await sticker.save();
    await interaction.reply(`Sticker ${stickerName}'s uri has been updated`);
}