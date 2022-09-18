cmake_minimum_required(VERSION 3.10)
project(ComputerDiscussForumBot)
set(CMAKE_CXX_STANDARD 20)
set(CMAKE_CXX_FLAGS_DEBUG "${CMAKE_CXX_FLAGS_DEBUG} -DDEBUG")
set(CMAKE_INCLUDE_CURRENT_DIR on)

find_package(dpp CONFIG REQUIRED)
find_package(nlohmann_json CONFIG REQUIRED)

set(HEADER_FILES
)

set(SOURCE_FILES
    "src/Main.cpp"
)

add_executable(${PROJECT_NAME} ${HEADER_FILES} ${SOURCE_FILES})

target_link_libraries(${PROJECT_NAME} PRIVATE dpp::dpp)
target_link_libraries(${PROJECT_NAME} PRIVATE nlohmann_json::nlohmann_json)

if (CMAKE_CXX_COMPILER_ID MATCHES "MSVC")
  target_compile_options(${PROJECT_NAME} PRIVATE "/ZI")
  target_link_options(${PROJECT_NAME} PRIVATE "/SAFESEH:NO")
endif()
