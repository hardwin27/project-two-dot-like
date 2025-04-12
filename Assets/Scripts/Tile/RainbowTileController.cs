using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowTileController : TileController
{
    [SerializeField] private TileEffectExecutor tileEffectExecutor;

    protected override void Start()
    {
        base.Start();
        colorId = ColorId.Wild;
    }

    protected override void Execute()
    {
        base.Execute();
        tileEffectExecutor.DestroySelectedColor();
    }
}
