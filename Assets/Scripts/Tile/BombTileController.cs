using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BombTileController : TileController
{
    [SerializeField] private TileEffectExecutor tileEffectExecutor;

    protected override void Start()
    {
        base.Start();
        ColorID = ColorId.None;
        preventCollapseOverwrite = true;
    }


    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Trigger();
        }
    }

    protected override void Execute()
    {
        tileEffectExecutor.DestorySurrounding(tileCoordinate);
        DestroyTile();
    }
}
