using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalFloorPiecesManager : MonoBehaviour
{
    [Tooltip("Color of the first floor piece")]
    [SerializeField]
    private Color startGradientColor = Color.yellow;

    [Tooltip("Color of the final floor piece")]
    [SerializeField]
    private Color endGradientColor = Color.red;

    private List<Transform> finalFloorPieces = new List<Transform>();

    // Number used to display on the first finish line floor piece
    // The first finish line will be currentMultiplierAmount + multiplierIncrement, in this case x1.0
    private float currentMultiplierAmount = 0.5f;
    private float multiplierIncrement = 0.5f;

    private void Start()
    {
        foreach (Transform child in transform)
        {
            finalFloorPieces.Add(child);
        }

        // Change automatically the color and multiplier of each floor piece
        for (int i = 0; i < finalFloorPieces.Count; i++)
        {
            FinalFloorPiece finalFloorPiece = finalFloorPieces[i].GetComponent<FinalFloorPiece>();
            if (finalFloorPiece == null) { return; }

            float thisFloorNumber = (float) i / (float) finalFloorPieces.Count;

            // Set dynamic color gradient
            Color thisFloorPieceColor = Color.Lerp(startGradientColor, endGradientColor, thisFloorNumber);
            finalFloorPiece.SetFloorPieceColor(thisFloorPieceColor);

            // Set dynamic multiplier
            currentMultiplierAmount += multiplierIncrement;
            finalFloorPiece.SetFloorPieceMultiplierText(currentMultiplierAmount);
        }
    }
}
