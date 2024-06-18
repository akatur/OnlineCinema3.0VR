using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using System;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Linq;
using System.Drawing.Printing;
using Google.Protobuf.WellKnownTypes;
using UnityEngine.UIElements.Experimental;
using Unity.VisualScripting;
using UnityEngine.Analytics;
using static CardControllerPresenter;

public class CardsControllerModel : MonoBehaviour
{
    [SerializeField] private UnityEngine.Video.VideoPlayer videoPlayer;

    public event Action OnInsertAllMovies;
    public event Action OnInsertLikes;
    public event Action OnInsertFav;
    public event Action OnInsertWatch;
    public event Action OnInsertToPanoram;

    public event Action OnInsertComment;

    public List<MovieCards> MovieList = new();
    public List<MovieCards> LikeList = new();
    public List<MovieCards> FavouritesList = new();
    public List<MovieCards> WatchedList = new();
    public List<MovieCards> ToPanoram = new();
    public List<MovieCards> CommentsList = new();

    public TMP_Dropdown genreDropdown;
    private HashSet<string> uniqueTitlesInt = new HashSet<string>();



    public class Row
    {
        [JsonProperty("rowCount")]
        public int rowCounts;

        public string ToJson()
        {
            return JsonUtility.ToJson(rowCounts);
        }
        public static Row FromJson(string json)
        {
            return JsonUtility.FromJson<Row>(json);
        }
    }


    [System.Serializable]
    public class GenreData
    {
        public string namegGenres;
    }


    private void Start()
    {
        
        GetMovie();
    }

    public void GetMovie()
    {
        StartCoroutine(GetMoviesFromServer());
        StartCoroutine(GetGenresFromServer());
    }

    public IEnumerator GetMoviesFromServer()
    {
        UnityWebRequest www = UnityWebRequest.Get($"http://localhost:3000/getmovie");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string json = www.downloadHandler.text;
            Debug.Log("Полученный JSON: " + json);
            MovieCards[] movies = JsonConvert.DeserializeObject<MovieCards[]>(json);
            foreach (var movie in movies)
            {
                if (!MovieList.Any(existingMovie => existingMovie.movieId == movie.movieId))
                {
                    MovieList.Add(movie);
                }
            }
            OnInsertAllMovies?.Invoke();
            
        }
        else
        {
            Debug.LogError("Ошибка получения данных фильма " + www.error);
        }

        www.Dispose();
    }

   

    public IEnumerator GetGenresFromServer()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://localhost:3000/getgenres");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string json = www.downloadHandler.text;
            Debug.Log("Полученный JSON: " + json);
            try
            {
                GenreData[] genreDataArray = JsonConvert.DeserializeObject<GenreData[]>(json);
                genreDropdown.ClearOptions();

                HashSet<string> uniqueGenres = new HashSet<string>();
                List<TMP_Dropdown.OptionData> dropdownOptions = new List<TMP_Dropdown.OptionData>();

                foreach (GenreData genreData in genreDataArray)
                {
                    if (uniqueGenres.Add(genreData.namegGenres)) 
                    {
                        dropdownOptions.Add(new TMP_Dropdown.OptionData(genreData.namegGenres));
                    }
                }

                genreDropdown.AddOptions(dropdownOptions);
            }
            catch (JsonException ex)
            {
                Debug.LogError("Ошибка разбора JSON: " + ex.Message);
            }
        }
        else
        {
            Debug.LogError("Ошибка получения данных о жанрах " + www.error);
        }

        www.Dispose();
    }



    public IEnumerator GetMoviesToSearchFromServer(string searchTerm)
    {
        string url = $"http://localhost:3000/searchmovie?term={UnityWebRequest.EscapeURL(searchTerm)}";

        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string json = www.downloadHandler.text;

            MovieCards[] movies = JsonConvert.DeserializeObject<MovieCards[]>(json);
            MovieList.Clear();
            foreach (var movie in movies)
            {
                string movieId = movie.movieId;
                if (MovieList.Any(existingMovie => existingMovie.movieId == movieId))
                {
                    continue;
                }
                string movieTitle = movie.movieTitle;
                string genre = movie.genre;
                string movieURL = movie.movieURL;
                string urlPhotoName = movie.urlPhotoName;
                string description = movie.discription;
                string likeId = movie.likeId;
                string favoriteId = movie.favoriteId;
                string watchedId = movie.watchedId;
                string created_at = movie.created_at;
                string comment_id = movie.comment_id;
                string discription = movie.discription;
                string userId = movie.userId;
                string username = movie.username;
                string comment = movie.comment;
                string release_year = movie.release_year;
                string duration = movie.duration;
                string rating = movie.rating;
                string userPhoto = movie.userPhoto;

                MovieCards movieCard = new MovieCards(movieTitle, userId, watchedId, favoriteId, release_year, duration, rating, genre, discription, urlPhotoName, movieURL, movieId, likeId, comment_id, username, comment, created_at, userPhoto);

                MovieList.Add(movieCard);
            }
            OnInsertAllMovies?.Invoke();
        }
        else
        {
            Debug.LogError("Ошибка получения данных фильма " + www.error);
        }

        www.Dispose();
    }


    public IEnumerator GetMoviesToGenreshFromServer(string genreTerm)
    {
        string url = $"http://localhost:3000/getgenresinmoviecard?term={UnityWebRequest.EscapeURL(genreTerm)}";

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            string json = www.downloadHandler.text;

            MovieCards[] movies = JsonConvert.DeserializeObject<MovieCards[]>(json);
            MovieList.Clear();
            foreach (var movie in movies)
            {
                int existingIndex = MovieList.FindIndex(existingMovie => existingMovie.movieId == movie.movieId);
                if (existingIndex != -1)
                {
                    MovieList[existingIndex] = movie;
                }
                else
                {
                    MovieList.Add(movie);
                }
            }

            OnInsertAllMovies?.Invoke();
        }
    }

   

    public void invokeLikesCards()
    {
        string userId = UserInfo.user_id;
        Debug.Log(userId);
        UnityWebRequest www = UnityWebRequest.Get("http://localhost:3000/likes?user_id=" + userId);

        StartCoroutine(ProcessRequestLike(www));
    }

    private IEnumerator ProcessRequestLike(UnityWebRequest www)
    {
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string json = www.downloadHandler.text;
            ParseAndSortDataLike(json);
        }
        else
        {
            Debug.LogError(www.error);
        }
    }

    private void ParseAndSortDataLike(string json)
    {
        MovieCards[] movies = JsonConvert.DeserializeObject<MovieCards[]>(json);
        Debug.Log(json + " like");
        LikeList.Clear();

        foreach (var movie in movies)
        {
            string movieId = movie.movieId;
            
            string movieid = movie.movieId;
            if (LikeList.Any(existingMovie => existingMovie.movieId == movieId))
            {
                continue;
            }
            string movieTitle = movie.movieTitle;
            string genre = movie.genre;
            string movieURL = movie.movieURL;
            string urlPhotoName = movie.urlPhotoName;
            string description = movie.discription;
            string likeId = movie.likeId;
            string favoriteId = movie.favoriteId;
            string watchedId = movie.watchedId;
            string created_at = movie.created_at;
            string comment_id = movie.comment_id;
            string discription = movie.discription;
            string userId = movie.userId;
            string username = movie.username;
            string comment = movie.comment;
            string release_year = movie.release_year;
            string duration = movie.duration;
            string rating = movie.rating;
            string userPhoto = movie.userPhoto;

            MovieCards movieCard = new MovieCards(movieTitle, userId, watchedId, favoriteId, release_year, duration, rating, genre, discription, urlPhotoName, movieURL, movieId, likeId, comment_id, username, comment, created_at, userPhoto);

            Debug.Log($"Title: {movieTitle}, Genre: {genre}, URL: {movieURL}, Photo: {urlPhotoName}, Description: {description}, ID: {movieId}");
            
            LikeList.Add(movieCard);
        }
        OnInsertLikes?.Invoke();
    }

    public void invokeFavCards()
    {
        string userId = UserInfo.user_id;
        Debug.Log(userId);
        UnityWebRequest www = UnityWebRequest.Get("http://localhost:3000/datafavorite?user_id=" + userId);

        StartCoroutine(ProcessRequestFav(www));
    }

    private IEnumerator ProcessRequestFav(UnityWebRequest www)
    {
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string json = www.downloadHandler.text;
            ParseAndSortDataFav(json);
        }
        else
        {
            Debug.LogError(www.error);
        }
    }

    private void ParseAndSortDataFav(string json)
    {
        MovieCards[] movies = JsonConvert.DeserializeObject<MovieCards[]>(json);
        Debug.Log(json + " fav");

        foreach (var movie in movies)
        {
            string movieid = movie.movieId;
            if (FavouritesList.Any(existingMovie => existingMovie.movieId == movieid))
            {
                continue;
            }
            string userId = UserInfo.user_id;
            string movieTitle = movie.movieTitle;
            string genre = movie.genre;
            string movieURL = movie.movieURL;
            string urlPhotoName = movie.urlPhotoName;
            string description = movie.discription;
            string likeId = movie.likeId;
            string favoriteId = movie.favoriteId;
            string watchedId = movie.watchedId;
            string created_at = movie.created_at;
            string comment_id = movie.comment_id;
            string discription = movie.discription;
            string username = movie.username;
            string comment = movie.comment;
            string release_year = movie.release_year;
            string duration = movie.duration;
            string rating = movie.rating;
            string userPhoto = movie.userPhoto;

            MovieCards movieCard = new MovieCards(movieTitle, userId, watchedId, favoriteId, release_year, duration, rating, genre, discription, urlPhotoName, movieURL, movieid, likeId, comment_id, username, comment, created_at, userPhoto);

            Debug.Log($"Title: {movieTitle}, Genre: {genre}, URL: {movieURL}, Photo: {urlPhotoName}, Description: {description}, ID: {movieid}");

            FavouritesList.Add(movieCard);
        }
        OnInsertFav?.Invoke();
    }

    public void invokeWatchCards()
    {
        string userId = UserInfo.user_id;
        Debug.Log(userId);
        UnityWebRequest www = UnityWebRequest.Get("http://localhost:3000/history?user_id=" + userId);

        StartCoroutine(ProcessRequestWatch(www));
    }

    private IEnumerator ProcessRequestWatch(UnityWebRequest www)
    {
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string json = www.downloadHandler.text;
            ParseAndSortDataWatch(json);
        }
        else
        {
            Debug.LogError(www.error);
        }
    }

    private void ParseAndSortDataWatch(string json)
    {
        MovieCards[] movies = JsonConvert.DeserializeObject<MovieCards[]>(json);
        Debug.Log(json + " fav");

        foreach (var movie in movies)
        {
            string movieId = movie.movieId;
            if (WatchedList.Any(existingMovie => existingMovie.movieId == movieId))
            {
                continue;
            }
            string movieTitle = movie.movieTitle;
            string genre = movie.genre;
            string movieURL = movie.movieURL;
            string urlPhotoName = movie.urlPhotoName;
            string description = movie.discription;
            string userId = UserInfo.user_id;
            string likeId = movie.likeId;
            string favoriteId = movie.favoriteId;
            string watchedId = movie.watchedId;

            string release_year = movie.release_year;
            string duration = movie.duration;
            string rating = movie.rating;

            string username = movie.username;
            string userPhoto = movie.userPhoto;
            string comment = movie.comment;
            string created_at = movie.created_at;
            string comment_id = movie.comment_id;
            MovieCards movieCard = new MovieCards(movieTitle, likeId, watchedId, favoriteId, release_year, duration, rating, genre, description, urlPhotoName, movieURL, movieId, likeId, comment_id, username, comment, created_at, userPhoto);
           
            WatchedList.Add(movieCard);
            Debug.Log(movieCard);
        }
        OnInsertWatch?.Invoke();
    }

    public void AddToLike(MovieCards movie)
    {
        StartCoroutine(AddToLikeCoroutine(Convert.ToInt32(movie.movieId), movie.movieTitle));
    }

    private IEnumerator AddToLikeCoroutine(int movieId, string movieTitle)
    {
        WWWForm form = new WWWForm();
        form.AddField("movieId", movieId.ToString());
        form.AddField("userId", UserInfo.user_id.ToString());
        form.AddField("title", movieTitle);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:3000/addtolikes", form))
        {
            yield return www.SendWebRequest();
            try
            {
                if (www.result == UnityWebRequest.Result.Success)
                {
                    string response = www.downloadHandler.text;

                    Debug.Log("Фильм добавлен в like: " + response);
                }
                else
                {
                    Debug.LogError("Ошибка при добавлении фильма в like: " + www.error);
                }
            }
            catch (System.Exception er)
            {

                Debug.LogError("нельзя добавлять несколько фильмов: " + er.Message);
            }
        }
        StopCoroutine(AddToLikeCoroutine(movieId, movieTitle));
    }

    public void GetMovieForPanoram(MovieCards movie)
    {
        StartCoroutine(GetMoviesForPanoramFromServer(Convert.ToInt32(movie.movieId)));
    }


    public IEnumerator GetMoviesForPanoramFromServer(int movieId)
    {
        string url = "http://localhost:3000/getmovieforpanoram?movie_id=" + movieId;


        string moviead = Convert.ToString(movieId);

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string json = www.downloadHandler.text;

                MovieCards[] movies = JsonConvert.DeserializeObject<MovieCards[]>(json);
                ToPanoram.Clear();
                foreach (var movie in movies)
                {
                    string movieid = movie.movieId;
                    if (ToPanoram.Any(existingMovie => existingMovie.movieId == moviead))
                    {
                        continue;
                    }

                    string movieTitle = movie.movieTitle;
                    string genre = movie.genre;
                    string movieURL = movie.movieURL;
                    string urlPhotoName = movie.urlPhotoName;
                    string description = movie.discription;
                    string likeId = movie.likeId;
                    string favoriteId = movie.favoriteId;
                    string watchedId = movie.watchedId;
                    string release_year = movie.release_year;
                    string duration = movie.duration;
                    string rating = movie.rating;
                    string username = movie.username;
                    string userPhoto = movie.userPhoto;
                    string comment = movie.comment;
                    string created_at = movie.created_at;
                    string comment_id = movie.comment_id;
                    MovieCards movieCard = new MovieCards(movieTitle, likeId, watchedId, favoriteId, release_year, duration, rating, genre, description, urlPhotoName, movieURL, movieid, likeId, comment_id, username, comment, created_at, userPhoto);

                    ToPanoram.Add(movieCard);
                }
                OnInsertToPanoram?.Invoke();
            }
            else
            {
                Debug.LogError("Ошибка получения данных фильма " + www.error);
            }
        }
    }

    public void GetCommentMovieForPanoram(MovieCards movie, Action OnSeces)
    {

        StartCoroutine(GetCommentForPanoramFromServer(Convert.ToInt32(movie.movieId ),OnSeces));

    }

    public IEnumerator GetCommentForPanoramFromServer(int movieId, Action OnSeces)
    {
        string url = "http://localhost:3000/commentsmovie?movie_id=" + movieId;
        string moviead = Convert.ToString(movieId);

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string json = www.downloadHandler.text;

                MovieCards[] movies = JsonConvert.DeserializeObject<MovieCards[]>(json);
                CommentsList.Clear();
                foreach (var movie in movies)
                {
                    string movieid = movie.movieId;
                    if (CommentsList.Any(existingMovie => existingMovie.movieId == moviead))
                    {
                        continue;
                    }

                    string movieTitle = movie.movieTitle;
                    string genre = movie.genre;
                    string movieURL = movie.movieURL;
                    string urlPhotoName = movie.urlPhotoName;
                    string description = movie.discription;
                    string likeId = movie.likeId;
                    string favoriteId = movie.favoriteId;
                    string watchedId = movie.watchedId;
                    string release_year = movie.release_year;
                    string duration = movie.duration;
                    string rating = movie.rating;
                    string username = movie.username;
                    string userPhoto = movie.userPhoto;
                    string comment = movie.comment;
                    string created_at = movie.created_at;
                    string comment_id = movie.comment_id;
                    MovieCards movieCard = new MovieCards(movieTitle, likeId, watchedId, favoriteId, release_year, duration, rating, genre, description, urlPhotoName, movieURL, movieid, likeId, comment_id, username, comment, created_at, userPhoto);

                    CommentsList.Add(movieCard);
                }
            //OnInsertComment?.Invoke();

            OnSeces?.Invoke();

            }
            else
            {
                Debug.LogError("Ошибка получения данных фильма " + www.error);
            }
        }
    }


    public IEnumerator AddComment(int movieId, string comment, Action onSocs)
    {
        WWWForm form = new WWWForm();
        form.AddField("movieId", movieId.ToString());
        form.AddField("userId", UserInfo.user_id.ToString());
        form.AddField("comment", comment);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:3000/addcomment", form))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Комментарий добавлен.");

                onSocs?.Invoke();
            }
            else
            {
                Debug.LogError(www.error);
            }
        }
    }

    public void AddToFavorites(MovieCards movie)
    {
        StartCoroutine(AddFavorite(Convert.ToInt32(movie.movieId), movie.movieTitle));
    }

    private IEnumerator AddFavorite(int movieId, string movieTitle)
    {
        WWWForm form = new WWWForm();
        form.AddField("movieId", movieId.ToString());
        form.AddField("userId", UserInfo.user_id.ToString());
        form.AddField("title", movieTitle);


        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:3000/addtofavorites", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Фильм добавлен в favorites.");
            }
            else
            {
                Debug.LogError(www.error);
            }
        }
        StopCoroutine(AddFavorite(movieId, movieTitle));
    }

    public void AddTWatched(MovieCards movie)
    {
        StartCoroutine(AddTWatch(Convert.ToInt32(movie.movieId), movie.movieTitle));
    }

    private IEnumerator AddTWatch(int movieId, string movieTitle)
    {
        WWWForm form = new WWWForm();
        form.AddField("movieId", movieId.ToString());
        form.AddField("userId", UserInfo.user_id.ToString());
        form.AddField("title", movieTitle);


        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:3000/addtowatched", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Фильм добавлен в watched.");
            }
            else
            {
                Debug.LogError(www.error);
            }
        }
        StopCoroutine(AddTWatch(movieId, movieTitle));
    }

    public void OnButtonClickDeleteLike(MovieCards movie)
    {
        StartCoroutine(DeleteLikesOnServer(movie));
    }

    private IEnumerator DeleteLikesOnServer(MovieCards movie)
    {
        string userId = UserInfo.user_id;
        int likeId = Convert.ToInt32(movie.likeId);
        string title = movie.movieTitle;

        Debug.Log("userId " + userId);
        Debug.Log("movieId " + likeId);

        string url = $"http://localhost:3000/likesDelete?user_id={userId}&like_id={likeId}";
        UnityWebRequest requests = UnityWebRequest.Delete(url);

        yield return requests.SendWebRequest();

        if (requests.result == UnityWebRequest.Result.Success)
        {
            uniqueTitlesInt.Remove(title);
            LikeList.Remove(movie);
            Debug.Log(userId + "  " + likeId + "удалось");
        }
        else
        {
            Debug.LogError(requests.error);
        }
        StopCoroutine(DeleteLikesOnServer(movie));
    }

    public void OnButtonClickDeleteFav(MovieCards movie)
    {
        StartCoroutine(DeleteFavOnServer(movie));
    }

    private IEnumerator DeleteFavOnServer(MovieCards movie)
    {
        string userId = UserInfo.user_id;
        int favoriteId = Convert.ToInt32(movie.favoriteId);
        string title = movie.movieTitle;

        Debug.Log("userId " + userId);
        Debug.Log("movieId " + favoriteId);

        string url = $"http://localhost:3000/favDelete?user_id={userId}&favorite_id={favoriteId}";
        UnityWebRequest request = UnityWebRequest.Delete(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            uniqueTitlesInt.Remove(title);
            FavouritesList.Remove(movie);
            Debug.Log(userId + "  " + favoriteId + "удалось");
        }
        else
        {
            Debug.LogError(request.error);
        }
        StopCoroutine(DeleteFavOnServer(movie));
    }

    public void OnButtonClickDeleteWatch(MovieCards movie)
    {
        StartCoroutine(DeleteWatchOnServer(movie));
    }

    private IEnumerator DeleteWatchOnServer(MovieCards movie)
    {
        string userId = UserInfo.user_id;
        int watchedId = Convert.ToInt32(movie.watchedId);
        string title = movie.movieTitle;

        Debug.Log("userId " + userId);
        Debug.Log("movieId " + watchedId);

        string url = $"http://localhost:3000/hisDelete?user_id={userId}&watched_id={watchedId}";
        UnityWebRequest request = UnityWebRequest.Delete(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            uniqueTitlesInt.Remove(title);
            WatchedList.Remove(movie);
            Debug.Log(userId + "  " + watchedId + "удалось");
        }
        else
        {
            Debug.LogError(request.error);
        }
        StopCoroutine(DeleteWatchOnServer(movie));
    }

    public static IEnumerator LoadImageFromURL(string url, Image image)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);

                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                image.sprite = sprite;
            }
            else
            {
                Debug.LogError("Ошибка загрузки пользователя изображения: " + www.error);
            }
        }
    }

    public void PlayMovie(MovieCards movie)
    {
        videoPlayer.url = movie.movieURL;
        videoPlayer.Play();
    }
}