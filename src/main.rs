#[macro_use]
extern crate lazy_static;

mod configs;
mod services;

use std::{
    fs::File,
    io::{read_to_string, BufReader},
};

use services::config_service::ConfigService;

struct Test;

impl Test {
    pub fn run(&self) {
        let cs = config_service!();
        println!("This is from test class: {}", cs.get_document().get_token());
    }
}

fn main() {
    let config_file_handler = File::open("config.toml").unwrap();
    let config_file_reader = BufReader::new(config_file_handler);
    let config_file = read_to_string(config_file_reader).unwrap();
    ConfigService::initialize(config_file);

    {
        let config_service = config_service!();

        print!("token: {}\n", config_service.get_document().get_token());
        print!("app_id: {}\n", config_service.get_document().get_app_id());
        print!(
            "guild_id: {}\n",
            config_service.get_document().get_guild_id()
        );
        println!(
            "database: {}",
            config_service
                .get_document()
                .get_database()
                .get_pocketbase_domain()
        );
    }

    let tester = Test {};
    tester.run();
}
