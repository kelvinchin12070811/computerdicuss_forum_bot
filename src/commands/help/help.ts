import {
    CommandInteraction,
    Client,
} from 'discord.js';

import { registerCommand } from './CommandFactory';

const { MessageEmbed } = require('discord.js');

const help = async (interaction: CommandInteraction, client: Client) => {
    await interaction.reply('help commands');
    const embed = {
        "color": 65280,
        "footer": {
            "icon_url": "https://cdn.discordapp.com/avatars/890272720120586260/c325408b5c71f09ee12dd1606917abb5.png"
        },
        "thumbnail": {
            "url": "https://cdn.discordapp.com/avatars/890272720120586260/c325408b5c71f09ee12dd1606917abb5.png"
        },
        "author": {
            "name": "Help Command",
            "url": "https://discordapp.com",
            "icon_url": "https://cdn.discordapp.com/avatars/890272720120586260/c325408b5c71f09ee12dd1606917abb5.png"
        },
        "fields": [
            {
                "name": "/help_sticker",
                "value": "help command for sricker"
            }
        ]
    };

    await interaction.reply({ embeds: [embed] });
};

registerCommand('help', help, {
    description: 'Check help command',
})