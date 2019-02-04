using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Rocket : MonoBehaviour {

    [SerializeField] float rcsThrust = 10f;
    [SerializeField] float mainThrust = 10f;
    [SerializeField] float levelLoadDelay = 2f
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip playerDeath;
    [SerializeField] AudioClip levelFinish;
    [SerializeField] ParticleSystem playerExplode;
    [SerializeField] ParticleSystem playerSucceed;
    [SerializeField] ParticleSystem playerBoost;



    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending};
    State state = State.Alive;
	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (state == State.Alive)
        {
            RespondToRotateInput();
            RespondToThrustInput();

        }

	}

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) { return;}
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //do nothing
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(playerDeath);
        playerExplode.Play();
        Invoke("LoadFirstLevel", levelLoadDelay);

    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(levelFinish);
        playerSucceed.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void RespondToRotateInput()
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

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
            playerBoost.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        playerBoost.Play();
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        
    }

    private void LoadNextLevel()
    {
        int n = SceneManager.GetActiveScene().buildIndex;
        if (n == 2)
        {
            LoadFirstLevel();
            
        }
        else SceneManager.LoadScene(n + 1);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }
}
