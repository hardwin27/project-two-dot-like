using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GridController gridController;
    [SerializeField] private ConnectorController connectorController;

    private void Start()
    {
        gridController.GenerateGrid();
    }
}
