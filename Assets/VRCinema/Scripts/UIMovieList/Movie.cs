using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class MovieCards 
{
    [JsonProperty("title")]
    public string movieTitle;
    [JsonProperty("movie_photo")]
    public string urlPhotoName;
    [JsonProperty("namegGenres")]
    public string genre;
    [JsonProperty("url_move")]
    public string movieURL;
    [JsonProperty("discription_movie")]
    public string discription;
    [JsonProperty("movie_id")]
    public string movieId;
    [JsonProperty("like_id")]
    public string likeId;
    [JsonProperty("user_id")]
    public string userId;
    [JsonProperty("favorite_id")]
    public string favoriteId;
    [JsonProperty("watched_id")]
    public string watchedId;
    [JsonProperty("rating")]
    public string rating;
    [JsonProperty("duration")]
    public string duration; 
    [JsonProperty("release_year")]
    public string release_year;
    [JsonProperty("comment_id ")]
    public string comment_id;
    [JsonProperty("username")]
    public string username;
    [JsonProperty("comment")]
    public string comment;
    [JsonProperty("created_at")]
    public string created_at;
    [JsonProperty("user_photo")]
    public string userPhoto;

    public MovieCards(string movieTitle, string userId, string watchedId, string favoriteId, 
        string release_year , string duration, 
        string rating, string genre, string discription,string urlPhotoName,  
        string movieURL, string movieId, string likeId, string comment_id, 
        string username, string comment, string created_at, string userPhoto)
    {
        this.movieTitle = movieTitle;
        this.urlPhotoName = urlPhotoName;
        this.discription = discription;
        this.genre = genre;
        this.movieURL = movieURL;
        this.movieId = movieId;
        this.likeId = likeId;
        this.userId = userId;
        this.favoriteId = favoriteId;
        this.watchedId = watchedId;
        this.release_year = release_year;
        this.duration = duration;
        this.rating = rating;
        this.userPhoto = userPhoto;
        this.created_at = created_at;
        this.comment = comment;
        this.username = username;
        this.comment_id = comment_id;
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public static MovieCards FromJson(string json)
    {
        return JsonUtility.FromJson<MovieCards>(json);
    }
}