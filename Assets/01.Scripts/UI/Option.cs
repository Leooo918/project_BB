using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Option : MonoBehaviour, IUIElement
{
    [SerializeField] private Button _close;
    [SerializeField] private Button _gototitle;

    private void Awake()
    {
        _close.onClick.AddListener(Disable);
        _gototitle.onClick.AddListener(() => SceneManager.LoadScene("TitleScene"));
    }

    public void EnableFor()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
