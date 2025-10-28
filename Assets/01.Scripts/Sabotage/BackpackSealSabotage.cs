using Unity.VisualScripting;
using UnityEngine;

public class BackpackSealSabotage : Sabotage
{
    [SerializeField] private int _sealTurn = 3;
    private BlockBackpack _backpack;
    private int _remainTurn;

    public override void DoSabotage(BlockMap map)
    {
        _backpack = UIManager.Instance.BlockBackpack;
        _backpack.Seal();
        _remainTurn = _sealTurn;

        Bus<BlockSetEvent>.OnEvent += OnUseTurn;
    }

    private void OnUseTurn(BlockSetEvent evt)
    {
        _remainTurn--;

        if(_remainTurn == 0)
        {
            _backpack.Reveal();
            Bus<BlockSetEvent>.OnEvent -= OnUseTurn;
        }
    }

    public override void OnDestroy()
    {
        Bus<BlockSetEvent>.OnEvent -= OnUseTurn;
    }
}
