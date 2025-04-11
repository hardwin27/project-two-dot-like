using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

public class TileController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TileVisual tileVisual;
    [SerializeField, ReadOnly] private ColorId colorId;
    [SerializeField, ReadOnly] private Vector2Int tileCoordinate;

    public event Action<TileController> TileClicked;

    public ColorId ColorID 
    { 
        get => colorId;
        set
        {
            colorId = value;
            UpdateVisual();
        }
    }

    public Vector2Int TileCoordinate
    {
        get => tileCoordinate;
        set => tileCoordinate = value;
    }

    private void Start()
    {
        UpdateVisual();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        TileClicked?.Invoke(this);
    }

    private void UpdateVisual()
    {
        tileVisual.SetVisualColor(colorId);
    }
}
