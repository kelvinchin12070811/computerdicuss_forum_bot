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
        let cs = ConfigService::instance();
        let cs = cs.lock().unwrap();

        println!("This is from test class: {}", cs.get_document().get_token());
    }
}

fn main() {
    let config_file_handler = File::open("config.toml").unwrap();
    let config_file_reader = BufReader::new(config_file_handler);
    let config_file = read_to_string(config_file_reader).unwrap();
    ConfigService::initialize(config_file);

    print!("token: {}\n", config_service!().get_document().get_token());
    print!(
        "app_id: {}\n",
        config_service!().get_document().get_app_id()
    );
    print!(
        "guild_id: {}\n",
        config_service!().get_document().get_guild_id()
    );

    let tester = Test {};
    tester.run();
}
