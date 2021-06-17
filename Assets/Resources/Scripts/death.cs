using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class death : MonoBehaviour {
	public bool player;
	private int deathtime;
	private int vspeed;

	// Use this for initialization
	void Start () {
		deathtime = 100;
		vspeed = 10;
	}
	
	// Update is called once per frame
	void Update () {
		Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D> ();
		rb.velocity = new Vector2 (0f, vspeed);
		if (deathtime == 0) {
			Destroy (gameObject);
			GameObject gamecontroller = GameObject.FindGameObjectWithTag ("GameController");
			GameController gcspawn = gamecontroller.GetComponent<GameController> ();
			if(player==true)
			gcspawn.Spawn ();
		}
	}

	void FixedUpdate(){
		--deathtime;
		--vspeed;
	}
}
