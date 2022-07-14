using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitCutter : MonoBehaviour
{
    [SerializeField]
    private GameObject cutSpongePrefab;

    [SerializeField]
    private FillCup cupToFill;

    [SerializeField]
    private Transform[] cutSpongeSpawnPoints;

    private Collider otherCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sponge"))
        {
            FoodSponge foodSponge = FindObjectOfType<FoodSponge>();
            if (foodSponge == null) { return; }
            // Activate cup
            StartCoroutine(ActivateAndFillCup(foodSponge));

            // Stop camera from following target and move it down
            FindObjectOfType<FollowTarget>().SetCanFollowTarget(false);
            //FindObjectOfType<FollowTarget>().SetCanLowerCameraToSeeJuice(true);

            otherCollider = other;

            SpawnAndLaunchCutSponge(otherCollider, 0);
            SpawnAndLaunchCutSponge(otherCollider, 1);
            SpawnAndLaunchCutSponge(otherCollider, 2);

            Destroy(otherCollider.gameObject);
        }
    }

    private IEnumerator ActivateAndFillCup(FoodSponge foodSponge)
    {
        yield return new WaitForSeconds(1.5f);
        cupToFill.gameObject.SetActive(true);
        cupToFill.CallFillCupMaterial(foodSponge.GetCurrentSpongeSize(),
            foodSponge.GetMinSpongeSize(), foodSponge.GetMaxSpongeSize());
    }

    private void SpawnAndLaunchCutSponge(Collider other, int cutSpawnPointIdex)
    {
        // Calculate cut sponge size
        float cutSpongeSize = other.transform.lossyScale.x * 12.5f / 0.25f;
        float cutSpongeRandomSize = Random.Range(cutSpongeSize - 20f, cutSpongeSize + 20f);

        // Random cut sponge rotation
        Quaternion cutSpongeRotation = Quaternion.Euler(new Vector3(Random.Range(0, 360f),
            Random.Range(0, 360f), Random.Range(0, 360f)));

        // Eject sponge cut half with random force and direction
        GameObject spongeCut = Instantiate(cutSpongePrefab, 
            cutSpongeSpawnPoints[cutSpawnPointIdex].position, cutSpongeRotation);

        spongeCut.transform.localScale = new Vector3(cutSpongeRandomSize, 
            cutSpongeRandomSize, cutSpongeRandomSize);

        Vector3 spongeCutForceDirection = new Vector3(Random.Range(0, 0.2f), 2f, Random.Range(0, 0.2f));
        float spongeCutForceAmount = Random.Range(50f, 200f);

        spongeCut.GetComponent<Rigidbody>().AddForce(spongeCutForceDirection * spongeCutForceAmount);
        spongeCut.GetComponent<Rigidbody>().drag = Random.Range(1f, 2f);
    }
}
