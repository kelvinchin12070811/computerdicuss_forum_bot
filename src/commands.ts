// Force registration in the commands to run.
import './commands/add_sticker';
import './commands/sticker';
import './commands/change_sticker';
import './commands/list_sticker';
import './commands/preview_sticker';
import './commands/remove_sticker';

export const commands = [
    {
        name: 'ping',
        description: 'reply with pong',
    },
];