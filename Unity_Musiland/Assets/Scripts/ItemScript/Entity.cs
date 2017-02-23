using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

    Rigidbody2D rigid;
    SpriteRenderer sprite;
    float radius;
    public float follow_porteemax;
    public float follow_porteemin;
    [SerializeField]
    Collider2D Maincollider;

    // == GoTo
    public float speed;
    public bool isgoto =false;
    Vector3 CurrentDestination;

    // == Patrol
    public bool ispatrol = false;
    Vector3 PatrolDest1, PatrolDest2;
    bool patrolphase;

    // == Rotation
    float CurrentDestinaRotation;
    public float rotaspeed;
    bool isrotating = false;
    float PatrolRota1;


    // == Follow & Fleeing
    public bool isfollowing=false;
    GameObject CurrentFollowTarget;
    [SerializeField]
    bool isflying;
    public bool isfleeing = false;

    // == Stay
    Vector3 CurrentPointToGuard;
    public bool isstaying;
    public float TimeAllowedToStayAway;
    float Timeaway;
    public HitDirection hitside;

    // Use this for initialization
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

    }


	// Update is called once per frame
	protected virtual void Update () {

        float step = speed * Time.deltaTime;
        float rotastep = rotaspeed * Time.deltaTime;

            // ========== Si mode one way ==========
            if (isgoto)
        {
            if ( (CurrentDestination.x > transform.position.x && hitside == HitDirection.Right) ||
                (CurrentDestination.x < transform.position.x && hitside == HitDirection.Left)) {
                isgoto = false;
                isfleeing = false;
                isfollowing = false;
            }
            transform.position = Vector3.MoveTowards(transform.position, CurrentDestination, step);
            if (transform.position == CurrentDestination)
            {
                isgoto = false;
            }
        } // ========================================

        // ========== Si mode patrouille ==========
        if (ispatrol) { 
        
            if (patrolphase)
            {
                transform.position = Vector3.MoveTowards(transform.position, PatrolDest1, step);
                if (transform.position == PatrolDest1) patrolphase = !patrolphase;
            }else
            {
                transform.position = Vector3.MoveTowards(transform.position, PatrolDest2, step);
                if (transform.position == PatrolDest2) patrolphase = !patrolphase;
            }
        } // ========================================

        // ========== ROTATION ========== 
        if (isrotating)
        {
			Vector3 to = new Vector3(0, 0, CurrentDestinaRotation);
            if (Vector3.Distance(transform.eulerAngles, to) > 0.01f)
            {
                transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, to, rotastep);
            }
            else
            {
                transform.eulerAngles = to;
                isrotating = false;
            }
            
        } // ========================================

        // ========== FOLLOW ========== 
        if (isfollowing)
        {

            if (Vector3.Distance(transform.position, CurrentFollowTarget.transform.position) < follow_porteemax &&
                Vector3.Distance(transform.position, CurrentFollowTarget.transform.position) > follow_porteemin)
            {
                if (isflying) // Si l'entité n'a pas de rigidbody
                {
                    Entity_GoTo(CurrentFollowTarget.transform.position, transform.rotation.z);
                }
                else
                {
                    Entity_GoTo(new Vector3 (CurrentFollowTarget.transform.position.x,transform.position.y, transform.position.z), transform.rotation.z);
                }
            } else
            {
                isfollowing = false;
            }
        } // ========================================

        // ========== FLEE ========== 
        if (isfleeing)
        {
            if (Vector3.Distance(transform.position, CurrentFollowTarget.transform.position) < follow_porteemax)
            {

                float dirx = transform.position.x - CurrentFollowTarget.transform.position.x;
                dirx = dirx / (Mathf.Abs(dirx));
                Vector3 dir = new Vector3(transform.position.x + dirx, transform.position.y, transform.position.z);
                if ((dir.x > transform.position.x && hitside != HitDirection.Right) ||
                    (dir.x < transform.position.x && hitside != HitDirection.Left))
                {

                    //print(dirx);
                    //Vector3 dir = new Vector3(dirx, 0, 0);
                    Entity_GoTo(new Vector3(transform.position.x+dirx, transform.position.y, transform.position.z), transform.rotation.z);
                    
                } else {  }
               
            }
        }

        if (isstaying)
        {
            if (transform.position != CurrentPointToGuard)
            {
                Timeaway += Time.deltaTime;
                if (Timeaway > TimeAllowedToStayAway)
                {
                    Timeaway = 0;
                    Entity_GoTo(CurrentPointToGuard, transform.rotation.z);
                }
            } else Timeaway = 0;
        }

    } // ==================================================================================================== //
      // ===================================== FIN UPDATE =================================================== //
      // ==================================================================================================== //


    // ===== L'entité va à l'endroit X
    public void Entity_GoTo(Vector3 location, float rotation)
    {
        isgoto = true;
		isrotating = true;
        CurrentDestination = location;
        CurrentDestinaRotation = rotation;
    }

    // ===== L'entité alterne entre deux endroits
    public void Entity_Patrol(Vector3 location1, Vector3 location2)
    {

        ispatrol = true; isfollowing = false; isfleeing = false;
        PatrolDest1 = location1;
        PatrolDest2 = location2;

    }

    // ===== Fait tourner l'entité jusqu'à l'angle choisi
    public void Entity_Rotate(float rotation1)
    {
        isrotating = true; isfollowing = false; isfleeing = false;
        PatrolRota1 = rotation1;
    }

    // ===== L'entité suit une autre
    public void Entity_Follow(GameObject cible)
    {
        CurrentFollowTarget = cible;
        ispatrol = false; isrotating = false; isgoto = false; isfleeing = false;
        isfollowing = true;

    }

    // ===== L'entité fuit une autre
    public void Entity_Flee(GameObject fuite)
    {
        CurrentFollowTarget = fuite;
        ispatrol = false; isrotating = false; isgoto = false; isfollowing = false;
        isfleeing = true;
    }

    // ===== L'entité essaye de rester à un endroit
    public void Entity_Stay(Vector3 positiontostay)
    {
        isstaying = true;
        CurrentPointToGuard = positiontostay;
    }

    public void Entity_StopStay()
    {
        isstaying = false;
    }

    // ===== STOP TOUT
    public void Entity_Stop()
    {
        ispatrol = false; isrotating = false; isgoto = false; isfollowing = false;
        isfleeing = false; isstaying = false;
    }


    // ================================ //
    // =========== COLLISION ========== //


    // =========================================
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (ReturnDirection(collision.gameObject, this.gameObject) == HitDirection.Left || ReturnDirection(collision.gameObject, this.gameObject) == HitDirection.Right)
        {
            hitside = ReturnDirection(collision.gameObject, this.gameObject);
            if (isgoto)
            {

            }
        }
    }
    // =========================================
    void OnCollisionStay2D(Collision2D collision)
    {
    }
    // =========================================
    void OnCollisionExit2D(Collision2D collision)
    {
        if (ReturnDirection(collision.gameObject, this.gameObject) == HitDirection.Left || ReturnDirection(collision.gameObject, this.gameObject) == HitDirection.Right)
        {
            hitside = HitDirection.None;
        }
    }

    // ========== COLLISION ============ //
    public enum HitDirection { None, Top, Bottom, Left, Right }
    private HitDirection ReturnDirection(GameObject Object, GameObject ObjectHit)
    {

        HitDirection hitDirection = HitDirection.None;
        RaycastHit MyRayHit;
        Vector3 direction = (Object.transform.position - ObjectHit.transform.position).normalized;
        Ray MyRay = new Ray(ObjectHit.transform.position, direction);

        if (Physics.Raycast(MyRay, out MyRayHit))
        {

            if (MyRayHit.collider != null)
            {

                Vector3 MyNormal = MyRayHit.normal;
                MyNormal = MyRayHit.transform.TransformDirection(MyNormal);

                if (MyNormal == MyRayHit.transform.up) { hitDirection = HitDirection.Top; }
                if (MyNormal == -MyRayHit.transform.up) { hitDirection = HitDirection.Bottom; }
                if (MyNormal == MyRayHit.transform.right) { hitDirection = HitDirection.Right; }
                if (MyNormal == -MyRayHit.transform.right) { hitDirection = HitDirection.Left; }
            }
        }
        return hitDirection;
    }

}


