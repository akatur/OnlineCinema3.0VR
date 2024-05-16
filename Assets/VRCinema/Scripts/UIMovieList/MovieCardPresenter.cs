using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MovieCardPresenter : MonoBehaviour
{

    [SerializeField] private Button btnPosterMovie;
    [SerializeField] private Button btnFavorit;
    [SerializeField] private Button btnLike;

    [SerializeField] private Button btnDeleteLike;
    [SerializeField] private Button btnDeleteFavorit;
    [SerializeField] private Button btnDeleteWatch;

    [SerializeField] private Button btnToPanoramCard;
    [SerializeField] private Button btnToBack;

    public TMP_Text movieTitle;
    public TMP_Text movieGenre;
    public TMP_Text rating;
    public TMP_Text duration;
    public TMP_Text realeaseYear;
    public TMP_Text movieDiscription;
    public Image PosterMovie;

    public string urlPhotoName;

    public MovieCards movie;
    public event Action <MovieCardPresenter> OnButtonFavorClick;
    public event Action <MovieCardPresenter> OnButtonLikeClick;
    public event Action <MovieCardPresenter> OnButtonWatchClick;

    public event Action <MovieCardPresenter> OnButtonToPanoramClick;
    public event Action <MovieCardPresenter> OnButtonDeleteLikeClick;
    public event Action <MovieCardPresenter> OnButtonDeleteFavorClick;
    public event Action <MovieCardPresenter> OnButtonDeleteWatchClick;

    private void Start()
    {
        if (btnToPanoramCard != null)
        {
            btnToPanoramCard.onClick.AddListener(ButtonToPanoramClick);
        }
        if (btnToBack != null)
        {
            btnToBack.onClick.AddListener(ButtonToDeletePanoramClick);
        }
        if (btnLike != null)
        {
            btnLike.onClick.AddListener(ButtonLikeClick);
        }
        if (btnFavorit != null)
        {
            btnFavorit.onClick.AddListener(ButtonFavClick);
        }
        if (btnPosterMovie != null)
        {
            btnPosterMovie.onClick.AddListener(ButtonWatchClick);
        }
        if (btnDeleteLike != null)
        {
            btnDeleteLike.onClick.AddListener(ButtonDeleteLikeClick);
        }
        if (btnDeleteFavorit != null)
        {
            btnDeleteFavorit.onClick.AddListener(ButtonDeleteFavClick);
        }
        if (btnDeleteWatch != null)
        {
            btnDeleteWatch.onClick.AddListener(ButtonDeleteWatchClick);
        }
    }

    public void Init(MovieCards movie)
    {
        movieTitle.text = movie.movieTitle;

        if (movieGenre != null )
        {
            movieGenre.text = movie.genre;
        }

        if (rating != null)
        {
            rating.text = movie.rating;
        }

        if (duration != null)
        {
            duration.text = movie.duration;
        }
        if (realeaseYear != null)
        {
            realeaseYear.text = movie.release_year;
        }

        if (movieDiscription != null)
        {
            movieDiscription.text = movie.discription;
        }
        urlPhotoName = movie.urlPhotoName;
        this.movie = movie;

        Debug.Log("PosterImage"+urlPhotoName);
        StartCoroutine(CardsControllerModel.LoadImageFromURL(urlPhotoName, PosterMovie));
    }

    public void ButtonToDeletePanoramClick()
    {
        gameObject.SetActive(false);
        
    }
    public void ButtonToPanoramClick()
    {
        OnButtonToPanoramClick?.Invoke(this);
    }
    public void ButtonDeleteLikeClick()
    {
        OnButtonDeleteLikeClick?.Invoke(this);
        Destroy(this.gameObject);
    }
    public void ButtonDeletePanoram()
    {
        OnButtonDeleteLikeClick?.Invoke(this);
        Destroy(this.gameObject);
    }
    public void ButtonDeleteFavClick()
    {
        OnButtonDeleteFavorClick?.Invoke(this);
        Destroy(this.gameObject);
    }

    public void ButtonDeleteWatchClick()
    {
        OnButtonDeleteWatchClick?.Invoke(this);
        Destroy(this.gameObject);
    }

    public void ButtonFavClick()
    {
        OnButtonFavorClick?.Invoke(this);
    } 

    public void ButtonLikeClick()
    {
        OnButtonLikeClick?.Invoke(this);
    }

    public void ButtonWatchClick()
    {
        OnButtonWatchClick?.Invoke(this);
    }

}
