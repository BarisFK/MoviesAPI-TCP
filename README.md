
# CSharpMovieServer 

A simple C# server-client project that interacts with movie data stored in a JSON file.

## Overview

This project demonstrates a basic client-server architecture using C# and the TCP/IP protocol. The server manages requests for movie information (filtering, searching, etc.) from a local JSON dataset ("movies.json"), while the client allows users to interact with this data through a command-line interface.

## Features

**Server:**

* **Handles requests:**
    * Retrieves a list of movies (code 1/4)
    * Gets detailed information for a specific movie (code 2)
    * Lists all unique genres (code 3)
    * Filters movies by year range (code 5)
* **Data Format:**  Exchanges data in JSON format for flexibility.
* **Error Handling:**  Manages invalid requests and missing data.

**Client:**

* **Command-line Interface:** Simple interface to send requests to the server.
* **Sends JSON requests:**  Includes code (request type) and optional parameters.
* **Displays responses:**  Presents movie data received from the server.

## Getting Started

1. **Clone the repository:**

   ```bash
   git clone [URL]
   ```

2. **Dependencies:**

   * This project requires the Newtonsoft.Json NuGet package:
     ```bash
     dotnet add package Newtonsoft.Json
     ```

3. **Run the server:**

   * Navigate to the `Server` directory and compile the project:
     ```bash
     dotnet build
     ```
   * Then run:
     ```bash
     dotnet run
     ```

4. **Run the client:**

   * In a separate terminal, navigate to the `Client` directory and compile the project:
     ```bash
     dotnet build
     ```
   * Run the client and follow the instructions:
     ```bash
     dotnet run
     ```

## Sample Requests (Client Input)

* Get a list of all movies:
  ```json
  {"Code":"1"} 
  ```
   **Server Response:**
  ```json
  {"Code": "1","Content": [{"Title": "The Shawshank Redemption","Id": "id1"},{"Title": "The Godfather","Id": "id2"},... *IDs are examples*]}
  ``` 


* Get details for a movie with ID "tt0167260":
  ```json
  {"Code":"2","Id":"tt0167260"}
  ```
  **Server Response:**
  ```json
  {"Code":"2","Content":[{"Title":"The Shawshank Redemption","Id":"tt0167260","Rank":"1","Rating":"9.3","Genres":["Drama"],"Year":"1994","Desc":"Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency."}]}
  ```


* List all genres:
  ```json
  {"Code":"3"}
  ```
  **Server Response:**
  ```json
  {"Code":"3","Content":[{"Genres":["Action","Adventure","Crime","Drama","Thriller","War","Western",...]}]}
  ```



* Get movies released between 2010 and 2015:
  ```json
  {"Code":"5","Start":"2010","End":"2015"}
  ```
  **Server Response:**
  ```json
  {"Code":"5","Content":[{"Title":"Inception","Id":"id1","Year":"2010"},{"Title":"The King's Speech","Id":"id2","Year":"2010"},...]}
  ```
  

* Get a list of movies that match all the genres listed:
  ```json
  {"Code": "4","Genres":["Comedy","Romance"]}
  ```
   
  **Server Response:**
  ```json
  {"Code": "4","Content": [{"Title": "The Princess Bride","Id": "id1"},{"Title": "Groundhog Day","Id": "id2"},...]}
  ```


* Get a list of movies that match all the genres listed, sorted by rank descending:
  ```json
  {"Code": "4","Genres":["Comedy","Romance"], "Cond":"Rank"} 
  ```
   **Server Response:**
  ```json
  {"Code": "4","Content": [{"Title": "The Princess Bride","Id": "id1"},{"Title": "Groundhog Day","Id": "id2"},...]}  // Movies are sorted by Rank
  ```



