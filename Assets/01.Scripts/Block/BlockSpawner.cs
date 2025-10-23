using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    private const int MAX_SPAWN_COUNT = 3;

    [SerializeField] private Block _block;
    [SerializeField] private Transform _blockParent;
    [SerializeField] private List<RectTransform> _blockSpawnPosition;
    [SerializeField] private List<BlockSO> _blockToSpawn;

    //private Block[] _remainBlockArray = new Block[MAX_SPAWN_COUNT];
    private int _remainBlockCount = 0;


    private void Awake()
    {
        StartSpawn();
        Bus<BlockSetEvent>.OnEvent += OnBlockSet;
        Bus<BlockReturnEvent>.OnEvent += OnBlockReturn;
    }

    private void OnDestroy()
    {
        Bus<BlockSetEvent>.OnEvent -= OnBlockSet;
        Bus<BlockReturnEvent>.OnEvent -= OnBlockReturn;
    }

    public void StartSpawn()
    {
        for (int i = 0; i < MAX_SPAWN_COUNT; i++)
        {
            Block block = Instantiate(_block, _blockParent);
            block.SetIndex(i);
            block.SetPosition(_blockSpawnPosition[i].position);
            block.Initialize(_blockToSpawn[Random.Range(0, _blockToSpawn.Count)]);
        }

        _remainBlockCount = MAX_SPAWN_COUNT;
    }

    private void OnBlockSet(BlockSetEvent evt)
    {
        _remainBlockCount--;
        if (_remainBlockCount == 0)
        {
            StartSpawn();
        }

        evt.block.DestroyBlock();
    }

    private void OnBlockReturn(BlockReturnEvent evt)
    {
        evt.block.SetPosition(_blockSpawnPosition[evt.block.Index].position);
    }
}
