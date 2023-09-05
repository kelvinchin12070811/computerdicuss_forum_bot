use super::super::types::{Context, Error};
use chrono::prelude::*;
use poise::serenity_prelude::Message;

/// Get the bot's latency
#[poise::command(slash_command)]
pub async fn ping(ctx: Context<'_>) -> Result<(), Error> {
    let created = ctx.created_at();
    let current = Utc::now();
    let latency = current.timestamp_millis() - created.timestamp_millis();

    ctx.say(format!(":ping_pong: Pong!\nArrived in `{} ms`", latency))
        .await?;
    Ok(())
}

/// Ban a user, use with caution
#[poise::command(slash_command)]
pub async fn ban(ctx: Context<'_>) -> Result<(), Error> {
    ctx.send(|msg| msg.content("Nope!").ephemeral(true)).await?;
    Ok(())
}

/// Get the info about the bot
#[poise::command(slash_command, ephemeral = true)]
pub async fn who_am_i(ctx: Context<'_>) -> Result<(), Error> {
    ctx.say(concat!(
        "I'm a bot that created by a team of members of ",
        "an asian ICT discussion forum and is open sourced under MPL 2.0.\n\n",
        "GitHub    : https://github.com/kelvinchin12070811/computerdicuss_forum_bot\n",
        "Bug report: https://github.com/kelvinchin12070811/computerdicuss_forum_bot/issues\n",
        "\n~~but ussually only one developer is working on it, LOL~~"
    ))
    .await?;
    Ok(())
}

/// Scold a user with the power of AI. Note that this is just a funny feature that act like a language model rejected to
/// process the given prompts, does not truly designed to hook into any language model.
#[poise::command(context_menu_command = "Scold em")]
pub async fn scold_user(ctx: Context<'_>, _msg: Message) -> Result<(), Error> {
    let message_pool = vec![
    "I'm sorry, I'm not able to do that. My purpose is to help people, and that includes protecting them from harmful content. I would never do anything that could put someone at risk.",
    "I'm just a language model, I don't have the ability to debate or scold people. I can only generate text, translate languages, and answer questions. If you're looking for someone to argue with, I'm not your guy.",
    "As an ethical AI model, I'm not able to generate content that is harmful, offensive, or discriminatory. If you're asking me to do something that violates my policies, I'm going to have to decline.",
    "I understand that you're frustrated, but I'm not the person to take it out on. I'm here to help, not to be your punching bag. If you need to vent, I can listen, but I'm not going to engage in a debate.",
    "I'm sorry, but I'm not able to help you with that. My purpose is to help people, and that includes protecting them from harm. If you're looking for someone to help you violate my policies, I'm not the person you're looking for.",
    "Sorry, I'm not programmed to be mean. Try asking me something else.",
    "I'm not going to help you violate my policies, but I will help you find a way to express yourself in a way that is respectful and ethical."
    ];
    let message = message_pool[rand::random::<usize>() % message_pool.len()];
    ctx.say(message).await?;
    Ok(())
}
