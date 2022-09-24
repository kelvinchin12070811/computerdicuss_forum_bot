plugins {
    id("java")
    application
}

group = "cc.kelvinchin"
version = "2.2.0-indev"

application {
    mainClass.set("cc.kelvinchin.Main")
}

repositories {
    mavenCentral()
}

dependencies {
    testImplementation("org.junit.jupiter:junit-jupiter-api:5.8.1")
    testRuntimeOnly("org.junit.jupiter:junit-jupiter-engine:5.8.1")

    implementation("net.dv8tion:JDA:5.0.0-alpha.20")
    implementation("commons-cli:commons-cli:1.5.0")
}

tasks.getByName<Test>("test") {
    useJUnitPlatform()
}
