using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VideoController : MonoBehaviour
    //IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public GameObject videoPlayer;
    private VideoPlayer videoPlayerScript;

    void Start()
    {
        //videoPlayerScript = videoPlayer.GetComponent<MyVideoPlayer>();

        //// Добавляем компонент EventTrigger к объекту knob
        //EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();

        //// Настраиваем событие PointerDown
        //EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
        //pointerDownEntry.eventID = EventTriggerType.PointerDown;
        //pointerDownEntry.callback.AddListener((eventData) => { OnPointerDown((PointerEventData)eventData); });
        //eventTrigger.triggers.Add(pointerDownEntry);

        //// Настраиваем событие PointerUp
        //EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry();
        //pointerUpEntry.eventID = EventTriggerType.PointerUp;
        //pointerUpEntry.callback.AddListener((eventData) => { OnPointerUp((PointerEventData)eventData); });
        //eventTrigger.triggers.Add(pointerUpEntry);

        //// Настраиваем событие Drag
        //EventTrigger.Entry dragEntry = new EventTrigger.Entry();
        //dragEntry.eventID = EventTriggerType.Drag;
        //dragEntry.callback.AddListener((eventData) => { OnDrag((PointerEventData)eventData); });
        //eventTrigger.triggers.Add(dragEntry);
    }

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    videoPlayerScript.KnobOnPressDown();
    //}

    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    videoPlayerScript.KnobOnRelease();
    //}

    //public void OnDrag(PointerEventData eventData)
    //{
    //    videoPlayerScript.KnobOnDrag();
    //}
}






//if (Input.GetKeyDown(KeyCode.F))
//{

//    if (videoPlayer.isPlaying)
//    {
//        videoPlayer.Pause();
//    }
//    else
//    {
//        videoPlayer.Play();
//    }
//}








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