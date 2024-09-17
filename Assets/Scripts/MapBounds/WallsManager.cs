using System;
using System.Collections.Generic;
using MapBounds;
using UnityEngine;

public class WallsManager : MonoBehaviour
{
#if UNITY_EDITOR || DEVELOPMENT_BUILD

    [Header("Config")]
    [SerializeField] private MapBoundsSO boundsConfig;

    [Header("Walls")]
    [SerializeField] private GameObject leftWall;

    [SerializeField] private GameObject rightWall;
    [SerializeField] private GameObject farWall;
    [SerializeField] private GameObject nearWall;

    private void Awake()
    {
        SetWalls();
    }

    private void SetWalls()
    {
        leftWall.transform.position = new Vector3(boundsConfig.horizontalBounds.min, leftWall.transform.position.y, 0);
        rightWall.transform.position = new Vector3(boundsConfig.horizontalBounds.max, rightWall.transform.position.y, 0);

        nearWall.transform.position = new Vector3(0, nearWall.transform.position.y, boundsConfig.depthBounds.min);
        farWall.transform.position = new Vector3(0, farWall.transform.position.y, boundsConfig.depthBounds.max);
    }
#endif
}