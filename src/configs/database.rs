use serde::Deserialize;

#[derive(Deserialize)]
pub struct Database {
    pocketbase_domain: String,
    pocketbase_username: String,
    pocketbase_password: String,
}

impl Database {
    pub fn new() -> Database {
        Self {
            pocketbase_domain: String::from(""),
            pocketbase_username: String::from(""),
            pocketbase_password: String::from(""),
        }
    }

    pub fn get_pocketbase_domain(&self) -> &str {
        &self.pocketbase_domain
    }

    pub fn get_pocketbase_username(&self) -> &str {
        &self.pocketbase_username
    }

    pub fn get_pocketbase_password(&self) -> &str {
        &self.pocketbase_password
    }
}
