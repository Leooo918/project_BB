using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockParent : MonoBehaviour
{
    [SerializeField] private BlockSO _blockSO;
    [SerializeField] private Vector2 _blockSize;

    private Vector2 _offset;
    private int _blockIndex;
    private int _selectedIndex;
    private CanvasGroup _canvasGroup;
    private Dictionary<Vector2Int, Block> _blockSlotDict;
    private float _offsetMultiplier = 1;

    private RectTransform RectTrm => transform as RectTransform;
    public BlockSO BlockInfo => _blockSO;
    public int SelectedIndex => _selectedIndex;
    public int Index => _blockIndex;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        //Initialize(_blockSO);
    }

    private void OnDestroy()
    {
        if (_blockSlotDict != null)
            _blockSlotDict.Keys.ToList().ForEach(key =>
            {
                _blockSlotDict[key].onMouseDownEvent -= OnPointerDown;
                _blockSlotDict[key].onMouseUpEvent -= OnPointerUp;
                _blockSlotDict[key].onDragEvent -= OnDragSlot;

            });
    }

    public void Initialize(BlockSO block)
    {
        _blockSO = block;
        _blockSlotDict = new Dictionary<Vector2Int, Block>();

        for (int i = 0; i < _blockSO.blockPositions.Count; i++)
        {
            int x = _blockSO.blockPositions[i].x;
            int y = _blockSO.blockPositions[i].y;

            Block slot = Instantiate(_blockSO.blockPrefab, transform);
            slot.transform.SetLocalPositionAndRotation(new Vector3(x * _blockSize.x, y * _blockSize.y, 0f), Quaternion.identity);
            slot.SetIndex(i);
            slot.onMouseDownEvent += OnPointerDown;
            slot.onMouseUpEvent += OnPointerUp;
            slot.onDragEvent += OnDragSlot;

            _blockSlotDict.Add(_blockSO.blockPositions[i], slot);
        }
    }

    public Block GetBlockSlot(Vector2Int position)
    {
        if (_blockSlotDict.TryGetValue(position, out Block slot))
            return slot;
        return null;
    }

    public void SetPosition(Vector2 position)
    {
        RectTrm.position = position;
        RectTrm.anchoredPosition -= _blockSO.centerOffset * _blockSize * transform.localScale;
    }

    public void SetIndex(int index)
    {
        _blockIndex = index;
    }

    public void DestroyBlock()
    {
        //나중에는 진짜 그냥 부숴버리는 것으로
        //_blockSlotDict.Keys.ToList().ForEach(key => _blockSlotDict[key].SetMovable(false));
        Destroy(gameObject);
    }

    public void SetBlockMovable(bool movable)
    {
        foreach (var block in _blockSlotDict)
        {
            block.Value.SetMovable(movable);
        }
    }

    public void SetLocalScale(float scale)
    {
        transform.localScale = Vector3.one * scale;
        if(scale == 0)
        {
            _offsetMultiplier = 0;
            return;
        }
        _offsetMultiplier = 1f / scale;
    }

    #region Event

    private void OnDragSlot(int index, Vector2 mousePosition)
    {
        RectTrm.anchoredPosition = mousePosition - (_offset * _offsetMultiplier);
    }

    private void OnPointerDown(PointerEventData eventData, int index)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(RectTrm, eventData.position, eventData.pressEventCamera, out _offset);

        SetLocalScale(1);
        _selectedIndex = index;
        _canvasGroup.blocksRaycasts = false;

        Bus<BlockSelectEvent>.Publish(new BlockSelectEvent(this, _selectedIndex));
    }

    private void OnPointerUp()
    {
        _canvasGroup.blocksRaycasts = true;
        Bus<BlockReleaseEvent>.Publish(new BlockReleaseEvent());
    }

    #endregion
}
