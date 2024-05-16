using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class VideoPlayer : MonoBehaviour
{
    [SerializeField] private Button btnPlay;
    [SerializeField] private Button btnPause;
    [SerializeField] private Slider progressBar;
    [SerializeField] private Slider knob;
    [SerializeField] private UnityEngine.Video.VideoPlayer videoPlayer;

    private bool isDraggingKnob = false;

    void Start()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<UnityEngine.Video.VideoPlayer>();
        }

        btnPause.gameObject.SetActive(false);
        btnPlay.gameObject.SetActive(true);
        progressBar.value = 0f;

        progressBar.onValueChanged.AddListener(OnProgressBarChanged);
        knob.onValueChanged.AddListener(OnKnobValueChanged);

        btnPause.onClick.AddListener(OnPauseButtonClicked);
        btnPlay.onClick.AddListener(OnPlayButtonClicked);

        AddEventTrigger(knob, EventTriggerType.PointerDown, OnKnobPointerDown);
        AddEventTrigger(knob, EventTriggerType.PointerUp, OnKnobPointerUp);
    }

    void Update()
    {
        if (videoPlayer.isPlaying && !isDraggingKnob)
        {
            float progress = (float)videoPlayer.time / (float)videoPlayer.length;
            progressBar.SetValueWithoutNotify(progress);
            knob.SetValueWithoutNotify(progress);
        }

        btnPause.gameObject.SetActive(videoPlayer.isPlaying);
        btnPlay.gameObject.SetActive(!videoPlayer.isPlaying);
    }

    public void OnPauseButtonClicked()
    {
        VideoStop();
    }

    public void OnPlayButtonClicked()
    {
        VideoPlay();
    }

    public void VideoStop()
    {
        videoPlayer.Pause();
        btnPause.gameObject.SetActive(false);
        btnPlay.gameObject.SetActive(true);
    }

    public void VideoPlay()
    {
        videoPlayer.Play();
        btnPause.gameObject.SetActive(true);
        btnPlay.gameObject.SetActive(false);
    }

    void OnProgressBarChanged(float value)
    {
        if (!isDraggingKnob)
        {
            double time = value * videoPlayer.length;
            videoPlayer.time = time;
        }
    }

    void OnKnobValueChanged(float value)
    {
        if (!isDraggingKnob)
        {
            progressBar.SetValueWithoutNotify(value);
            double time = value * videoPlayer.length;
            videoPlayer.time = time;
        }
    }

    public void OnKnobPointerDown(BaseEventData data)
    {
        isDraggingKnob = true;
        videoPlayer.Pause();
    }

    public void OnKnobPointerUp(BaseEventData data)
    {
        isDraggingKnob = false;
        double time = knob.value * videoPlayer.length;
        videoPlayer.time = time;
        videoPlayer.Play();
    }

    private void AddEventTrigger(Slider slider, EventTriggerType eventType, UnityEngine.Events.UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = slider.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = slider.gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventType };
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }
}