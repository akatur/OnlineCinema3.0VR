using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LikeControllerPresenter : MonoBehaviour
{


    [SerializeField] private LikesCardsPresenter btnCardLike;

    [SerializeField] private Transform PanelCardsLike;

    [SerializeField] private CardsControllerModel cardsControllerModel;


    void Start()
    {
        //foreach (var item in cardsControllerModel.LikeList)
        //{
        //    LikesCardsPresenter likeCard;
        //    likeCard = Instantiate(btnCardLike, Vector3.zero, Quaternion.identity, PanelCardsLike);
        //    likeCard.Init(item);
        //}
    }

    
}
