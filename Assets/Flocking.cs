using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocking : MonoBehaviour {

    public float speed;
    public float rotatespeed;
    public float speedadjustment;
    private bool catchup;


	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime * (catchup ? speedadjustment : 1));



    }
}
