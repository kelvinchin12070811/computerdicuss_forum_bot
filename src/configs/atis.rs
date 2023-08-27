use serde::Deserialize;

#[derive(Deserialize)]
pub struct ATIS {
    webhook_report_endpoint: String,
    interval: u64,
}

impl ATIS {
    pub fn new() -> Option<ATIS> {
        Some(Self {
            webhook_report_endpoint: String::from(""),
            interval: 0,
        })
    }

    pub fn get_webhook_report_endpoint(&self) -> &str {
        &self.webhook_report_endpoint
    }

    pub fn get_interval(&self) -> u64 {
        self.interval
    }
}
