using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileVisual : MonoBehaviour
{
    [SerializeField] private Image tintedImage;
    [SerializeField] private List<Color> colorLibrary = new List<Color>();

    public void SetVisualColor(ColorId colorId)
    {
        int colorInd = (int)colorId;
        tintedImage.color = colorLibrary[colorInd];
    }
}
