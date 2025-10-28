using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionController : MonoSingleton<PlayerActionController>
{
    [SerializeField] private PlayerActionButton _rerollButton;
    private Dictionary<PlayerActionEnum, PlayerActionButton> _buttonDict;

    protected override void Awake()
    {
        _buttonDict = new Dictionary<PlayerActionEnum, PlayerActionButton>();
        _buttonDict.Add(PlayerActionEnum.Reroll, _rerollButton);

        if (_rerollButton != null)
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

    public bool CanTakeAction()
    {
        foreach (PlayerActionEnum action in Enum.GetValues(typeof(PlayerActionEnum)))
        {
            if (GetButton(action) != null && GetButton(action).RemainCount > 0)
                return true;
        }
        return false;
    }
}
