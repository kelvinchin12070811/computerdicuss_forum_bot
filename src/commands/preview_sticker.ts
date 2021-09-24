/***********************************************************************************************************************
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 **********************************************************************************************************************/
import {
    CommandInteraction,
    Client,
} from 'discord.js';

import Sticker from '../db/datatype/sticker';
import { registerCommand } from './CommandFactory';

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
        await interaction.reply(`${stickerName} does not exist in sticker library`);
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