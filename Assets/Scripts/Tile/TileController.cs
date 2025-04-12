using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

public class TileController : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, /*IPointerUpHandler,*/ IPointerEnterHandler
{
    private bool isActive;

    [SerializeField] protected TileVisual tileVisual;
    [SerializeField, ReadOnly] protected ColorId colorId;
    [SerializeField, ReadOnly] protected Vector2Int tileCoordinate;

    public event Action<TileController> OnTileClick;
    public event Action<TileController> OnTilePointerEnter;
    public event Action<TileController> OnTilePointerDown;
    public event Action<TileController> OnTileDestroyed;

    public bool IsActive { get => isActive; }

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

    protected virtual void Start()
    {
        UpdateVisual();
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        return;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnTilePointerEnter?.Invoke(this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnTilePointerDown?.Invoke(this);
        }
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
        
        // Temporary solution before pooling
        gameObject.SetActive(false);
        Destroy(gameObject, 1f);
    }

    public void Trigger()
    {
        if (isActive)
        {
            Execute();
        }
    }

    protected virtual void Execute()
    {
        DestroyTile();
    }
}
