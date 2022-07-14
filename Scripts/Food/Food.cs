using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum FoodState
{
    Idle,
    Stuck,
    Unstuck
}

public class Food : MonoBehaviour
{
    [Tooltip("Set negative value for junk food and positive for fruit")]
    [SerializeField]
    protected float spongeSizeToIncrease = 5f;

    [Tooltip("Set negative value for junk food and positive for fruit")]
    [SerializeField]
    protected float speedToIncrease = 0.5f;

    [SerializeField]
    private Mesh[] fruitsMeshes;

    [SerializeField]
    protected ParticleSystem[] splashParticles;

    private ParticleSystem splashParticle;

    private FoodState state;
    private Vector3 localPointToBeStuckToSponge;

    private Vector3 initialFoodScale;

    private float randomRotationAmount = 250f;

    public int SelectedFruitIndex { get; private set; }

    private void Awake()
    {
        state = FoodState.Idle;
        SelectRandomFruitMesh();
    }

    private void SelectRandomFruitMesh()
    {
        int randomMeshIndex = UnityEngine.Random.Range(0, fruitsMeshes.Length -1);
        Mesh randomFruitMesh = fruitsMeshes[randomMeshIndex];

        GetComponent<MeshFilter>().mesh = randomFruitMesh;
        SelectedFruitIndex = randomMeshIndex;
        splashParticle = splashParticles[randomMeshIndex];
    }

    private void Start()
    {
        if (GetComponentInParent<FruitSign>() == null)
        {
            float randomScaleAmount = Random.Range(-10, 25);
            transform.localScale = new Vector3(100f + randomScaleAmount, 100f + randomScaleAmount, 183f + randomScaleAmount);
        }
        else
        {
            transform.localScale = new Vector3(275, 275, 275);
        }
        
        initialFoodScale = transform.lossyScale;
        randomRotationAmount += Random.Range(-50f, 50f);
    }

    private void Update()
    {
        if (state == FoodState.Stuck)
        {
            // Is already stuck
            transform.localPosition = localPointToBeStuckToSponge;

            //float newScaleX = foodScaleParentedToFood.x / transform.parent.localScale.x;
            //float newScaleY = foodScaleParentedToFood.y / transform.parent.localScale.y;
            //float newScaleZ = foodScaleParentedToFood.z / transform.parent.localScale.z;

            //transform.localScale = new Vector3(
            //    transform.parent.localScale.x * newScaleX, transform.parent.localScale.y * newScaleY,
            //    transform.parent.localScale.z * newScaleZ);
        }
        else if (state == FoodState.Idle)
        {
            transform.Rotate(Vector3.forward * randomRotationAmount * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sponge") && state == FoodState.Idle)
        {
            FoodSponge otherFruitSponge = other.GetComponentInParent<FoodSponge>();
            ForwardMoveController otherForwardMoveController = other.GetComponentInParent<ForwardMoveController>();
            if (otherFruitSponge == null || otherForwardMoveController == null) { return; }

            // Stuck fruit on overlapped point
            localPointToBeStuckToSponge = otherFruitSponge.StuckFruit(this, transform.position);
            state = FoodState.Stuck;

            otherFruitSponge.IncreaseSpongeSize(spongeSizeToIncrease);
            otherForwardMoveController.IncreaseMoveSpeed(speedToIncrease);

            SoundManager.Instance.PlayRandomFoodSmash();
            Instantiate(splashParticle, transform.position, splashParticle.transform.rotation);
        }
    }

    public void SetFoodState(FoodState newState)
    {
        state = newState;
    }
    public Vector3 GetInitialFoodSize()
    {
        return initialFoodScale;
    }
}
