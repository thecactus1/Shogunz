using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {

	public GameObject[] player;       //Public variable to store a reference to the player game object
	private float offset;
	private float increase;
	private float timer;
	private float speed;
    public float minfov;

	// Use this for initialization
	void Start () 
	{
        minfov = 5f;
		player = GameObject.FindGameObjectsWithTag ("Player");
		timer = 45;
		offset = 0;
		increase = 0;
	}

	// LateUpdate is called after Update each frame
	void Update () 
	{
        player = GameObject.FindGameObjectsWithTag("Player");
        Vector3 pos = new Vector3(0, 0, -10);
		if (player.Length>0) {
            for (int i = 0; i<player.Length; ++i)
            {
               Vector3 playerpos = player[i].transform.position;
                pos += playerpos;
                Camera cam = GetComponent<Camera>();
                BoxCollider2D[] box = GetComponents<BoxCollider2D>();
                if (!player[i].GetComponent<BoxCollider2D>().IsTouching(box[0])){
                    box[1].size += new Vector2(0.1f, 0.05f);
                    box[0].size += new Vector2(0.1f, 0.05f);
                    cam.orthographicSize += 0.02f;
                }
                if(player[i].GetComponent<BoxCollider2D>().IsTouching(box[1]))
                {
                    box[0].size -= new Vector2(0.1f, 0.05f);
                    box[1].size -= new Vector2(0.1f, 0.05f);
                    if (cam.orthographicSize > minfov)
                        cam.orthographicSize -= 0.02f;
                }
            }
            pos = pos / player.Length;
            
            transform.position = pos;
		}
	}
	void FixedUpdate(){

	}
}