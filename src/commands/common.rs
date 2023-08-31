use super::super::types::{Context, Error};
use chrono::prelude::*;

#[poise::command(slash_command)]
pub async fn ping(ctx: Context<'_>) -> Result<(), Error> {
    let created = ctx.created_at();
    let current = Utc::now();
    let latency = current.timestamp_millis() - created.timestamp_millis();

    ctx.say(format!(":ping_pong: Pong!\nArrived in `{} ms`", latency))
        .await?;
    Ok(())
}

#[poise::command(slash_command)]
pub async fn ban(ctx: Context<'_>) -> Result<(), Error> {
    ctx.send(|msg| msg.content("Nope!").ephemeral(true)).await?;
    Ok(())
}
