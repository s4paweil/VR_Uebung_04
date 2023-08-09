using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class SparrowController : MonoBehaviour
{
    public float movementSpeed;
    
    public GameObject world;

    private Vector3 currentPosition;
    private Vector3 targetPosition;
    private Vector3 defaultPosition;

    // Start is called before the first frame update
    void Start()
    {
        currentPosition = targetPosition = defaultPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("up"))
        {
            if (targetPosition.y < defaultPosition.y + 1)
            {
                targetPosition = targetPosition + Vector3.up;
                //Debug.Log(targetPosition);
            }
        }
        if (Input.GetKeyDown("down"))
        {
            if (targetPosition.y > defaultPosition.y - 1)
            targetPosition = targetPosition + Vector3.down;
            //Debug.Log(targetPosition);
        }
        if (Input.GetKeyDown("left"))
        {
            if (targetPosition.x > defaultPosition.x - 1)
            {
                targetPosition = targetPosition + Vector3.left;
                //Debug.Log(targetPosition);
            }
        }
        if (Input.GetKeyDown("right"))
        {
            if (targetPosition.x < defaultPosition.x + 1)
            {
                targetPosition = targetPosition + Vector3.right;
                //Debug.Log(targetPosition);
            }
        }

        if (currentPosition != targetPosition)
        {
            transform.position = Vector3.Lerp(currentPosition, targetPosition, movementSpeed * Time.deltaTime);
            currentPosition = transform.position;
        }
        
    }
}
