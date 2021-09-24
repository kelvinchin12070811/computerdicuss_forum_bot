/***********************************************************************************************************************
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 **********************************************************************************************************************/
import { Model, DataTypes } from 'sequelize';
import { sequelize } from '../database_provider';

class Sticker extends Model { }

Sticker.init({
    keyword: {
        type: DataTypes.STRING,
    },
    uri: {
        type: DataTypes.STRING,
    },
}, {
    sequelize,
    modelName: 'sticker',
    timestamps: false,
});

export default Sticker;