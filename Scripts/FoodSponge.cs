using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSponge : MonoBehaviour
{
    [SerializeField]
    private Transform spongeTransform;

    [SerializeField]
    private float maxSpongeSize = 150f;

    [SerializeField]
    private float minSpongeSize = 50f;

    [SerializeField]
    private LayerMask spongeLayer;

    private SphereCollider spongeSphereCollider;

    private float currentSpongeSize;

    private List<Food> stuckFood = new List<Food>();

    private void Awake()
    {
        spongeSphereCollider = spongeTransform.GetComponent<SphereCollider>();
        currentSpongeSize = minSpongeSize;
    }

    public Vector3 StuckFruit(Food foodToStuck, Vector3 pointToStuck)
    {
        Vector3 stuckPoint = spongeSphereCollider.ClosestPoint(pointToStuck);

        // Place food at stuck point
        foodToStuck.transform.position = pointToStuck;

        // Returns vector pointing from stuck point to original food position (equal to normal)
        Vector3 stuckFruitNormal = foodToStuck.transform.position - stuckPoint;
        // Face normal
        foodToStuck.transform.up = stuckFruitNormal;

        // Set sponge fruits holder as parent (rotates with it but does not scale with sponge)
        foodToStuck.transform.SetParent(spongeTransform);

        stuckFood.Add(foodToStuck);

        // Used to make sure food is always stuck to sponge
        Vector3 stuckPointRelativeSponge = spongeTransform.InverseTransformPoint(stuckPoint);
        return stuckPointRelativeSponge;
    }

    public void IncreaseSpongeSize(float increaseAmount)
    {
        Vector3 currentSize = spongeTransform.localScale;

        Vector3 newSize = new Vector3(currentSize.x + increaseAmount,
            currentSize.y + increaseAmount, currentSize.z + increaseAmount);

        // If is already at max size, don't grow
        if (newSize.x >= maxSpongeSize)
        {
            newSize = new Vector3(maxSpongeSize, maxSpongeSize, maxSpongeSize);
        }
        else if (newSize.x <= minSpongeSize)
        {
            newSize = new Vector3(minSpongeSize, minSpongeSize, minSpongeSize);
        }

        currentSpongeSize = newSize.x;

        spongeTransform.localScale = newSize;
        
        // Make fruits keep always the same scale
        foreach (Food food in stuckFood)
        {
            food.transform.parent = null;
            food.transform.localScale = food.GetInitialFoodSize();
            food.transform.parent = spongeTransform;
        }
    }

    public void UnstuckFruit(Food fruitToUnstuck, Vector3 fruitUnstuckPosition)
    {
        if (stuckFood.Contains(fruitToUnstuck))
        {
            stuckFood.Remove(fruitToUnstuck);
            fruitToUnstuck.SetFoodState(FoodState.Unstuck);
            fruitToUnstuck.transform.SetParent(null);

            // Avoids unstacking fruit outside collider when sweeping
            fruitToUnstuck.transform.position = fruitUnstuckPosition;
        }
    }

    public LayerMask GetSpongeLayer()
    {
        return spongeLayer;
    }

    public float GetMinSpongeSize()
    {
        return minSpongeSize;
    }

    public float GetMaxSpongeSize()
    {
        return maxSpongeSize;
    }

    public float GetCurrentSpongeSize()
    {
        return currentSpongeSize;
    }

    // Called by punch animation on player
    public void CallFinalSpongePunch()
    {
        SpongeRoll spongeRoll = spongeTransform.GetComponent<SpongeRoll>();
        FindObjectOfType<FinishLine>().FinalSpongePunch(spongeTransform.lossyScale.x, spongeRoll);
    }
}
