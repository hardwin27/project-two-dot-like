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
        colorId = ColorId.None;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            tileEffectExecutor.DestorySurrounding(tileCoordinate);
        }
    }
}
