using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Tooltip("Use a negative value")]
    [SerializeField]
    private float spongeSizeToDecrease = -1;

    [Tooltip("Use a negative value")]
    [SerializeField]
    private float speedToDecrease = -1;

    [SerializeField]
    private bool bCanUnstuckfruits = true;

    private bool bCanDamageSponge = true;
    private bool bCanUnstsuckFruit = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sponge") && bCanDamageSponge)
        {
            print("Sponge");

            FoodSponge otherFruitSponge = other.GetComponentInParent<FoodSponge>();
            ForwardMoveController otherForwardMoveController = other.GetComponentInParent<ForwardMoveController>();
            if (otherFruitSponge == null || otherForwardMoveController == null) { return; }

            otherFruitSponge.IncreaseSpongeSize(spongeSizeToDecrease);
            otherForwardMoveController.IncreaseMoveSpeed(speedToDecrease);

            bCanDamageSponge = false;
        }

        if (other.CompareTag("Fruit") && bCanUnstsuckFruit)
        {
            // Ignore already unstuck fruits
            if (other.transform.parent == null) { return; }

            FoodSponge otherFruitSponge = other.transform.parent.GetComponentInParent<FoodSponge>();
            if (otherFruitSponge == null) { return; }

            Vector3 pointToUnstuck = GetComponent<Collider>().ClosestPointOnBounds(other.transform.position);

            otherFruitSponge.UnstuckFruit(other.GetComponent<Food>(), pointToUnstuck);

            bCanUnstsuckFruit = false;
            Invoke(nameof(EnableCanUnstuckFruit), 0.05f);
        }
    }

    private void EnableCanUnstuckFruit()
    {
        bCanUnstsuckFruit = true;
    }
}
