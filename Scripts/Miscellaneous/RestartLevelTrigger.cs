using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartLevelTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sponge"))
        {
            LevelManager.Instance.RestartLevel();
        }
    }
}
