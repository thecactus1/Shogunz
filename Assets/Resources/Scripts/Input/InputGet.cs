using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputGet : MonoBehaviour {

    public int playernum;
    public bool ready;

	// Use this for initialization
	void Start()
    {
        ready = false;
        Xbox360[] xbox = GetComponents<Xbox360>();
        for (int i = 0; i < xbox.Length; ++i)
        {
            xbox[i].controllernum = i + 1;
        }
    }
	
	// Update is called once per frame
	public void ControllerCheck () {
        Xbox360[] xbox = GetComponents<Xbox360>();
        for (int i = 0; i < xbox.Length; ++i)
        {
            if (xbox[i].IsConnected && GameController.controllerused[i + 1] == false)
            {
                if (xbox[i].GetButtonDown("A"))
                {
                    GameController.controllerused[i + 1] = true;
                    GameController.playerids[playernum].controllernum = xbox[i].controllernum;
                    transform.Find("Logo").GetComponent<CharacterSelect>().Ac = true;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Z) && GameController.controllerused[0]==false)
        {
            GameController.playerids[playernum].controllernum = 0;
            GameController.controllerused[0] = true;
            transform.Find("Logo").GetComponent<CharacterSelect>().Ac = true;
            transform.Find("Logo").GetComponent<CharacterSelect>().inputtimer = 3f;
        }
	}

    void Update()
    {
        if (ready)
        {
            GameController.playerids[playernum].ready = true;
            Ready();
        }
        else
        {
            transform.Find("Ready").GetComponent<SpriteRenderer>().enabled = false;
            AnimationManager anim = transform.Find("Ready").GetComponent<AnimationManager>();
            anim.spritechange("Ready");
            anim.speed = 0f;
            GameController.playerids[playernum].ready = false;
            anim.loop = false;
        }

    }

    public void Ready()
    {
        transform.Find("Ready").GetComponent<SpriteRenderer>().enabled = true;
        AnimationManager anim = transform.Find("Ready").GetComponent<AnimationManager>();
        if(anim.spr!="Ready")
        anim.spritechange("Ready");
        anim.speed = 30f;
        anim.loop = false;
        if (transform.Find("Logo").GetComponent<MenuInputs>().inputs["Sword"].isPressed)
        {
            GameController.playerids[playernum].ready = false;
            ready = false;
            transform.Find("Logo").GetComponent<CharacterSelect>().Ac = true;
        }
    }
}
