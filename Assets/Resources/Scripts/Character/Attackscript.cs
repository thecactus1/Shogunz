using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackscript : MonoBehaviour {

	public BoxCollider2D[] attacks;
	public BoxCollider2D test;
    public BoxCollider2D active;
	public float[] offsetx;
    public bool directionface;

	// Use this for initialization
	void Start () {
		
		for (int i=0; i<attacks.Length; ++i){
			if (attacks [i] != null)
				offsetx [i] = attacks [i].offset.x;
			else
				offsetx [i] = 0f;
				}
	}
	
	// Update is called once per frame
	void Update () {
        
		transform.localPosition = new Vector2 (0f, 0f);
		PController player = gameObject.GetComponentInParent<PController> ();
		if (player) {
			if (player.directionface == true) {
                directionface = true;
				for (int i = 0; i < attacks.Length; ++i) {
					if (attacks [i] != null)
						attacks [i].offset = new Vector2 (Mathf.Abs (offsetx [i]), attacks [i].offset.y);
				}
			}
			if (player.directionface == false) {
                directionface = false;
				for (int i = 0; i < attacks.Length; ++i) {
					if (attacks [i] != null)
						attacks [i].offset = new Vector2 (-Mathf.Abs (offsetx [i]), attacks [i].offset.y);
				}
			}
		}
		EnemyController enemy = gameObject.GetComponentInParent<EnemyController> ();
		if (enemy) {
			if (enemy.directionface == true) {
                directionface = true;
				for (int i = 0; i < attacks.Length; ++i) {
					if (attacks [i] != null)
						attacks [i].offset = new Vector2 (-Mathf.Abs (offsetx [i]), attacks [i].offset.y);
				}
			}
			if (enemy.directionface == false) {
                directionface = false;
				for (int i = 0; i < attacks.Length; ++i) {
					if (attacks [i] != null)
						attacks [i].offset = new Vector2 (Mathf.Abs (offsetx [i]), attacks [i].offset.y);
				}
			}
		}
	}
	public List <GameObject> AttackCheck(int frame) {
        if (attacks[frame]) {
            active = attacks[frame];
            List<GameObject> collisions = new List<GameObject>();
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0; i < enemies.Length; i++)
            {
                if (attacks[frame].GetComponent<Collider2D>().IsTouching(enemies[i].GetComponent<BoxCollider2D>()))
                {
                    collisions.Add(enemies[i]);
                }
            }
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < players.Length; i++)
            {
                if (attacks[frame].GetComponent<Collider2D>().IsTouching(players[i].GetComponent<Collider2D>()))
                {
                    collisions.Add(players[i]);
                }
            }
            GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
            for (int i = 0; i < bullets.Length; i++)
            {
                if (attacks[frame].GetComponent<Collider2D>().IsTouching(bullets[i].GetComponent<Collider2D>()))
                {
                    collisions.Add(bullets[i]);
                }
            }
            GameObject[] attack = GameObject.FindGameObjectsWithTag("Attack");
            for (int i = 0; i < attack.Length; i++)
            {
                if (attacks[frame].GetComponent<Collider2D>().IsTouching(attack[i].GetComponent<Collider2D>()))
                {
                    collisions.Add(attack[i]);
                }
            }
            return collisions;
    }
		return null;
	}
		
}
