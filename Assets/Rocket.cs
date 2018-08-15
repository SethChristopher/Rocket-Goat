using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    [SerializeField] float rcsThrust = 10f;
    [SerializeField] float mainThrust = 10f;
    Rigidbody rigidBody;
    AudioSource audioSource;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        Rotate();
        Thrust();
        PlaySound();
	}

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("friendly!");//do nothing
                break;
            case "Fuel":
                print("fueled");
                break;
            default:
                print("Dead");
                break;
        }
    }

    private void PlaySound()
    {
        if (Input.GetKeyDown("space"))
        {
            audioSource.Play();
        }
        if (Input.GetKeyUp("space"))
        {
            audioSource.Stop();
        }
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true; //take manual control of rotation
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; //give physics control of rotation
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
        }
    }
}
