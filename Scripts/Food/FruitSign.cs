using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FruitSign : MonoBehaviour
{
    [Tooltip("Order needs to be the same of the food mesh serialized array")]
    [SerializeField]
    private string[] fruitsNames = new string[8]
    {
        "Apple", "Avocado", "Banana", "Cherries", "Peach", "Pear", "Strawberry", "Watermelon"
    };

    [Tooltip("Order needs to be the same of the food mesh serialized array")]
    private Color[] fruitsColors = new Color[8]
    {
        Color.red, Color.green, Color.yellow, Color.red, new Color(1f, 1f, 0f), Color.green, Color.red, Color.red
    };

    [SerializeField]
    private TextMeshPro fruitNameText;

    private int selectedFruitIndex;

    private void Start()
    {
        selectedFruitIndex = GetComponentInChildren<Food>().SelectedFruitIndex;
        fruitNameText.text = fruitsNames[selectedFruitIndex];

        Color selectedColor = new Color(fruitsColors[selectedFruitIndex].r, 
            fruitsColors[selectedFruitIndex].g, fruitsColors[selectedFruitIndex].b, 0.5f);

        //GetComponent<Renderer>().material.color = selectedColor;
    }

}
