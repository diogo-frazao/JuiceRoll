using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustPlayerPosition : MonoBehaviour
{
    [SerializeField]
    private Transform playerHipsTransform;

    [SerializeField]
    private LayerMask groundLayerMask;

    public float playerHipsYPosition = 0.7f;
    public float playerHipsZPosition = -7.1f;

    private LayerMask spongeLayerMask;
    private Animator myAnimator;

    private bool bCanAdjustPlayerToSponge = true;

    private void Awake()
    {
        bCanAdjustPlayerToSponge = true;
        myAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        spongeLayerMask = GetComponent<FoodSponge>().GetSpongeLayer();
    }

    private void Update()
    {
        if (!bCanAdjustPlayerToSponge) { return; }

        // Adjust position to be near sponge
        AdjustFrontTraceToSponge(true);
        // Adjust position to be always stuck to the floor
        AdjustTopDownTraceToGround(true);

        playerHipsTransform.position = new Vector3(transform.position.x,
            playerHipsYPosition, playerHipsZPosition);
    }

    private void AdjustFrontTraceToSponge(bool bEnableTraceDebug)
    {
        RaycastHit frontHit;
        if (Physics.Raycast(playerHipsTransform.position, Vector3.forward, out frontHit, 3f, spongeLayerMask))
        {
            playerHipsZPosition = frontHit.point.z - 0.2f;

            if (!bEnableTraceDebug) { return; }
            Debug.DrawLine(playerHipsTransform.position,
                playerHipsTransform.position + Vector3.forward * 3f, Color.blue);
        }
        else
        {
            if (!bEnableTraceDebug) { return; }
            Debug.DrawLine(playerHipsTransform.position,
                playerHipsTransform.position + Vector3.forward * 3f, Color.red);
        }
    }

    private void AdjustTopDownTraceToGround(bool bEnableTraceDebug)
    {
        RaycastHit topDownTrace;
        if (Physics.Raycast(playerHipsTransform.position, Vector3.down, out topDownTrace, 3f, groundLayerMask))
        {
            playerHipsYPosition = topDownTrace.point.y + 0.125f;

            if (!bEnableTraceDebug) { return; }
            Debug.DrawLine(playerHipsTransform.position,
                playerHipsTransform.position + Vector3.down * 3f, Color.green);
        }
        else
        {
            if (!bEnableTraceDebug) { return; }
            Debug.DrawLine(playerHipsTransform.position,
                playerHipsTransform.position + Vector3.down * 3f, Color.red);
        }
    }

    public void EnablePlayerPunch()
    {
        ForwardMoveController forwardMoveController = GetComponent<ForwardMoveController>();
        if (forwardMoveController == null) { return; }
        forwardMoveController.SetMoveSpeed(0.5f);
        myAnimator.SetTrigger("punchSponge");
    }

    public void SetCanAdjustPlayerToSponge(bool value)
    {
        bCanAdjustPlayerToSponge = value;
        // Don't go trhough floor
        GetComponent<Rigidbody>().useGravity = false;
    }
}
