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

    [SerializeField] private TMP_Text nickNames;
    [SerializeField] private TMP_InputField nickName;
    [SerializeField] private TMP_InputField movie_id;
    [SerializeField] private TMP_InputField comment;
    [SerializeField] private TMP_Text created_at;

    [SerializeField] private TMP_InputField InputComment;
    [SerializeField] private Button btnSendMessage;
    [SerializeField] private CardsControllerModel cardsControllerModel;
    [SerializeField] private CardControllerPresenter cardsControllerPresenter;
    public string userPhoto;
    public Image PhotoUser;

    public MovieCards movie;
    public event Action<MovieCardPresenter> OnButtonFavorClick;
    public event Action<MovieCardPresenter> OnButtonLikeClick;
    public event Action<MovieCardPresenter> OnButtonWatchClick;
    public event Action<MovieCardPresenter> OnButtonToPanoramClick;
    public event Action<MovieCardPresenter> OnButtonToCommentClick;

    public event Action<MovieCardPresenter> OnButtonToAddCommentClick;


    public event Action<MovieCardPresenter> OnButtonDeleteLikeClick;
    public event Action<MovieCardPresenter> OnButtonDeleteFavorClick;
    public event Action<MovieCardPresenter> OnButtonDeleteWatchClick;

    private void Start()
    {
        cardsControllerModel = new CardsControllerModel();
        cardsControllerPresenter = new CardControllerPresenter();


        if (btnSendMessage != null)
        {
            btnSendMessage.onClick.AddListener(ButtonAddCommentClick);
        }


        

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
        if (nickName != null)
        {
            nickName.text = movie.username;
        }
        if (comment != null)
        {
            comment.text = movie.comment;
        }
        if (created_at != null)
        {
            created_at.text = movie.created_at;
        }
        if (movieTitle != null)
        {
            movieTitle.text = movie.movieTitle;
        }
        if (movieGenre != null)
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
        if (urlPhotoName != null)
        {
            urlPhotoName = movie.urlPhotoName;
        }
        if (userPhoto != null)
        {
            userPhoto = movie.userPhoto;
        }

        this.movie = movie;
        StartCoroutine(CardsControllerModel.LoadImageFromURL(urlPhotoName, PosterMovie));
    }
    public void ButtonAddCommentClick()
    {
        OnButtonToAddCommentClick?.Invoke(this);
        string comment = InputComment.text.Trim();
        //cardsControllerModel.AddToComment(comment);
        StartCoroutine(cardsControllerModel.AddComment(Convert.ToInt32(movie.movieId), comment, ButtonToPanoramClick));
        OnButtonToPanoramClick?.Invoke(this);
    }
    
    public void ButtonToDeletePanoramClick()
    {
        Debug.Log("delwin");
        this.gameObject.SetActive(false);
    }

    public void ButtonToPanoramClick()
    {
        OnButtonToPanoramClick?.Invoke(this);
        OnButtonToCommentClick?.Invoke(this);
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
