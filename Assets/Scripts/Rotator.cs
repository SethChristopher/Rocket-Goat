using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Rotator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    
    [SerializeField] float rotateSpeed = 2f;

    Quaternion movementFactor; // 0 for not moved 1 for moved
    Vector3 startingPos;
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (rotateSpeed <= Mathf.Epsilon) { return; }
        float cycles = Time.time / rotateSpeed; // grows continually from 0

        const float TAU = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles * TAU);


        movementFactor = rotateSpeed;
        Quaternion offset = movementFactor;
        transform.rotation = startingPos + offset;
      
    }
}
