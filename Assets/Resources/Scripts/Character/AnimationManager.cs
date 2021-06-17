using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour {


    public Folder folder;
    public string spr;
    private string prevspr;
    public SpriteRenderer selfrender;
    public float frametime;
    public float speed;
    public bool loop;
    public bool ended;
    public string chr;
    public int frames;
    public int frame;
    public bool killonend;

    public enum Folder
    {
        Character,
        None,
        TitleCard
    };

    // Use this for initialization
    void Start()
    {
        frametime = 0;
        selfrender = gameObject.GetComponent<SpriteRenderer>();
    }

    public bool Animend()
    {
        if (frametime > frames)
            return true;
        else
            return false;
    }
    // Update is called once per frame
    void Update()
    {
        if (prevspr != spr)
            frametime = 0f;
        prevspr = spr;
        string fold = "def";
        if (folder == Folder.Character)
            fold = "Character";
        if (folder == Folder.TitleCard)
            fold = "TitleCard";
        Sprite[] anim = Resources.LoadAll<Sprite>("Sprites/" + fold + "/" + spr);
        if (folder == Folder.None)
            anim = Resources.LoadAll<Sprite>("Sprites/" + spr);
        if (chr != "")
        {
            if (folder == Folder.None)
                anim = Resources.LoadAll<Sprite>("Sprites/" + chr + "/" + spr);
            else
                anim = Resources.LoadAll<Sprite>("Sprites/" + fold + "/" + chr + "/" + spr);
        }
        frames = anim.Length;
        frametime += speed * Time.deltaTime;
        if (frametime > frames)
        {
            if (killonend == true)
                Destroy(gameObject);
            PController player = gameObject.GetComponent<PController>();
            if (player && player.attacking == true && player.pstate!=state.LandingLag)
            {
                player.Attackend();
            }
            if (player && player.pstate == state.Firing)
            {
                player.Fireend();
            }
            EnemyController enemy = gameObject.GetComponent<EnemyController>();
            if (enemy && enemy.attacking == true)
            {
                enemy.Attackend();
            }
            if (loop == true)
                frametime = 0f;
            else
            {
                frametime -= 1f;
                if (frametime < 0f)
                    frametime = 0f;
            }
        }
        frame = Mathf.FloorToInt(frametime);
        Sprite change = null;
        if (anim != null && anim[frame])
            change = anim[frame];
        if (change != null)
            selfrender.sprite = change;
        else
            Debug.Log("NULL");
    }

    public void spritechange(string sprite)
    {
        spr = sprite;
        frametime = 0f;
    }
    public void spritechange(string sprite, Folder fold)
    {
        spr = sprite;
        folder = fold;
        frametime = 0f;
    }
    public void spritechange(string sprite, Folder fold, int framestart)
    {
        spr = sprite;
        folder = fold;
        frametime = (float)framestart;
    }


}
