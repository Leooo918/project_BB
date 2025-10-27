using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    private const int MAX_SPAWN_COUNT = 3;
    private const float BLOCK_SMALL_SIZE = 0.7f;

    [SerializeField] private BlockParent _block;
    [SerializeField] private Transform _blockParent;
    [SerializeField] private List<RectTransform> _blockSpawnPosition;
    [SerializeField] private BlockSetSO _blockToSpawn;

    [SerializeField] private BlockBackpack _blockBackpack;

    //private Block[] _remainBlockArray = new Block[MAX_SPAWN_COUNT];
    private int _remainBlockCount = 0;
    private BlockParent[] _remainBlock;
    private BlockParent[] _backpack = new BlockParent[MAX_SPAWN_COUNT];


    private void Awake()
    {
        Initialize();
        Bus<BlockSetEvent>.OnEvent += OnBlockSet;
        Bus<BlockReturnEvent>.OnEvent += OnBlockReturn;
    }

    private void OnDestroy()
    {
        Bus<BlockSetEvent>.OnEvent -= OnBlockSet;
        Bus<BlockReturnEvent>.OnEvent -= OnBlockReturn;
    }
    private void Initialize()
    {
        _remainBlock = new BlockParent[MAX_SPAWN_COUNT];
        for (int i = 0; i < MAX_SPAWN_COUNT; i++)
        {
            BlockParent block = Instantiate(_block, _blockParent);
            block.SetIndex(i);
            block.Initialize(_blockToSpawn.GetRandomBlock());
            block.SetBlockMovable(true);
            block.SetLocalScale(BLOCK_SMALL_SIZE);

            block.SetPosition(_blockSpawnPosition[i].position);     //After Initialize
            _remainBlock[i] = block;
        }
        _remainBlockCount = MAX_SPAWN_COUNT;


        for (int i = 0; i < MAX_SPAWN_COUNT; i++)
        {
            BlockParent block = Instantiate(_block, _blockParent);
            block.Initialize(_blockToSpawn.GetRandomBlock());
            block.SetBlockMovable(false);
            _backpack[i] = block;
        }
        _blockBackpack.SetBackpack(_backpack);
    }

    public void Spawn()
    {
        for (int i = 0; i < MAX_SPAWN_COUNT; i++)
        {
            BlockParent block = _backpack[i];
            block.SetLocalScale(BLOCK_SMALL_SIZE);
            block.SetPosition(_blockSpawnPosition[i].position);
            block.SetBlockMovable(true);
            block.SetIndex(i);

            _remainBlock[i] = block;
        }
        _remainBlockCount = MAX_SPAWN_COUNT;
        Bus<RemainBlockChangeEvent>.Publish(new RemainBlockChangeEvent(_remainBlock));

        for (int i = 0; i < MAX_SPAWN_COUNT; i++)
        {
            BlockParent block = Instantiate(_block, _blockParent);
            block.Initialize(_blockToSpawn.GetRandomBlock());
            block.SetBlockMovable(false);
            _backpack[i] = block;
        }
        _blockBackpack.SetBackpack(_backpack);
    }

    public void Reroll()
    {
        for (int i = 0; i < MAX_SPAWN_COUNT; i++)
        {
            if (_remainBlock[i] == null) continue;

            Destroy(_remainBlock[i].gameObject);

            BlockParent block = Instantiate(_block, _blockParent);
            block.SetIndex(i);
            block.Initialize(_blockToSpawn.GetRandomBlock());
            block.SetBlockMovable(true);
            block.SetLocalScale(BLOCK_SMALL_SIZE);

            block.SetPosition(_blockSpawnPosition[i].position);     //After Initialize
            _remainBlock[i] = block;
        }
    }

    #region Event Handler

    private void OnBlockSet(BlockSetEvent evt)
    {
        _remainBlockCount--;

        for(int i = 0; i < _remainBlock.Length; i++)
        {
            if (_remainBlock[i] == evt.block)
            {
                _remainBlock[i] = null;
                break;
            }
        }

        if (_remainBlockCount == 0) Spawn();
        Bus<RemainBlockChangeEvent>.Publish(new  RemainBlockChangeEvent(_remainBlock));
        evt.block.DestroyBlock();
    }

    private void OnBlockReturn(BlockReturnEvent evt)
    {
        evt.block.transform.localScale = Vector3.one * BLOCK_SMALL_SIZE;
        evt.block.SetPosition(_blockSpawnPosition[evt.block.Index].position);
    }

    #endregion
}
