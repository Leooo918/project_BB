using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/BlockSO")]
public class BlockSO : ScriptableObject
{
    [field: SerializeField] public List<Vector2Int> blockPositions { get; private set; }
    [field: SerializeField] public Vector3 centerOffset { get; private set; }

    [field: SerializeField] public Block blockPrefab { get; private set; }
}
