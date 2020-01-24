using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayVideo : MonoBehaviour {

	//Raw Image to Show Video Images [Assign from the Editor]
	public RawImage image;
	//Video To Play [Assign from the Editor]
	//public VideoClip videoToPlay;

	private VideoPlayer videoPlayer;
	private VideoSource videoSource;

	//Audio
	//private AudioSource audioSource;

	void Start(){
		Application.runInBackground = true;
		videoPlayer = gameObject.GetComponent<VideoPlayer>();

	}
	// Use this for initialization
	public void StartVideo()
	{
		
		image.gameObject.SetActive (true);
		//Application.runInBackground = true;
		StartCoroutine(playVideo());
	}

	IEnumerator playVideo()
	{
		//Add VideoPlayer to the GameObject
		//videoPlayer = gameObject.AddComponent<VideoPlayer>();

		//Add AudioSource
		//audioSource = gameObject.AddComponent<AudioSource>();

		//Disable Play on Awake for both Video and Audio
		//videoPlayer.playOnAwake = true;
		//audioSource.playOnAwake = true;

		//We want to play from video clip not from url
		//videoPlayer.source = VideoSource.VideoClip;

		//Set Audio Output to AudioSource
		//videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

		//Assign the Audio from Video to AudioSource to be played
		//videoPlayer.EnableAudioTrack(0, true);
		//videoPlayer.SetTargetAudioSource(0, audioSource);
		//videoPlayer.waitForFirstFrame = false;
		//Set video To Play then prepare Audio to prevent Buffering
//		string url = "file://" + Application.streamingAssetsPath + "/" + "wildzebra.mp4";
//
//		#if !UNITY_EDITOR && UNITY_ANDROID
//		url = Application.streamingAssetsPath + "/" + "wildzebra.mp4";
//		#endif

		//url="https://www.quirksmode.org/html5/videos/big_buck_bunny.mp4";
		//url = "http://slingview.com/wowbooks/videos/babychicks.mp4";
		//url = "http://slingview.com/wowbooks/videos/wildlions.mp4";
		//We want to play from url
		//videoPlayer.source = VideoSource.Url;
		//videoPlayer.url = url;
		videoPlayer.Prepare();

		//Wait until video is prepared
		while (!videoPlayer.isPrepared)
		{
			Debug.Log("Preparing Video");
			yield return null;
		}
		/*
		//Wait until video is prepared
		WaitForSeconds waitTime = new WaitForSeconds(5);
		while (!videoPlayer.isPrepared)
		{
			Debug.Log("Preparing Video");
			//Prepare/Wait for 5 sceonds only
			yield return waitTime;
			//Break out of the while loop after 5 seconds wait
			break;
		}
		*/
		Debug.Log("Done Preparing Video");

		//Assign the Texture from Video to RawImage to be displayed
		image.texture = videoPlayer.texture;

		//Play Video
		videoPlayer.Play();

		//Play Sound
		//audioSource.Play();

		Debug.Log("Playing Video");
		while (videoPlayer.isPlaying)
		{
			Debug.LogWarning("Video Time: " + Mathf.FloorToInt((float)videoPlayer.time));
			yield return null;
		}

		Debug.Log("Done Playing Video");
		//Disable the raw image here
		image.gameObject.SetActive(false);
	}

}
