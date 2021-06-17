using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    private bool alert;
    public bool directionface;
    public GameObject player;
	private BoxCollider2D[] collide;
	public int speedlimit;
	public int sp;
	public int vspeed;
	private Transform trans;
	private Rigidbody2D rb;
	public LayerMask layer;
	public LayerMask playermask;
	public bool attacking;
	public string attack;
    public int attackcool;
	private bool surprised;
	private AnimationManager anim;
	public GameObject death;
	enum CollisionType
	{
		down,
		left,
		right,
		up
	}

	public void Kill(){
		GameObject d=(GameObject)Instantiate (death);
		d.transform.position = trans.position;
		Destroy (gameObject);
	}

	// Use this for initialization
	void Start () {
		surprised = false;
        attackcool = 0;
		anim = gameObject.GetComponent<AnimationManager> ();
		attacking = false;
		rb = gameObject.GetComponent<Rigidbody2D> ();
		trans = gameObject.transform;
		sp = 0;
		vspeed = 0;
		collide = gameObject.GetComponents<BoxCollider2D>();
        alert = false;
        player = GameObject.FindGameObjectWithTag("Player");
	}

	public void Attackend(){
		attacking = false;
        if (attack == "combo1")
        {
            attackcool = 60;
        }
    }

    void Attacking()
    {
        if (attack == "combo1")
            sp = 0;
        List<GameObject> collision = new List<GameObject>();
        GameObject child = gameObject.transform.Find(attack).gameObject;
        Attackscript Atk = child.GetComponent<Attackscript>();
        collision = Atk.AttackCheck(anim.frame);
        for (int i = 0; i < collision.Count; i++)
        {
			if (collision [i] && collision [i].tag == "Player") {
				PController Pcon = collision [i].GetComponent<PController> ();
				Pcon.Kill (!directionface);
			}
        }
    }

    void Alert(){
		if (player!=null) {
			if (surprised == false) {
				GameObject exclaim=Resources.Load<GameObject> ("Prefabs/exclaim");
				GameObject exc = (GameObject)Instantiate (exclaim);
				exc.transform.transform.position = new Vector2 (trans.position.x, trans.position.y +1f);
				surprised = true;
				attackcool = 15;
			}
			GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
			for (int i = 0; i < enemies.Length; ++i) {
				if (enemies [i] && enemies [i] != gameObject && Vector2.Distance (gameObject.transform.position, enemies [i].transform.position) < 10f) {
					EnemyController enemy = enemies [i].GetComponent<EnemyController> ();
					enemy.alert = true;
				}
			}
			Vector2 playervector = player.GetComponent<BoxCollider2D>().transform.position - gameObject.transform.position;
			float distance = playervector.magnitude;
            if (distance < 0.5f && attackcool==0)
            {
				sp = 0;
                attack = "combo1";
                attacking = true;
            }
			if (sp > speedlimit)
				sp = speedlimit;
			if (sp < -speedlimit)
				sp = -speedlimit;
			if (player.transform.position.x > trans.position.x && !WallCheck (CollisionType.right, 0, false) && distance > 1.0f)
				++sp;
			if (player.transform.position.x < trans.position.x && !WallCheck (CollisionType.left, 0, false) && distance > 1.0f)
				--sp;
			if (WallCheck (CollisionType.left, 0, true) == true && sp<0)
				sp = 0;
			if (WallCheck (CollisionType.right, 0, true) == true && sp>0)
				sp = 0;
		}
	}

	void FixedUpdate(){
        if (attackcool > 0)
            --attackcool;
		if (WallCheck (CollisionType.down, 1, true) == false) {
			--vspeed;
		}
		if (player == null)
			sp = 0;
		if (alert == true)
			Alert ();
	}
	
	// Update is called once per frame
	void Update () {
        player = GameObject.FindGameObjectWithTag("Player");
        
        Animation ();
        if (attacking == true)
            Attacking();
		if (sp > 0)
			directionface = false;
		if (sp < 0)
			directionface = true;
		
		rb.velocity = new Vector2 (sp, vspeed);
		if (PlayerCheck () == true)
			alert = true;
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
		Vector2 extentsx = new Vector2(collide[0].bounds.max.x-0.1f , collide[0].bounds.min.x+0.1f );
		Vector2 extentsy = new Vector2(collide[0].bounds.max.y-0.1f , collide[0].bounds.min.y+0.1f );
		Vector2 position = collide[0].transform.position;
		Vector2 position2 = new Vector2 (collide [0].transform.position.x,extentsy.x);
		Vector2 position3 = new Vector2 (collide [0].transform.position.x,extentsy.y);
		if (type == CollisionType.down) {
			position2 = new Vector2 (extentsx.x, collide [0].transform.position.y);
			position3 = new Vector2 (extentsx.y, collide [0].transform.position.y);
		}
		float distance = collide[0].size.y*2;
		if(type==CollisionType.left||type==CollisionType.right)
			distance = collide[0].size.x*2 + 0.1f;


		RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, layer);
		RaycastHit2D hit2 = Physics2D.Raycast(position2, direction, distance, layer);
		RaycastHit2D hit3 = Physics2D.Raycast(position3, direction, distance, layer);
		if (hit.collider != null)
		{
			if (type == CollisionType.down && vspeed < 0 && move==true)
			{
				rb.velocity = new Vector2(0f, 0f);
				trans.position = new Vector2(hit.point.x, hit.point.y + collide[0].size.y*2);
				vspeed = 0;
			}
			if (type == CollisionType.right && move==true && sp>0)
			{
				sp = 0;
				trans.position = new Vector2(hit.point.x - collide[0].size.x * 2, hit.point.y);
			}
			if (type == CollisionType.left && move==true && sp<0)
			{
				sp = 0;
				trans.position = new Vector2(hit.point.x + collide[0].size.x * 2, hit.point.y);
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

	void Animation(){
		anim.chr = "White";
		int wallhang = 0;
		bool air = true;
		anim.loop = true;
		if (WallCheck (CollisionType.down, 0, false))
			air = false;
		if(directionface==false)
			anim.selfrender.flipX = true;
		if(directionface==true)
			anim.selfrender.flipX = false;
		if(attacking==false)
			anim.speed = 15f;
		if (air == false && attacking==false) {
			if (sp != 0 && anim.spr != "shogunrun")
				anim.spritechange ("shogunrun");
			if (sp == 0)
				anim.spritechange ("shogun");
		}
		if (air == true && attacking==false) {
			if (anim.spr != "shogunjump" && wallhang==0)
				anim.spritechange ("shogunjump");
			if (anim.spr != "shogunbackflip" && wallhang == 0) {
				if (sp < 0)
					directionface = false;
				if (sp > 0)
					directionface = true;
				anim.spritechange ("shogunbackflip");
				anim.loop = false;
				}
			}
		if (attacking == true) {
			anim.speed = 30f;
			anim.loop = false;
			if (anim.spr != attack)
				anim.spritechange(attack);
		}
	}

    bool PlayerCheck()
    {
        
        if (player == null)
            return false;
        var playervector = player.transform.position - gameObject.transform.position;
        var distance = playervector.magnitude;
        Vector2 direction = playervector / distance;
        if (directionface == true && direction.x > -0.7f)
        {
            return false;

        }
		if (directionface==false && direction.x < 0.7f)
        {
            return false;
        }

		Debug.DrawRay (gameObject.transform.position, new Vector2 (1f, 1f));
		Debug.DrawRay (gameObject.transform.position, new Vector2 (1f, -1f));
		RaycastHit2D ray = Physics2D.Raycast(gameObject.transform.position, direction, 6.0f);
        Debug.DrawRay (gameObject.transform.position, direction, Color.red);
        if (ray)
        {
            Debug.Log(ray.collider.gameObject.tag);
            Debug.DrawRay (gameObject.transform.position, direction, Color.red);
			if (ray.collider.gameObject.tag == "Player")
                return true;
        }
        return false;
    }
}
