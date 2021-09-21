import { REST } from '@discordjs/rest';
import { Routes } from 'discord-api-types/v9';
import { config } from 'dotenv';
import { Client, Intents } from 'discord.js';

import { returnSticker } from './sticker_handler';
import { sequelize } from './db/database_provider';
import Sticker from './db/datatype/sticker';

config();

const TOKEN = process.env.TOKEN as string;
const CLIENT_ID = process.env.CLIENT_ID as string;
const GUILD_ID = process.env.GUILD_ID as string;
//const GUILD_ID = null;

const commands = [
    {
        name: 'ping',
        description: 'reply with pong',
    },
    {
        name: 'sticker',
        description: 'Send a sticker to command invoked channel',
        options: [
            {
                'name': 'sticker_name',
                'description': 'Unique id or name of the sticker',
                'type': 3,
                'required': true,
            },
        ]
    },
];

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
        await Sticker.create({ 'keyword': 'tst', 'uri': 'test.com' });
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

    if (interation.commandName === 'ping') {
        await interation.reply('Pong!');
        return;
    }

    if (interation.commandName === 'sticker') {
        const stickerName = interation.options.get('sticker_name');
        await interation.reply(returnSticker(stickerName?.value as string));
        return;
    }

});

client.on('messageCreate', async message => {
    if (message.author.bot) return;

    if (message.content === '???server id???') {
        await message.reply({ content: message.guildId });
        return;
    }
});

client.login(TOKEN);