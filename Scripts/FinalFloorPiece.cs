using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinalFloorPiece : MonoBehaviour
{
    [SerializeField]
    private Transform[] floorPiecesToChange;
    
    [SerializeField]
    private TextMeshPro multiplierText;

    // Called by final floor pieces manager do lerp colors automatically
    public void SetFloorPieceColor(Color color)
    {
        foreach (Transform floorPiece in floorPiecesToChange)
        {
            floorPiece.GetComponent<Renderer>().material.color = color;
        }
    }

    // Called by final floor pieces manager do set floor piece text automatically
    // @param multiplierAmount is what will be displayed. Ex: param = 0.5 -> Text during gameplay : x0.5
    public void SetFloorPieceMultiplierText(float multiplierAmount)
    {
        multiplierText.text = "x" + multiplierAmount.ToString("0.0");
    }
}
