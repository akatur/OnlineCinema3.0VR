using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardControllerPresenter : MonoBehaviour
{
    [SerializeField] private List<MovieCardPresenter> cardListMovies = new List<MovieCardPresenter>();
    [SerializeField] private List<MovieCardPresenter> cardListLikes = new List<MovieCardPresenter>();
    [SerializeField] private List<MovieCardPresenter> cardListFav = new List<MovieCardPresenter>();
    [SerializeField] private List<MovieCardPresenter> cardListWatch = new List<MovieCardPresenter>();

    [SerializeField] private MovieCardPresenter btnCard;
    [SerializeField] private MovieCardPresenter btnCardFav;

    //[SerializeField] private GameObject[] btnFavorit;
    //[SerializeField] private GameObject[] btnLike;

    [SerializeField] private Button btnLikes;

    [SerializeField] private GameObject[] btnGen;

    [SerializeField] private CardsControllerModel cardsControllerModel;
    
    [SerializeField] private Transform PanelCards;
    [SerializeField] private Transform PanelCardsFav;
    //likes
    [SerializeField] private MovieCardPresenter btnCardLike;
    [SerializeField] private Transform PanelCardsLike;
    //panoram
    [SerializeField] private MovieCardPresenter windowPanoram;
    [SerializeField] private Transform PanelPanoram;
    [SerializeField] private GameObject UIPanoram;
    [SerializeField] private GameObject UIfill;
    [SerializeField] private GameObject UIScroll;
    //watched
    [SerializeField] private MovieCardPresenter btnCardWatched;
    [SerializeField] private Transform PanelCardsWatched;


    private void Start()
    {
        LoadingCards();
        cardsControllerModel.OnInsertLikes += InstCardsLikes;
        cardsControllerModel.OnInsertFav += InstCardsFav;
        cardsControllerModel.OnInsertWatch += InstCardsWatch;
        cardsControllerModel.OnInsertAllMovies += LoadingCards;
        cardsControllerModel.OnInsertToPanoram += InstCardsToPanoram;
    }

    public void LoadingCards()
    {
        foreach (var item in cardListMovies)
        {
            Destroy(item.gameObject);
        }
        cardListMovies.Clear();
        foreach (var item in cardsControllerModel.MovieList)
        {
            MovieCardPresenter movieCard;
            
            movieCard = Instantiate(btnCard, Vector3.zero, Quaternion.identity, PanelCards);
            movieCard.Init(item);
            cardListMovies.Add(movieCard);
            movieCard.OnButtonFavorClick += AddToFavorites;
            movieCard.OnButtonLikeClick += AddToLikes;
            movieCard.OnButtonWatchClick += AddTWatched;
            movieCard.OnButtonWatchClick += PlayMovies;
            movieCard.OnButtonToPanoramClick += SelectPanoram;
        }
    }


    private void InstCardsToPanoram()
    {
        foreach (var item in cardsControllerModel.ToPanoram)
        {
            windowPanoram.Init(item);
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