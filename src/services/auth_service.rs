use log::{debug, error};
use tokio::sync::broadcast::error;

use crate::{config_service, dtos::auth::Auth};
use chrono::prelude::*;
use chrono::Duration;
use std::{collections::HashMap, ops::Add, sync::Mutex};
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

    if let Err(err) = res {
        error!("{}", err);
        return;
    };

    let data = res.unwrap().json::<Auth>().await;

    if let Err(data) = data {
        error!("{}", data);
        return;
    }

    let data = data.unwrap();
    let mut auth_service = auth_service!();
    auth_service.set_token(data.get_token());
    auth_service.set_fetch_time(Utc::now());
}

pub async fn refresh_token() {
    let configs = config_service!().get_document();
    let mut auth_service = auth_service!();
    let domain = configs.get_database().get_pocketbase_domain();
    let token = auth_service.get_token();

    let client = reqwest::Client::new();
    let res = client
        .post(format!("{}/api/collections/users/auth-refresh", domain))
        .header("Authorization", format!("Bearer {}", token))
        .send()
        .await;
    if let Err(error) = res {
        error!("{}", error);
        return;
    }

    let data = res.unwrap().json::<Auth>().await;
    if let Err(error) = data {
        error!("{}", error);
        return;
    }

    let data = data.unwrap();
    auth_service.set_token(data.get_token());
    auth_service.set_fetch_time(Utc::now());
}

pub async fn get_token() -> String {
    let (token, fetched_time) = {
        let mut auth_service = auth_service!();
        (
            auth_service.get_token().to_owned(),
            auth_service.get_fetch_time().to_owned(),
        )
    };

    if token.is_empty() {
        login_db().await;
        let mut auth_service = auth_service!();
        return auth_service.get_token().to_owned();
    }

    if fetched_time.add(Duration::minutes(30)).timestamp() < Utc::now().timestamp() {
        refresh_token().await;
        let mut auth_service = auth_service!();
        return auth_service.get_token().to_owned();
    }

    token.to_owned()
}
