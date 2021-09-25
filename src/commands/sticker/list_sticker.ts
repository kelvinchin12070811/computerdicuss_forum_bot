/***********************************************************************************************************************
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 **********************************************************************************************************************/
import {
    CommandInteraction,
    Client,
} from 'discord.js';
import { Op } from 'sequelize';

import Sticker from '../../db/datatype/sticker';
import { registerCommand } from '../CommandFactory';

/**
 * List all sticker stored in the library.
 * @param interaction Interaction that invoked this command.
 * @param client Current bot client that handling this command.
 */
const listSticker = async (interaction: CommandInteraction, client: Client) => {
    const stickers = (await Sticker.findAll({
        where: {
            keyword: {
                [Op.like]: '%',
            },
        },
    })).map((sticker: Sticker) => (sticker as any).keyword);

    const stickerNames = stickers.join(', ');
    await interaction.reply(`Here are all available stickers:\n${stickerNames}`);
}

registerCommand('list_sticker', listSticker, {
    description: 'Get all stickers\' name stored in the library',
});