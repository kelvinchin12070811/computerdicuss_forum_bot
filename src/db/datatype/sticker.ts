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
}, { sequelize, modelName: 'sticker' });

export default Sticker;