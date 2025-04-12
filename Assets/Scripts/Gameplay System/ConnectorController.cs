using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ConnectorController : MonoBehaviour
{
    [SerializeField] private GridController gridController;

    [SerializeField, ReadOnly] private bool isDragging = false;
    [SerializeField] private List<TileController> selectedTiles = new List<TileController>();

    [SerializeField] private GameObject lineSegmentPrefab;
    [SerializeField, ReadOnly] private List<GameObject> activeLines = new List<GameObject>();


    private void OnEnable()
    {
        gridController.OnTileGenerated += HandleTileGenerated;
    }

    private void OnDisable()
    {
        gridController.OnTileGenerated -= HandleTileGenerated;
    }

    private void HandlePointerUp()
    {

    }

    private void HandleTileGenerated(TileController tileController)
    {
        tileController.OnTilePointerDown += HandleTilePointerDown;
        tileController.OnTilePointerEnter += HandleTilePointerEnter;
    }

    private void HandleTileRemoved(TileController tileController)
    {
        tileController.OnTilePointerDown -= HandleTilePointerDown;
        tileController.OnTilePointerEnter -= HandleTilePointerEnter;
    }

    private void HandleTilePointerDown(TileController tileController)
    {
        if (isDragging)
        {
            return;
        }

        isDragging = true;
        selectedTiles.Add(tileController);
    }

    private void HandleTilePointerEnter(TileController tileController)
    {
        if (!isDragging)
        {
            return;
        }

        
    }
}
