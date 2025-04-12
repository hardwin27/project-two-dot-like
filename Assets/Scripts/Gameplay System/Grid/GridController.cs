using DG.Tweening;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private RectTransform gridParent;

    [SerializeField] private Vector2 startPos = Vector2.zero;
    [SerializeField] private Vector2 tileSize = Vector2.zero;
    [SerializeField] private Vector2Int gridSize = Vector2Int.zero;

    public TileController[,] TileControllers;
    public Vector2[,] TilePositions;

    public Vector2Int GridSize { get => gridSize; }

    public Action<TileController> OnTileGenerated;
    public Action<TileController> OnTileDestroyed;

    private void Start()
    {
        /*GenerateGrid();*/
    }

    public void GenerateGrid()
    {
        TileControllers = new TileController[gridSize.x, gridSize.y];
        TilePositions = new Vector2[gridSize.x, gridSize.y];

        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                TilePositions[x, y] = new Vector2(
                        (x * tileSize.x) + startPos.x,
                        (y * tileSize.y) + startPos.y
                    );

                GenerateTile(x, y);
            }
        }
    }

    private void GenerateTile(int x, int y)
    {
        GameObject tileObject = Instantiate(tilePrefab, gridParent);
        if (tileObject.TryGetComponent(out RectTransform tileRect))
        {
            tileRect.anchoredPosition = TilePositions[x, y];
        }

        if (tileObject.TryGetComponent(out INeedGrid gridNeedComp))
        {
            gridNeedComp.Grid = this;
        }

        if (tileObject.TryGetComponent(out TileController tileController))
        {
            TileControllers[x, y] = tileController;
            tileController.OnTileDestroyed += HandleTileDestroyed;

            tileController.TileCoordinate = new Vector2Int(x, y);
            int colorInd = UnityEngine.Random.Range(1, Enum.GetValues(typeof(ColorId)).Length - 1);
            if (Enum.IsDefined(typeof(ColorId), colorInd))
            {
                tileController.ColorID = (ColorId)colorInd;
            }

            OnTileGenerated?.Invoke(tileController);
        }
    }

    private void CollapseColumn(Vector2Int tileCoor)
    {
        for (int y = tileCoor.y; y < gridSize.y - 1; y++)
        {
            if (TileControllers[tileCoor.x, y] == null)
            {
                for (int aboveY = y + 1; aboveY < gridSize.y; aboveY++)
                {
                    if (TileControllers[tileCoor.x, aboveY] != null)
                    {
                        TileControllers[tileCoor.x, y] = TileControllers[tileCoor.x, aboveY];
                        TileControllers[tileCoor.x, aboveY] = null;

                        TileControllers[tileCoor.x, y].TileCoordinate = new Vector2Int(tileCoor.x, y);

                        if (TileControllers[tileCoor.x, y].TryGetComponent(out RectTransform tileRect))
                        {
                            tileRect.DOAnchorPos(TilePositions[tileCoor.x, y], 0.25f).SetEase(Ease.OutCubic);
                        }
                        break;
                    }
                }
            }
        }

        GenerateTile(tileCoor.x, gridSize.y - 1);
    }

    private void HandleTileDestroyed(TileController tileController)
    {
        tileController.OnTileDestroyed -= HandleTileDestroyed;
        Vector2Int clickedCoord = tileController.TileCoordinate;
        TileControllers[clickedCoord.x, clickedCoord.y] = null;

        OnTileDestroyed?.Invoke(tileController);

        CollapseColumn(clickedCoord);
    }
}
