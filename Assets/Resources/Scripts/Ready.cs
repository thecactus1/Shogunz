using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ready : MonoBehaviour {

    public int scenetrans;
    public string gamemode;
    private bool ready;

	// Use this for initialization
	void Start () {
		ready = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!ready)
        {
            SpriteRenderer spr = GetComponent<SpriteRenderer>();
            spr.enabled = false;
            AnimationManager anim = GetComponent<AnimationManager>();
            anim.spritechange("Readyup");
            anim.speed = 0f;
            anim.loop = false;
        }
        if (ready)
        {
            if (Input.GetKey(KeyCode.Return))
            {
                GameController.gamemode = gamemode;
                if(GameController.gamemode=="Duel")
                SceneManager.LoadScene (GameController.levellist[LevelSelect.Levelselect].name);
                if (GameController.gamemode == "Challenge")
                    SceneManager.LoadScene("T2");
            }
            SpriteRenderer spr = GetComponent<SpriteRenderer>();
            spr.enabled = true;
            AnimationManager anim = GetComponent<AnimationManager>();
            if(anim.spr!="Readyup")
            anim.spritechange("Readyup");
            anim.speed = 30f;
            anim.loop = false;
        }


        if (GameController.Players() >= GameController.GameMode[gamemode].playermin && GameController.Players() <= GameController.GameMode[gamemode].playermax)
        {
            ready = true;
        }
        else
        {
            ready = false;
        }
    }
}
