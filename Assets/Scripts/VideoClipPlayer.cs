using UnityEngine;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class VideoClipPlayer : MonoBehaviour
{

    //const string FILE_URL_MP4 = "http://www.sample-videos.com/video/mp4/720/big_buck_bunny_720p_30mb.mp4";
    //const string FILE_URL_FLV = "http://www.sample-videos.com/video/flv/720/big_buck_bunny_720p_30mb.flv";
    private string URL = "http://slingview.com/wowbooks/babypuppies.mp4";
    bool videoStarted;
    VideoPlayer videoPlayer = null;
    //public RenderTexture renderTexture;
    public Canvas mainUICanvas;
    public Canvas videoCanvas;
    public GameObject VideoLoadingPanel;
    bool isVideoPaused;
    bool isVideoActivated;
    float preparationTimer;

    // Use this for initialization
    void Start()
    {
        isVideoActivated = false;
        preparationTimer = 0f;
        isVideoPaused = false;
        VideoLoadingPanel.SetActive(false);

        videoPlayer = GetComponent<VideoPlayer>();
    }

    public void playVideoClip(string url)
    {
        URL = url; //set video URL for downloading
        // create downloader instance
        videoStarted = false;

        Debug.Log("video clip file path: " + URL);
        if (System.IO.File.Exists(URL))
        {
            Debug.Log("file exists");
        }
        playVideoFromURL();
    }

    public void setVideoCanvasToFront() {
        videoCanvas.sortingOrder = 1;

    }
    void clearRenderTexture() {
        videoPlayer.targetTexture.Release();
    }
    public void exitButton()
    {
        //Screen.sleepTimeout = (int)SleepTimeout.SystemSetting;

        VideoLoadingPanel.SetActive(false);
        //stops the video
        videoPlayer.Stop();


    }

    public void DEBUG_LOG_FROM_BOOK_SESSIO_MANAGER() {
        Debug.Log("videoStarted: " + videoStarted);
        Debug.Log("isVideoPaused: " + isVideoPaused);
        Debug.Log("isVideoActivated: " + isVideoActivated);


    }

    void Update()
    {
      
        //if we tap on the screen, we need to show the main menu canvas
        //or if it's already shown, we need to show the UI canvas on top of it
        if (isVideoActivated)
        {
            preparationTimer += Time.deltaTime;
            // Check if there is a touch
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (videoCanvas.sortingOrder == 1)
                {
                    //we pause the video as well
                    videoPlayer.Pause();
                    isVideoPaused = true;
                    videoCanvas.sortingOrder = -1;

                }
                else if (videoCanvas.sortingOrder == -1)
                {
                    if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                    {
                        bool isOverMainCanvasButton = true;
                        PointerEventData pointer = new PointerEventData(EventSystem.current);
                        pointer.position = Input.mousePosition;

                        List<RaycastResult> raycastResults = new List<RaycastResult>();
                        EventSystem.current.RaycastAll(pointer, raycastResults);

                        if (raycastResults.Count > 0)
                        {
                            foreach (var go in raycastResults)
                            {
                                Debug.Log(go.gameObject.name, go.gameObject);
                            }
                            if (raycastResults[0].gameObject.name == "VideoTexture")
                            {
                                isOverMainCanvasButton = false;
                            }
                        }

                        if (isOverMainCanvasButton == false)
                        {
                            videoCanvas.sortingOrder = 1;
                            videoPlayer.Play();
                            isVideoPaused = false;
                        }
                    }
                }
            }
        }
    }



   

    public void playVideoFromURL()
    {
        StartCoroutine(playVideo());
    }

    public void stopVideoPlayback(){
        if(videoPlayer!=null)
            videoPlayer.Stop();
        isVideoPaused = false;

    }

    public bool isVideoClipActive(){
        return isVideoActivated;
    }
    public void activateLoadingPanel()
    {
        VideoLoadingPanel.SetActive(true);
    }

    IEnumerator playVideo() { 
        isVideoPaused = false;
        isVideoActivated = true;
        VideoLoadingPanel.SetActive(true);

        //We want to play from url
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = URL;
        videoPlayer.Prepare();
        preparationTimer = 0f;

        yield return new WaitUntil(() => videoPlayer.isPrepared || preparationTimer >= 5.0f);


        Debug.Log("Done Preparing Video in: " + preparationTimer + "seconds");

        Debug.Log("Video URL: " + URL);
        //here, we prepared our video, now we switch to the video canvas

        //main canvas is at 0, so this makes the video canvas visible
        //yield return null;
        //videoCanvas.sortingOrder = 1;
        //Play Video
        videoPlayer.Play();
        VideoLoadingPanel.SetActive(false);

        Debug.Log("Playing Video");
        //while (videoPlayer.isPlaying)
        //{
        //  Debug.LogWarning("Video Time: " + Mathf.FloorToInt((float)videoPlayer.time));
        //  yield return null;
        //}

        yield return new WaitUntil(() => videoPlayer.isPlaying == false && isVideoPaused == false);
        //yield return new WaitForSeconds(0.3f);
        isVideoActivated = false;

        videoPlayer.Stop();
        clearRenderTexture();        //disable raw image
        //rawImage.gameObject.SetActive(false);
        videoCanvas.sortingOrder = -1; //show it behind the main canvas
        Debug.Log("Done Playing Video");
        Screen.sleepTimeout = (int)SleepTimeout.SystemSetting;

    }



}
