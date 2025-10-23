using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockFrame : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public event Action<Vector2Int> onPointerEnterEvent;
    public event Action onPointerExitEvent;

    private Vector2Int _framePosition;
    [SerializeField] private GameObject _selectedObject;
    private RectTransform RectTrm => transform as RectTransform;
    public Vector2 Position => RectTrm.position;



    public void SetFramePosition(Vector2Int position)
    {
        _framePosition = position;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointerEnterEvent?.Invoke(_framePosition);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onPointerExitEvent?.Invoke();
    }

    public void SetSelection(bool isSelected)
    {
        _selectedObject.SetActive(isSelected);
    }
}
