use log::{debug, error};

use crate::{config_service, dtos::auth::Auth};
use chrono::prelude::*;
use std::{collections::HashMap, sync::Mutex};
#[macro_use]
use super::config_service::ConfigService;

pub struct AuthService {
    token: String,
    fetch_time: DateTime<Utc>,
}

impl AuthService {
    pub fn new() -> AuthService {
        AuthService {
            token: Default::default(),
            fetch_time: Default::default(),
        }
    }

    pub fn instance() -> &'static Mutex<AuthService> {
        &*AUTH_SERVICE
    }

    pub fn get_token(&mut self) -> &str {
        &self.token
    }

    pub fn set_token(&mut self, token: &str) {
        self.token = token.to_owned();
    }

    pub fn get_fetch_time(&self) -> &DateTime<Utc> {
        &self.fetch_time
    }

    pub fn set_fetch_time(&mut self, fetch_time: DateTime<Utc>) {
        self.fetch_time = fetch_time;
    }
}

lazy_static! {
    static ref AUTH_SERVICE: Mutex<AuthService> = Mutex::new(AuthService::new());
}

#[macro_export]
macro_rules! auth_service {
    () => {{
        let auth_service = AuthService::instance();
        let auth_service = auth_service.lock().unwrap();
        auth_service
    }};
}

pub async fn login_db() {
    let config_service = config_service!();
    let database = config_service.get_document().get_database();
    let host = database.get_pocketbase_domain();
    let credential = HashMap::from([
        ("identity", database.get_pocketbase_username()),
        ("password", database.get_pocketbase_password()),
    ]);

    let client = reqwest::Client::new();
    let res = client
        .post(format!("{}/api/collections/users/auth-with-password", host))
        .json(&credential)
        .send()
        .await;

    match res {
        Ok(data) => {
            let data = data.json::<Auth>().await;
            match data {
                Ok(data) => {
                    let mut auth_service = auth_service!();
                    auth_service.set_token(data.get_token());
                    auth_service.set_fetch_time(Utc::now());
                }
                Err(error) => error!("{}", error),
            }
        }
        Err(error) => error!("{}", error),
    }
}

pub async fn get_token() -> String {
    let token = {
        let mut auth_service = auth_service!();
        auth_service.get_token().to_owned()
    };

    if token.is_empty() {
        login_db().await;
        let mut auth_service = auth_service!();
        auth_service.get_token().to_owned()
    } else {
        token.to_owned()
    }
}
