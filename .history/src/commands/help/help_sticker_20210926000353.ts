import {
    CommandInteraction,
    Client,
    MessageEmbed,
} from 'discord.js';

import { registerCommand } from '../CommandFactory';

const helpSticker = async (interaction: CommandInteraction, client: Client) => {
    // const embed = {
    //     "color": 65280,
    //     "footer": {
    //         "icon_url": "https://cdn.discordapp.com/avatars/890272720120586260/c325408b5c71f09ee12dd1606917abb5.png"
    //     },
    //     "thumbnail": {
    //         "url": "https://cdn.discordapp.com/avatars/890272720120586260/c325408b5c71f09ee12dd1606917abb5.png"
    //     },
    //     "author": {
    //         "name": "Sticker Command Help",
    //         "url": "https://discordapp.com",
    //         "icon_url": "https://cdn.discordapp.com/avatars/890272720120586260/c325408b5c71f09ee12dd1606917abb5.png"
    //     },
    //     "fields": [
    //         {
    //             "name": "/add_sticker",
    //             "value": "Add Sticker into Library"
    //         },
    //         {
    //             "name": "/change_sticker",
    //             "value": "Change Own Sticker to New URL"
    //         },
    //         {
    //             "name": "/list_sticker",
    //             "value": "List All Command in Library"
    //         },
    //         {
    //             "name": "/priview_sticker",
    //             "value": "Priview Sticker in 5sec"
    //         },
    //         {
    //             "name": "/remove_sticker",
    //             "value": "Remove Sticker from Library"
    //         },
    //         {
    //             "name": "/rename_sticker",
    //             "value": "Rename Sticker to OWN Sticker"
    //         },
    //         {
    //             "name": "/sticker",
    //             "value": "Use Sticker"
    //         }
    //     ]
    // };

    const embed = new MessageEmbed()
        .setColor('#00ff00')
        .setAuthor(
            'Sticker Command Help',
            'https://cdn.discordapp.com/avatars/890272720120586260/c325408b5c71f09ee12dd1606917abb5.png',
            'https://discordapp.com'
        )
        .setFooter("", )

    await interaction.reply({ embeds: [embed] });
};

registerCommand('helpSticker', helpSticker, {
    description: 'help sticker command',
})