#[macro_use]
extern crate lazy_static;

mod commands;
mod configs;
mod dtos;
mod services;
mod types;

use commands::*;
use env_logger::Builder;
use log::{debug, info, LevelFilter};
use poise::serenity_prelude as serenity;
use services::auth_service;
use services::auth_service::AuthService;
use services::config_service::ConfigService;
use std::{
    fs::File,
    io::{read_to_string, BufReader},
};
use tokio;
use types::Data;

#[tokio::main]
async fn main() {
    Builder::new()
        .filter_module("computerdicuss_forum_bot", LevelFilter::Trace)
        .init();

    info!("Starting up");
    info!("Reading configurations");
    let config_file_handler = File::open("config.toml").unwrap();
    let config_file_reader = BufReader::new(config_file_handler);
    let config_file = read_to_string(config_file_reader).unwrap();
    ConfigService::initialize(config_file);
    let token = {
        let cs = config_service!();
        cs.get_document().get_token().to_owned()
    };

    debug!("bot token is: {}", token);

    {
        let auth_token = auth_service::get_token().await;
        let auth_service = auth_service!();
        debug!("auth token is : {}", auth_token);
        debug!("fetched at    : {}", auth_service.get_fetch_time());
    }

    info!("Initializing framework");
    let framework = poise::Framework::builder()
        .options(poise::FrameworkOptions {
            commands: get_commands::get_commands(),
            ..Default::default()
        })
        .token(token)
        .intents(serenity::GatewayIntents::non_privileged())
        .setup(move |ctx, _ready, framework| {
            Box::pin(async move {
                poise::builtins::register_globally(ctx, &framework.options().commands).await?;
                Ok(Data {})
            })
        });

    info!("Connecting to Discord server");
    framework.run().await.unwrap();
}
