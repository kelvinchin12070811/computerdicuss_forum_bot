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
 * Allow admin to add a new sticker into the sticker library. The sticker is identified with a unique name and will be
 * stored in all lowercase.
 * @param interaction Command interaction triggered this command.
 * @param client Bot client instance.
 */
const addSticker = async (interaction: CommandInteraction, client: Client) => {
    if (!(interaction.member?.permissions as Permissions).has(Permissions.FLAGS.ADMINISTRATOR)) {
        await interaction.reply('You do not have permission to access this command');
        return;
    }

    const stickerName = (interaction.options.get('sticker_name')?.value as string).toLowerCase();
    const stickerURI = interaction.options.get('uri')?.value as string;

    if ((await Sticker.findOne({ where: { keyword: stickerName } })) !== null) {
        await interaction.reply(`There's already has a sticker call ${stickerName}`);
        return;
    }

    await Sticker.create({ keyword: stickerName, uri: stickerURI });
    await interaction.reply(`Sticker ${stickerName} added to library`);
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