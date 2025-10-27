using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/BlockSetSO")]
public class BlockSetSO : ScriptableObject
{
    [field: SerializeField] public List<BlockSO> blockSet { get; private set; }

    public BlockSO GetRandomBlock()
    {
        return blockSet[Random.Range(0, blockSet.Count)];
    }
}
