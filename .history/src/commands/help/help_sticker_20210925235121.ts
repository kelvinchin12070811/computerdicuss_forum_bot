import {
    CommandInteraction,
    Client,
} from 'discord.js';

import { registerCommand } from '../CommandFactory';

const { MessageEmbed } = require('discord.js');

const help = async (interaction: CommandInteraction, client: Client) => {
    await interaction.reply('help_sticker commands');
    const embed = {
        "color": 65280,
        "footer": {
            "icon_url": "https://cdn.discordapp.com/avatars/890272720120586260/c325408b5c71f09ee12dd1606917abb5.png"
        },
        "thumbnail": {
            "url": "https://cdn.discordapp.com/avatars/890272720120586260/c325408b5c71f09ee12dd1606917abb5.png"
        },
        "author": {
            "name": "Sticker Command Help",
            "url": "https://discordapp.com",
            "icon_url": "https://cdn.discordapp.com/avatars/890272720120586260/c325408b5c71f09ee12dd1606917abb5.png"
        },
        "fields": [
            {
                "name": "/add_sticker",
                "value": "Add Sticker into Library"
            },
            {
                "name": "/change_sticker",
                "value": "Change Own Sticker to New URL"
            },
            {
                "name": "/list_sticker",
                "value": "List All Command in Library"
            },
            {
                "name": "/priview_sticker",
                "value": "Priview Sticker in 5sec"
            },
            {
                "name": "/remove_sticker",
                "value": "Remove Sticker from Library"
            },
            {
                "name": "/rename_sticker",
                "value": "Rename Sticker to OWN Sticker"
            },
            {
                "name": "/sticker",
                "value": "Use Sticker"
            }
        ]
    };

    await interaction.reply({ embeds: [embed] });
};

registerCommand('help_sticker', help, {
    description: 'help sticker command',
})