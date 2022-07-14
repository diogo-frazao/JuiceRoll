using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ForwardMoveController : MonoBehaviour
{
    [SerializeField]
    private float startingMoveSpeed = 3f;

    [SerializeField]
    private float maxMoveSpeed = 5f;

    [SerializeField]
    private float minMoveSpeed = 2f;

    private float currentMoveSpeed;

    private Rigidbody myRigidbody;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        currentMoveSpeed = startingMoveSpeed;
    }

    private void FixedUpdate()
    {
        // Forward Movement
        Vector3 moveOffset = Vector3.forward * currentMoveSpeed * Time.fixedDeltaTime;
        myRigidbody.MovePosition(myRigidbody.position + moveOffset);
    }

    public void IncreaseMoveSpeed(float increaseAmount)
    {
        if (currentMoveSpeed + increaseAmount >= maxMoveSpeed)
        {
            currentMoveSpeed = maxMoveSpeed;
        }
        else if (currentMoveSpeed + increaseAmount <= minMoveSpeed)
        {
            currentMoveSpeed = minMoveSpeed;
        }
        else
        {
            currentMoveSpeed += increaseAmount;
        }
    }

    public void SetMoveSpeed(float newMoveSpeed)
    {
        currentMoveSpeed = newMoveSpeed;
    }

    public float GetCurrentMoveSpeed()
    {
        return currentMoveSpeed;
    }
}
