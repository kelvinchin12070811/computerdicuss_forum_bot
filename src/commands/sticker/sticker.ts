/***********************************************************************************************************************
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 **********************************************************************************************************************/
import {
    Client,
    CommandInteraction,
} from 'discord.js';

import Sticker from '../../db/datatype/sticker';
import { registerCommand } from '../CommandFactory';

const { MessageEmbed } = require('discord.js');

/**
 * Command that reply the user with sticker with name that match the name supplied by a user.
 * @param interaction Command interaction triggered this command.
 * @param client Bot client of current bot instance.
 */
const returnSticker = async (interaction: CommandInteraction, client: Client) => {
    const stickerName = (interaction.options.get('sticker_name')?.value as string).toLowerCase();
    let message = '';

    const sticker = await Sticker.findOne({ where: { keyword: stickerName } });


    if (sticker == null) {
        const embed = new MessageEmbed()
            .setColor('#ff0000')
            .setTitle('Command Factory')
            .setThumbnail('https://cdn.discordapp.com/avatars/890272720120586260/c325408b5c71f09ee12dd1606917abb5.png?')
            .addFields(
                { name: 'Error', value: `Sticker "${stickerName}" does not exist` },
            )
            .setTimestamp()
            .setFooter(
                client.user?.username as string,
                "https://cdn.discordapp.com/avatars/890272720120586260/c325408b5c71f09ee12dd1606917abb5.png"
            );
        await interaction.reply({ embeds: [embed] });
        return;
    }
        
    else message = (sticker as any).uri as string;

    await interaction.reply(message);
}

registerCommand('sticker', returnSticker, {
    description: 'Send a sticker to command invoked channel',
    options: [
        {
            name: 'sticker_name',
            description: 'Unique id or name of the sticker',
            type: 3,
            required: true,
        },
    ],
});