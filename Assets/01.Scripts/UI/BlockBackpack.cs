using UnityEngine;

public class BlockBackpack : MonoBehaviour
{
    private const float BLOCK_ICON_SIZE = 0.2f;

    [SerializeField] private RectTransform _originPos;
    [SerializeField] private float _xDiff;

    public void SetBackpack(BlockParent[] blocks)
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i].SetLocalScale(BLOCK_ICON_SIZE);
            blocks[i].SetPosition((Vector2)_originPos.position + (Vector2.right * _xDiff * i));
        }
    }
}
