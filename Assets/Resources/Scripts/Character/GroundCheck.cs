using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollisionType
{
    down,
    left,
    right,
    up
}

public class GroundCheck : MonoBehaviour {

    public LayerMask layer;

    // Use this for initialization
    void Start()
    {

    }
	// Update is called once per frame
	void Update () {
		
	}
    public bool WallCheck(BoxCollider2D Pcollide,CollisionType type, int distanceadd,bool bound, float ysp, bool holding)
    {
        PController pc = GetComponent<PController>();
        bool move = true;
        Transform trans = Pcollide.transform;
        Vector2 direction = Vector2.down;
        if (type == CollisionType.up)
            direction = Vector2.up;
        if (type == CollisionType.left)
            direction = Vector2.left;
        if (type == CollisionType.right)
            direction = Vector2.right;
        Vector2 extentsx = new Vector2(Pcollide.bounds.max.x - 0.1f, Pcollide.bounds.min.x + 0.1f);
        Vector2 extentsy = new Vector2(Pcollide.bounds.max.y - 0.1f, Pcollide.bounds.min.y + 0.1f);
        if (bound == false)
        {
            extentsx = new Vector2(Pcollide.bounds.max.x + 0.1f, Pcollide.bounds.min.x - 0.1f);
            extentsy = new Vector2(Pcollide.bounds.max.y + 0.1f, Pcollide.bounds.min.y - 0.1f);
        }
        Vector2 position = Pcollide.transform.position;
        Vector2 position2 = new Vector2(Pcollide.transform.position.x, extentsy.x);
        Vector2 position3 = new Vector2(Pcollide.transform.position.x, extentsy.y);
        if (type == CollisionType.down)
        {
            position2 = new Vector2(extentsx.x, Pcollide.transform.position.y);
            position3 = new Vector2(extentsx.y, Pcollide.transform.position.y);
        }
        float distance = Pcollide.size.y * 2 +0.1f;
        if (type == CollisionType.left || type==CollisionType.right)
            distance = Pcollide.size.x * 2 + 0.1f;
        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, layer);
        RaycastHit2D hit2 = Physics2D.Raycast(position2, direction, distance, layer);
        RaycastHit2D hit3 = Physics2D.Raycast(position3, direction, distance, layer);
        if (hit.collider != null)
        {
            if (type == CollisionType.down && ysp < 0 && move == true)
            {

                trans.position = new Vector2(hit.point.x, hit.point.y + Pcollide.size.y * 2);
            }
            if (type == CollisionType.up && ysp > 0 && move == true && bound==true)
            {
                pc.ysp = 0;
                trans.position = new Vector2(hit.point.x, hit.point.y - Pcollide.size.y * 2);
            }
            if (type == CollisionType.right && pc.xsp>0 && move == true)
            {
                pc.xsp = 0;
                trans.position = new Vector2(hit.point.x - Pcollide.size.x * 2-0.01f, hit.point.y);
            }
            if (type == CollisionType.left && pc.xsp < 0 && move == true)
            {
                pc.xsp = 0;
                trans.position = new Vector2(hit.point.x + Pcollide.size.x * 2+0.02f, hit.point.y);
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
}
