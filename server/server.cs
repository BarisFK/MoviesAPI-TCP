using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

class Server
{
    // Sunucu uygulamasının ana metodu
    public static void ServerMain(string[] args)
    {
        TcpListener? server = null;
        try
        {
            // Bağlantıların dinleneceği port numarası
            Int32 port = 23000;
            // Sunucunun IP adresi
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");

            // TcpListener nesnesi oluşturarak belirtilen portta bağlantıları dinlemeye başla
            server = new TcpListener(localAddr, port);
            server.Start();

            // Alınacak veri için buffer
            Byte[] bytes = new Byte[2048];
            String? data = null;

            // İstemciden gelen ilk bağlantıyı kabul et
            Console.Write("Bağlantı bekleniyor... ");
            TcpClient client1 = server.AcceptTcpClient();
            Console.WriteLine("Bağlandı!");

            // İstemciyle iletişim kurmak için ağ akışı oluştur
            using (NetworkStream stream1 = client1.GetStream())
            {
                int i;
                while ((i = stream1.Read(bytes, 0, bytes.Length)) != 0)
                {
                    data = Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine("CLIENT: {0}", data);

                    try
                    {
                        // Movie sınıfı örneğine çevir
                        var Movie = JsonConvert.DeserializeObject<Movie>(data);

                        switch (Movie!.Code)
                        {
                            case 1:
                                string moviesList = Kod1(data); // JSON string olarak aktar
                                byte[] msg = Encoding.ASCII.GetBytes(moviesList); //mesaj oluştur
                                stream1.Write(msg, 0, msg.Length); //mesajı aktar
                                break;

                            case 2:
                                string movieDetails = Kod2(data);
                                byte[] msg2 = Encoding.ASCII.GetBytes(movieDetails);
                                stream1.Write(msg2, 0, msg2.Length);
                                break;

                            case 3:
                                string allGenres = Kod3(data);
                                byte[] msg3 = Encoding.ASCII.GetBytes(allGenres);
                                stream1.Write(msg3, 0, msg3.Length);
                                break;

                            case 4:
                                string movieWithGenres = Kod1(data);
                                byte[] msg4 = Encoding.ASCII.GetBytes(movieWithGenres);
                                stream1.Write(msg4, 0, msg4.Length);
                                break;
                            case 5:
                                string movieByYear = Kod4(data);
                                byte[] msg5 = Encoding.ASCII.GetBytes(movieByYear);
                                stream1.Write(msg5, 0, msg5.Length);
                                break;

                            default:
                                break;
                        }

                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine("Invalid JSON received: {0}", ex.Message);
                    }
                }
            }
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
        }
        finally
        {

            server!.Stop();
        }
    }

    public class Movie
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Title { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Rank { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Id { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string>? Genres { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Desc { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Year { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Rating { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Code { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Cond { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Movie>? Content { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Start { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? End { get; set; }





    }
    public static IEnumerable<Movie> Filter(IEnumerable<Movie> movies, Movie request)
    {
        // Varsa Genre eşleştirmesi yap
        if (request.Genres != null && request.Genres.Count > 0)
        {
            movies = movies.Where(m => m.Genres != null && request.Genres.All(genre => m.Genres.Contains(genre)));
        }

        // Varsa şart ile sırlama yap
        if (request.Cond != null)
        {
            switch (request.Cond)
            {
                case "Rank":
                    return movies.OrderBy(m => m.Rank);
                case "Title":
                    return movies.OrderBy(m => m.Title);
                case "Rating":
                    return movies.OrderBy(m => m.Rating);
                default:
                    return movies;
            }
        }
        else
        {
            return movies; // şart yoksa normal liste döndür
        }
    }

    public static string Kod1(string jsonRequest)
    {
        var request = JsonConvert.DeserializeObject<Movie>(jsonRequest);

        // İstek doğrulama (hem Kod1 hem Kod4 için gerekli)
        if (request == null)
        {
            return JsonConvert.SerializeObject(new
            {
                Code = "hata",
                Message = "Geçersiz istek."
            });
        }

        // Film verilerini okuma
        List<Movie> movies;
        using (StreamReader r = new StreamReader("movies.json"))
        {
            string json = r.ReadToEnd();
            movies = JsonConvert.DeserializeObject<List<Movie>>(json)!;
        }

        // Filtreleme ve sıralama
        IEnumerable<Movie> filteredMovies = movies;

        // Tür filtrelemesi 
        if (request.Genres != null && request.Genres.Count > 0)
        {
            filteredMovies = filteredMovies.Where(m => m.Genres != null && request.Genres.All(genre => m.Genres.Contains(genre)));
        }

        // Genel filtreleme ve sıralama 
        filteredMovies = Filter(filteredMovies, request);

        // Yanıt içeriği oluşturma
        var responseContent = filteredMovies.Select(movie => new Movie
        {
            Title = movie.Title,
            Id = movie.Id
        }).ToList();

        // Yanıt oluşturma
        var response = new Movie
        {
            Code = request.Code!,
            Content = responseContent
        };

        // JSON olarak döndürme
        return JsonConvert.SerializeObject(response);
    }

    public static string Kod2(string jsonRequest)
    {
        var request = JsonConvert.DeserializeObject<Movie>(jsonRequest);

        List<Movie> movies;
        using (StreamReader r = new StreamReader("movies.json"))
        {
            string json = r.ReadToEnd();
            movies = JsonConvert.DeserializeObject<List<Movie>>(json)!;
        }

        // Find the movie with the matching ID
        var matchingMovie = movies.FirstOrDefault(m => m.Id == request!.Id);

        var response = new Movie
        {
            Code = request!.Code!
        };

        if (matchingMovie != null)
        {
            // Film bilgilerini döndür
            response.Content = new List<Movie> { new Movie
            {
                Title = matchingMovie.Title,
                Id=matchingMovie.Id,
                Rank = matchingMovie.Rank,
                Rating=matchingMovie.Rating,
                Genres=matchingMovie.Genres,
                Year=matchingMovie.Year,
                Desc=matchingMovie.Desc
            }};
        }
        else
        {
            System.Console.WriteLine("hata");
        }

        return JsonConvert.SerializeObject(response);
    }

    public static string Kod3(string jsonRequest)
    {
        var request = JsonConvert.DeserializeObject<Movie>(jsonRequest);

        List<Movie> movies;
        using (StreamReader r = new StreamReader("movies.json"))
        {
            string json = r.ReadToEnd();
            movies = JsonConvert.DeserializeObject<List<Movie>>(json)!;
        }


        HashSet<string> uniqueGenres = new HashSet<string>();
        foreach (var movie in movies)
        {
            if (movie.Genres != null)
            {
                foreach (var genre in movie.Genres)
                {
                    uniqueGenres.Add(genre);
                }
            }
        }

        var response = new
        {
            Code = request!.Code!,
            Content = uniqueGenres
        };

        return JsonConvert.SerializeObject(response);
    }

    public static string Kod4(string jsonRequest)
    {
        var request = JsonConvert.DeserializeObject<Movie>(jsonRequest);

        List<Movie> movies;
        using (StreamReader r = new StreamReader("movies.json"))
        {
            string json = r.ReadToEnd();
            movies = JsonConvert.DeserializeObject<List<Movie>>(json)!;
        }

        if (request == null || request.Start == null || request.End == null)
        {
            return JsonConvert.SerializeObject(new
            {
                Code = "hata",
                Message = "Yanlış istek. 'Start' and 'End' fields are required."
            });
        }

        // Yıl aralığını parse ve validate et
        if (!int.TryParse(request.Start, out int startYear) || !int.TryParse(request.End, out int endYear) || startYear > endYear)
        {
            return JsonConvert.SerializeObject(new
            {
                Code = "hata",
                Message = "Invalid year range. 'Start' must be a valid year less than or equal to 'End'."
            });
        }

        // Filtreleme
        var filteredMovies = movies.Where(m =>
            m.Year != null && int.TryParse(m.Year, out int movieYear) && movieYear >= startYear && movieYear <= endYear
        );

        var responseContent = filteredMovies.Select(movie => new Movie
        {
            Title = movie.Title,
            Id = movie.Id,
            Year = movie.Year
        }).ToList();

        var response = new Movie
        {
            Code = request!.Code!,
            Content = responseContent
        };

        return JsonConvert.SerializeObject(response);
    }

    static void Main(string[] args)
    {
        ServerMain(args);
    }
}
