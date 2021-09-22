export const commands = [
    {
        name: 'ping',
        description: 'reply with pong',
    },
    {
        name: 'sticker',
        description: 'Send a sticker to command invoked channel',
        options: [
            {
                name: 'sticker_name',
                description: 'Unique id or name of the sticker',
                type: 3,
                required: true,
            },
        ]
    },
    {
        name: 'add_sticker',
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
];