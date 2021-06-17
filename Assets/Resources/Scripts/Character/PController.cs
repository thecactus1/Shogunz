using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum state
{
    Default,
    Wallrun,
    Walljump,
    Flip,
    Superflip,
    Attack,
    LandingLag,
    Firing,
    Dead,
    Parry
}

public class inputlist
{
    public bool isPressed;
    public bool isReleased;
    public bool down;
}



public class PController: MonoBehaviour {

    public GameObject counter;
    private int bullets;
    private int timer;
    public int playernum;
    public Attackscript Atk;
    public int parrystrength;
    private bool parried;
    public Vector3 offset;
    private bool fire;
    public float deathtimer;
    private bool firing;
    private float attackcool;
    public bool landing;
    private int combostring;
    private float landinglag;
    AnimationManager anim;
    public bool flip;
    public float ysp;
    public float xsp;
	public float speed;
    public float vspeed;
	public bool sprinting;
	private float sprintimer;
    public float gravitydelay;
    public float movedelay;
    private float combotimer;
	public bool directionface;
    private bool wallrun;
    public string attack;
    public bool attacking;

	public struct Walls
    {
        public bool bounds;
        public bool nobounds;
    }

    inputlist Createinput(inputlist i)
    {
        i = new inputlist();
        i.down = false;
        i.isPressed = false;
        i.isReleased = false;
        return i;
    }

    public state pstate;

    public inputlist jump, l, r, u, d, sword, gun, special, taunt;

    public GroundCheck chk;

    public Dictionary<string, inputlist> inputs;
    public Dictionary<string, float> inputtimes;
    public Dictionary<string, bool> prev;
	public Dictionary<string, Walls> walls;

    private Walls left, up, down, right;



    // Use this for initialization
    void Start(){
        bullets = 3;
        GameObject c = (GameObject)Instantiate(counter);
        c.transform.parent = transform;
        c.name = "Counter";
        timer = 0;
        playernum = GetComponent<PInput>().playernum;
        fire = false;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; ++i)
        {
            Physics2D.IgnoreCollision(players[i].GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
        deathtimer = 180f;
        firing = false;
        landinglag = 0f;
        anim = GetComponent<AnimationManager>();
        attackcool = 0;
        combostring = 0;
        attacking = false;
        attack = "";
        pstate = state.Default;
        wallrun = true;
        movedelay = 0f;
        gravitydelay = 0f;
        chk = GetComponent<GroundCheck>();
        walls = new Dictionary<string, Walls>();
        SetUpInputs();
        xsp = 0f;
		sprintimer = 0f;
        ysp = 0f;
		speed = 7f;
        vspeed = 16f;
    }

    // Update is called once per frame
    void Update(){
        AnimationManager anim2 = transform.Find("Counter").GetComponent<AnimationManager>();
        anim2.spritechange(anim2.spr, anim2.folder, bullets);
        GameController.playerids[playernum].alive = true;
        if (GameController.playerids[playernum].ready == false)
            Destroy(gameObject);
        GameObject child = gameObject.transform.Find(attack).gameObject;
        Atk = child.GetComponent<Attackscript>();
        Groundcheck();
        if (walls["Up"].bounds==true && ysp>0)
        {
            ysp = 0;
            GetComponent<Rigidbody2D>().velocity = new Vector2(xsp, 0);
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(xsp, ysp);
        }
        if (pstate == state.Default)
            Default();
        if (pstate == state.Wallrun)
            Wallrun();
        if (pstate == state.Walljump)
            Wallrunjump(directionface);
        if (pstate == state.Flip)
            Flip(directionface);
        if (pstate == state.LandingLag)
            LandingLag();
        if (pstate == state.Superflip)
            Superflip(directionface);
        if (pstate == state.Attack)
            Attack();
        else
            parrystrength = 0;
        if (pstate == state.Parry)
            Parry();
        if (pstate == state.Firing)
            Gunfire(3);
        if (pstate == state.Dead)
            Dead();
        if (combotimer == 0 || combostring == 3)
            combostring = 0;
    }

    public void Attackend()
    {
        attacking = false;
        landing = false;
        if (attack == "shogunslashroll")
            sprinting = true;
        pstate = state.Default;
    }

    void Gunfire(int frame)
    {
        if (bullets == 0)
        {
            transform.Find("Counter").GetComponent<Bulletcounter>().act = 3f;
            pstate = state.Default;
            return;
        }
        Gravity();
        movedelay = 0f;
        gravitydelay = 0f;
        if(walls["Down"].bounds==true)
        xsp = 0;
        if (inputs["Gun"].isPressed &&  (anim.frame >= 5 || fire==true))
        {
            Fire(directionface);
            anim.frametime = 0f;
            fire = false;
            anim.spritechange(anim.spr, anim.folder, 3);
        }
        if (((inputs["Left"].down && walls["Down"].bounds || inputs["Right"].down && walls["Down"].bounds) || inputs["Jump"].down || inputs["Sword"].down) && (fire == false || anim.frame>=4))
        {
            pstate = state.Default;
            if (inputs["Sword"].down)
            {
                AttackCheck();
            }
        }
        if (walls["Down"].bounds == false)
            landing = true;
        if (landing == true && walls["Down"].bounds == true && fire == false)
            pstate = state.Default;
        if (landing == true && walls["Down"].bounds == true && fire==true)
            pstate = state.LandingLag;
    }

    void Fire(bool dir)
    {
        --bullets;
        transform.Find("Counter").GetComponent<Bulletcounter>().act = 3f;
        GameObject bullet = (GameObject)Instantiate(Resources.Load("Prefabs/Gun/Bullet"));
        Bulletscript bs = bullet.GetComponent<Bulletscript>();
        bs.source = gameObject;
        bs.dir = directionface;
        bullet.transform.position = gameObject.transform.position + offset;
        if (!directionface)
            bullet.transform.position = new Vector2(gameObject.transform.position.x - offset.x, bullet.transform.position.y);
    }

    public void Fireend()
    {
        
    }


    void FixedUpdate()
    {
        --timer;
        if (timer < 0 && playernum==1)
        {
            GameController.totalseconds += 1;
            GameController.seconds += 1;
            timer = 60;
        }
    }
    void Dead()
    {
        Groundcheck();
        Gravity();
        if(deathtimer>175f)
        {
            if (!directionface)
                xsp = 5;
            if (directionface)
                xsp = -5;
            ysp = 7;
        }
        if (walls["Down"].bounds)
        {
            xsp = 0;
        }
        deathtimer = Generictimer(deathtimer);
        GameController.playerids[playernum].alive = false;
        if (deathtimer == 0)
        {
            
            ++GameController.death;
            Destroy(gameObject);
            GameObject gamecontroller = GameObject.FindGameObjectWithTag("GameController");
            GameController gcspawn = gamecontroller.GetComponent<GameController>();
            if(GameController.gamemode=="Challenge")
            gcspawn.Spawn();
        }
    }

    public void Startparry(bool dir)
    {
        combotimer = 0;
        Attackend();
        pstate = state.Parry;
        directionface = !dir;
        transform.position = new Vector2(transform.position.x, transform.position.y + 0.1f);
    }

    void Parry()
    {
        if (parried == true)
        {
            if (!directionface)
                xsp = 5;
            if (directionface)
                xsp = -5;
            ysp = 7;
            parried = false;
        }
        Groundcheck();
        Gravity();
        if (walls["Down"].bounds && parried==false)
        {
            landing = true;
            pstate = state.LandingLag;
        }
    }

    void Default()
    {
        parried = true;
        landing = false;
        if (inputs["Gun"].isPressed)
        {
            fire = true;
            pstate = state.Firing;
        }
        flip = true;
        if(walls["Up"].bounds == true && ysp>0f)
        {
            ysp = 0f;
        }
        if (sprinting == true && walls["Down"].bounds == true)
            speed = 10f;
        if (sprinting == false && walls["Down"].bounds==true)
            speed = 7f;
        if (xsp > speed)
            xsp = speed;
        if (xsp < -speed)
            xsp = -speed;
        if (ysp < -vspeed)
            ysp = -vspeed;
        if (inputs["Sword"].isPressed)
            AttackCheck();
        if (xsp == 0f || (directionface == false && inputs["Left"].down == false) || (directionface == true && inputs["Right"].down == false))
            sprinting = false;
        if (inputs["Left"].down == true && movedelay == 0f)
        {
            if (!inputs["Right"].down)
                MoveLeft();
            if (directionface == false && (sprintimer != 0f && sprintimer != 45f))
                sprinting = true;
            sprintimer = 45f;
            if(!inputs["Right"].down)
            directionface = false;
        }
        if (inputs["Right"].down == true && movedelay == 0f)
        {
            MoveRight();
            if (directionface == true && (sprintimer != 0f && sprintimer != 45f))
                sprinting = true;
            directionface = true;
            sprintimer = 45f;
        }
        if (inputs["Left"].down == false && inputs["Right"].down == false && movedelay==0 && walls["Down"].nobounds==true)
        {
            NotMove();
        }
        if (gravitydelay == 0f && walls["Down"].bounds == true)
            wallrun = true;
        
        if (gravitydelay != 0f)
            pstate = state.Wallrun;
        Gravity();
        
        Timers();
        if (inputs["Jump"].down == true)
        {
            Jumpinput();
        }
        if (walls["Down"].bounds == false && ((walls["Right"].bounds == true)|| (walls["Left"].bounds == true)))
        {
            if(inputs["Left"].down || inputs["Right"].down)
                vspeed = 5f;
            else
                vspeed = 16f;
            if (walls["Right"].bounds == true)
            {
                directionface = false;
            }
            if (walls["Left"].bounds == true)
                directionface = true;
        }
        else
            vspeed = 16f;
    }

    void Flip(bool dir)
    {
        Groundcheck();
        Gravity();  
        gravitydelay = 0;
        movedelay = 0;
        if (flip == true)
        {
            if (dir==false)
            {
                xsp = 3;
                ysp = 7;
            }
            if (dir)
            {
                xsp = -3;
                ysp = 7;
            }
        }
        flip = false;
        if (walls["Down"].nobounds == true)
        {
            directionface = !directionface;
            pstate = state.Default;
        }
        if (inputs["Sword"].isPressed)
        {
            AttackCheck();
            directionface = !directionface;
        }
        if (inputs["Gun"].isPressed)
        {
            directionface = !directionface;
            fire = true;
            pstate = state.Firing;
        }
    }

    void Superflip(bool dir)
    {
        Groundcheck();
        Gravity();
        gravitydelay = 0;
        movedelay = 0;
        if (flip == true)
        {
            if (dir == false)
            {
                xsp = -2;
                ysp = 22;
            }
            if (dir == true)
            {
                xsp = 2;
                ysp = 22;
            }
        }
        if (inputs["Sword"].isPressed && ysp < -3f)
            AttackCheck();
        if (inputs["Gun"].isPressed && ysp < -3f)
        {
            fire = true;
            pstate = state.Firing;
        }
        flip = false;
        if (ysp < 0 && (walls["Left"].bounds || walls["Right"].bounds))
        {
            Debug.Log("ha");
            pstate = state.Default;
        }
        if (walls["Down"].bounds == true)
        {
            Debug.Log("haha");
            directionface = !directionface;
            pstate = state.Default;
        }
    }

    void Wallrunjump(bool dir)
    {
        Groundcheck();
        Gravity();
        gravitydelay = 0;
        movedelay = 0;
        if (flip == false)
        {
            pstate = state.Default;
            movedelay = 30f;
        }
        if (flip == true)
        {
            if (dir == false)
            {
                xsp = -15;
                ysp = 14;
            }
            if (dir)
            {
                xsp = 15;
                ysp = 14;
            }
        }
        speed = 9;
        flip = false;
        if (inputs["Sword"].isPressed && ysp < -3f)
            AttackCheck();
        if (inputs["Gun"].isPressed && ysp < -3f)
        {
            fire = true;
            pstate = state.Firing;
        }
    }

    void Jumpinput()
    {

        if ((walls["Right"].bounds == true || walls["Left"].bounds == true) && wallrun==true && inputs["Up"].down==true)
        {
            Wallrun();
            return;
        }
        if (walls["Right"].bounds == true && inputs["Jump"].isPressed && walls["Down"].bounds == false)
        {
            Jump(true);
            return;
        }
        if (walls["Left"].bounds == true && inputs["Jump"].isPressed && walls["Down"].bounds == false)
        {
            Jump(false);
            return;
        }

        if (walls["Down"].bounds == true || (walls["Down"].nobounds == true && walls["Left"].bounds == false && walls["Right"].bounds == false))
        {
            Jump();
            return;
        }
    }

	void Timers(){
		if (sprintimer > 0f && (inputs["Left"].down==false && inputs["Right"].down==false)) {
			sprintimer -= 60f * Time.deltaTime;
			if (sprintimer < 0f)
				sprintimer = 0f;
		}
        gravitydelay = Generictimer(gravitydelay);
        movedelay = Generictimer(movedelay);
        if (pstate != state.Attack)
            combotimer = Generictimer(combotimer);
        if(pstate!=state.Attack)
        attackcool = Generictimer(attackcool);
    }

    void LandingLag()
    {
        xsp = 0;
        ysp = 0;
        if (landing == true)
        {
            landinglag = 20f;
            landing = false;
        }
        landinglag = Generictimer(landinglag);
        if (landinglag == 0f)
        {
            pstate = state.Default;
        }
    }

    void Attack()
    {

        if (walls["Down"].bounds == false)
            landing = true;
        if (walls["Down"].bounds == true && landing == true)
        {
            xsp = 0;
            ysp = 0;
            pstate = state.LandingLag;
        }
            sprinting = false;
        if(walls["Left"].bounds==true || walls["Right"].bounds == true)
        {
            xsp = 0;
        }
        Timers();
        Gravity();
        if (attack != "shogunslashroll" && attack!="shogunjumpSlash")
        {
            xsp = 0;
        }
        if (attack == "shogunslashroll")
        {
            if (xsp > 0)
                xsp = speed;
            if(xsp<0)
                xsp = -speed;
        }
        if (attack == "combo3")
        {
            if (directionface==true)
                xsp = 2;
            if (directionface==false)
                xsp = -2;
        }
        anim = GetComponent<AnimationManager>();
        List<GameObject> collision = new List<GameObject>();
        
        collision = Atk.AttackCheck(anim.frame);
        bool par = false;
        if (collision != null)
        {
            for (int i = 0; i < collision.Count; i++)
            {
                if (collision[i] && collision[i].tag == "Enemy")
                {
                    EnemyController enemy = collision[i].GetComponent<EnemyController>();
                    enemy.Kill();
                }
                if (collision[i] && collision[i].tag == "Attack")
                {
                    GameObject source = collision[i].transform.parent.gameObject;
                    if (source.tag == "Player")
                    {
                        PController enemy = source.GetComponent<PController>();
                        List<GameObject> enemycol = new List<GameObject>();
                        enemycol = enemy.Atk.AttackCheck(enemy.anim.frame);
                        bool collided = false;
                        if (enemycol!=null && enemy.Atk.active.IsTouching(Atk.active))
                            collided = true;
                        if (parrystrength >= enemy.parrystrength && collided==true && source!=gameObject)
                        {
                            enemy.Startparry(directionface);
                            if (parrystrength == enemy.parrystrength)
                                Startparry(!directionface);
                            par = true;
                        }
                    }
                }
                if (collision[i] && collision[i].tag == "Player")
                {
                    PController enemy = collision[i].GetComponent<PController>();
                    if(collision[i]!=gameObject && par==false)
                    enemy.Kill(directionface);
                }
                if (collision[i] && collision[i].tag == "Bullet")
                {
                    Bulletscript enemy = collision[i].GetComponent<Bulletscript>();
                    enemy.dir = directionface;
                    enemy.bspeed += 2;
                    enemy.Timereset();
                    enemy.source = gameObject;
                }
            }
        }
    }

    public void Kill(bool dir)
    {
        if (pstate == state.Dead)
            return;
        ysp = 7;
        if (!dir)
        {
            xsp = 5;
            directionface = true;
        }
        if (dir)
        {
            xsp = -5;
            directionface = false;
        }
        pstate = state.Dead;
    }

    float Generictimer(float var)
    {
        if (var > 0f)
            var -= 60f * Time.deltaTime;
        if (var < 0f)
            var = 0f;
        return var;
    }

    void Wallrun()
    {
        Timers();
        Groundcheck();
        if (wallrun == false)
        {
            if (inputs["Jump"].isPressed == true && walls["Down"].bounds == false)
            {
                if (inputs["Up"].down == false)
                    pstate = state.Walljump;
                if (inputs["Up"].down == true)
                    pstate = state.Superflip;
            }

        }
        if (wallrun == true)
        {
            gravitydelay = 30f;
            movedelay = 30f;
            wallrun = false;
        }
        if (walls["Left"].bounds == true)
            directionface = true;
        if (walls["Right"].bounds == true)
            directionface = false;
        ysp = 10f;
        xsp = 0f;
        if (gravitydelay == 0 || walls["Up"].bounds == true)
        {
            gravitydelay = 0;
            pstate = state.Default;
        }
        if (!walls["Right"].nobounds && !walls["Left"].nobounds && inputs["Jump"].isPressed == false)
        {
            pstate = state.Flip;


        }

    }

    void Groundcheck()
    {
        up.bounds = WallCheck(true, CollisionType.up, 0);
        up.nobounds = WallCheck(false, CollisionType.up, 0);
        down.nobounds = WallCheck(false, CollisionType.down, 0);
        down.bounds = WallCheck(true, CollisionType.down, 0);
        left.bounds = WallCheck(true, CollisionType.left, 0);
        left.nobounds = WallCheck(false, CollisionType.left, 0);
        right.nobounds = WallCheck(false, CollisionType.right, 0);
        right.bounds = WallCheck(true, CollisionType.right, 0);
        walls["Left"] = left;
        walls["Right"] = right;
        walls["Up"] = up;
        walls["Down"] = down;
    }

    bool WallCheck(bool bound, CollisionType type, int distadd)
    {
        return chk.WallCheck(GetComponent<BoxCollider2D>(), type, distadd,bound, ysp, false);
    }

    bool WallCheck(bool bound, CollisionType type, int distadd, string inputstring)
    {
        return chk.WallCheck(GetComponent<BoxCollider2D>(), type, distadd, bound, ysp,inputs[inputstring].down);
    }

    void SetUpInputs()
    {
        prev = new Dictionary<string, bool>();
        inputs = new Dictionary<string, inputlist>();
        inputs["Up"] = Createinput(u);
        inputs["Down"] = Createinput(d);
        inputs["Left"] = Createinput(l);
        inputs["Right"] = Createinput(r);
        inputs["Jump"] = Createinput(jump);
        inputs["Sword"] = Createinput(sword);
        inputs["Special"] = Createinput(special);
        inputs["Taunt"] = Createinput(taunt);
        inputs["Gun"] = Createinput(gun);
    }

    public void MoveLeft()
    {
        {
            if (walls["Left"].bounds == false)
            {
                if (sprinting == true)
                {
                    xsp = -speed;
                }
                else if (walls["Down"].bounds == false)
                {
                    xsp -= 30f * Time.deltaTime;
                }
                else
                {
                    xsp -= 80f * Time.deltaTime;
                }
            }
            else
                xsp = 0f;
        }
    }

    public void Gravity()
    {
        if (walls["Down"].bounds == true || (walls["Down"].nobounds == true && walls["Left"].bounds == false && walls["Right"].bounds == false) && ysp<0)
        {
            ysp = 0f;
        }
        else
        {
            ysp -= 37f * Time.deltaTime;
        }
    }

    public void MoveRight()
    {
        if (walls["Right"].bounds == false)
        {
            if (sprinting == true)
            {
                xsp = speed;
            }
            else if (walls["Down"].bounds == false)
            {
                xsp += 30f * Time.deltaTime;
            }
            else
            {
                xsp += 80f * Time.deltaTime;
            }
        }
        else
            xsp = 0f;
    }

    public void Jump()
    {
        ysp = 10f;
    }

    public void Jump(bool walldir)
    {
        if (walldir == false)
        {
            xsp = 15;
            directionface = true;
        }
        if (walldir == true)
        {
            xsp = -15;
            directionface = false;
        }
        movedelay = 30f;
        ysp = 10f;
        speed = 9;
    }

    public void NotMove()
    {
		if (xsp < 0f) {
			xsp += 80f * Time.deltaTime;
			if (xsp > 0f)
				xsp = 0f;
		}
		if (xsp > 0f) {
			xsp -= 80f * Time.deltaTime;
			if (xsp < 0f)
				xsp = 0f;
		}
    }

    void AttackCheck()
    {
        landing = false;
        pstate = state.Attack;
        if (walls["Down"].nobounds == false)
        {
            gravitydelay = 0;
            attacking = true;
            attack = "shogunjumpSlash";
            parrystrength = 1;
            anim.speed = 30f;
        }
        if (walls["Down"].nobounds==true && sprinting == true)
        {
            parrystrength = 1;
            anim.speed = 30f;
            gravitydelay = 0;
            attacking = true;
            anim.frametime = 0f;
            attack = "shogunslashroll";
        }

        if (walls["Down"].nobounds == true && sprinting == false)
        {
            anim.speed = 15f;
            gravitydelay = 0;
            attacking = true;
            anim.frametime = 0f;
            combotimer = 10f;
            if (combostring == 0)
            {
                parrystrength = 2;
                attack = "combo1";
                anim.speed = 30f;
            }
            if (combostring == 1)
            {
                parrystrength = 2;
                attack = "combo2";
                anim.speed = 15f;
            }
            if (combostring == 2)
            {
                parrystrength = 3;
                attack = "combo3";
                anim.speed = 30f;
            }
            ++combostring;
        }
    }

}
