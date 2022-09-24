plugins {
    id("java")
}

group = "cc.kelvinchin"
version = "1.0-SNAPSHOT"

repositories {
    mavenCentral()
}

dependencies {
    testImplementation("org.junit.jupiter:junit-jupiter-api:5.8.1")
    testRuntimeOnly("org.junit.jupiter:junit-jupiter-engine:5.8.1")
    implementation("net.dv8tion:JDA:5.0.0-alpha.20")
}

tasks.getByName<Test>("test") {
    useJUnitPlatform()
}