import {
    CommandInteraction,
    Client,
    MessageEmbed,
} from 'discord.js';

import { registerCommand } from '../CommandFactory';

const helpSticker = async (interaction: CommandInteraction, client: Client) => {
    const embed = new MessageEmbed()
        .setColor('#00ff00')
        .setTitle('Help Sticker Command')
        .setURL('https://computerdiscuss.github.io/')
        .setThumbnail('https://cdn.discordapp.com/avatars/890272720120586260/c325408b5c71f09ee12dd1606917abb5.png?')
        .addFields(
            { name: '/sticker', value: 'Use Sticker' },
            { name: '/list_sticker', value: 'List Sticker from Libary' },
            { name: '/add_sticker', value: 'Add Sticker to Libary(Admin-Only)' },
            { name: '/change_sticker', value: 'Change OWN Sticker to Libary(Admin-Only)' },
            { name: '/rename_sticker', value: 'Rename Sticker to Libary(Admin-Only)' },
            { name: '/remove_sticker', value: 'Remove Sticker from Libary(Admin-Only)' },
        )
        .setTimestamp()
        .setFooter(
            client.user?.username as string,
            "https://cdn.discordapp.com/avatars/890272720120586260/c325408b5c71f09ee12dd1606917abb5.png"
        );

    await interaction.reply({ embeds: [embed] });
};

registerCommand('help_sticker', helpSticker, {
    description: 'help sticker command',
})