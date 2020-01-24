using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using System;
public class VideoFileDownloader : MonoBehaviour {
    private string URL = "http://slingview.com/videofactbook/video.mp4";
    //here is a link to the All Audio for See Hear Read - the files are on slingview.com/seehearread
    private string slingviewURL = "https://test-videos.co.uk/vids/bigbuckbunny/mp4/h264/360/";
    public Slider downloadSlider;
    public GameObject downloaderPanel;
    string filePath;
    UnityWebRequest downloader;
    bool isDownloadComplete;
    bool fileExists;
    //bool audioStarted;
    //VideoPlayer videoPlayer;
    VideoClip videoClip;
    public float videoSize = 5f;
    UnityWebRequestAsyncOperation operation;
    //public GameObject middlePanel;
    //public RawImage rawImage;
    //public GameObject VideoLoadingPanel;
    // Use this for initialization
    void Start()
    {
        downloader = null;
        fileExists = false;
        isDownloadComplete = false;
        //videoSizes = new float[contentSize.videoSizesInMegabytes.Count];
    }

    public void setVideoSize(int videoIndex) {
        Debug.Log("VideoFileDownloader_scriptableobject_video_size:" + videoSize);
    }

    void InitializeData(string url, string pathInCache)
    {
        filePath = System.IO.Path.Combine(Application.persistentDataPath, pathInCache);
        string directory = System.IO.Path.GetDirectoryName(filePath);
        if (!System.IO.Directory.Exists(directory))
        {
            System.IO.Directory.CreateDirectory(directory);
        }
        if (System.IO.File.Exists(filePath))
        {
            Debug.Log("file exists");
            isDownloadComplete = true;
            fileExists = true;
            //prepareVideoAfterDownloading();

        }
        else
        {
            //extension is .mp4
            //we are given a name
            //and now we build a URL out of it(slingview path )
            URL = url;
            //middlePanel.SetActive(false);
            // create downloader instance
            //if(downloader!=null)
            //    downloader.Dispose();
            downloader = null;

            isDownloadComplete = false;
            fileExists = false;
            //videoStarted = false;
            videoClip = null;

            Debug.Log("file path: " + filePath);
            //Debug.Log("get full path: "+ System.IO.Path.GetFullPath(filePath));
            //Debug.Log("get  path root: " + System.IO.Path.GetPathRoot(filePath));
            //Debug.Log(" GetDirectoryName: " + System.IO.Path.GetDirectoryName(filePath));
            //Debug.Log("get full path: " + System.IO.Path.get(filePath));

            //System.IO.Directory.Exists()
        }

    }


   

    public bool videoClipDownloaded()
    {
        return isDownloadComplete;
    }

    public bool fileExistsInMemory() {
        return fileExists;
    }

    public VideoClip getLoadedVideoClip()
    {
        return videoClip;
    }

    void Update()
    {

        if (downloader != null && !downloader.isDone)
        {
            downloadSlider.value = (downloader.downloadedBytes/videoSize) * 100;
            //downloader.dow
            //slider.value = (www.downloadedBytes / 35980732f) * 100;
            Debug.Log("progress: "+operation.progress);
        }
    }


    public void downloadVideoUponTargetRecognition(string nameWithExtension)
    {
        string url = slingviewURL + nameWithExtension; //set audio URL for downloading
        Debug.Log("Video URL: " + url);
        if (URL == url)
        {
            //rawImage.gameObject.SetActive (false);
            //do nothing
        }
        else
        {
            InitializeData(url, nameWithExtension);
            // start downloading if file doesn't exist
            if (!fileExists)
            {
                downloadSlider.value = 0f;
                //enable download panel
                downloaderPanel.SetActive(true);
                //Debug.LogError ("Video Does not exist");
                StartCoroutine(DownloadVideo());
                Debug.LogWarning("file path in downloading: " + filePath);
            }
            else {
                Debug.LogWarning("file already exists");

            }

        }
    }

    //string getBytesFromURL() {
    //    //using (UnityWebRequest uwr = UnityWebRequest.GetTexture(uri))
    //    //{
    //    //    uwr.method = "HEAD";
    //    //    yield return uwr.SendWebRequest();

    //    //    Debug.LogError(uwr.GetResponseHeader("Content-Length"));
    //    //    Debug.LogError(uwr.downloadedBytes);
    //    //}
    //    UnityWebRequest webRequest = UnityWebRequest.Head(URL);
    //    webRequest.SendWebRequest();
    //    while (!webRequest.isDone)
    //    {
    //        yield return null;
    //    }
    //    return  webRequest.GetResponseHeader("Content-Length");
    //}

    IEnumerator DownloadVideo()
    {
        Debug.Log("VideoFileDownloader_DownloadVideo_URL: " + URL);

        //UnityWebRequest webRequest = UnityWebRequest.Head(URL);
        //webRequest.SendWebRequest();
        //while (!webRequest.isDone)
        //{
        //    yield return null;
        //}
        //int bytes = Int32.Parse(webRequest.GetResponseHeader("Content-Length"));
        //Debug.LogError("Video size in KB: " + webRequest.GetResponseHeader("Content-Length"));

        downloader = UnityWebRequest.Get(URL);
        //downloader.chunkedTransfer = false;
        yield return (operation=downloader.SendWebRequest());

        if (downloader.isNetworkError || downloader.isHttpError)
        {
            Debug.Log(downloader.error);
        }
        else
        {
            downloaderPanel.SetActive(false);
            isDownloadComplete = true;
            Debug.Log("VideoFileDownloader_video_size " + downloader.downloadedBytes);

            Debug.Log("Download completed successfully");
            File.WriteAllBytes(Path.Combine(Application.persistentDataPath, filePath), downloader.downloadHandler.data);
        }
    }


    public string getVideoPathAfterDownloading()
    {
        #if !UNITY_EDITOR && UNITY_ANDROID
        return filePath;
        #endif
        return "file://" + filePath;
    }

}
