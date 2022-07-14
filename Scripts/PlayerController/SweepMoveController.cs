using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SweepMoveController : MonoBehaviour
{
    [SerializeField]
    private float leftMovementLimit = -1f;

    [SerializeField]
    private float rightMovementLimit = 1f;

    [SerializeField]
    private float sideMoveSpeed = 1f;

    private float lastFingerPositionX;
    private float moveDeltaX;

    private bool bCanSweep = true;

    private Rigidbody myRigidbody;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (bCanSweep) 
        {
            SetFingerInput();

            transform.position = new Vector3(Mathf.Clamp(transform.position.x,
                leftMovementLimit, rightMovementLimit),
                transform.position.y, transform.position.z);
        }
        else
        {
            Vector3 targetLocation = new Vector3(0f, transform.position.y, transform.position.z);

            if (Vector3.Distance(transform.position, targetLocation) > 0.05f)
            {
                const float finalMoveSpeed = 1.25f;
                transform.position = Vector3.MoveTowards(transform.position, targetLocation, 
                    finalMoveSpeed * Time.deltaTime);
            }
        }
    }

    private void FixedUpdate()
    {
        if (!bCanSweep) { return; }

        // Sideways Movement (sweep)
        Vector3 moveOffset = Vector3.right * moveDeltaX * sideMoveSpeed * Time.fixedDeltaTime;
        if (myRigidbody.position.x + moveOffset.x < rightMovementLimit && 
            myRigidbody.position.x + moveOffset.x > leftMovementLimit)
        {
            myRigidbody.MovePosition(myRigidbody.position + moveOffset);
        }
    }

    private void SetFingerInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastFingerPositionX = Input.mousePosition.x;
        }
        else if (Input.GetMouseButton(0))
        {
            moveDeltaX = Input.mousePosition.x - lastFingerPositionX;
            lastFingerPositionX = Input.mousePosition.x;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            moveDeltaX = 0;
        }
    }

    // Called by end level slow down (keep character in the middle)
    public void SetCanSweep(bool value)
    {
        bCanSweep = value;
    }
}
