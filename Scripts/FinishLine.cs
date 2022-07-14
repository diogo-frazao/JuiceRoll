using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    [Tooltip("Keep value from 0-1 in all axis")]
    [SerializeField]
    private Vector3 spongePunchForceDirection;

    [SerializeField]
    private float spongePunchForceMultiplier = 20f;

    [SerializeField]
    private float slowDownTimeScale = 0.35f;

    private bool bCanActivatatePlayerPunch = true;

    // Final player taps when crosses finish line
    public float FinalPlayerTaps { get; private set; }

    private void Awake()
    {
        bCanActivatatePlayerPunch = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sponge") && bCanActivatatePlayerPunch)
        {
            AdjustPlayerPosition otherForwardMoveController = other.GetComponentInParent<AdjustPlayerPosition>();
            otherForwardMoveController.EnablePlayerPunch();
            UIManager.Instance.ActivatePlayerTapBar(false);
            bCanActivatatePlayerPunch = false;

            FinalPlayerTaps = FindObjectOfType<EndLevelSlowDown>().CurrentNumberTaps;

            Time.timeScale = slowDownTimeScale;
        }
    }

    public void FinalSpongePunch(float finalSpongeScale, SpongeRoll spongeRef)
    {
        // Reset Time to normal
        Time.timeScale = 1f;

        float forceToApplyToSponge = FinalPlayerTaps;

        // Stop player from following sponge
        spongeRef.GetComponentInParent<AdjustPlayerPosition>().SetCanAdjustPlayerToSponge(false);

        // Camera Follow Sponge
        spongeRef.transform.SetParent(null);
        FindObjectOfType<FollowTarget>().SetCameraTarget(spongeRef.transform);

        // Add force to Sponge
        Rigidbody spongeRigidbody = spongeRef.gameObject.AddComponent<Rigidbody>();
        spongeRigidbody.AddForce(spongePunchForceDirection * forceToApplyToSponge
            * spongePunchForceMultiplier, ForceMode.Impulse);
    }
}
