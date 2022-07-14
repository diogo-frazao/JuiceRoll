using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillCup : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer liquidMeshRenderer;

    private float fillAmount = 0f;
    private float desiredFillAmount;

    private void Start()
    {
        liquidMeshRenderer.material.SetFloat("Vector1_d54fc2dd899f41318e7c741a89171179", 0f);
    }

    public void CallFillCupMaterial(float finalSpongeSize, float minSpongeSize, float maxSpongeSize)
    {
        desiredFillAmount = MapClampRanged(finalSpongeSize, minSpongeSize, maxSpongeSize, 0.25f, 1f);
        InvokeRepeating(nameof(IncrementFill), 0f, 0.05f);
    }
    private void IncrementFill()
    {
        fillAmount += 0.01f;
        if (fillAmount <= desiredFillAmount)
        {
            liquidMeshRenderer.material.SetFloat("Vector1_d54fc2dd899f41318e7c741a89171179", fillAmount);
        }
        else
        {
            CancelInvoke(nameof(IncrementFill));
            Invoke(nameof(CallGoToNextLevel), 2f);
        }
    }

    private void CallGoToNextLevel()
    {
        LevelManager.Instance.GoToNextLevel();
    }

    public float MapClampRanged(float OldValue, float OldMin, float OldMax, float NewMin, float NewMax)
    {
        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }
}
