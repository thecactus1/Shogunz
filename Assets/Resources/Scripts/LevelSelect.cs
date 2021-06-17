using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : MonoBehaviour {

    public static int Levelselect;

	// Use this for initialization
	void Start () {
        Levelselect = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (Levelselect > GameController.levellist.Count-1)
            Levelselect = 0;
        if (Levelselect < 0)
            Levelselect = GameController.levellist.Count-1;

        gameObject.GetComponent<TextMesh>().text = "Select a Stage using up and down!\n" + GameController.levellist[Levelselect].desc;
	}
}
