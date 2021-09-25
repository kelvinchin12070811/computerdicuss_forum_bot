import {
    CommandInteraction,
    Client,
} from 'discord.js';

import { registerCommand } from '../CommandFactory';

const { MessageEmbed } = require('discord.js');

const help = async (interaction: CommandInteraction, client: Client) => {
    const embed = new MessageEmbed()
        .setColor('#00ff00')
        .setTitle('Help Command')
        .setURL('https://computerdiscuss.github.io/')
        .setThumbnail('https://cdn.discordapp.com/avatars/890272720120586260/c325408b5c71f09ee12dd1606917abb5.png?')
        .addFields(
            { name: '/help_sticker', value: 'Help Sticker Command' },
        )
        .setTimestamp()
        .setFooter(
            client.user?.username as string,
            "https://cdn.discordapp.com/avatars/890272720120586260/c325408b5c71f09ee12dd1606917abb5.png"
        );

    await interaction.reply({ embeds: [embed] });
};

registerCommand('help', help, {
    description: 'Check help command',
})