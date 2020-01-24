using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class DownloadAndPlayVideoAutomatically : MonoBehaviour
{
    int numberOfMarkers; // Associated with Page
    bool dataFullyLoaded;
    bool sessionStarted;
    //bool dataDownloaded;
    public GameObject AfterMarkerPanel;
    public RawImage pageRawImage;
    public GameObject boundingFrame;
    //BooksDatabase databaseManager;
    string videoURL = "Big_Buck_Bunny_360_10s_5MB.mp4";
    VideoFileDownloader videoDownloaderScript;
    //VideoClipPlayer videoPlayerScript;
    string localVideoPath = "";
    VideoClipPlayer videoPlayerScript;



    // Use this for initialization
    void Start()
    {
        videoDownloaderScript = GetComponent<VideoFileDownloader>();
        videoPlayerScript = GetComponent<VideoClipPlayer>();
        dataFullyLoaded = false;
        sessionStarted = false;
    }

    public void exitCurrentSession(){
        dataFullyLoaded = false;
    	sessionStarted = false;
        videoPlayerScript.stopVideoPlayback();
        AfterMarkerPanel.SetActive(false);

    }

    
    public void downloadAndPlay()
    {
        Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
        //first, we check whether audio is playing
        //if that's the case, we should not interrupt
        Debug.Log("sessionStarted: " + sessionStarted);
        Debug.Log("dataFullyLoaded: " + dataFullyLoaded);
        videoPlayerScript.DEBUG_LOG_FROM_BOOK_SESSIO_MANAGER();
        if (sessionStarted == false)
        {

            sessionStarted = true;
            dataFullyLoaded = false;

            StartCoroutine(downloadData());

            AfterMarkerPanel.gameObject.SetActive(false);
            
            StartCoroutine(runSession());
        }else {
            Debug.LogError("Another Session In Progress");
        }
    }

    IEnumerator downloadData()
    {
        videoDownloaderScript.downloadVideoUponTargetRecognition(videoURL);
        yield return new WaitUntil(() => videoDownloaderScript.videoClipDownloaded() == true || videoDownloaderScript.fileExistsInMemory() == true);
        //we should not replace it if the file already exists
        if (videoDownloaderScript.videoClipDownloaded() == true) {
            localVideoPath = videoDownloaderScript.getVideoPathAfterDownloading();
        }
        dataFullyLoaded = true;
        yield return null;
    }


    IEnumerator runSession()
    {
        //wait until the audio and texture files are downloaded and loaded into the Pages List
        yield return new WaitUntil(() => dataFullyLoaded == true);
        Debug.Log("Data Fully Loaded");
        videoPlayerScript.setVideoCanvasToFront();

        AfterMarkerPanel.gameObject.SetActive(true);
        videoPlayerScript.activateLoadingPanel();
        boundingFrame.SetActive(true);

        videoPlayerScript.playVideoClip(localVideoPath);

        yield return new WaitUntil(() => videoPlayerScript.isVideoClipActive() == false);
        boundingFrame.SetActive(false);

        AfterMarkerPanel.gameObject.SetActive(false);
        sessionStarted = false;
        dataFullyLoaded = false;
        yield return null;
    }


    public string getPersistentPath()
    {
        string markersPath = "";

        #if UNITY_ANDROID && !UNITY_EDITOR
            markersPath = Application.streamingAssetsPath;
        #elif UNITY_IPHONE && !UNITY_EDITOR
            markersPath = Application.streamingAssetsPath;
        #else
            markersPath = Application.streamingAssetsPath;
        #endif
        return markersPath;
    }


//    void readMarkerFromStreamingAssets(int pageIndex)
//    {
//        string markerUrl = getPersistentPath() + "/" + databaseManager.getMarkerPath(pageIndex);//path of the marker for a particualr book and index
//#if UNITY_EDITOR
//        markerUrl = "file://" + markerUrl;
//#endif
//        Debug.LogWarning("ARManager: marker url: " + "'" + markerUrl + "'");
//        StartCoroutine(LoadMarkerFromStreamingAssetsFolder(markerUrl, pageIndex));
//    }



//    IEnumerator LoadMarkerFromStreamingAssetsFolder(string markerUrl, int pageIndex)
//    {
//        Texture2D markerTexture = new Texture2D(2, 2);

//#if UNITY_ANDROID && !UNITY_EDITOR
//            using (WWW www = new WWW(markerUrl))
//            { 
//                yield return www;
//                www.LoadImageIntoTexture(markerTexture);
//                //Sprite s = Sprite.Create(markerTexture, new Rect(0.0f, 0.0f, markerTexture.width, markerTexture.height), new Vector2(0.5f, 0.5f), 100.0f); d
//            }

//#elif UNITY_IPHONE && !UNITY_EDITOR
//            byte[] bytes = System.IO.File.ReadAllBytes(markerUrl);//FullName gives us the full file name and path
//            markerTexture.LoadImage(bytes);
//            //Sprite s = Sprite.Create(markerTexture, new Rect(0.0f, 0.0f, markerTexture.width, markerTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
//            yield return null;
//#else
//        using (WWW www = new WWW(markerUrl))
//        {
//            yield return www;
//            www.LoadImageIntoTexture(markerTexture);
//            //GetComponent<Renderer>().material.mainTexture = tex;
//            //Sprite s = Sprite.Create(markerTexture, new Rect(0.0f, 0.0f, markerTexture.width, markerTexture.height), new Vector2(0.5f, 0.5f), 100.0f);

//        }

//#endif

//        ////now, we add it to our ArrayList
//        //loadingSlider.value = MarkersAndPhotos.Count * 1.0f / numberOfMarkers;
//        //if (MarkersAndPhotos.Count == numberOfMarkers)
//        //{
//        //    LoadingScreen.SetActive(false);
//        //}
//        pageMarkers[pageIndex] = markerTexture;
//        markersLoadStatus[pageIndex] = true;
//        yield return null;
//    }
}
