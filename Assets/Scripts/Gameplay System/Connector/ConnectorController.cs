using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ConnectorController : MonoBehaviour
{
    [SerializeField] private GridController gridController;
    [SerializeField] private int minMatchCount;
    [SerializeField] private GameObject bombTilePrefab;
    [SerializeField] private GameObject rainbowTIlePrefab;

    [SerializeField, ReadOnly] private bool isDragging = false;
    private List<TileController> selectedTiles = new List<TileController>();
    [SerializeField, ReadOnly] private ColorId currentSelectedColor = ColorId.None;

    [SerializeField] private RectTransform lineParent;
    [SerializeField] private GameObject lineSegmentPrefab;
    private List<GameObject> activeLines = new List<GameObject>();
    private GameObject previewLine;

    public ColorId CurrentSelectedColor { get => currentSelectedColor; }

    private void OnEnable()
    {
        gridController.OnTileGenerated += HandleTileGenerated;
        gridController.OnTileDestroyed += HandleTileRemoved;
    }

    private void OnDisable()
    {
        gridController.OnTileGenerated -= HandleTileGenerated;
        gridController.OnTileDestroyed -= HandleTileRemoved;
    }

    private void Update()
    {
        if (isDragging)
        {
            if (Input.GetMouseButtonUp(0))
            {
                HandlePointerUp();
            }

            if (selectedTiles.Count > 0) 
            {
                Vector2 startPos = selectedTiles[selectedTiles.Count - 1].GetComponent<RectTransform>().anchoredPosition;
                Vector2 targetPos = GetMouseAnchoredPosition();

                if (previewLine == null)
                    previewLine = Instantiate(lineSegmentPrefab, lineParent);

                DrawLine(previewLine.GetComponent<RectTransform>(), startPos, targetPos);
            }
        }
    }

    private Vector2 GetMouseAnchoredPosition()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            lineParent,
            Input.mousePosition,
            null,
            out Vector2 localPos);
        return localPos;
    }

    private void DrawLine(RectTransform lineRect, Vector2 fromPos, Vector2 toPos)
    {
        Vector2 direction = (toPos - fromPos).normalized;
        float distance = Vector2.Distance(fromPos, toPos);

        lineRect.sizeDelta = new Vector2(lineRect.sizeDelta.x, distance);
        lineRect.anchoredPosition = (fromPos + toPos) / 2f;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        lineRect.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    private IEnumerator SpawnSpecialTileAfterDelay(float delay, Vector2Int coord, GameObject prefab)
    {
        yield return new WaitForSeconds(delay);
        gridController.GenerateTile(coord.x, coord.y, prefab);
        gridController.ClearReservedPosition(coord);
    }


    private void HandlePointerUp()
    {
        isDragging = false;

        bool shouldSpawnSpecial = selectedTiles.Count >= 6;
        Vector2Int specialTileCoord = Vector2Int.zero;
        GameObject specialPrefab = null;

        if (shouldSpawnSpecial)
        {
            TileController lastTile = selectedTiles[selectedTiles.Count - 1];
            specialTileCoord = lastTile.TileCoordinate;
            specialPrefab = selectedTiles.Count >= 9 ? rainbowTIlePrefab : bombTilePrefab;
        }

        if (selectedTiles.Count >= minMatchCount)
        {
            foreach (TileController tileController in selectedTiles)
            {
                if (shouldSpawnSpecial)
                {
                    gridController.ReserveTilePosition(specialTileCoord);
                }

                tileController.Trigger();
            }
        }

        if (shouldSpawnSpecial)
        {
            StartCoroutine(SpawnSpecialTileAfterDelay(0.3f, specialTileCoord, specialPrefab));
        }

        selectedTiles.Clear();
        currentSelectedColor = ColorId.None;

        foreach (var line in activeLines)
            Destroy(line);
        activeLines.Clear();

        if (previewLine != null)
        {
            Destroy(previewLine);
            previewLine = null;
        }
    }



    private void HandleTileGenerated(TileController tileController)
    {
        tileController.OnTilePointerDown += HandleTilePointerDown;
        tileController.OnTilePointerEnter += HandleTilePointerEnter;

        if (tileController.gameObject.TryGetComponent(out INeedConnector connectorComponent))
        {
            connectorComponent.Connector = this;
        }
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

        if (tileController.ColorID == ColorId.None || 
            tileController.ColorID == ColorId.Wild)
        {
            return;
        }

        currentSelectedColor = tileController.ColorID;
        isDragging = true;
        selectedTiles.Add(tileController);
    }

    private bool IsAdjacent(Vector2Int currentTile, Vector2Int targetTile)
    {
        bool isHorizontalAdj = Mathf.Abs(currentTile.x - targetTile.x) == 1
            && currentTile.y == targetTile.y;
        bool isVerticalAdj = Mathf.Abs(currentTile.y - targetTile.y) == 1
            && currentTile.x == targetTile.x;

        return isHorizontalAdj || isVerticalAdj;
    }

    private void DrawLineBetween(TileController tileFrom, TileController tileTo)
    {
        RectTransform fromRect = tileFrom.GetComponent<RectTransform>();
        RectTransform toRect = tileTo.GetComponent<RectTransform>();

        Vector2 fromPos = fromRect.anchoredPosition;
        Vector2 toPos = toRect.anchoredPosition;

        GameObject line = Instantiate(lineSegmentPrefab, lineParent);
        RectTransform lineRect = line.GetComponent<RectTransform>();

        Vector2 dir = (toPos - fromPos).normalized;
        float distance = Vector2.Distance(fromPos, toPos);

        lineRect.sizeDelta = new Vector2(lineRect.sizeDelta.x, distance);
        lineRect.anchoredPosition = (fromPos + toPos) / 2f;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        lineRect.rotation = Quaternion.Euler(0, 0, angle - 90f);

        activeLines.Add(line);
    }


    private void HandleTilePointerEnter(TileController tileController)
    {
        if (!isDragging || selectedTiles.Contains(tileController)) return;

        TileController lastTile = selectedTiles[selectedTiles.Count - 1];

        if (!IsAdjacent(tileController.TileCoordinate, lastTile.TileCoordinate))
        {
            return;
        }

        if (tileController.ColorID == currentSelectedColor || tileController.ColorID == ColorId.Wild)
        {
            selectedTiles.Add(tileController);
            DrawLineBetween(lastTile, tileController);
        }
    }
}
