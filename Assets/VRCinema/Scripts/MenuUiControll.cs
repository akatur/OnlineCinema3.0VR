
using UnityEngine;

public class MenuUiControll : MonoBehaviour
{
    [SerializeField] private GameObject UISrollCards;
    [SerializeField] private GameObject TopTable;
    [SerializeField] private GameObject UIAddMovie;
    [SerializeField] private  GameObject UIProfile;
    [SerializeField] private  GameObject UIFav;
    [SerializeField] private  GameObject UILike;
    [SerializeField] private  GameObject UIWatch;

    [SerializeField] private  GameObject VideoPlayerController;
    [SerializeField] private  GameObject MovePlayer;

    [SerializeField] private  GameObject UIMenu;
    //[SerializeField] private  GameObject UISettings;
    [SerializeField] private movePlayer movePlayer;

    

    //[SerializeField] public  GameObject UIFull;

    public bool isOpenedOne;
    public bool isOpenedTwo;
    public bool isOpenedTree;
    public bool isOpenedFore;

    [SerializeField] private authModel authModel;

    void Start()
    {
        Cursor.visible = true;
       
        UISrollCards.SetActive(false);
        TopTable.SetActive(false);
        UIAddMovie.SetActive(false);

        VideoPlayerController.gameObject.SetActive(false);
    }

    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isOpenedOne = !isOpenedOne;

            if (isOpenedOne)
            {
                UISrollCards.SetActive(true);
                TopTable.SetActive(true);
                movePlayer.sensitivity = 0;
                Cursor.visible = true;
                
            }
            else
            {
                movePlayer.sensitivity = 2;
                Cursor.visible = false;
                

                UISrollCards.SetActive(false);
                TopTable.SetActive(false);
                UIProfile.SetActive(false);
                UIFav.SetActive(false);
                UILike.SetActive(false);
                UIWatch.SetActive(false);
            }
        }

        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    isOpenedTree = !isOpenedTree;

        //    if (isOpenedTree)
        //    {
        //        movePlayer.sensitivity = 0;
        //        Cursor.visible = false;
        //        UIMenu.SetActive(true);
        //        UISrollCards.SetActive(false);
        //        TopTable.SetActive(false);
        //        UIProfile.SetActive(false);
        //        UIFav.SetActive(false);
        //        UILike.SetActive(false);
        //        UIWatch.SetActive(false);
                

        //    }
        //    else
        //    {
        //        Cursor.visible = true;
        //        UIMenu.SetActive(false);
        //        //UISettings.SetActive(false);
        //        movePlayer.sensitivity = 2;
        //    }
        //}

        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            isOpenedFore = !isOpenedFore;

            if (isOpenedFore)
            {
                VideoPlayerController.gameObject.SetActive(true);
                movePlayer.sensitivity = 0;
                Cursor.visible = true;

            }
            else
            {
                VideoPlayerController.gameObject.SetActive(false);
                movePlayer.sensitivity = 2;
                Cursor.visible = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            if (UserInfo.currentLogin == "admin")
            {
                isOpenedTwo = !isOpenedTwo;
                if (isOpenedTwo)
                {
                    UIAddMovie.SetActive(true);
                    movePlayer.sensitivity = 0;
                    Cursor.visible = true;
                }
                else
                {
                    UIAddMovie.SetActive(false);
                    movePlayer.sensitivity = 2;
                    Cursor.visible = false;
                }
            }
            else
            {
                Debug.Log("Не админ");
            }
        }


    }
}
