using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocking : MonoBehaviour {
    public Animator anim;

    public float speed;
    public float maxSpeed = 1.2f;
    public float minSpeed = 0.8f;
    
    public float speedAdjustment;
    public int CheckOtherNumber = 3;

    //need to work out allocation of the array for the bird locating other birds
    public Collider[] ObjectsHit;

    public float CheckOtherSearchRadius;
    public bool AlignWithOther = false;
    public float AlignSpeed;
    public bool Turning = false;
    private float TurningTimer;
    public float TurningTime;
    public bool Circling = false;
    public float CirclingRotateMin;
    public float CirclingRotateMax;

    public bool Catchup;
    public float CatchupSpeed = 1;
    public float CatchupMinDistance;

    public bool TooClose;
    public float TooCloseSpeed = 1;
    public float TooCloseDistance;

    

    
    private int turndir;
    // Use this for initialization
    void Start ()
    {
        anim.speed = Random.Range(1, 3);
        //create a new array to the size of our spec
        ObjectsHit = new Collider[CheckOtherNumber];
        speed = Random.Range(minSpeed, maxSpeed);
        turndir = Random.Range(0, 2);
    }


	// Update is called once per frame
	void Update ()
    {
        if (Turning)
        {
            TurningTimer -= Time.deltaTime;
            if (Circling)
            {
                transform.Rotate(Vector3.up * Time.deltaTime * Random.Range(CirclingRotateMin, CirclingRotateMax));
            }
            else
            {
                if (TurningTimer < 0)
                {
                    turndir = Random.Range(0, 2);
                    TurningTimer = TurningTime;
                }

                    if (turndir == 0)
                        transform.Rotate(Vector3.up * Time.deltaTime * Random.Range(CirclingRotateMin, CirclingRotateMax));
                    if (turndir == 1)
                        transform.Rotate(Vector3.down * Time.deltaTime * Random.Range(CirclingRotateMin, CirclingRotateMax));
            }

        }

            //move the object forward
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

        //set the array to null before use
        for (int i = 0; i < CheckOtherNumber; i++)
        {
            ObjectsHit[i] = null;
        }

        //check for other objects within range
        Physics.OverlapSphereNonAlloc(transform.position, CheckOtherSearchRadius, ObjectsHit);
        
            foreach (Collider CObject in ObjectsHit)
            {
            if (CObject != null)
            {
                //make sure where not hitting ourself
                if (CObject.transform.position != transform.position)
                {
                    //try and catchup with the other object
                    if (Catchup)
                    {
                        //get a vector 3 offset from the other object
                        Vector3 offset = transform.position - CObject.transform.position;
                        //get the magnitude
                        float sqrLen = offset.sqrMagnitude;
                        //when too close we will run this next lot of code
                        if (sqrLen > CatchupMinDistance)
                        {
                            Debug.Log("catching up, mag = " + sqrLen + " , this is the catchup distance " + CatchupMinDistance);
                            transform.position = Vector3.MoveTowards(transform.position, CObject.transform.position, CatchupSpeed * Time.deltaTime);
                        }
                    }

                    if (TooClose)
                    {
                        //get a vector 3 offset from the other object
                        Vector3 offset2 = transform.position - CObject.transform.position;
                        //get the magnitude
                        float sqrLen2 = offset2.sqrMagnitude;
                        //when too close we will run this next lot of code
                        if (sqrLen2 < TooCloseDistance)
                        {
                            Quaternion old = transform.rotation;
                            Debug.Log("cMoving away, mag = " + sqrLen2 + " , this is the move away distance " + TooCloseDistance);
                            transform.LookAt(CObject.transform);
                            transform.Rotate(0, 180, 0);
                            transform.Translate(Vector3.forward * TooCloseSpeed * Time.deltaTime);
                            transform.rotation = old;
                        }
                    }


                    if (AlignWithOther)
                    {
                        //alline our rotation to the other objects
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, CObject.transform.rotation, AlignSpeed * Time.deltaTime);
                    }

                  

                    //get the other objects speed
                    float otherspeed = CObject.transform.GetComponent<Flocking>().speed;

                    //average the speed out between th other objects
                    if (speed < otherspeed && speed <= maxSpeed)
                    {
                        speed += speedAdjustment * 2;
                    }
                    if (speed > otherspeed && speed >= minSpeed)
                    {
                        speed -= speedAdjustment * 2;
                    }
                    //break;
                    //adding break here increased performance massivly
                }
            }
        }
        /////////////
       

        if (ObjectsHit.Length <= 1)
        {
            int RND = Random.Range(0, 2);
            if (RND == 0 && speed <= maxSpeed)
                speed += speedAdjustment;
            else if (RND == 1 && speed >= minSpeed)
                speed -= speedAdjustment;
        }
    }
}
