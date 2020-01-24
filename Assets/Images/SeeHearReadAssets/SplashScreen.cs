using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour {

	//public GameObject LoadingPanel;
	float timer;

	// Use this for initialization
	void Start () {
		//LoadingPanel.SetActive (false);
		timer = 0f;
	}

	// Update is called once per frame
	void Update () {
		if( timer>3f) {
			//LoadingPanel.SetActive (true);
			SceneManager.LoadScene(1);//load the Easy AR Landmarks Scene
		}
		timer += Time.deltaTime;
	}
}
