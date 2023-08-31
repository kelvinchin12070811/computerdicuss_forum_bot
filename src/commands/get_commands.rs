use super::super::types::Error;
use crate::commands::*;

pub fn get_commands() -> Vec<poise::Command<crate::types::Data, Error>> {
    vec![common::ping(), common::ban()]
}
