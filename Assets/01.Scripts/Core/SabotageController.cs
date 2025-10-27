using System.Collections.Generic;
using UnityEngine;

public class SabotageController : MonoBehaviour
{
    private const int SABOTAGE_REQUIRE_TURN = 5;

    //[SerializeField] private PlayerActionButton _actionButton;
    [SerializeField] private SabotageIndicator _indicator;
    [SerializeField] private List<SabotageSO> _sabotage;
    private SabotageSO _readySabotage;
    private BlockMap _blockMap;
    private int _remainTurn;

    private void OnDestroy()
    {
        Bus<BlockSetEvent>.OnEvent -= OnSetBlock;
    }

    public void StartSabotage(BlockMap map)
    {
        _blockMap = map;
        _remainTurn = SABOTAGE_REQUIRE_TURN;
        _readySabotage = _sabotage.GetSingleElementInList();

        Bus<BlockSetEvent>.OnEvent += OnSetBlock;
    }

    public void OnSetBlock(BlockSetEvent evt)
    {
        if (--_remainTurn <= 0)
        {
            _remainTurn = SABOTAGE_REQUIRE_TURN;
            _readySabotage.sabotage.DoSabotage(_blockMap);
            _readySabotage = _sabotage.GetSingleElementInList();
        }

        _indicator.SetIndicator(_readySabotage, _remainTurn);
    }
}
