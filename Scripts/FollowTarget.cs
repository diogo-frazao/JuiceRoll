using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField]
    private Transform targetTransform;

    [SerializeField]
    private float cameraMinX = -0.35f;

    [SerializeField]
    private float cameraMaxX = 0.35f;

    private float offsetZ = 0f;
    private bool bCanFollowTarget = true;

    private bool bCanLowerCameraToSeeJuice = false;

    private FoodSponge foodSponge;
    private Camera myCamera;

    private void Start()
    {
        offsetZ = transform.position.z - targetTransform.position.z;
        print(offsetZ);
        foodSponge = FindObjectOfType<FoodSponge>();
        myCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        // Final Camera behavior, lower to see juice
        Vector3 finalCameraLowPosition = new Vector3(transform.position.x, -1.68f, transform.position.z);
        if (bCanLowerCameraToSeeJuice)
        {
            transform.position = Vector3.MoveTowards(transform.position, finalCameraLowPosition, 
                3f * Time.deltaTime);
        }

        if (!bCanFollowTarget) { return; }

        // Normal Camera Behavior (follow target)
        float desiredXPosition = Mathf.Lerp(transform.position.x, 
            targetTransform.position.x, 25f * Time.deltaTime);

        desiredXPosition = Mathf.Clamp(desiredXPosition, cameraMinX, cameraMaxX);

        float dynamicYPosition = MapClampRanged(foodSponge.GetCurrentSpongeSize(), foodSponge.GetMinSpongeSize(),
            foodSponge.GetMaxSpongeSize(), 2.528f, 4.14f);

        float targetYPosition = Mathf.Lerp(transform.position.y, dynamicYPosition, 3f * Time.deltaTime);

        float dynamicZPosition = MapClampRanged(foodSponge.GetCurrentSpongeSize(), foodSponge.GetMinSpongeSize(),
            foodSponge.GetMaxSpongeSize(), -2.909f, -3.75f);

        offsetZ = Mathf.Lerp(offsetZ, dynamicZPosition, 3f * Time.deltaTime);

        transform.position = new Vector3(desiredXPosition, targetYPosition,
            targetTransform.position.z + offsetZ);

        //// Adjust camera zoom depending on sponge's size
        //float fovAmount = MapClampRanged(foodSponge.GetCurrentSpongeSize(), foodSponge.GetMinSpongeSize(),
        //    foodSponge.GetMaxSpongeSize(), 63.9f, 90f);
        //myCamera.fieldOfView = fovAmount;
    }

    public void SetCameraTarget(Transform newTarget)
    {
        targetTransform = newTarget;
        offsetZ = transform.position.z - targetTransform.position.z;
    }

    // Called by Fruit Cutter (camera remains still at end)
    public void SetCanFollowTarget(bool value)
    {
        bCanFollowTarget = value;
    }

    public void SetCanLowerCameraToSeeJuice(bool value)
    {
        bCanLowerCameraToSeeJuice = value;
    }

    public static float MapClampRanged(float OldValue, float OldMin, float OldMax, float NewMin, float NewMax)
    {
        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }
}
