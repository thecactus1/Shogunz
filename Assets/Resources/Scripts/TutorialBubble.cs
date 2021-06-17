using UnityEngine;
using System.Collections;

public class TutorialBubble : MonoBehaviour {

    public GameObject player;
    private BoxCollider2D self;
    private TextMesh text;

	// Use this for initialization
	void Start () {
        self = gameObject.GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        text = gameObject.GetComponentInChildren<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
        //enable and disable renderers based on player trigger
        BoxCollider2D playerbox = player.GetComponent<BoxCollider2D>();
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        Renderer tr = text.GetComponent<Renderer>();
        if (self.IsTouching(playerbox))
        {
            sr.enabled = true;
            tr.enabled = true;
            
        }
        else
        {
            sr.enabled = false;
            tr.enabled = false;
        }
	}
}
