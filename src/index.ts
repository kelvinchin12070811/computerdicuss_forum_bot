import { REST } from '@discordjs/rest';
import { Routes } from 'discord-api-types/v9';
import { config } from 'dotenv';
import { Client, Intents, Permissions } from 'discord.js';

import { sequelize } from './db/database_provider';
import './commands';

import {
    getRegistredCommand,
    getCommandsRegristrationList,
    CommandInvoker,
} from './commands/CommandFactory';

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
            await rest.put(Routes.applicationGuildCommands(CLIENT_ID, GUILD_ID), { body: getRegistredCommand() });
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

client.on('interactionCreate', async interaction => {
    if (!interaction.isCommand()) return;

    (getCommandsRegristrationList()[interaction.commandName] as CommandInvoker)(interaction, client);
});

client.on('messageCreate', async message => {
    if (message.author.bot) return;

    if (message.content === '???server id???') {
        if (GUILD_ID) return;

        if (!message.member?.permissions.has(Permissions.FLAGS.ADMINISTRATOR)) {
            console.log(`${message.author.username}'s interaction has been rejected'`);
            return;
        }

        await message.author.send({ content: `your server id is ${message.guildId}` });
        await message.reply('Server id has been sent to your DM');
    }
});

console.log('\nApplication Command form sent to Discord:');
console.log(JSON.stringify(getRegistredCommand()));

console.log('\nRegistered command invokers:');
console.log(getCommandsRegristrationList());

client.login(TOKEN);