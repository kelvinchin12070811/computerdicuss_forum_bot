use super::super::types::{Context, Error};

#[poise::command(slash_command)]
pub async fn ping(ctx: Context<'_>) -> Result<(), Error> {
    ctx.say("Pong!").await?;
    Ok(())
}

#[poise::command(slash_command)]
pub async fn ban(ctx: Context<'_>) -> Result<(), Error> {
    ctx.send(|msg| msg.content("Nope!").ephemeral(true)).await?;
    Ok(())
}
