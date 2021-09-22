import { REST } from '@discordjs/rest';
import { Routes } from 'discord-api-types/v9';
import { config } from 'dotenv';
import { Client, Intents, Permissions } from 'discord.js';

import { returnSticker } from './commands/sticker';
import { sequelize } from './db/database_provider';
import { addSticker } from './commands/add_sticker';
import { commands } from './commands';
import { renameSticker } from './commands/rename_sticker';
import { removeSticker } from './commands/remove_sticker';

config();

const TOKEN = process.env.TOKEN as string;
const CLIENT_ID = process.env.CLIENT_ID as string;
const GUILD_ID = process.env.GUILD_ID as string;
//const GUILD_ID = null;

const rest = new REST({ version: '9' }).setToken(TOKEN);

(async () => {
    try {
        console.log('Started refreshing application (/) commands.');
        if (!!GUILD_ID) {
            await rest.put(Routes.applicationGuildCommands(CLIENT_ID, GUILD_ID), { body: commands });
            console.log('Successfully reloaded application (/) commands.');
        }
        else {
            console.log('GUILD_ID not set, bot still work but no application (/) commands.');
        }

        console.log('Initializing database');
        await sequelize.sync();
    }
    catch (error) {
        console.log({ error });
        process.abort();
    }
})();

const client = new Client({
    intents: [
        Intents.FLAGS.GUILDS,
        Intents.FLAGS.DIRECT_MESSAGES,
        Intents.FLAGS.GUILD_MESSAGES,
        Intents.FLAGS.GUILD_INTEGRATIONS,
    ]
});

client.on('ready', () => {
    console.log(`Logged in as ${client.user?.tag}`);
});

client.on('interactionCreate', async interation => {
    if (!interation.isCommand()) return;

    switch (interation.commandName) {
        case 'ping':
            await interation.reply('Pong!');
            break;

        case 'sticker':
            const stickerName = interation.options.get('sticker_name');
            const stickerURI = await returnSticker(stickerName?.value as string);
            await interation.reply(stickerURI);
            break;

        case 'add_sticker':
            addSticker(interation, client);
            break;

        case 'rename_sticker':
            renameSticker(interation, client);
            break;

        case 'remove_sticker':
            removeSticker(interation, client);
            break;
    }
});

client.on('messageCreate', async message => {
    if (message.author.bot) return;

    if (message.content === '???server id???') {
        if (!message.member?.permissions.has(Permissions.STAGE_MODERATOR))
            return;

        if (GUILD_ID) return;
        await message.author.send({ content: `your server id is ${message.guildId}` });
    }
});

client.login(TOKEN);