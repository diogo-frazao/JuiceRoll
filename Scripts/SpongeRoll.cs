using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpongeRoll : MonoBehaviour
{
    [SerializeField]
    private float startingBallRollSpeed = 300f;

    [SerializeField]
    private float rollingSpeedMultiplier = 300f;

    [SerializeField]
    private float maxRollSpeed = 50f;

    [Range(0.01f, 0.1f)]
    [SerializeField]
    private float spongeShrinkEveryFrame = 0.025f;

    private float currentBallRollSpeed;

    private ForwardMoveController playerForwardMoveController;
    private FoodSponge playerFoodSponge;

    public bool bCanRool { get; private set; } = false;
    public bool bCanShrinkSponge { get; private set; } = true;
    private void Awake()
    {
        bCanRool = true;
        bCanShrinkSponge = true;
        currentBallRollSpeed = startingBallRollSpeed;
    }

    private void Start()
    {
        playerForwardMoveController = GetComponentInParent<ForwardMoveController>();
        playerFoodSponge = GetComponentInParent<FoodSponge>();
    }

    private void Update()
    {
        // Roll sponge
        if (bCanRool)
        {
            currentBallRollSpeed = playerForwardMoveController.GetCurrentMoveSpeed() * rollingSpeedMultiplier;
            if (currentBallRollSpeed > maxRollSpeed)
            {
                currentBallRollSpeed = maxRollSpeed;
            }
            transform.Rotate(Vector3.right * currentBallRollSpeed * Time.deltaTime);
        }

        // Shrink sponge
        if (bCanShrinkSponge)
        {
            Vector3 spongeShrinkedSize = transform.localScale -= new Vector3(spongeShrinkEveryFrame,
                spongeShrinkEveryFrame, spongeShrinkEveryFrame);

            if (spongeShrinkedSize.x <= playerFoodSponge.GetMinSpongeSize())
            {
                spongeShrinkedSize = new Vector3(playerFoodSponge.GetMinSpongeSize(),
                    playerFoodSponge.GetMinSpongeSize(),
                    playerFoodSponge.GetMinSpongeSize());
            }

            transform.localScale = spongeShrinkedSize;
        }
    }

    // Called by end level slow down
    public void SetCanShrinkSponge(bool value)
    {
        bCanShrinkSponge = value;
    }
}
