#[macro_use]
extern crate lazy_static;

mod commands;
mod configs;
mod dtos;
mod services;
mod types;

use commands::*;
use poise::serenity_prelude as serenity;
use services::auth_service::{self, AuthService};
use services::config_service::ConfigService;
use std::{
    fs::File,
    io::{read_to_string, BufReader},
};
use tokio;
use types::Data;

#[tokio::main]
async fn main() {
    let config_file_handler = File::open("config.toml").unwrap();
    let config_file_reader = BufReader::new(config_file_handler);
    let config_file = read_to_string(config_file_reader).unwrap();
    ConfigService::initialize(config_file);
    let token: String;

    {
        let mut auth_service = auth_service!();
        println!("auth token: {}", auth_service.get_token());

        let cs = config_service!();
        token = cs.get_document().get_token().to_owned();
        println!(
            "pocket base url: {}",
            cs.get_document().get_database().get_pocketbase_domain()
        );
    }

    let framework = poise::Framework::builder()
        .options(poise::FrameworkOptions {
            commands: vec![ping::ping()],
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

    framework.run().await.unwrap();
}
