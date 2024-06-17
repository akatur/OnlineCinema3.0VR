using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    [SerializeField] private GameObject UIMenu;
    [SerializeField] private GameObject UISettings;

    [SerializeField] private Button btnSettings;
    [SerializeField] private Button btnNext;
    [SerializeField] private Button btnBack;

    //private GameObject activeWindow = null;
    [SerializeField] private Button btnWxitFromAuth;


    private void Awake()
    {
        btnSettings.onClick.AddListener(StateWindowUISettings);
        btnNext.onClick.AddListener(StateWindowUIMenu);
        btnBack.onClick.AddListener(StateWindowCloseUISettings);
        btnBack.onClick.AddListener(StateWindowCloseUISettings);

    }

    private void StateWindowUISettings()
    {
        UIMenu.SetActive(false);
        UISettings.SetActive(true);
    }

    private void StateWindowUIMenu()
    {
        UIMenu.SetActive(false);
        UISettings.SetActive(false);
    }

    private void StateWindowCloseUISettings()
    {
        UIMenu.SetActive(true);
        UISettings.SetActive(false);
    }

}

