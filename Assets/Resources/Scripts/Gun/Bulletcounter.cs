using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulletcounter : MonoBehaviour {

    public float act;

    // Use this for initialization
    void Start () {
        act = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (act > 0)
            act -= Time.deltaTime;
        if (act< 0)
            act = 0;
        if(act!=0)
        {
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        }
        else
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.2f);
        transform.localPosition = new Vector2(0f, 0.25f);
    }
}
