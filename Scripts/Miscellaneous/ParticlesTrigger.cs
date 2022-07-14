using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesTrigger : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particlesToSpawn;

    [SerializeField]
    private Transform[] spawnPositions;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sponge"))
        {
            Instantiate(particlesToSpawn, spawnPositions[0].position, particlesToSpawn.transform.rotation);
            Instantiate(particlesToSpawn, spawnPositions[1].position, particlesToSpawn.transform.rotation);
        }
    }
}
