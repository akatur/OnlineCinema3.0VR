using System.Collections;
using UnityEngine;
using MySql.Data.MySqlClient;
using System;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using Google.Protobuf.WellKnownTypes;
using UnityEngine.UIElements.Experimental;

public static class ConnectionInfo
{
    public static string ip = "127.0.0.1";
    public static string uid = "root";
    public static string pwd = "";
    public static string database = "cinemabd";
}

public static class UserInfo
{
    public static string currentName;
    public static string currentLogin;
    public static string user_id;
    public static string role_id;
    public static string role_name;
    public static string currentPassword;
}


[Serializable] 
public class ResponseData
{
    public int user_id;
    public int role_id;
    public string role_name;

}

public class authModel : MonoBehaviour
{
    [SerializeField]  public movePlayer movePlayer;

    public event Action OnEnableCameraControl;
    public event Action OnDisableCameraControl;


    static string conectionString = $"server={ConnectionInfo.ip}; " +
       $"uid={ConnectionInfo.uid}; " +
       $"pwd={ConnectionInfo.pwd}; " +
       $"Database = {ConnectionInfo.database}; " +
       $"SSLMode=none";

    private void Awake()
    {
        con = new MySqlConnection(conectionString);
        try
        {
            con.Open();
            Debug.Log("Подключение удачное!");
        }
        catch (Exception ex)
        {
            Debug.Log("Произошла ошибка!" + ex);
        }
    }

    static MySqlConnection con;


    private void Start()
    {
        OnEnableCameraControl += movePlayer.EnableCameraControl;
        OnDisableCameraControl += movePlayer.DisableCameraControl;
    }


    private void OnApplicationQuit()
    {
        con.Close();
    }

    [SerializeField] private GameObject UIAuth;
    [SerializeField] private GameObject UIGame;


    public static bool isAny(string query)
    {
        var cmd = new MySqlCommand(query, con);
        try
        {
            var reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                cmd.Dispose();
                return true;
            }
            else
            {
                cmd.Dispose();
                return false;
            }
        }
        catch (Exception)
        {
            cmd.Dispose();
            return false;
        }
    }

    public void LoginUser(string login, string password)
    {
        if (login == "" || password == "")
        {
            Debug.Log("Введите логин или пароль");
            return;
        }
        else
        {
            StartCoroutine(Authenticate(login, password));
            Debug.Log("Данные приняты");
        }
    }

    private IEnumerator Authenticate(string login, string password)
    { 
        string url = "http://localhost:3000/login";
        WWWForm form = new WWWForm();
        form.AddField("login", login);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string response = www.downloadHandler.text;
                if (response.Contains("user_id"))
                {
                    var responseData = JsonUtility.FromJson<ResponseData>(response);
                    UserInfo.currentLogin = login;
                    UserInfo.user_id = responseData.user_id.ToString();
                    UserInfo.role_id = responseData.role_id.ToString();
                    UserInfo.role_name = responseData.role_name;

                    Debug.Log(UserInfo.role_id);
                    Debug.Log(UserInfo.role_name);

                    UIAuth.SetActive(false);
                    UIGame.SetActive(true);
                    OnEnableCameraControl?.Invoke();

                    yield return true;
                }
                else
                {
                    OnDisableCameraControl?.Invoke();
                    Debug.Log("Ошибка аутентификации");
                }
            }
            else
            {
                OnDisableCameraControl?.Invoke();
                Debug.Log("Ошибка при отправке запроса: " + www.error);
            }
        }

        StopCoroutine(Authenticate(login, password));
    }



    public void RegUser(string login, string password, string nickname)
    {
        if (login == "" || password == "" || nickname == "")
        {
            Debug.Log("Введите логин, пароль и никнейм");
        }
        else
        {
            if (ContainsForbiddenCharacters(login) || ContainsForbiddenCharacters(password) || ContainsForbiddenCharacters(nickname))
            {
                Debug.Log("Логин, пароль или никнейм содержат запрещенные символы");
            }
            else
            {
                StartCoroutine(GetRegUser(login, password, nickname));
                Debug.Log("Регистрация прошла успешно");
            }
        }
    }

    private bool ContainsForbiddenCharacters(string input)
    {
        return Regex.IsMatch(input, @"^\d+$");
    }

   

    private IEnumerator GetRegUser(string login, string password, string nickname)
    {
        string url = "http://localhost:3000/register";

        WWWForm form = new WWWForm();
        form.AddField("login", login);
        form.AddField("password", password);
        form.AddField("username", nickname);

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string response = www.downloadHandler.text;
                ResponseData responseData = JsonUtility.FromJson<ResponseData>(response);

                if (responseData != null && responseData.user_id > 0)
                {
                    UserInfo.user_id = responseData.user_id.ToString();
                    UserInfo.role_id = responseData.role_id.ToString();

                    UIAuth.SetActive(false);
                    UIGame.SetActive(true);
                    OnEnableCameraControl?.Invoke();

                    Debug.Log("Регистрация успешно завершена");
                }
                else
                {
                    OnDisableCameraControl?.Invoke();
                    Debug.Log("Ошибка регистрации: неверный формат ответа сервера");
                }
            }
            else
            {
                OnDisableCameraControl?.Invoke();
                Debug.Log("Ошибка при отправке запроса: " + www.error);
            }
        }
        StopCoroutine(GetRegUser(login, password, nickname));
    }

    public void AddMovie(string nameMovie, string genre, string urlMovie, string photoUrlMovie, string discription, string movieRating, string movieDuration, string releaseyear)
    {
        if (nameMovie == "" || genre == "" || urlMovie == "")
        {
            Debug.Log("Введите данные фильма");
        }
        else
        {
            StartCoroutine(RegisterMovie(nameMovie, genre, urlMovie,  photoUrlMovie, discription, movieRating, movieDuration, releaseyear));
        }
    }

    private IEnumerator RegisterMovie(string nameMovie, string genre, string urlMovie, string photoUrlMovie, string description, string movieRating, string movieDuration, string releaseyear)
    {
        string url = "http://localhost:3000/registermovie";

        WWWForm form = new WWWForm();
        form.AddField("nameMovie", nameMovie);
        form.AddField("nameGenre", genre);
        form.AddField("urlMovie", urlMovie);
        form.AddField("photoUrlMovie", photoUrlMovie);
        form.AddField("description", description);
        form.AddField("movieRating", movieRating);
        form.AddField("movieDuration", movieDuration);
        form.AddField("releaseyear", releaseyear);

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string response = www.downloadHandler.text;
                if (response == "Success")
                {
                    Debug.Log("Фильм успешно добавлен: " + nameMovie + " " + genre + " " + urlMovie);
                }
                else
                {
                    Debug.Log("Ошибка регистрации фильма.");
                }
            }
            else
            {
                Debug.Log("Ошибка во время запроса: " + www.error);
            }
        }
    }
}

