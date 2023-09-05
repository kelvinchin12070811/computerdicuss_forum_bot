use crate::configs::document::Config;
use std::sync::Mutex;

pub struct ConfigService {
    document: Config,
}

impl ConfigService {
    pub fn new() -> ConfigService {
        Self {
            document: Config::new(),
        }
    }

    pub fn instance() -> &'static Mutex<ConfigService> {
        &*CONFIG_SERVICE
    }

    pub fn initialize(content: String) {
        let mut this = CONFIG_SERVICE.lock().unwrap();
        let new_content = toml::from_str(&content.to_string()).unwrap();
        this.set_document(new_content);
    }

    pub fn get_document(&self) -> &Config {
        &self.document
    }

    pub fn set_document(&mut self, config: Config) {
        self.document = config
    }
}

lazy_static! {
    static ref CONFIG_SERVICE: Mutex<ConfigService> = Mutex::new(ConfigService::new());
}

#[macro_export]
macro_rules! config_service {
    () => {{
        info!("Getting config service");
        let config_service = ConfigService::instance();
        info!("Got config service, locking mutex");
        let config_service = config_service.lock().unwrap();
        info!("Got it, returning config service");
        config_service
    }};
}
