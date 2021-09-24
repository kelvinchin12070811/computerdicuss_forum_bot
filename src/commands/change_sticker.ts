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

import Sticker from '../db/datatype/sticker';
import { registerCommand } from './CommandFactory';

/**
 * Allow an admin to update the sticker URI.
 * @param interaction Interaction that triggered this command.
 * @param client Current bot client where the command run.
 */
const changeSticer = async (interaction: CommandInteraction, client: Client) => {
    if (!(interaction.member?.permissions as Permissions).has(Permissions.FLAGS.ADMINISTRATOR)) {
        await interaction.reply('You do not have permission to access this command');
        return;
    }

    const stickerName = (interaction.options.get('sticker_name')?.value as string).toLowerCase();
    const stickerURI = interaction.options.get('uri')?.value as string;
    const sticker = await Sticker.findOne({ where: { keyword: stickerName } });

    if (sticker === null) {
        await interaction.reply(`Sticker "${stickerName}" does not exist`);
        return;
    }

    (sticker as any).uri = stickerURI;
    await sticker.save();
    await interaction.reply(`Sticker ${stickerName}'s uri has been updated`);
}

registerCommand('change_sticker', changeSticer, {
    description: 'Change the sticker assigned to the sticker name, only admin can access this command',
    options: [
        {
            name: 'sticker_name',
            description: 'Unique id or name used to define the sticker',
            type: 3,
            required: true,
        },
        {
            name: 'uri',
            description: 'URI to locate the new sticker',
            type: 3,
            required: true,
        },
    ],
});