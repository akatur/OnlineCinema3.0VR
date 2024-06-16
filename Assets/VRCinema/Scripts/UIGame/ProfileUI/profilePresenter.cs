using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class profilePresenter : MonoBehaviour
{
    [SerializeField] private TMP_Text login;
    [SerializeField] private TMP_Text nickName;
    [SerializeField] private TMP_Text city;
    public string userPhoto;
    public Image PosterMovie;

    [SerializeField] private authModel authmodel;

    //[SerializeField] private TMP_Text city;

    [SerializeField] private Button btnSelectWindowChengeProfile;

    [SerializeField] private TMP_InputField utlPhotoProfile;
    [SerializeField] private TMP_InputField inputFieldNameProfile;
    [SerializeField] private TMP_InputField inputFieldCityProfile;
    [SerializeField] private TMP_InputField inputFieldLoginProfile;

    [SerializeField] private Button btnAcceptChengeProfile;

    [SerializeField] private GameObject WindowSelectDataProfile;

    [Header("Scripts")]
    [SerializeField] private authModel authModel;
    [SerializeField] private ProfileModel profileModel;

    //[SerializeField] private Button authUser;

    public event Action<profilePresenter> OnButtonProfileClick;


   
    public Profile profile;

    private void Start()
    {

        if (btnSelectWindowChengeProfile != null)
        {
            btnSelectWindowChengeProfile.onClick.AddListener(SelectWindowChengeProfile);
        }

        if (btnAcceptChengeProfile != null)
        {
            btnAcceptChengeProfile.onClick.AddListener(SendDataProfile);
        }

    }

   
    private void ExitWindowChengeProfile()
    {
        WindowSelectDataProfile.gameObject.SetActive(false);
    }


    public void Init(Profile profile)
    {
        nickName.text = profile.username;
        userPhoto = profile.userPhoto;
        login.text = profile.login;
        city.text = profile.city;
        this.profile = profile;

        Debug.Log("PhotoUser" + userPhoto);
        StartCoroutine(ProfileModel.LoadImageFromURL(userPhoto, PosterMovie));

        //if (authUser != null)
        //{
        //    authUser.onClick.AddListener(ButtonProfileClick);
        //}
    }

    public void ButtonProfileClick()
    {
        OnButtonProfileClick?.Invoke(this);
    }
    public void SelectWindowChengeProfile()
    {
        WindowSelectDataProfile.gameObject.SetActive(true);
    }


    public void SendDataProfile()
    {
        if (profileModel == null)
        {
            Debug.LogError("profileModel is not initialized!");
            return;
        }
        string utlPhoto = utlPhotoProfile.text.Trim();
        string inputFieldName = inputFieldNameProfile.text.Trim();
        string inputFieldCity = inputFieldCityProfile.text.Trim();
        string inputFieldLogin = inputFieldLoginProfile.text.Trim();

        profileModel.ChangeDataProfiles(utlPhoto, inputFieldName, inputFieldCity, inputFieldLogin);
        //authModel.AddMovie(namemovie, genre, urlMovie, photoUrlMovie, discription, movieRating, movieDuration, releaseyear);

        WindowSelectDataProfile.gameObject.SetActive(false);
    }
}