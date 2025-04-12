using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEffectExecutor : MonoBehaviour, INeedGrid, INeedConnector
{
    public GridController Grid { get; set; }
    public ConnectorController Connector { get; set; }

    public void DestorySurrounding(Vector2Int tileCoord)
    {
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0)
                {
                    continue;
                }

                int targetX = tileCoord.x + dx;
                int targetY = tileCoord.y + dy;

                if (targetX < 0 || targetX >= Grid.GridSize.x ||
                    targetY < 0 || targetY >= Grid.GridSize.y)
                    continue;

                TileController tile = Grid.TileControllers[targetX, targetY];

                if (tile != null)
                {
                    tile.Trigger();
                }
            }
        }
    }

    public void DestroySelectedColor()
    {
        if (Connector.CurrentSelectedColor == ColorId.None 
            || Connector.CurrentSelectedColor == ColorId.Wild)
        {
            return;
        }

        for (int x = 0; x < Grid.GridSize.x; x++)
        {
            for (int y = 0; y < Grid.GridSize.y; y++)
            {
                TileController tile = Grid.TileControllers[x, y];

                if (tile != null && tile.ColorID == Connector.CurrentSelectedColor)
                {
                    tile.Trigger();
                }
            }
        }
    }
}
