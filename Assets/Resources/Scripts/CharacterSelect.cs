using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour {

    public float inputtimer;
    public float inputtimer2;
    public bool Ac;
    public int character;
    private Dictionary<int,string> Characters;
    private MenuInputs menu;
    public int Characteramount;
    public bool Z;

    inputlist Createinput(inputlist i)
    {
        i = new inputlist();
        i.down = false;
        i.isPressed = false;
        i.isReleased = false;
        return i;
    }

    void CharacterList()
    {
        Characters = new Dictionary<int, string>();
        Characters[0] = "Akira";
        Characters[1] = "AnKu";
        Characters[2] = "Akira2";
        Characters[3] = "AnKu2";
    }

    // Use this for initialization
    void Start () {
        inputtimer2 = 0;
        Z = false;
        inputtimer = 3f;
        menu = GetComponent<MenuInputs>();
        
        character = 0;
        CharacterList();
	}
	
	// Update is called once per frame
	void Update () {
        Characteramount = Characters.Count;
        if (inputtimer==0 && Ac)
        {
            Z = true;
        }
        if(Ac && Z == true)
        {
            if (menu.inputs["Jump"].isPressed)
            {
                Ac = false;
                transform.parent.gameObject.GetComponent<InputGet>().ready = true;
                GameController.playerids[transform.parent.GetComponent<InputGet>().playernum].character = Characters[character];
            }
        }
        if (Ac && inputtimer == 0)
        {
            if (menu.inputs["Left"].down)
            {
                inputtimer = 10f;
                --character;
                if (character < 0)
                {
                    character = Characteramount - 1;
                }
            }
            if (menu.inputs["Right"].down)
            {
                inputtimer = 10f;
                ++character;
                if (character > Characteramount - 1)
                {
                    character = 0;
                }
            }
        }
        if (inputtimer2 == 0)
        {
            if (menu.inputs["Up"].down)
            {
                inputtimer2 = 10f;
                ++LevelSelect.Levelselect;
            }
            if (menu.inputs["Down"].down)
            {
                Debug.Log("Down");
                inputtimer2 = 10f;
                --LevelSelect.Levelselect;
            }
        }
            AnimationManager anim = GetComponent<AnimationManager>();
        AnimationManager anim2 = transform.Find("Titlecard").gameObject.GetComponent<AnimationManager>();
        if (Ac || transform.parent.gameObject.GetComponent<InputGet>().ready == true)
        {
            anim.chr = Characters[character];
            anim2.chr = Characters[character];
        }
        else
        {
            anim.chr = "None";
            anim2.chr = "None";
        }
            if (inputtimer2 > 0f)
            {
                inputtimer2 -= Time.deltaTime * 60f;
            }
            else
                inputtimer2 = 0f;
            if (inputtimer > 0f && Ac)
            {
                inputtimer -= Time.deltaTime * 60f;
            }
            else if (Ac)
                inputtimer = 0f;

        }
    }
