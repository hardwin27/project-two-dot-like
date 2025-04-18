using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileVisual : MonoBehaviour
{
    [SerializeField] private Image tintedImage;
    [SerializeField] private List<Color> colorLibrary = new List<Color>();

    public event Action OnTileVisualDestroyed;

    public void SetVisualColor(ColorId colorId)
    {
        int colorInd = (int)colorId;
        if (colorInd <= 0)
        {
            tintedImage.color = Color.white;
        }
        else
        {
            tintedImage.color = colorLibrary[colorInd - 1];
        }
    }

    public void PlayDestroyVisual()
    {
        transform.DOScale(Vector3.zero, 0.25f).OnComplete(() =>
        {
            OnTileVisualDestroyed?.Invoke();
        });
    }
}
