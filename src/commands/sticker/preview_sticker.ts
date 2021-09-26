/***********************************************************************************************************************
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 **********************************************************************************************************************/
import {
    CommandInteraction,
    Client,
} from 'discord.js';

import Sticker from '../../db/datatype/sticker';
import { registerCommand } from '../CommandFactory';
const { MessageEmbed } = require('discord.js');


/**
 * Allow user to preview the sticker before sending it out, the reply will be deleted after a predefined amount of time.
 * @param interaction Interaction that triggered this command.
 * @param client Current client that this command executed in.
 */
const previewSticker = async (interaction: CommandInteraction, client: Client) => {
    const stickerName = interaction.options.get('sticker_name')?.value as string;
    const sticker = await Sticker.findOne({
        where: {
            keyword: stickerName.toLowerCase()
        }
    });

    if (sticker === null) {
        const embed = new MessageEmbed()
            .setColor('#ff0000')
            .setTitle('Command Factory')
            .setThumbnail('https://cdn.discordapp.com/avatars/890272720120586260/c325408b5c71f09ee12dd1606917abb5.png?')
            .addFields(
                { name: 'Error', value: ` ${stickerName}  does not exist in sticker library` },
            )
            .setTimestamp()
            .setFooter(
                client.user?.username as string,
                "https://cdn.discordapp.com/avatars/890272720120586260/c325408b5c71f09ee12dd1606917abb5.png"
            );
        await interaction.reply({ embeds: [embed] });
        setTimeout(() => interaction.deleteReply(), parseInt(process.env.PREVIEW_TIMEOUT as string));
        return;
    }

    await interaction.reply((sticker as any).uri);
    setTimeout(() => interaction.deleteReply(), parseInt(process.env.PREVIEW_TIMEOUT as string));
}

registerCommand('preview_sticker', previewSticker, {
    description: 'Preview a sticker, message will auto delete after certain amount of time',
    options: [
        {
            name: 'sticker_name',
            description: 'Unique id or name of the sticker',
            type: 3,
            required: true,
        },
    ],
});