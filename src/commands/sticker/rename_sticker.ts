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

const { MessageEmbed } = require('discord.js');


/**
 * Allow an admin to rename the sticker. As add_sticker it must be unique.
 * @param interaction Interaction that invoke this command.
 * @param client Client that called this command..
 */
export const renameSticker = async (interaction: CommandInteraction, client: Client) => {
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
        return;
    }

    const stickerName = (interaction.options.get('sticker_name')?.value as string).toLowerCase();
    const newStickerName = (interaction.options.get('new_sticker_name')?.value as string).toLowerCase();
    const targetSticker = await Sticker.findOne({ where: { keyword: stickerName } });

    if (targetSticker === null) {
        await interaction.reply(`Sticker "${stickerName}" not found`);
        return;
    }

    if ((await Sticker.findOne({ where: { keyword: newStickerName } })) != null) {
        const embed = new MessageEmbed()
            .setColor('#ffa500')
            .setTitle('Command Factory')
            .setThumbnail('https://cdn.discordapp.com/avatars/890272720120586260/c325408b5c71f09ee12dd1606917abb5.png?')
            .addFields(
                { name: 'There s already has a sticker call', value: `${newStickerName}` },
            )
            .setTimestamp()
            .setFooter(
                client.user?.username as string,
                "https://cdn.discordapp.com/avatars/890272720120586260/c325408b5c71f09ee12dd1606917abb5.png"
            );
        await interaction.reply({ embeds: [embed] });
        return;
    }

    (targetSticker as any).keyword = newStickerName;
    targetSticker.save();
    const embed = new MessageEmbed()
        .setColor('#00ff00')
        .setTitle('Command Factory')
        .setThumbnail('https://cdn.discordapp.com/avatars/890272720120586260/c325408b5c71f09ee12dd1606917abb5.png?')
        .addFields(
            { name: 'Complete', value: `Sticker ${stickerName}'s name changed to ${newStickerName}` },
        )
        .setTimestamp()
        .setFooter(
            client.user?.username as string,
            "https://cdn.discordapp.com/avatars/890272720120586260/c325408b5c71f09ee12dd1606917abb5.png"
        );

    await interaction.reply({ embeds: [embed] });
}

registerCommand('rename_sticker', renameSticker, {
    description: 'Rename the name of the sticker, only admin can access this command',
    options: [
        {
            name: 'sticker_name',
            description: 'Unique id or name used to define the sticker',
            type: 3,
            required: true,
        },
        {
            name: 'new_sticker_name',
            description: 'New unique id to define the sticker',
            type: 3,
            required: true,
        },
    ],
});