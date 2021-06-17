using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn : MonoBehaviour {

    public int playerspawn;
	public GameObject spawner;
	public bool directionface;

	// Use this for initialization
	void Start () {
		Spawn ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Spawn(){
		GameObject obj = (GameObject)Instantiate (spawner);
		if (obj.tag == "Enemy") {
			EnemyController enem = obj.GetComponent<EnemyController> ();
			enem.directionface = directionface;
		}
        if (obj.tag == "Player")
        {
            PController enem = obj.GetComponent<PController>();
            Xbox360 xbox = obj.GetComponent<Xbox360>();
            PInput pin = obj.GetComponent<PInput>();
            enem.directionface = directionface;
            pin.playernum = playerspawn;
            xbox.controllernum = GameController.playerids[playerspawn].controllernum;
            AnimationManager anim = obj.GetComponent<AnimationManager>();
            anim.chr = GameController.playerids[playerspawn].character;
        }
        obj.transform.position = transform.position;
	}
}
