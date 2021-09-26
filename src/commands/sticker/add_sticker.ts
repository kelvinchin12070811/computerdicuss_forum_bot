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
 * Allow admin to add a new sticker into the sticker library. The sticker is identified with a unique name and will be
 * stored in all lowercase.
 * @param interaction Command interaction triggered this command.
 * @param client Bot client instance.
 */
const addSticker = async (interaction: CommandInteraction, client: Client) => {
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
    const stickerURI = interaction.options.get('uri')?.value as string;

    if ((await Sticker.findOne({ where: { keyword: stickerName } })) !== null) {
        const embed = new MessageEmbed()
            .setColor('#ffa500')
            .setTitle('Command Factory')
            .setThumbnail('https://cdn.discordapp.com/avatars/890272720120586260/c325408b5c71f09ee12dd1606917abb5.png?')
            .addFields(
                { name: 'There s already has a sticker call', value: `${stickerName}` },
            )
            .setTimestamp()
            .setFooter(
                client.user?.username as string,
                "https://cdn.discordapp.com/avatars/890272720120586260/c325408b5c71f09ee12dd1606917abb5.png"
            );
        await interaction.reply({ embeds: [embed] });
        return;
    }

    await Sticker.create({ keyword: stickerName, uri: stickerURI });
    const embed = new MessageEmbed()
        .setColor('#00ff00')
        .setTitle('Command Factory')
        .setThumbnail('https://cdn.discordapp.com/avatars/890272720120586260/c325408b5c71f09ee12dd1606917abb5.png?')
        .addFields(
            { name: 'Add Complete', value: `Sticker ${stickerName} added  to library` },
        )
        .setTimestamp()
        .setFooter(
            client.user?.username as string,
            "https://cdn.discordapp.com/avatars/890272720120586260/c325408b5c71f09ee12dd1606917abb5.png"
        );

    await interaction.reply({ embeds: [embed] });
}

registerCommand('add_sticker', addSticker, {
    description: 'Add sticker to the library, only admin can access this command',
    options: [
        {
            name: 'sticker_name',
            description: 'Unique id or name used to define the sticker, must not have space in between',
            type: 3,
            required: true,
        },
        {
            name: 'uri',
            description: 'URI to locate the sticker, it must be available to the public.',
            type: 3,
            required: true,
        }
    ],
});