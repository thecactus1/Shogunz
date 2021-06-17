using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Z)) {
			SceneManager.LoadScene ("InputGetSolo");
		}
        if (Input.GetKeyDown(KeyCode.X))
        {
            SceneManager.LoadScene("InputGetTest");
        }
        if (Input.GetKeyDown (KeyCode.C)) {
			SceneManager.LoadScene ("controls");
		}
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit();
		}
	}
}
