import {
    CommandInteraction,
    Client,
} from 'discord.js';

import { registerCommand } from '../CommandFactory';

const { MessageEmbed } = require('discord.js');

const help = async (interaction: CommandInteraction, client: Client) => {
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
            "url": "https://computerdiscuss.github.io/",
            "icon_url": "https://cdn.discordapp.com/avatars/890272720120586260/c325408b5c71f09ee12dd1606917abb5.png"
        },
        "fields": [
            {
                "name": "/helpSticker",
                "value": "Help command for Sricker"
            }
        ]
    };

    await interaction.reply({ embeds: [embed] });
};

registerCommand('help', help, {
    description: 'Check help command',
})