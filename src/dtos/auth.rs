use serde::Deserialize;

#[derive(Deserialize)]
#[allow(dead_code)]
pub struct Record {
    avatar: String,
    #[serde(rename = "collectionId")]
    collection_id: String,
    #[serde(rename = "collectionName")]
    collection_name: String,
    created: String,
    email: String,
    #[serde(rename = "emailVisibility")]
    email_visibility: bool,
    id: String,
    name: String,
    updated: String,
    username: String,
    verified: bool,
}

#[allow(dead_code)]
impl Record {
    pub fn get_avatar(&self) -> &str {
        &self.avatar
    }

    pub fn get_collection_id(&self) -> &str {
        &self.collection_id
    }

    pub fn get_collection_name(&self) -> &str {
        &self.collection_name
    }

    pub fn get_created(&self) -> &str {
        &self.created
    }

    pub fn get_email(&self) -> &str {
        &self.email
    }

    pub fn get_email_visibility(&self) -> bool {
        self.email_visibility
    }

    pub fn get_id(&self) -> &str {
        &self.id
    }

    pub fn get_name(&self) -> &str {
        &self.name
    }

    pub fn get_updated(&self) -> &str {
        &self.updated
    }

    pub fn get_username(&self) -> &str {
        &self.username
    }

    pub fn get_verified(&self) -> bool {
        self.verified
    }
}

impl Default for Record {
    fn default() -> Self {
        Self {
            avatar: Default::default(),
            collection_id: Default::default(),
            collection_name: Default::default(),
            created: Default::default(),
            email: Default::default(),
            email_visibility: Default::default(),
            id: Default::default(),
            name: Default::default(),
            updated: Default::default(),
            username: Default::default(),
            verified: Default::default(),
        }
    }
}

#[derive(Deserialize)]
#[allow(dead_code)]
pub struct Auth {
    record: Record,
    token: String,
}

#[allow(dead_code)]
impl Auth {
    pub fn get_record(&self) -> &Record {
        &self.record
    }

    pub fn get_token(&self) -> &str {
        &self.token
    }
}

impl Default for Auth {
    fn default() -> Self {
        Self {
            record: Default::default(),
            token: Default::default(),
        }
    }
}
