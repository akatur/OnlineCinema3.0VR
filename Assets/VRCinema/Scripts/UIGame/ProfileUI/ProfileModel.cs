using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using System;
using Unity.VisualScripting;
using Newtonsoft.Json;
using UnityEngine.Networking;
using UnityEngine.Analytics;
using System.Linq;

public class ProfileModel : MonoBehaviour
{
    public List<Profile> ProfileList = new();

    private HashSet<string> uniqueTitlesInt = new HashSet<string>();

    public event Action OnInsertProfile;


    

    public void invokeProfile()
    {
        string userId = UserInfo.user_id;
        Debug.Log(userId);
        UnityWebRequest www = UnityWebRequest.Get("http://localhost:3000/getdataprofile?user_id=" + userId);

        StartCoroutine(ProcessRequest(www));
    }

    private IEnumerator ProcessRequest(UnityWebRequest www)
    {
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string json = www.downloadHandler.text;
            ParseAndSortData(json);
        }
        else
        {
            Debug.LogError(www.error);
        }
    }

    private void ParseAndSortData(string json)
    {
        Profile[] profiles = JsonConvert.DeserializeObject<Profile[]>(json);
        Debug.Log(json + " like");
        ProfileList.Clear();

        foreach (var profile in profiles)
        {
            string userId = UserInfo.user_id;
            string username = profile.username;
            string userPhoto = profile.userPhoto;
            string city = profile.city;
            string login = profile.login;

            if (ProfileList.Any(existingUser => existingUser.userId == userId))
            {
                continue;
            }

            Debug.Log($"User: {username}");
            Profile prof = new Profile( userId, username, city, login, userPhoto);
            ProfileList.Add(prof);
        }
        OnInsertProfile?.Invoke();
    }


    public void ChangeDataProfiles(string utlPhoto, string inputFieldName, string inputFieldCity, string inputFieldLogin)
    {
        if (utlPhoto == "" || inputFieldName == "" || inputFieldCity == "" || inputFieldLogin == "")
        {
            Debug.Log("Введите данные");
        }
        else
        {
            StartCoroutine(ChangeDataProfilesCor(utlPhoto, inputFieldName, inputFieldCity, inputFieldLogin)); 
        }
    }

    public IEnumerator ChangeDataProfilesCor(string utlPhoto, string inputFieldName, string inputFieldCity, string inputFieldLogin)
    {
        
        string userId = UserInfo.user_id;
        Debug.Log(userId);


        string url = $"http://localhost:3000/changedataprofile?user_id={userId}";
        WWWForm form = new WWWForm();

        form.AddField("user_id", userId);
        form.AddField("user_photo", utlPhoto);
        form.AddField("username", inputFieldName);
        form.AddField("city", inputFieldCity);
        form.AddField("login", inputFieldLogin);

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string response = www.downloadHandler.text;
                if (response.Contains("Success"))
                {
                    Debug.Log("Успешно изменено");
                }
                else
                {
                    Debug.Log("Ошибка: " + response);
                }
            }
            else
            {
                Debug.Log("Ошибка во время запроса: " + www.error);
            }
        }
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

}
