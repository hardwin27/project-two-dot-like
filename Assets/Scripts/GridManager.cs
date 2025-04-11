using DG.Tweening;
using System;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private RectTransform gridParent;

    [SerializeField] private Vector2 startPos = Vector2.zero;
    [SerializeField] private Vector2 tileSize = Vector2.zero;
    [SerializeField] private Vector2Int gridSize = Vector2Int.zero;

    TileController[,] tileControllers;
    Vector2[,] tilePositions;

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        tileControllers = new TileController[gridSize.x, gridSize.y];
        tilePositions = new Vector2[gridSize.x, gridSize.y];

        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                tilePositions[x, y] = new Vector2(
                        (x * tileSize.x) + startPos.x,
                        (y * tileSize.y) + startPos.y
                    );

                GenerateTile(x, y);

                /*GameObject tileObject = Instantiate(tilePrefab, gridParent);
                if (tileObject.TryGetComponent(out RectTransform tileRect))
                {
                    tileRect.anchoredPosition = tilePositions[x, y];
                }

                if (tileObject.TryGetComponent(out TileController tileController))
                {
                    tileControllers[x, y] = tileController;
                    tileController.TileClicked += TileClickedHandler;

                    tileController.TileCoordinate = new Vector2Int(x, y);
                    int colorInd = UnityEngine.Random.Range(1, 5);
                    if (Enum.IsDefined(typeof(ColorId), colorInd))
                    {
                        tileController.ColorID = (ColorId)colorInd;
                    }
                }*/
            }
        }
    }

    private void GenerateTile(int x, int y)
    {
        GameObject tileObject = Instantiate(tilePrefab, gridParent);
        if (tileObject.TryGetComponent(out RectTransform tileRect))
        {
            tileRect.anchoredPosition = tilePositions[x, y];
        }

        if (tileObject.TryGetComponent(out TileController tileController))
        {
            tileControllers[x, y] = tileController;
            tileController.TileClicked += TileClickedHandler;

            tileController.TileCoordinate = new Vector2Int(x, y);
            int colorInd = UnityEngine.Random.Range(1, 5);
            if (Enum.IsDefined(typeof(ColorId), colorInd))
            {
                tileController.ColorID = (ColorId)colorInd;
            }
        }
    }

    private void CollapseColumn(Vector2Int tileCoor)
    {
        for (int y = tileCoor.y; y < gridSize.y - 1; y++)
        {
            if (tileControllers[tileCoor.x, y] == null)
            {
                for (int aboveY = y + 1; aboveY < gridSize.y; aboveY++)
                {
                    if (tileControllers[tileCoor.x, aboveY] != null)
                    {
                        tileControllers[tileCoor.x, y] = tileControllers[tileCoor.x, aboveY];
                        tileControllers[tileCoor.x, aboveY] = null;

                        tileControllers[tileCoor.x, y].TileCoordinate = new Vector2Int(tileCoor.x, y);

                        if (tileControllers[tileCoor.x, y].TryGetComponent(out RectTransform tileRect))
                        {
                            tileRect.DOAnchorPos(tilePositions[tileCoor.x, y], 0.25f).SetEase(Ease.OutCubic);
                        }
                        break;
                    }
                }
            }
        }

        GenerateTile(tileCoor.x, gridSize.y - 1);
    }


    // Debug
    private void TileClickedHandler(TileController tileController)
    {
        tileController.TileClicked -= TileClickedHandler;

        Vector2Int clickedCoord = tileController.TileCoordinate;
        Destroy(tileController.gameObject);
        tileControllers[clickedCoord.x, clickedCoord.y] = null;

        CollapseColumn(clickedCoord);

    }
}
