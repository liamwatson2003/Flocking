using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocking : MonoBehaviour {

    public float speed;
    public float maxSpeed = 1.2f;
    public float minSpeed = 0.8f;
    public float rotateSpeed;
    public float speedAdjustment;
    public int CheckNumber = 3;
    Collider[] ObjectsHit;

    public float searchRadius;


	// Use this for initialization
	void Start ()
    {
        ObjectsHit = new Collider[CheckNumber];
        speed = Random.Range(minSpeed, maxSpeed);
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * Random.Range(3,45));
        //move the object forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        //check for other objects within range
        
        Physics.OverlapSphereNonAlloc(transform.position, searchRadius, ObjectsHit);
        if (ObjectsHit.Length > 1)
        {
            foreach (Collider CObject in ObjectsHit)
            {

                //change rotation
                float step = rotateSpeed * Time.deltaTime;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, CObject.transform.rotation, step);

                //get the other objects speed
                float otherspeed = CObject.transform.GetComponent<Flocking>().speed;

                    //average the speed out between th other objects
                    if (speed < otherspeed && speed <= maxSpeed)
                    {  
                        speed += speedAdjustment *2;
                    }
                    if (speed > otherspeed && speed >= minSpeed)
                    {
                        speed -= speedAdjustment*2;
                    }
                //adding break here increased performance massivly
                break;
            }
        }
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
