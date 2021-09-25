/***********************************************************************************************************************
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 **********************************************************************************************************************/
import {
    Client,
    CommandInteraction,
} from 'discord.js';

let commandRegistry: any = {};
let registeredCommand: Array<any> = [];

/**
 * CommandInvoker function signature, all command function must match this syntax to register.
 */
export type CommandInvoker = {
    (interaction: CommandInteraction, client: Client): void;
};

/**
 * Register a discord command.
 * @param commandName Name of the command, in string and must not be null.
 * @param invoker Command invoker function, must not be null.
 * @param botInfo Additional information required by discord to register application command refer official discord
 * documentation (https://discord.com/developers/docs/interactions/application-commands) for more info. Note, name field
 * is not required as commandName will be used as name field.
 */
export const registerCommand = (commandName: string, invoker: CommandInvoker, botInfo: any) => {
    if (commandName == null) throw 'registerCommand: Error on register command, commandName is null';
    if (invoker == null) throw 'registerCommand: Error on register command, invoker is null';

    consol

    commandRegistry[commandName] = invoker;
    registeredCommand.push({
        name: commandName,
        ...botInfo,
    });
}

/**
 * Get a list of registered commands in the registry.
 * @returns All registered commands in the bot. Use to send to Discord to register application command.
 */
export const getRegistredCommand = () => registeredCommand;

/**
 * Get a list of registered commands as in the registry, this contains the invoker functions to execute the selected
 * commands.
 * @returns List of registered commands' invoker function, stored in hash map.
 */
export const getCommandsRegristrationList = () => commandRegistry;