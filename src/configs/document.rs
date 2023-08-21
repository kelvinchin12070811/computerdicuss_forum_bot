use serde::Deserialize;

#[derive(Deserialize)]
pub struct Config {
    token: String,
    app_id: String,
    guild_id: String,
}

impl Config {
    pub fn new() -> Config {
        Self {
            token: String::from(""),
            app_id: String::from(""),
            guild_id: String::from(""),
        }
    }
    pub fn get_token(&self) -> &String {
        &self.token
    }

    pub fn get_app_id(&self) -> &String {
        &self.app_id
    }

    pub fn get_guild_id(&self) -> &String {
        &self.guild_id
    }
}
