using UnityEngine.Video;
using UnityEngine;

public class videocontroll : MonoBehaviour
{
    private UnityEngine.Video.VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<UnityEngine.Video.VideoPlayer>();
    }

    void Update()
    {

        //if(Input.Get) { }


        if (Input.GetKeyDown(KeyCode.F))
        {

            if (videoPlayer.isPlaying)
            {
                videoPlayer.Pause();
            }
            else
            {
                videoPlayer.Play();
            }
        }

        //if (Input.GetKeyDown(KeyCode.M))
        //{

        //    if (videoPlayer.isPlaying)
        //    {
        //        videoPlayer.Mute;
        //    }
        //    else
        //    {
        //        videoPlayer.Play();
        //    }
        //}
    }
}
