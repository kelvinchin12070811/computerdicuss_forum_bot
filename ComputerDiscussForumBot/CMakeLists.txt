cmake_minimum_required(VERSION 3.10)
project(ComputerDiscussForumBot)
set(CMAKE_CXX_STANDARD 20)
set(CMAKE_CXX_FLAGS_DEBUG "${CMAKE_CXX_FLAGS_DEBUG} -DDEBUG")
set(CMAKE_INCLUDE_CURRENT_DIR on)

set(HEADER_FILES
)

set(SOURCE_FILES
    "src/Main.cpp"
)

add_executable(${PROJECT_NAME} ${HEADER_FILES} ${SOURCE_FILES})

if (CMAKE_CXX_COMPILER_ID MATCHES "MSVC")
  target_compile_options(${PROJECT_NAME} PRIVATE "/ZI")
  target_link_options(${PROJECT_NAME} PRIVATE "/SAFESEH:NO")
endif()