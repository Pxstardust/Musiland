using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

    Rigidbody2D rigid;
    SpriteRenderer sprite;
    float radius;
    public float followdistance;

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
    bool isfollowing=false;
    GameObject CurrentFollowTarget;
    [SerializeField]
    bool isflying;
    bool isfleeing = false;

    // == Stay
    Vector3 CurrentPointToGuard;
    private bool isstaying;
    public float TimeAllowedToStayAway;
    float Timeaway;

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
            Vector3 to = new Vector3(0, 0, PatrolRota1);
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

            if (Vector3.Distance(transform.position, CurrentFollowTarget.transform.position) < followdistance)
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
            if (Vector3.Distance(transform.position, CurrentFollowTarget.transform.position) < followdistance)
            {
                float dirx = transform.position.x - CurrentFollowTarget.transform.position.x;
                Vector3 dir = new Vector3(dirx, 0,0);
                transform.Translate(dir * speed * Time.deltaTime);
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

}


