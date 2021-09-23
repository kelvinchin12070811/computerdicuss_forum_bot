// Force registration in the commands to run.
import './commands/add_sticker';
import './commands/sticker';

export const commands = [
    {
        name: 'ping',
        description: 'reply with pong',
    },
    {
        name: 'list_sticker',
        description: 'Get all stickers\' name stored in the library',
    },
    {
        name: 'preview_sticker',
        description: 'Preview a sticker, message will auto delete after certain amount of time',
        options: [
            {
                name: 'sticker_name',
                description: 'Unique id or name of the sticker',
                type: 3,
                required: true,
            },
        ],
    },
    {
        name: 'rename_sticker',
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
    },
    {
        name: 'remove_sticker',
        description: 'Remove a sticker from library, only admin can access this command',
        options: [
            {
                name: 'sticker_name',
                description: 'Unique id or name used to define the sticker',
                type: 3,
                required: true,
            }
        ],
    },
    {
        name: 'change_sticker',
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
    },
];