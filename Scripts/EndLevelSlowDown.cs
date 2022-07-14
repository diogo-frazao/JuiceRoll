using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelSlowDown : MonoBehaviour
{
    [SerializeField]
    private float playerSlowDownSpeed = 1f;

    [SerializeField]
    private float tapBarDecreaseAmount = 0.5f;

    private bool bCanPlayerActivateSlowDown = true;
    private bool bIsReadingPlayerTaps = true;

    public float CurrentNumberTaps { get; private set; } = 0f;
    public static float MaxNumberTaps { get; private set; } = 5f;

    private void Update()
    {
        if (bCanPlayerActivateSlowDown || !bIsReadingPlayerTaps) { return; }

        // Count player taps to update UI
        if (Input.GetMouseButtonDown(0) && CurrentNumberTaps < MaxNumberTaps)
        {
            CurrentNumberTaps += 1f;
        }

        // Make bar go down over time
        if (CurrentNumberTaps > 0f)
        {
            // How much the bar decreases per second. In this case 10% of total
            CurrentNumberTaps -= Time.deltaTime * CurrentNumberTaps * tapBarDecreaseAmount;
        }
        else
        {
            CurrentNumberTaps = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sponge") && bCanPlayerActivateSlowDown)
        {
            ForwardMoveController otherForwardMoveController = other.GetComponentInParent<ForwardMoveController>();
            SweepMoveController otherSweepMoveController = other.GetComponentInParent<SweepMoveController>();
            if (otherForwardMoveController == null || otherSweepMoveController == null) { return; }

            // Slow Down Player
            otherForwardMoveController.SetMoveSpeed(playerSlowDownSpeed);
            otherSweepMoveController.SetCanSweep(false);
            // Stop sponge from shrinking
            other.GetComponent<SpongeRoll>().SetCanShrinkSponge(false);
            // Activate tap bar
            UIManager.Instance.ActivatePlayerTapBar(true);
            // Start bar half filled
            CurrentNumberTaps = MaxNumberTaps * 0.5f;

            bCanPlayerActivateSlowDown = false;
        }
    }
}
