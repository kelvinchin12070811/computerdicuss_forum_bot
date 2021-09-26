/***********************************************************************************************************************
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 **********************************************************************************************************************/
import {
    CommandInteraction,
    Client,
    Permissions,
} from 'discord.js';

import Sticker from '../../db/datatype/sticker';
import { registerCommand } from '../CommandFactory';

/**
 * Allow admin to remove a sticker from the library.
 * @param interaction Interaction that triggered this command.
 * @param client Client that called this command.
 */

const { MessageEmbed } = require('discord.js');

const removeSticker = async (interaction: CommandInteraction, client: Client) => {
    if (!(interaction.member?.permissions as Permissions).has(Permissions.FLAGS.ADMINISTRATOR)) {
        const embed = new MessageEmbed()
            .setColor('#ff0000')
            .setTitle('Command Factory')
            .setThumbnail('https://cdn.discordapp.com/avatars/890272720120586260/c325408b5c71f09ee12dd1606917abb5.png?')
            .addFields(
                { name: 'Error', value: 'You do not have permission to access this command' },
            )
            .setTimestamp()
            .setFooter(
                client.user?.username as string,
                "https://cdn.discordapp.com/avatars/890272720120586260/c325408b5c71f09ee12dd1606917abb5.png"
            );
        await interaction.reply({ embeds: [embed] });
    }

    const stickerName = (interaction.options.get('sticker_name')?.value as string).toLowerCase();
    const sticker = await Sticker.findOne({ where: { keyword: stickerName } });

    if (sticker === null) {
        const embed = new MessageEmbed()
            .setColor('#ff0000')
            .setTitle('Command Factory')
            .setThumbnail('https://cdn.discordapp.com/avatars/890272720120586260/c325408b5c71f09ee12dd1606917abb5.png?')
            .addFields(
                { name: 'Error', value: `${stickerName} does not exist` },
            )
            .setTimestamp()
            .setFooter(
                client.user?.username as string,
                "https://cdn.discordapp.com/avatars/890272720120586260/c325408b5c71f09ee12dd1606917abb5.png"
            );
        await interaction.reply({ embeds: [embed] });
        return;
    }

    await sticker.destroy();
    const embed = new MessageEmbed()
        .setColor('#00ff00')
        .setTitle('Command Factory')
        .setThumbnail('https://cdn.discordapp.com/avatars/890272720120586260/c325408b5c71f09ee12dd1606917abb5.png?')
        .addFields(
            { name: 'Complete', value: `Sticker ${stickerName} has been removed` },
        )
        .setTimestamp()
        .setFooter(
            client.user?.username as string,
            "https://cdn.discordapp.com/avatars/890272720120586260/c325408b5c71f09ee12dd1606917abb5.png"
        );

    await interaction.reply({ embeds: [embed] });
}

registerCommand('remove_sticker', removeSticker, {
    description: 'Remove a sticker from library, only admin can access this command',
    options: [
        {
            name: 'sticker_name',
            description: 'Unique id or name used to define the sticker',
            type: 3,
            required: true,
        }
    ],
});