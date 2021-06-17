using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private BoxCollider2D[] Walls;
	public BoxCollider2D[] Pcollide;
	public bool runsave;
    public KeyCode Jump;
    public KeyCode Left;
    public KeyCode Right;
	public KeyCode Sword;
	public KeyCode up;
    public int sp;
	public bool directionface;
	private int timer;
    public int vspeed;
    private Transform trans;
    public bool Bottom;
    public int speedlimit;
    private Rigidbody2D rb;
    public LayerMask layer;
    private int jumpspeed;
	private int jumpcool;
	private int wallskip;
    private bool leftlock;
    private bool rightlock;
	private bool walljump;
	public bool walllock;
	AnimationManager anim;
	public int gravitydelay;
	public bool attacking;
	public string attack;
	private int attackcool;
	private int combostring;
	private int combotimer;
	private bool buffered;
	private int sprinttimer;
	private bool sprinting;
	public GameObject death;
	private bool sprintsave;
	private bool wallflip;
    enum CollisionType
    {
        down,
        left,
        right,
        up
    }

	public void Kill(){
		GameController.death += 1;
		GameObject d=(GameObject)Instantiate (death);
		d.transform.position = trans.position;
		Destroy (gameObject);
	}

    // Use this for initialization
    void Star() {
		timer = 60;
		runsave = true;
		wallflip = false;
		sprinttimer = 0;
		buffered = false;
		combotimer = 0;
		attackcool = 0;
		attacking = false;
		anim = GetComponent<AnimationManager>();
		gravitydelay = 0;
		walljump = true;
        leftlock = false;
        rightlock = false;
		wallskip = 17;
		jumpcool = 0;
        jumpspeed = 5;
        sp = 0;
        trans = gameObject.transform;
        rb = gameObject.GetComponent<Rigidbody2D>();
        Pcollide = gameObject.GetComponents<BoxCollider2D>();
        GameObject[] wall = GameObject.FindGameObjectsWithTag("Wall");
        int walllength = wall.Length;
        Walls = new BoxCollider2D[wall.Length];
        for (int i = 0; i < wall.Length; i++)
        {
            Walls[i] = wall[i].GetComponent<BoxCollider2D>();
        }
    }
    void FixedUpdate()
    {
		--timer;
		if (timer < 0) {
			GameController.totalseconds += 1;
			GameController.seconds += 1;
			timer = 60;
		}
        if (sprinttimer > 0)
			--sprinttimer;
		if (combotimer > 0)
			--combotimer;
		if (attackcool > 0)
			--attackcool;
		if (gravitydelay > 0)
			--gravitydelay;
		if (jumpcool > 0)
			--jumpcool;

		if (WallCheck (CollisionType.down, 1,true)) {

			if(sprinting==false)
				speedlimit = 7;
			if (sprinting == true)
				speedlimit = 10;

		}
        if (WallCheck(CollisionType.down,1,true)==false){
            if (sp > 0)
                sp = jumpspeed;
            if (sp < 0)
                sp = -jumpspeed;
        }
		if (attacking == false && Input.GetKey(Left)  && WallCheck(CollisionType.left,0,true)==false && ExtraCheck(CollisionType.down,0,true))
        {
			--sp;
			if(sprinting==true)
				sp=-speedlimit;
        }
		if (sp < 0 && Input.GetKey(Left)==false && (attacking==false))
        {
			++sp;
        }
		if (attacking == false && Input.GetKey(Right) && ExtraCheck(CollisionType.down, 0, true) == true && WallCheck(CollisionType.right,0,true)==false)
        {
            ++sp;
			if (sprinting == true)
				sp = speedlimit;
        }
		if (sp > 0 && Input.GetKey(Right) == false && (attacking==false))
        {
            --sp;
        }
        if (sp > speedlimit)
            sp = speedlimit;
        if (sp < -speedlimit)
            sp = -speedlimit;
        if (WallCheck(CollisionType.down, 1, true) == true||(ExtraCheck(CollisionType.down,1,true) == true && WallCheck(CollisionType.right,1,false)==false && WallCheck(CollisionType.left, 1, false) == false) && gravitydelay==0)
        {
            walljump = true;
            if(vspeed<0)
            vspeed = 0;
            wallskip = 17;
        }
        else
        {
            if (wallskip > 10)
                wallskip = 10;
            if(gravitydelay==0)
            --vspeed;
        }
        if (vspeed < -10)
            vspeed = -10; 
    }

	void Animation(){
		anim.chr = "Akira";
		int wallhang = 0;
		bool air = true;
		if (WallCheck (CollisionType.down, 0, false))
			air = false;
		if (WallCheck (CollisionType.right, 0, false))
			wallhang = 1;
		if (WallCheck (CollisionType.left, 0, false))
			wallhang = 2;
		if(directionface==false)
			anim.selfrender.flipX = true;
		if(directionface==true)
			anim.selfrender.flipX = false;
		if(attacking==false && sprinting==false)
		anim.speed = 15f;
		if (sprinting == true)
			anim.speed = 25f;
		if (air == false && attacking==false) {
			if (sp != 0 && anim.spr != "shogunrun" && sprinting==false)
				anim.spritechange ("shogunrun");
			if (sp != 0 && anim.spr != "Shogunsprint" && sprinting==true)
				anim.spritechange ("Shogunsprint");
			if (sp == 0)
				anim.spritechange ("shogun");
		}
		if (air == true && attacking==false && (anim.spr!="shogunbackflip")) {
			if (anim.spr != "shogunjump" && wallhang==0 && walljump==true)
				anim.spritechange ("shogunjump");
			if ((anim.spr != "shogunbackflip" && wallhang == 0 && walljump == false)||(anim.spr != "shogunbackflip" && wallflip == true)) {
				if (sp < 0) {
					if(wallflip==true)
						directionface = true;
					if(wallflip==false)
						directionface = false;
				}
				if (sp > 0) {
					if(wallflip==false)
					directionface = true;
					if(wallflip==true)
					directionface = false;
				}
				anim.spritechange ("shogunbackflip");
				anim.loop = false;
			}
			if (wallhang != 0) {
				if (anim.spr != "shogunwallcling" && gravitydelay==0)
					anim.spritechange ("shogunwallcling");
				if (anim.spr != "shogunrunup" && gravitydelay!=0) {
					anim.spritechange ("shogunrunup");
					anim.speed = 30f;
				}
			}
			if (wallhang == 1)
				directionface = true;
			if (wallhang == 2)
				directionface = false;
		}
		if (anim.spr != "shogunbackflip" && attacking==false)
			anim.loop = true;
		if (attacking == true) {
			walljump = true;
			anim.loop = false;
			if (anim.spr != attack)
				anim.spritechange(attack);
		}
	}

	public void Attackend(){
		attacking = false;
		if(attack=="shogunjumpSlash")
		attackcool = 5;
		if (attack == "combo1") {
			combostring = 1;
			attackcool = 0;
			combotimer = 10;
		}
		if (attack == "combo2") {
			combostring = 2;
			attackcool = 0;
			combotimer = 10;
		}
		if (attack == "combo3") {
			combostring = 3;
			attackcool = 7;
			combotimer = 0;
		}
	}

	void Attacking(){
		if (attack == "combo1" || attack == "combo2" || attack == "combo3")
			sp = 0;
		List <GameObject> collision = new List<GameObject> ();
		GameObject child = gameObject.transform.Find (attack).gameObject;
		Attackscript Atk = child.GetComponent<Attackscript> ();
		collision = Atk.AttackCheck (anim.frame);	
		if (collision!=null) {
			for (int i = 0; i < collision.Count; i++) {
				if (collision [i] && collision [i].tag == "Enemy") {
					EnemyController enemy = collision [i].GetComponent<EnemyController> ();
					enemy.Kill ();
				}
					
			}
		}
	}

	void AttackCheck(){
		buffered = false;
		if (WallCheck (CollisionType.down, 0, false) == false) {
			gravitydelay = 0;
			attacking = true;
			attack = "shogunjumpSlash";
			anim.speed = 30f;
		}
		if (WallCheck (CollisionType.down, 0, false) == true && sprinting == true) {
			anim.speed = 15f;
			gravitydelay = 0;
			attacking = true;
			anim.frametime = 0f;
			attack = "shogunslashroll";
		}
			
		if (WallCheck (CollisionType.down, 0, false) == true && sprinting==false) {
			anim.speed = 15f;
			gravitydelay = 0;
			attacking = true;
			anim.frametime = 0f;
			if (combostring == 0) {
				attack = "combo1";
				anim.speed = 30f;
			}
			if (combostring == 1) {
				attack = "combo2";
				anim.speed = 15f;
			}
			if (combostring == 2) {
				attack = "combo3";
				anim.speed = 30f;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		if (WallCheck (CollisionType.down, 0, false))
			wallflip = false;
		if(WallCheck (CollisionType.down, 0, true) && vspeed==0)
			runsave = true;
		WallCheck (CollisionType.down, 0, true);
		if (sp == 0) {
			sprinting = false;
            if(attacking==false)
			sprintsave = false;
		}
		if (attackcool <= 5 && Input.GetKeyDown (Sword))
			buffered = true;
		if (combotimer == 0 || combostring == 3)
			combostring = 0;
		if (attacking == true)
			Attacking ();
		if (attacking == false && (Input.GetKeyDown(Sword)||buffered==true) && attackcool==0) {
			AttackCheck ();
		}
		if (((Input.GetKeyDown (Left) && directionface == true) || (Input.GetKeyDown (Right) && directionface == false)) && sprinttimer != 0 && sprinttimer != 10) {
			sprinting = true;
			sprintsave = true;
		}
		if (WallCheck (CollisionType.down, 3, false) == false && sprinting==true) {
			sprinting = false;
			jumpspeed = 10;
		} else {
			if (sprintsave == true)
				sprinting = true;
		}
		if (Input.GetKey (Left) == false && Input.GetKey (Right) == false) {
			sprinting = false;
			sprintsave = false;
		}
		if (Input.GetKey (Left) && !Input.GetKey(Right) && walljump == true && attacking == false && leftlock==false && rightlock==false && gravitydelay==0 && (WallCheck (CollisionType.down, 3, false) || attacking==true)) {
			directionface = true;
			sprinttimer = 10;
		}
		if (Input.GetKey(Right) && !Input.GetKey(Left) && walljump == true && attacking==false && leftlock==false && rightlock==false && gravitydelay==0 && (WallCheck (CollisionType.down, 3, false) || attacking==true)){
		directionface = false;
			sprinttimer = 10;
		}
		Animation ();
        if (WallCheck(CollisionType.down, 0, false) || WallCheck(CollisionType.right, 0, false))
            leftlock = false;
        if (WallCheck(CollisionType.down, 0, false) || WallCheck(CollisionType.left, 0, false))
            rightlock = false;
		if (WallCheck (CollisionType.down, 0, false)) {
			wallskip = 17;
			walljump = true;
		}
		if (vspeed <= 10)
			walllock = false;
		if (WallCheck (CollisionType.left, 0, true) && sp<0)
			sp = 0;
		if (WallCheck (CollisionType.right, 0, true) && sp>0)
			sp = 0;
        rb.velocity = new Vector2(sp, vspeed);
		if (Input.GetKey (Left) && WallCheck (CollisionType.left, 3,false) && vspeed < -3)
			vspeed = -3;
		if (Input.GetKey (Right) && WallCheck (CollisionType.right, 3,false) && vspeed < -3)
			vspeed = -3;
		if ((WallCheck (CollisionType.right, 0, true) || WallCheck (CollisionType.left, 0, true)) && runsave==true && Input.GetKey(up) && Input.GetKeyDown(Jump) && gravitydelay==0) {
			vspeed = 10;
			runsave = false;
			gravitydelay=20;
            jumpcool = 5;
			walllock = true;
		}
        //gameObject.transform.position = new Vector2(trans.position.x + (sp * Time.deltaTime), trans.position.y + (vspeed * Time.deltaTime));
		if (Input.GetKey(Jump) && WallCheck(CollisionType.down,0,true) && jumpcool==0 && vspeed==0)
        {
			if(sprintsave==false)
			jumpspeed = 7;
            jumpcool = 5;
			speedlimit = jumpspeed;
            vspeed = 12;
            if (Input.GetKey(Right) == false && Input.GetKey(Left) == false)
                sp = 0;
			gravitydelay = 0;
        }
		if (Input.GetKeyDown(Jump) && WallCheck(CollisionType.left,1, false) && (gravitydelay==0||Input.GetKey(Right)) && jumpcool==0)
		{
         
			walljump = true;
            leftlock = true;
			jumpspeed = 10;
			speedlimit = 15;
			sp = 15;
			vspeed = 13;
			jumpcool = 2;
			gravitydelay = 0;
		}
		if (Input.GetKeyDown(Jump) && WallCheck(CollisionType.right ,1, false) && (gravitydelay==0||Input.GetKey(Left)) && jumpcool==0)
		{
			walljump = true;
            rightlock = true;
			jumpspeed = 10;
			gravitydelay = 0;
			speedlimit = 15;
			sp = -15;
			vspeed = 13;
			jumpcool = 2;
		}
			if (Input.GetKeyDown(Jump) && WallCheck(CollisionType.left,1, false) && gravitydelay>0 && jumpcool==0)
		{
			walljump = false;
            leftlock = true;
			jumpspeed = 3;
			speedlimit = 3;
			sp = 3;
			vspeed = 24;
            Debug.Log(jumpcool);
			jumpcool = 2;
			gravitydelay = 0;
		}
		if (Input.GetKeyDown(Jump) && WallCheck(CollisionType.right ,1,false) && gravitydelay>0 && jumpcool==0)
		{
			walljump = false;
            rightlock = true;
			jumpspeed = 3;
			speedlimit = 3;
			sp = -3;
			vspeed = 24;
			jumpcool = 2;
			gravitydelay = 0;
		}
			if (!WallCheck (CollisionType.right, 1, false) && !WallCheck (CollisionType.left, 1, false) && gravitydelay > 0 && !directionface) {
			walljump = true;
			jumpspeed = 3;
			speedlimit = 3;
			sp = -3;
			vspeed = 10;
			jumpcool = 2;
			gravitydelay = 0;
			wallflip = true;
		}
		if (!WallCheck (CollisionType.right, 1, false) && !WallCheck (CollisionType.left, 1, false) && gravitydelay > 0 && directionface) {
			walljump = true;
			jumpspeed = 3;
			speedlimit = 3;
			sp = 3;
			vspeed = 10;
			jumpcool = 2;
			gravitydelay = 0;
			wallflip = true;
		}
    }

    bool ExtraCheck(CollisionType type, int distanceadd, bool move)
    {
        Vector2 direction = Vector2.down;
        if (type == CollisionType.up)
            direction = Vector2.up;
        if (type == CollisionType.left)
            direction = Vector2.left;
        if (type == CollisionType.right)
            direction = Vector2.right;
        Vector2 extentsx = new Vector2(Pcollide[0].bounds.max.x, Pcollide[0].bounds.min.x);
        Vector2 extentsy = new Vector2(Pcollide[0].bounds.max.y, Pcollide[0].bounds.min.y);
        Vector2 position = Pcollide[0].transform.position;
        Vector2 position2 = new Vector2(Pcollide[0].transform.position.x, extentsy.x);
        Vector2 position3 = new Vector2(Pcollide[0].transform.position.x, extentsy.y);
        if (type == CollisionType.down)
        {
            position2 = new Vector2(extentsx.x, Pcollide[0].transform.position.y);
            position3 = new Vector2(extentsx.y, Pcollide[0].transform.position.y);
        }
        float distance = Pcollide[0].size.y * 2;
        if (type == CollisionType.left || type == CollisionType.right)
            distance = Pcollide[0].size.x * 2 + 0.1f;

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, layer);
        RaycastHit2D hit2 = Physics2D.Raycast(position2, direction, distance, layer);
        RaycastHit2D hit3 = Physics2D.Raycast(position3, direction, distance, layer);
        if (hit.collider != null)
        {
            if (type == CollisionType.down && vspeed < 0 && move == true)
            {
                trans.position = new Vector2(hit.point.x, hit.point.y + Pcollide[0].size.y * 2);
            }
            if (type == CollisionType.up && vspeed > 0 && move == true)
            {
                trans.position = new Vector2(hit.point.x, hit.point.y - Pcollide[0].size.y * 2);
            }
            if (type == CollisionType.right && Input.GetKey(Right) && move == true && rightlock == false)
            {
                trans.position = new Vector2(hit.point.x - Pcollide[0].size.x * 2, hit.point.y);
            }
            if (type == CollisionType.left && Input.GetKey(Left) && move == true && leftlock == false)
            {
                trans.position = new Vector2(hit.point.x + Pcollide[0].size.x * 2, hit.point.y);
            }
            return true;
        }
        if (hit2.collider != null)
        {
            return true;
        }
        if (hit3.collider != null)
        {
            return true;
        }
        return false;

    }

    bool WallCheck(CollisionType type, int distanceadd, bool move)
    {
		Vector2 direction = Vector2.down;
		if(type==CollisionType.up)
			direction = Vector2.up;
		if (type == CollisionType.left)
			direction = Vector2.left;
		if (type == CollisionType.right)
			direction = Vector2.right;
        Vector2 extentsx = new Vector2(Pcollide[0].bounds.max.x-0.01f , Pcollide[0].bounds.min.x+0.01f );
        Vector2 extentsy = new Vector2(Pcollide[0].bounds.max.y-0.01f , Pcollide[0].bounds.min.y+0.01f );
        Vector2 position = Pcollide[0].transform.position;
		Vector2 position2 = new Vector2 (Pcollide [0].transform.position.x,extentsy.x);
		Vector2 position3 = new Vector2 (Pcollide [0].transform.position.x,extentsy.y);
		if (type == CollisionType.down) {
			position2 = new Vector2 (extentsx.x, Pcollide [0].transform.position.y);
			position3 = new Vector2 (extentsx.y, Pcollide [0].transform.position.y);
		}
        float distance = Pcollide[0].size.y*2;
		if(type==CollisionType.left||type==CollisionType.right)
		distance = Pcollide[0].size.x*2 + 0.1f;
        
        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, layer);
        RaycastHit2D hit2 = Physics2D.Raycast(position2, direction, distance, layer);
        RaycastHit2D hit3 = Physics2D.Raycast(position3, direction, distance, layer);
        if (hit.collider != null)
        {
			if (type == CollisionType.down && vspeed < 0 && move==true)
            {
                trans.position = new Vector2(hit.point.x, hit.point.y + Pcollide[0].size.y*2);
            }
			if (type == CollisionType.up && vspeed > 0 && move==true)
			{
				trans.position = new Vector2(hit.point.x, hit.point.y - Pcollide[0].size.y*2);
			}
            if (type == CollisionType.right && Input.GetKey(Right) && move==true && rightlock==false)
            {
                trans.position = new Vector2(hit.point.x - Pcollide[0].size.x * 2, hit.point.y);
            }
            if (type == CollisionType.left && Input.GetKey(Left) && move==true && leftlock==false)
            {
                trans.position = new Vector2(hit.point.x + Pcollide[0].size.x * 2, hit.point.y);
            }
            return true;
        }
        if (hit2.collider != null)
        {
            return true;
        }
        if (hit3.collider != null)
        {
            return true;
		}
        return false;

    }

    void OnCollisionEnter(Collision collision)
    {
		if (collision.gameObject.tag == "Enemy")
		{
			Physics.IgnoreCollision(Pcollide[0].GetComponent<Collider>(),collision.gameObject.GetComponent<Collider>());
		}
    }
}