using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/BlockMap")]
public class BlockMapSO : ScriptableObject
{
    [field: SerializeField] public Vector2Int mapSize { get; private set; }
    [field: SerializeField] public List<Vector2Int> obstaclePosition { get; private set; }
}

[Serializable]
public struct MapObstacle
{
    //public Vector2Int 
}
