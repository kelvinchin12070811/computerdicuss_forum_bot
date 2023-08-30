// {
//     "record": {
//       "avatar": "",
//       "collectionId": "_pb_users_auth_",
//       "collectionName": "users",
//       "created": "2023-08-28 17:51:33.004Z",
//       "email": "",
//       "emailVisibility": false,
//       "id": "drbe82m6t68rnf3",
//       "name": "bot",
//       "updated": "2023-08-28 18:10:02.975Z",
//       "username": "bot",
//       "verified": true
//     },
//     "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJjb2xsZWN0aW9uSWQiOiJfcGJfdXNlcnNfYXV0aF8iLCJleHAiOjE2OTQ2MzIyNDcsImlkIjoiZHJiZTgybTZ0NjhybmYzIiwidHlwZSI6ImF1dGhSZWNvcmQifQ.ilS6xaES5QQVvhrTos7rXpR64tPj2raxiP4Qj6t4yH4"
//   }

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
