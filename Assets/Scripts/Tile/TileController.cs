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
    public event Action<TileController> OnTileDestroyed;

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

    protected void OnEnable()
    {
        tileVisual.OnTileVisualDestroyed += HandleVisualDestroyed;
    }

    protected void OnDisable()
    {
        tileVisual.OnTileVisualDestroyed -= HandleVisualDestroyed;
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

    public void OnPointerDown(PointerEventData eventData)
    {
        OnTilePointerDown?.Invoke(this);
    }

    protected void UpdateVisual()
    {
        tileVisual.SetVisualColor(colorId);
    }
    
    protected void DestroyTile()
    {
        tileVisual.PlayDestroyVisual();
    }

    protected virtual void HandleVisualDestroyed()
    {
        OnTileDestroyed?.Invoke(this);
        
        // Temporary solution
        gameObject.SetActive(false);
        Destroy(gameObject, 1f);
    }

    public virtual void Trigger()
    {
        DestroyTile();
    }
}
