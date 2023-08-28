use crate::config_service;
use http::{Request, Response};
use std::{collections::HashMap, sync::Mutex};
#[macro_use]
use super::config_service::ConfigService;

pub struct AuthService {
    token: String,
}

impl AuthService {
    pub fn new() -> AuthService {
        AuthService {
            token: "".to_owned(),
        }
    }

    pub fn instance() -> &'static Mutex<AuthService> {
        &*AUTH_SERVICE
    }

    pub fn get_token(&mut self) -> &str {
        if self.token == "" {
            self.login_db()
        }

        &self.token
    }

    pub fn login_db(&mut self) {
        let config_service = config_service!();
        let database = config_service.get_document().get_database();
        let host = database.get_pocketbase_domain();
        let username = database.get_pocketbase_username();
        let password = database.get_pocketbase_password();
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
