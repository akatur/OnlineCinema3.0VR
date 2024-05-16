using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LikesCardsPresenter : MonoBehaviour
{

    //[SerializeField] private Button btnFavorit;
    //[SerializeField] private Button btnLike;

    public Text movieTitle;
    public Image urlPhotoName;

    public MovieCards like;


    //public event Action<Movie> OnButtonFavorClick;
    //public event Action<Movie> OnButtonLikeClick;

    //public void Init(MovieCards like)
    //{
    //    movieTitle.text = like.movieTitle;
    //    this.like = like;

    //    //btnFavorit.onClick.AddListener(ButtonFavClick);
    //    //btnLike.onClick.AddListener(ButtonLikeClick);
    //}

    //public void ButtonFavClick()
    //{
    //    OnButtonFavorClick?.Invoke(movie);
    //}

    //public void ButtonLikeClick()
    //{
    //    OnButtonLikeClick?.Invoke(movie);
    //}
}
