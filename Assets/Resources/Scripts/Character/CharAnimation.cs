using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharAnimation : MonoBehaviour {

    public SpriteRenderer spr;
    public PController pc;
    public AnimationManager anim;

    // Use this for initialization
    void Start () {
        AnimationManager anim = GetComponent<AnimationManager>();
        anim.spritechange("shogun");
    }
	
	// Update is called once per frame
	void Update () {

        spr = GetComponent<SpriteRenderer>();
        pc = GetComponent<PController>();
        anim = GetComponent<AnimationManager>();
        anim.chr = GameController.playerids[pc.playernum].character;
        state pstate = pc.pstate;
        if (pstate == state.Superflip)
            Superflip();
        if (pstate == state.Flip)
            Flip();
        if (pstate == state.Attack)
            Attack();
        if (pstate == state.Default)
            Default();
        if (pstate == state.Wallrun)
            Wallrun();
        if (pstate == state.Walljump)
            Walljump();
        if (pstate == state.LandingLag)
            LandingLag();
        if(pstate==state.Firing)
            Gun();
        if (pstate == state.Dead || pstate==state.Parry)
            Dead();
    }

    void Gun()
    {
        anim.speed = 60f;
        anim.loop = false;
        if (pc.walls["Down"].bounds == true && anim.spr != "gunfire")
            anim.spritechange("gunfire", anim.folder,1);
        if (pc.walls["Down"].bounds == false && anim.spr != "jumpgun")
            anim.spritechange("jumpgun",anim.folder,1);
    }

    void Dead()
    {
        GetComponent<SpriteRenderer>().flipX = pc.directionface;
        if (pc.walls["Down"].bounds)
        {
            anim.spritechange("die");
        }
        else
            anim.spritechange("fall");
    }

    void LandingLag()
    {
        if(anim.spr!="shogunland")
        anim.spritechange("shogunland");
    }

    void Attack()
    {
        spr.flipX = pc.directionface;
        anim.loop = false;
        if (anim.spr != pc.attack)
        {
            anim.spritechange(pc.attack);
        }
    }

    void Walljump()
    {
        anim.speed = 15f;
        anim.loop = true;
        if (anim.spr != "shogunjump")
            anim.spritechange("shogunjump");
    }

    void Wallrun()
    {
        anim.speed = 15f;
        anim.loop = true;
        if(anim.spr != "shogunrunup")
        anim.spritechange("shogunrunup");
    }

    void Flip()
    {
        GetComponent<SpriteRenderer>().flipX = !pc.directionface;
        anim.loop = false;
        if (anim.spr != "shogunbackflip")
        {
            anim.spritechange("shogunbackflip");
        }
    }

    void Superflip()
    {
        GetComponent<SpriteRenderer>().flipX = !pc.directionface;
        anim.loop = false;
        if (anim.spr != "shogunbackflip")
        {
            anim.spritechange("shogunbackflip");
        }
    }

    void Default()
    {
        anim.speed = 30f;
        anim.loop = true;
        spr.flipX = pc.directionface;
        if (pc.walls["Down"].nobounds == true && pc.xsp == 0 && anim.spr != "shogun")
            anim.spritechange("shogun");
        if (pc.sprinting == false)
        {
            if (pc.walls["Down"].bounds == true && pc.xsp != 0 && anim.spr != "shogunrun")
                anim.spritechange("shogunrun");
        }
        else
        {
            if (pc.walls["Down"].bounds == true && pc.xsp != 0 && anim.spr != "shogunsprint")
                anim.spritechange("shogunsprint");
        }
        if (pc.walls["Down"].bounds == false)
        {
            if (((pc.walls["Left"].nobounds) || (pc.walls["Right"].nobounds)) && anim.spr != "shogunwallcling")
                anim.spritechange("shogunwallcling");
            if (((pc.walls["Left"].nobounds) || (pc.walls["Right"].nobounds))==false && pc.walls["Down"].nobounds==false && anim.spr != "shogunjump")
                anim.spritechange("shogunjump");
            if(anim.spr!="shogun" && pc.walls["Down"].nobounds==true && ((pc.walls["Left"].bounds)==false && (pc.walls["Right"].bounds)==false))
                anim.spritechange("shogun");
        }
    }

}
