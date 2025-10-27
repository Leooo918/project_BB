using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerActionButton : MonoBehaviour, IPointerClickHandler
{
     public UnityEvent onInteract;
    [SerializeField] private TextMeshProUGUI _remainText;
    [SerializeField] private int _remainCount = 0;

    public int RemainCount => _remainCount;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (_remainCount <= 0) return;

        Debug.Log("asdf");
        onInteract?.Invoke();
        AddCount(-1);
    }

    public void SetCount(int count)
    {
        _remainCount = count;
        UpdateText();
    }

    public void AddCount(int amount = 1)
    {
        _remainCount += amount;
        UpdateText();
    }

    public void UpdateText()
    {
        _remainText.SetText(_remainCount.ToString());
    }
}
