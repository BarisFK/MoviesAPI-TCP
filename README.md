
# MoviesAPI-TCP

A simple C# server-client project that interacts with movie data stored in a JSON file.

## Overview

This project demonstrates a basic client-server architecture using C# and the TCP/IP protocol. The server manages requests for movie information (filtering, searching, sorting, and updating) from a local JSON dataset ("movies.json"), while the client allows users to interact with this data through a command-line interface.

## Features

**Server:**

* **Handles requests:**
    * Retrieves a list of movies (code 1)
    * Gets detailed information for a specific movie (code 2)
    * Lists all unique genres (code 3)
    * Filters movies by year range (code 5)
    * Filters movies by multiple genres, optionally sorting the results (code 4)
    * Updates movie data based on the given movie ID (code 92)
* **Data Format:** Exchanges data in JSON format for flexibility.
* **Error Handling:** Manages invalid requests and missing data.
* **Sorting:**  Allows sorting of filtered results by Rank, Title, or Rating (code 4).
* **Data Update:** Supports updating movie details in the JSON file (code 92).

**Client:**

* **Command-line Interface:** Simple interface to send requests to the server.
* **Sends JSON requests:** Includes code (request type) and optional parameters.
* **Displays responses:** Presents movie data received from the server.

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

* Get details for a movie with ID "tt0167260":
  ```json
  {"Code":"2","Id":"tt0167260"}
  ```

* List all genres:
  ```json
  {"Code":"3"}
  ```


* Get movies released between 2010 and 2015:
  ```json
  {"Code":"5","Start":"2010","End":"2015"}
  ```

* Get a list of movies that match all the genres listed, sorted by Title descending:
  ```json
  {"Code": "4","Genres":["Comedy","Romance"], "Cond":"Title"}  
  ```

* Update movie data:
  ```json
  {"Code":"92","Id":"tt0167260", "Title": "Updated Title", "Genres":["Drama","Action"]} 
  ```

 ## Server Responses

* **Successful Request (Code 1):**

```json
{"Code": "1","Content": [{"Title": "The Shawshank Redemption","Id": "id1"},{"Title": "The Godfather","Id": "id2"},...]}
```

* **Successful Request (Code 2):**

```json
{"Code":"2","Content":[{"Title":"The Shawshank Redemption","Id":"tt0167260","Rating":"9.3","Genres":["Drama"],"Year":"1994","Desc":"Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency."}]}
```

* **Successful Request (Code 3):**

```json
{"Code":"3","Content":["Action","Adventure","Crime","Drama","Thriller","War","Western",...]}
```

* **Successful Request (Code 4):**

```json
{"Code": "4","Content": [{"Title": "When Harry Met Sally","Id": "id3"}, {"Title": "Groundhog Day","Id": "id4"},...]} // Movies sorted by Title in descending order
```

* **Successful Update (Code 92):**

```json
{"Code": 92, "Message": "Movie updated successfully!"}
```


* **Error Response:**

```json
{"Code": "Error", "Message": "Invalid request."} 
```

