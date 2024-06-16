using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static CardControllerPresenter;

public class CardControllerPresenter : MonoBehaviour
{
    [SerializeField] private List<MovieCardPresenter> cardListMovies = new List<MovieCardPresenter>();
    [SerializeField] private List<MovieCardPresenter> cardListLikes = new List<MovieCardPresenter>();
    [SerializeField] private List<MovieCardPresenter> cardListFav = new List<MovieCardPresenter>();
    [SerializeField] private List<MovieCardPresenter> cardListWatch = new List<MovieCardPresenter>();

    [SerializeField] private List<MovieCardPresenter> cardListCommentMovies = new List<MovieCardPresenter>();
    [Header("Prefabs cards")]
    [SerializeField] private MovieCardPresenter btnCard;
    [SerializeField] private MovieCardPresenter btnCardFav;
    [SerializeField] private MovieCardPresenter movieCardPresenter1;

    //
    //[SerializeField] private MovieCardPresenter commentModel;
    [SerializeField] private MovieCardPresenter cardComment;

    [Header("Buttons back")]
    [SerializeField] private GameObject toNextPageComment;
    [SerializeField] private GameObject toBackPageComment;

    [SerializeField] private UnityEngine.UI.Button btnNextPageComment;
    [SerializeField] private UnityEngine.UI.Button btnBackPageComment;

    //
    [SerializeField] private GameObject toNextPage;
    [SerializeField] private GameObject toBackPage;

    [SerializeField] private UnityEngine.UI.Button btnNextPage;
    [SerializeField] private UnityEngine.UI.Button btnBackPage;

    [SerializeField] private UnityEngine.UI.Button btnLikes;

    [SerializeField] private GameObject[] btnGen;

    [SerializeField] private CardsControllerModel cardsControllerModel;
        
    [SerializeField] private Transform PanelCards;
    [SerializeField] private Transform PanelCardsFav;
    //likes
    [SerializeField] private MovieCardPresenter btnCardLike;
    [SerializeField] private Transform PanelCardsLike;
    //panoram
    [SerializeField] private MovieCardPresenter windowPanoram;
    //[SerializeField] private CommentPresenter commentPresenter;
    [SerializeField] private Transform PanelPanoram;

    [Header("Panel")]
    [SerializeField] private GameObject UIPanoram;
    [SerializeField] private GameObject UIfill;
    [SerializeField] private GameObject UIScroll;
    [SerializeField] private Transform UIScrollComment;
    //watched
    [SerializeField] private MovieCardPresenter btnCardWatched;
    [SerializeField] private Transform PanelCardsWatched;

    [SerializeField] private TMP_InputField InputComment;
    [SerializeField] private UnityEngine.UI.Button btnSendMessage;
    public const int CardsMoviePerPage = 9;
    public int currentPageMovie = 0;

    public const int CardsMoviePerPageComment = 9;
    public int currentPageMovieComment = 0;


    public string movdd;

    private void Start()
    {
        windowPanoram.OnButtonToCommentClick += (x) => { LoadingComment(); };

        if (btnSendMessage != null)
        {
            btnSendMessage.onClick.AddListener(AddComments);
        }


        LoadingCards();
        cardsControllerModel.OnInsertLikes += InstCardsLikes;
        cardsControllerModel.OnInsertFav += InstCardsFav;
        cardsControllerModel.OnInsertWatch += InstCardsWatch;
        cardsControllerModel.OnInsertAllMovies += LoadingCards;
        cardsControllerModel.OnInsertToPanoram += InstCardsToPanoram;
        cardsControllerModel.OnInsertComment += LoadingComment;

        btnNextPage.onClick.AddListener(LoadNextPage);
        btnBackPage.onClick.AddListener(LoadPreviousPage);

        btnNextPageComment.onClick.AddListener(LoadNextPageComment);
        btnBackPageComment.onClick.AddListener(LoadPreviousPageComment);

        UpdateNavigationButtons();
        UpdateNavigationButtons();
    }


    public float scaleZ = 0.001f;

    public void LoadingCards()
    {
        foreach (var item in cardListMovies)
        {
            Destroy(item.gameObject);
        }
        cardListMovies.Clear();

        int startIndex = currentPageMovie * CardsMoviePerPage;
        int endIndex = Mathf.Min(startIndex + CardsMoviePerPage, cardsControllerModel.MovieList.Count);

        for (int i = startIndex; i < endIndex; i++)
        {
            var item = cardsControllerModel.MovieList[i];
            Vector3 position = new Vector3(0, 0, 0);
            MovieCardPresenter movieCard = Instantiate(btnCard, position, Quaternion.identity, PanelCards).GetComponent<MovieCardPresenter>();

            ScaleOnStart scaleOnStart = movieCard.gameObject.AddComponent<ScaleOnStart>();
            scaleOnStart.scaleZ = scaleZ;

            movieCard.Init(item);
            cardListMovies.Add(movieCard);
            movieCard.OnButtonFavorClick += AddToFavorites;
            movieCard.OnButtonLikeClick += AddToLikes;
            movieCard.OnButtonWatchClick += AddTWatched;
            movieCard.OnButtonWatchClick += PlayMovies;
            movieCard.OnButtonToPanoramClick += SelectPanoram;
            movieCard.OnButtonToCommentClick += SelectCommentCard;
        }
        UpdateNavigationButtons();
    }

    public void LoadNextPage()
    {
        if ((currentPageMovie + 1) * CardsMoviePerPage < cardsControllerModel.MovieList.Count)
        {
            currentPageMovie++;
            LoadingCards();
        }
    }

    public void LoadPreviousPage()
    {
        if (currentPageMovie > 0)
        {
            currentPageMovie--;
            LoadingCards();
        }
    }

    private void UpdateNavigationButtons()
    {
        toNextPage.SetActive((currentPageMovie + 1) * CardsMoviePerPage < cardsControllerModel.MovieList.Count);
        toBackPage.SetActive(currentPageMovie > 0);
    }

    MovieCardPresenter currentMovie;


    public void LoadingComment()
    {
        //SelectCommentCard(currentMovie);


        cardsControllerModel.GetCommentMovieForPanoram(currentMovie.movie, ()=> {
            foreach (var item in cardListCommentMovies)
            {
                Destroy(item.gameObject);
            }
            cardListCommentMovies.Clear();

            int startIndex = currentPageMovie * CardsMoviePerPageComment;
            int endIndex = Mathf.Min(startIndex + CardsMoviePerPageComment, cardsControllerModel.CommentsList.Count);

            for (int i = startIndex; i < endIndex; i++)
            {
                var item = cardsControllerModel.CommentsList[i];
                MovieCardPresenter movieCard = Instantiate(cardComment, Vector3.zero, Quaternion.identity, UIScrollComment).GetComponent<MovieCardPresenter>();
                movieCard.Init(item);
                cardListCommentMovies.Add(movieCard);
            }

            UpdateNavigationButtonsComment();
        } );
        
    }

    public void LoadNextPageComment()
    {
        if ((currentPageMovieComment + 1) * CardsMoviePerPageComment < cardsControllerModel.CommentsList.Count)
        {
            currentPageMovie++;
            LoadingComment();
        }
    }
    public void LoadPreviousPageComment()
    {
        if (currentPageMovieComment > 0)
        {
            currentPageMovieComment--;
            LoadingComment();
        }
    }
    private void UpdateNavigationButtonsComment()
    {
        toNextPageComment.SetActive((currentPageMovieComment + 1) * CardsMoviePerPageComment < cardsControllerModel.CommentsList.Count);
        toBackPageComment.SetActive(currentPageMovieComment > 0);
    }
    public void SetMovieId(string id)
    {
        movdd = id;
    }
    private void AddComments()
    {
        string comment = InputComment.text.Trim();
        string movieId = movdd;
        StartCoroutine(cardsControllerModel.AddComment(Convert.ToInt32(movieId), comment, LoadingComment));
    }

    private void SelectCommentCard(MovieCardPresenter movieCardPresenter)
    {
        currentMovie = movieCardPresenter;

        cardsControllerModel.GetCommentMovieForPanoram(movieCardPresenter.movie, LoadingComment);
    }

    public void InstCardsToPanoram()
    {
        foreach (var item in cardsControllerModel.ToPanoram)
        {
            windowPanoram.Init(item);

            movdd = item.movieId;
            Debug.Log(movdd);
        }
    }

   
    private void SelectPanoram(MovieCardPresenter movieCardPresenter)
    {
        cardsControllerModel.GetMovieForPanoram(movieCardPresenter.movie);
        UIPanoram.SetActive(true);
        UIfill.SetActive(true);
    }

    

    private void InstCardsLikes()
    {
        foreach (var item in cardListLikes)
        {
            Destroy(item.gameObject);
        }
        cardListLikes.Clear();
        foreach (var item in cardsControllerModel.LikeList)
        {
            MovieCardPresenter likeCard;
            likeCard = Instantiate(btnCardLike, Vector3.zero, Quaternion.identity, PanelCardsLike);
            likeCard.Init(item);
            cardListLikes.Add(likeCard);
            likeCard.OnButtonDeleteLikeClick += OnButtonClickDeleteLikes;
        }
    }

    private void InstCardsFav()
    {
        foreach (var item in cardListFav)
        {
            Destroy(item.gameObject);
        }
        cardListFav.Clear();
        foreach (var item in cardsControllerModel.FavouritesList)
        {
            MovieCardPresenter likeCard;
            likeCard = Instantiate(btnCardFav, Vector3.zero, Quaternion.identity, PanelCardsFav);
            likeCard.Init(item);
            cardListFav.Add(likeCard);
            likeCard.OnButtonDeleteFavorClick += OnButtonClickDeleteFav;
        }
    }

    private void InstCardsWatch()
    {
        foreach (var item in cardListWatch)
        {
            Destroy(item.gameObject);
        }
        cardListWatch.Clear();
        foreach (var item in cardsControllerModel.WatchedList)
        {
            MovieCardPresenter likeCard;
            likeCard = Instantiate(btnCardWatched, Vector3.zero, Quaternion.identity, PanelCardsWatched);
            likeCard.Init(item);
            cardListWatch.Add(likeCard);
            likeCard.OnButtonDeleteWatchClick += OnButtonClickDeleteWatch;
        }
    }
   



    private void AddToLikes(MovieCardPresenter movieCardPresenter)
    {
        cardsControllerModel.AddToLike(movieCardPresenter.movie);
    }
    private void AddToFavorites(MovieCardPresenter movieCardPresenter)
    {
        cardsControllerModel.AddToFavorites(movieCardPresenter.movie);
    }
    private void AddTWatched(MovieCardPresenter movieCardPresenter)
    {
        cardsControllerModel.AddTWatched(movieCardPresenter.movie);
    }
    private void PlayMovies(MovieCardPresenter movieCardPresenter)
    {
        cardsControllerModel.PlayMovie(movieCardPresenter.movie);
    }
    private void OnButtonClickDeleteLikes(MovieCardPresenter movieCardPresenter)
    {
        cardsControllerModel.OnButtonClickDeleteLike(movieCardPresenter.movie);
        cardListLikes.Remove(movieCardPresenter);
    }
    private void OnButtonClickDeleteFav(MovieCardPresenter movieCardPresenter)
    {
        cardsControllerModel.OnButtonClickDeleteFav(movieCardPresenter.movie);
        cardListFav.Remove(movieCardPresenter);
    }
    private void OnButtonClickDeleteWatch(MovieCardPresenter movieCardPresenter)
    {
        cardsControllerModel.OnButtonClickDeleteWatch(movieCardPresenter.movie);
        cardListWatch.Remove(movieCardPresenter);
    }
}