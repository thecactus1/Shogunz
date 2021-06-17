using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikescript : MonoBehaviour {
	public GameObject player;
	public GameObject[] enemy;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        bool block = false;
        if (player.GetComponent<PController>().pstate == state.Dead)
            block = true;
        if (GetComponent<PolygonCollider2D> ().IsTouching (player.GetComponent<Collider2D> ())) {
			PController playerclass = player.GetComponent<PController> ();
			playerclass.Kill (!playerclass.directionface);
		}
		enemy = GameObject.FindGameObjectsWithTag ("Enemy");
		for (int i = 0; i < enemy.Length; ++i) {

			if (GetComponent<PolygonCollider2D> ().IsTouching (enemy [i].GetComponent<Collider2D> ())) {
                EnemyController enemyclass = enemy [i].GetComponent<EnemyController> ();
				enemyclass.Kill ();
			}
		}
	}
}
