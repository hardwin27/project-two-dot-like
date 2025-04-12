using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

public class TileController : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, /*IPointerUpHandler,*/ IPointerEnterHandler
{
    [SerializeField] private TileVisual tileVisual;
    [SerializeField, ReadOnly] private ColorId colorId;
    [SerializeField, ReadOnly] private Vector2Int tileCoordinate;

    public event Action<TileController> OnTileClick;
    public event Action<TileController> OnTilePointerEnter;
    public event Action<TileController> OnTilePointerDown;
    /*public event Action OnTilePointerUp;*/

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
        OnTileClick?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnTilePointerEnter?.Invoke(this);
    }

    /*public void OnPointerUp(PointerEventData eventData)
    {
        OnTilePointerUp?.Invoke();
    }
*/
    public void OnPointerDown(PointerEventData eventData)
    {
        OnTilePointerDown?.Invoke(this);
    }

    private void UpdateVisual()
    {
        tileVisual.SetVisualColor(colorId);
    }
}
