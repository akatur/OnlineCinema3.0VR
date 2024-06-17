using System;
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


    [SerializeField] private Button btnWxitFromAuth;

    void Start()
    {

        if (btnWxitFromAuth != null)
        {
            btnWxitFromAuth.onClick.AddListener(ExtiFromaAcaunt);
        }

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

            
        }
    }

    private void ExtiFromaAcaunt()
    {
        UserInfo.currentLogin = null;
        UserInfo.role_id = null;
        UserInfo.user_id = null;
        UserInfo.currentName = null;
        UserInfo.currentPassword = null;
        UserInfo.role_name = null;
        Debug.Log("Выход из акккаунта успешен");



    }



}
