using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocking : MonoBehaviour {

    public float speed;
    public float maxSpeed = 1.2f;
    public float minSpeed = 0.8f;
    
    public float speedAdjustment;
    public int CheckOtherNumber = 3;
    Collider[] ObjectsHit;

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

    public bool rotateAwayFromOther = false;
    public float rotateAwayFromOtherSpeed;

    public float rotateAwayFromDetectionRange = 5;
    private int turndir;
    // Use this for initialization
    void Start ()
    {
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
        //is the flock circling.
        

        //move the object forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime * CatchupSpeed);


        //check for other objects within range
        Physics.OverlapSphereNonAlloc(transform.position, CheckOtherSearchRadius, ObjectsHit);
        
            foreach (Collider CObject in ObjectsHit)
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
                        Debug.Log("catching up");
                        transform.position = Vector3.MoveTowards(transform.position, CObject.transform.position, CatchupSpeed * Time.deltaTime);
                    }
                }

                if (AlignWithOther)
                {
                    //alline our rotation to the other objects
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, CObject.transform.rotation, AlignSpeed * Time.deltaTime);
                }

                if (rotateAwayFromOther)
                {
                    //get a vector 3 offset from the other object
                    Vector3 offset = transform.position - CObject.transform.position;
                    //get the magnitude
                    float sqrLen = offset.sqrMagnitude;
                    //when too close we will run this next lot of code
                    if (sqrLen < rotateAwayFromDetectionRange)
                    {
                        transform.rotation *= Quaternion.Euler(0, Random.Range(rotateAwayFromOtherSpeed - rotateAwayFromOtherSpeed, rotateAwayFromOtherSpeed), 0);
                    }
                    

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
                break;
                //adding break here increased performance massivly
            }
            //testing to see if the birds put different birds in there storage 
            System.Array.Clear(ObjectsHit, 0, CheckOtherNumber);
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
