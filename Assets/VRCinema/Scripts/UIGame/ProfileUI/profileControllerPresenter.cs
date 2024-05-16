using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class profileControllerPresenter : MonoBehaviour
{

    [SerializeField] private profilePresenter cardProfile;
    [SerializeField] private Transform PanelCardsProfile;

    [SerializeField] private List<profilePresenter> cardListProfile = new List<profilePresenter>();

    [SerializeField] private ProfileModel profileModel;
    void Start()
    {
        profileModel.OnInsertProfile += LoadingProfile;
    }

    public void LoadingProfile()
    {
        foreach (var item in cardListProfile)
        {
            Destroy(item.gameObject);
        }
        cardListProfile.Clear();
        foreach (var item in profileModel.ProfileList)
        {
            profilePresenter profile;
            profile = Instantiate(cardProfile, Vector3.zero, Quaternion.identity, PanelCardsProfile);
            profile.Init(item);
            cardListProfile.Add(profile);
            profile.transform.localPosition = Vector3.zero;

            //profile.Add(profile);
            //profile.OnButtonFavorClick += AddToFavorites;
            //profile.OnButtonLikeClick += AddToLikes;
            //profile.OnButtonWatchClick += AddTWatched;
            //profile.OnButtonWatchClick += PlayMovies;
        }
    }
}
