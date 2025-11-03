using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlockMap : MonoBehaviour
{
    [SerializeField] private BlockMapSO _blockMapSO;
    [SerializeField] private BlockFrame _blockFrame;
    [SerializeField] private Transform _blockParent;
    [Space]
    [SerializeField] private PerfectText _perfectText;

    private Vector2Int _selectedPosition;
    private bool _canInsertItem = false;
    private BlockParent _selectedBlock;
    private int _selectedIndex;
    
    private BlockMapInfo _mapInfo;
    private BlockFrame[,] _blockFrameArray;
    private List<Vector2Int> _selectedFrame = new();
    private List<Vector2Int> _readyBreakingBlock = new();

    public BlockMapInfo MapInfo => _mapInfo;
        

    private void Awake()
    {
        if (_blockMapSO != null) Initialize(_blockMapSO);
        Bus<RemainBlockChangeEvent>.OnEvent += CheckInsertableBlockExsist;
        Bus<BlockSelectEvent>.OnEvent += OnSelectedBlock;
        Bus<BlockReleaseEvent>.OnEvent += TrySetBlock;
    }

    private void OnDestroy()
    {
        Bus<RemainBlockChangeEvent>.OnEvent -= CheckInsertableBlockExsist;
        Bus<BlockSelectEvent>.OnEvent -= OnSelectedBlock;
        Bus<BlockReleaseEvent>.OnEvent -= TrySetBlock;
    }

    public void Initialize(BlockMapSO blockMap)
    {
        _blockMapSO = blockMap;
        _mapInfo.blockMap = new Block[_blockMapSO.mapSize.x, _blockMapSO.mapSize.y];
        _blockFrameArray = new BlockFrame[_blockMapSO.mapSize.x, _blockMapSO.mapSize.y];

        for (int i = 0; i < _blockMapSO.mapSize.y; i++)
        {
            for (int j = 0; j < _blockMapSO.mapSize.x; j++)
            {
                BlockFrame frame = Instantiate(_blockFrame, transform);
                frame.SetFramePosition(new Vector2Int(j, i));
                frame.onPointerEnterEvent += OnPointerEnterSlot;
                frame.onPointerExitEvent += OnPointerExitSlot;

                _blockFrameArray[j, i] = frame;
            }
        }

        _blockParent.SetAsLastSibling();
    }



    #region Event Handlers

    private void CheckInsertableBlockExsist(RemainBlockChangeEvent evt)
    {
        if (PlayerActionController.Instance.CanTakeAction() == false)
        {
            if (BlockMapController.CanInsertBlock(_mapInfo.blockMap, evt.remainBlock) == false) //Something went wrong when build
            {
                Bus<GameOverEvent>.Publish(new GameOverEvent());
            }
        }
    }

    private void OnSelectedBlock(BlockSelectEvent evt)
    {
        _selectedBlock = evt.selectedBlock;
        _selectedIndex = evt.selectedIndex;
    }

    private void TrySetBlock(BlockReleaseEvent evt)
    {
        if (_canInsertItem && _selectedFrame != null)
        {
            foreach (Vector2Int pos in _selectedFrame)
            {
                Vector2Int offset = _selectedPosition - _selectedBlock.BlockInfo.blockPositions[_selectedIndex];
                SetBlock(_selectedBlock.GetBlockSlot(pos - offset), pos);
            }
            CheckBlockDestroy();
            CheckPerfect();

            Bus<BlockSetEvent>.Publish(new BlockSetEvent(_selectedBlock));
        }
        else
        {
            Bus<BlockReturnEvent>.Publish(new BlockReturnEvent(_selectedBlock));
        }

        _selectedBlock = null;
        _canInsertItem = false;
        _selectedFrame?.Clear();
    }

    public void SetBlock(Block block, Vector2Int position)
    {
        _blockFrameArray[position.x, position.y].SetSelection(false);
        _mapInfo.blockMap[position.x, position.y] = block;
        block.transform.SetParent(_blockParent);
        block.transform.localScale = Vector3.one;
        block.SetPosition(_blockFrameArray[position.x, position.y].Position);
    }

    private void CheckPerfect()
    {
        bool isPerfect = true;
        for (int i = 0; i < _mapInfo.blockMap.GetLength(0); i++)
        {
            for (int j = 0; j < _mapInfo.blockMap.GetLength(1); j++)
            {
                if (_mapInfo.blockMap[i, j] != null)
                {
                    isPerfect = false;
                    break;
                }
            }
            if (isPerfect == false) break;
        }

        if (isPerfect) _perfectText.Perfect();
    }

    public void CheckBlockDestroy()
    {
        _readyBreakingBlock = BlockMapController.CheckBreakingBlocks(_mapInfo.blockMap);
        Bus<AddScoreEvent>.Publish(new AddScoreEvent(_readyBreakingBlock.Count));
        foreach (Vector2Int pos in _readyBreakingBlock)
        {
            _mapInfo.blockMap[pos.x, pos.y]?.DestroyBlock();
            _mapInfo.blockMap[pos.x, pos.y] = null;
        }
        _readyBreakingBlock.Clear();
    }

    private void OnPointerEnterSlot(Vector2Int position)
    {
        _selectedPosition = position;

        if (_selectedBlock != null)
        {
            _canInsertItem = BlockMapController.TryInsertBlock(_mapInfo.blockMap, _selectedBlock.BlockInfo,
                _selectedIndex, position, out _selectedFrame);

            if (_canInsertItem)
            {
                for (int i = 0; i < _selectedFrame.Count; ++i)
                {
                    _blockFrameArray[_selectedFrame[i].x, _selectedFrame[i].y].SetSelection(true);
                }
            }
            else
            {
                _selectedFrame.Clear();
            }
        }
    }

    private void OnPointerExitSlot()
    {
        if (_selectedFrame != null)
        {
            for (int i = 0; i < _selectedFrame.Count; ++i)
            {
                _blockFrameArray[_selectedFrame[i].x, _selectedFrame[i].y].SetSelection(false);
            }
            _canInsertItem = false;
            _selectedFrame = null;
        }
    }

    #endregion
}
