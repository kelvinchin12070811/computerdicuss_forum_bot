import {
    CommandInteraction,
    Client,
    Permissions,
} from 'discord.js';

export const addSticker = async (interaction: CommandInteraction, client: Client) => {
    const author = interaction.user;
    const server = interaction.guild;

    if (!(interaction.member?.permissions as Permissions).has(Permissions.STAGE_MODERATOR))
        return;

    interaction.reply('hi admin!');
}