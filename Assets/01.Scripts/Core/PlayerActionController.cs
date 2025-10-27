using System.Collections.Generic;
using UnityEngine;

public class PlayerActionController : MonoBehaviour
{
    [SerializeField] private PlayerActionButton _rerollButton;
    private Dictionary<PlayerActionEnum, PlayerActionButton> _buttonDict;

    private void Start()
    {
        SetCount(PlayerActionEnum.Reroll, 5);   //임시로 여기서 나중에 난이도에 따라서 바뀌게
    }

    public PlayerActionButton GetButton(PlayerActionEnum actionType)
    {
        return _buttonDict[actionType];
    }

    public void AddCount(PlayerActionEnum actionType, int count)
    {
        _buttonDict[actionType].AddCount(count);
    }

    public void SetCount(PlayerActionEnum actionType, int count)
    {
        _buttonDict[actionType].SetCount(count);
    }
}
