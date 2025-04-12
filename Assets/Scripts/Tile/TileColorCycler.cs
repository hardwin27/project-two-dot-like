using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileColorCycler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TileController tileController; 

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right) 
        {
            CycleColor();
        }
    }

    private void CycleColor()
    {
        int colorInd = (int)tileController.ColorID;

        if (colorInd <= 0)
        {
            return;
        }

        colorInd++;
        if (colorInd > Enum.GetValues(typeof(ColorId)).Length - 2)
        {
            colorInd = 1;
        }

        tileController.ColorID = (ColorId)colorInd;
    }
}
