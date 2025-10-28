using UnityEngine;

public class SafeArea : MonoBehaviour
{
    private RectTransform _rectTrm;
    private void Awake()
    {
        _rectTrm = GetComponent<RectTransform>();
        _rectTrm.anchorMin = 
            new Vector2(
            Screen.safeArea.min.x / Screen.width, 
            Screen.safeArea.min.y / Screen.height);

        _rectTrm.anchorMax =
            new Vector2(
            Screen.safeArea.max.x / Screen.width,
            Screen.safeArea.max.y / Screen.height);
    }
}
