using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigateMainScene : MonoBehaviour {
	public GameObject MainPanel;
	public GameObject PrivacyPanel;
	public GameObject LoadingPanel;
	public GameObject HowToPanel;
	public static bool privacyButtonPress;
	// Use this for initialization
	void Start () {
		privacyButtonPress = false;
		//PlayerPrefs.DeleteAll ();
		if (!PlayerPrefs.HasKey ("first_time")) {
			PlayerPrefs.SetInt ("first_time", 1);
			HowToPanel.SetActive (true);
			MainPanel.SetActive (false);
			PrivacyPanel.SetActive (false);
			LoadingPanel.SetActive (false);

		} else {
			HowToPanel.SetActive (false);
			MainPanel.SetActive (true);
			PrivacyPanel.SetActive (false);
			LoadingPanel.SetActive (false);

		}
	}
	
	public void selectPrivacyPanel(){
		privacyButtonPress = true;
		MainPanel.SetActive (false);
		HowToPanel.SetActive (false);
		PrivacyPanel.SetActive (true);
		LoadingPanel.SetActive (false);

	}

	public void selectMainPanel(){
		privacyButtonPress = false;
		MainPanel.SetActive (true);
		HowToPanel.SetActive (false);
		PrivacyPanel.SetActive (false);
		LoadingPanel.SetActive (false);
	}

	public void LoadScene(){
		MainPanel.SetActive (false);
		HowToPanel.SetActive (false);
		PrivacyPanel.SetActive (false);
		LoadingPanel.SetActive (true);
		SceneManager.LoadScene(1);
	}

}
