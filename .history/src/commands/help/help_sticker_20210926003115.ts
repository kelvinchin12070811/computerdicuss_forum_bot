import {
    CommandInteraction,
    Client,
    MessageEmbed,
} from 'discord.js';

import { registerCommand } from '../CommandFactory';

const helpSticker = async (interaction: CommandInteraction, client: Client) => {
    const embed = new MessageEmbed()
        .setColor('#00ff00')
        .setTitle('Help Command')
        .setURL('https://discord.js.org/')
        .setAuthor('Some name', 'https://i.imgur.com/AfFp7pu.png', 'https://discord.js.org')
        .setDescription('Some description here')
        .setThumbnail('https://cdn.discordapp.com/avatars/890272720120586260/c325408b5c71f09ee12dd1606917abb5.png?size=4096')
        .addFields(
            { name: 'Regular field title', value: 'Some value here' },
            { name: '\u200B', value: '\u200B' },
            { name: 'Inline field title', value: 'Some value here', inline: true },
            { name: 'Inline field title', value: 'Some value here', inline: true },
        )
        .addField('Inline field title', 'Some value here', true)
        .setImage('https://i.imgur.com/AfFp7pu.png')
        .setTimestamp()
        .setFooter('Some footer text here', 'https://i.imgur.com/AfFp7pu.png');

    await interaction.reply({ embeds: [embed] });
};

registerCommand('help_sticker', helpSticker, {
    description: 'help sticker command',
})