using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public event Action<int, Vector2> onDragEvent;

    [SerializeField] private GameObject _breakingEffect;
    private int _slotIndex;
    private bool _isBlockMovable = true;

    private RectTransform rectTrm;
    private RectTransform canvasRect;
    public event Action<PointerEventData, int> onMouseDownEvent;
    public event Action onMouseUpEvent;

    private void Awake()
    {
        rectTrm = transform as RectTransform;
        canvasRect = GetComponentInParent<Canvas>().transform as RectTransform;
    }


    public void SetIndex(int index) => _slotIndex = index;

    public void OnDrag(PointerEventData eventData)
    {
        if (_isBlockMovable == false) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, eventData.position, eventData.pressEventCamera, out Vector2 localMousePosition);

        onDragEvent?.Invoke(_slotIndex, localMousePosition);
    }

    public void SetMovable(bool isBlockMovable)
    {
        _isBlockMovable = isBlockMovable;
    }

    public void DestroyBlock()
    {
        //풀링으로 바꾸기
        Destroy(gameObject);
        Instantiate(_breakingEffect, transform.position, Quaternion.identity);
    }

    #region Events

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_isBlockMovable == false) return;
        onMouseDownEvent?.Invoke(eventData, _slotIndex);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_isBlockMovable == false) return;
        onMouseUpEvent?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isBlockMovable == false) return;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isBlockMovable == false) return;
    }

    #endregion
}
