using System;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    private const int MAX_SPAWN_COUNT = 3;
    private const float BLOCK_SMALL_SIZE = 0.7f;

    [SerializeField] private BlockParent _block;
    [SerializeField] private Transform _blockParent;
    [SerializeField] private BlockSetSO _blockToSpawn;

    [SerializeField] private BlockBackpack _blockBackpack;
    [SerializeField] private RectTransform _originPos;

    //private Block[] _remainBlockArray = new Block[MAX_SPAWN_COUNT];
    private int _remainBlockCount = 0;
    private List<Vector2> _blockSpawnPositions;
    private BlockParent[] _remainBlock;
    private BlockParent[] _backpack = new BlockParent[MAX_SPAWN_COUNT];
    private RectTransform RectTrm => transform as RectTransform;


    private void Awake()
    {
        Bus<BlockSetEvent>.OnEvent += OnBlockSet;
        Bus<BlockReturnEvent>.OnEvent += OnBlockReturn;
        Bus<BlockSelectEvent>.OnEvent += OnSelectBlock;

        float space = (RectTrm.rect.width - (_originPos.anchoredPosition.x * 2)) / (MAX_SPAWN_COUNT - 1);

        _blockSpawnPositions = new List<Vector2>();
        _blockSpawnPositions.Add(_originPos.position);
        for (int i = 1; i < MAX_SPAWN_COUNT; i++)
        {
            _originPos.anchoredPosition += new Vector2(space, 0);
            _blockSpawnPositions.Add(_originPos.position);
        }
        Initialize();
    }


    private void OnDestroy()
    {
        Bus<BlockSetEvent>.OnEvent -= OnBlockSet;
        Bus<BlockReturnEvent>.OnEvent -= OnBlockReturn;
        Bus<BlockSelectEvent>.OnEvent -= OnSelectBlock;
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
            block.SetPosition(_blockSpawnPositions[i]);     //After Initialize
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
        _blockBackpack.Reveal();
    }

    public void Spawn()
    {
        for (int i = 0; i < MAX_SPAWN_COUNT; i++)
        {
            BlockParent block = _backpack[i];
            block.SetLocalScale(BLOCK_SMALL_SIZE);
            block.SetPosition(_blockSpawnPositions[i]);
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

            block.SetPosition(_blockSpawnPositions[i]);     //After Initialize
            _remainBlock[i] = block;
        }

        Bus<RemainBlockChangeEvent>.Publish(new RemainBlockChangeEvent(_remainBlock));
    }

    #region Event Handler

    private void OnBlockSet(BlockSetEvent evt)
    {
        for (int i = 0; i < _remainBlock.Length; i++)
        {
            if (_remainBlock[i] != null)
            {
                _remainBlock[i].SetBlockMovable(true);
            }
        }

        _remainBlockCount--;

        for (int i = 0; i < _remainBlock.Length; i++)
        {
            if (_remainBlock[i] == evt.block)
            {
                _remainBlock[i] = null;
                break;
            }
        }

        if (_remainBlockCount == 0) Spawn();
        Bus<RemainBlockChangeEvent>.Publish(new RemainBlockChangeEvent(_remainBlock));
        evt.block.DestroyBlock();
    }

    private void OnBlockReturn(BlockReturnEvent evt)
    {
        for(int i = 0; i < _remainBlock.Length; i++)
        {
            if(_remainBlock[i] != null)
            {
                _remainBlock[i].SetBlockMovable(true);
            }
        }

        evt.block.transform.localScale = Vector3.one * BLOCK_SMALL_SIZE;
        evt.block.SetPosition(_blockSpawnPositions[evt.block.Index]);
    }

    private void OnSelectBlock(BlockSelectEvent evt)
    {
        for (int i = 0; i < _remainBlock.Length; i++)
        {
            if (_remainBlock[i] != null && _remainBlock[i] != evt.selectedBlock)
            {
                _remainBlock[i].SetBlockMovable(false);
            }
        }
    }

    #endregion
}
