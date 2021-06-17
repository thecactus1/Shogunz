using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulletscript : MonoBehaviour {


    public bool dir;
    public float bspeed;
    public GameObject source;
    public LayerMask layermask;
    public float timer;

	// Use this for initialization
	void Start () {
        timer = 60 * 6;
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime * 60f;
        float speed = bspeed;
        if (dir == false)
            speed = -bspeed;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(speed, 0f);
        if (timer < 0)
            Destroy(gameObject);
	}

    public void Timereset()
    {
        timer = 60 * 6;
    }

    void OnTriggerEnter2D(Collider2D mycol)
    {
        if (mycol.gameObject != source) {
            if (mycol.gameObject.tag == "Player")
            {
                PController pc = mycol.gameObject.GetComponent<PController>();
                pc.Kill(dir);
                Destroy(gameObject);
            }
            if (mycol.gameObject.tag == "Enemy")
            {
                EnemyController pc = mycol.gameObject.GetComponent<EnemyController>();
                pc.Kill();
                Destroy(gameObject);
            }
            if (mycol.gameObject.tag == "Wall")
            {
                Destroy(gameObject);
            }
        }
    }
}
