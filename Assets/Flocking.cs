using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocking : MonoBehaviour {

    public float speed;
    public float maxSpeed = 1.2f;
    public float minSpeed = 0.8f;
    public float rotateSpeed;
    public float speedAdjustment;
    private bool catchup;

    public float searchRadius;


	// Use this for initialization
	void Start ()
    {
        catchup = false;
        speed = Random.Range(minSpeed, maxSpeed);
	}
	
	// Update is called once per frame
	void Update ()
    {
        //move the object forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime * (catchup ? speedAdjustment : 1));

        //check for other objects within range
        Collider[] ObjectsHit = Physics.OverlapSphere(transform.position, searchRadius);
        if (ObjectsHit.Length > 1)
        {
            foreach (Collider CObject in ObjectsHit)
            {
                
                    Debug.Log("collider working");
                    //get the other objects speed
                    float otherspeed = CObject.transform.GetComponent<Flocking>().speed;

                    //average the speed out between th other objects
                    if (speed < otherspeed && speed <= maxSpeed)
                    {
                        speed += speedAdjustment;
                    }
                    else if (speed > otherspeed && speed >= minSpeed)
                    {
                        speed -= speedAdjustment;
                    }
                    //adding break here increased performance massivly
                    break;
                
            }
        }
    }


}
