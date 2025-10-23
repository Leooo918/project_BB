using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

public class BlockMap : MonoBehaviour
{
    [SerializeField] private BlockMapSO _blockMapSO;
    [SerializeField] private BlockFrame _blockFrame;

    private Vector2Int _selectedPosition;
    private bool _canInsertItem = false;
    private Block _selectedBlock;
    private int _selectedIndex;

    private BlockSlot[,] _blockSlotArray;
    private BlockFrame[,] _blockFrameArray;
    private List<Vector2Int> _selectedFrame = new();

    private void Awake()
    {
        if (_blockMapSO != null) Initialize(_blockMapSO);
        Bus<BlockSelectEvent>.OnEvent += OnSelectedBlock;
        Bus<BlockReleaseEvent>.OnEvent += OnSetBlock;
    }

    private void OnDestroy()
    {
        Bus<BlockSelectEvent>.OnEvent -= OnSelectedBlock;
        Bus<BlockReleaseEvent>.OnEvent -= OnSetBlock;
    }

    public void Initialize(BlockMapSO blockMap)
    {
        _blockMapSO = blockMap;
        _blockSlotArray = new BlockSlot[_blockMapSO.mapSize.x, _blockMapSO.mapSize.y];
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
    }


    private void OnSelectedBlock(BlockSelectEvent evt)
    {
        _selectedBlock = evt.selectedBlock;
        _selectedIndex = evt.selectedIndex;
    }

    private void OnSetBlock(BlockReleaseEvent evt)
    {
        if (_canInsertItem)
        {
            foreach (Vector2Int pos in _selectedFrame)
            {
                _blockFrameArray[pos.x, pos.y].SetSelection(false);

                Vector2Int offset = _selectedPosition - _selectedBlock.BlockInfo.blockPositions[_selectedIndex];
                _blockSlotArray[pos.x, pos.y] = _selectedBlock.GetBlockSlot(pos - offset);
            }
            //위치 찾아주기
            Vector2Int originPos = _selectedPosition - _selectedBlock.BlockInfo.blockPositions[_selectedIndex];
            _selectedBlock.SetPosition(_blockFrameArray[originPos.x, originPos.y].Position);

            List<Vector2Int> destroyBlock = BlockMapController.CheckBreakingBlocks(_blockSlotArray);
            foreach (Vector2Int pos in destroyBlock)
            {
                _blockSlotArray[pos.x, pos.y].DestroyBlock();
                _blockSlotArray[pos.x, pos.y] = null;
            }

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


    private void OnPointerEnterSlot(Vector2Int position)
    {
        _selectedPosition = position;

        if (_selectedBlock != null)
        {
            _canInsertItem = BlockMapController.TryInsertBlock(_blockSlotArray, _selectedBlock.BlockInfo,
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
        }
    }
}
