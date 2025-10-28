using System.Collections.Generic;
using UnityEngine;

public class BlockBackpack : MonoBehaviour
{
    private const float BLOCK_ICON_SIZE = 0.2f;

    [SerializeField] private GameObject _sealObject;
    [SerializeField] private RectTransform _originPos;

    private List<Vector2> _blockSpawnPositions;
    private BlockParent[] _backpack;
    private bool _isSealed = false;

    private RectTransform RectTrm => transform as RectTransform;

    public void SetBackpack(BlockParent[] blocks)
    {
        if(_blockSpawnPositions == null || _blockSpawnPositions.Count == 0)
        {
            _blockSpawnPositions = new List<Vector2>();
            float space = (RectTrm.rect.width - (_originPos.anchoredPosition.x * 2)) / (blocks.Length - 1);

            _blockSpawnPositions = new List<Vector2>();
            _blockSpawnPositions.Add(_originPos.position);
            for (int i = 1; i < blocks.Length; i++)
            {
                _originPos.anchoredPosition += new Vector2(space, 0);
                _blockSpawnPositions.Add(_originPos.position);
            }
        }

        _backpack = blocks;
        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i].SetLocalScale(_isSealed ? 0 : BLOCK_ICON_SIZE);
            blocks[i].SetPosition(_blockSpawnPositions[i]);
        }
    }

    public void Seal()
    {
        _isSealed = true;
        _sealObject.SetActive(true);
        foreach (BlockParent block in _backpack)
        {
            block.SetLocalScale(0);
        }
    }

    public void Reveal()
    {
        _isSealed = false;
        _sealObject.SetActive(false);
        foreach (BlockParent block in _backpack)
        {
            block.SetLocalScale(BLOCK_ICON_SIZE);
        }
    }

}
