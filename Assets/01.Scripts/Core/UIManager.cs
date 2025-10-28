using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private BlockBackpack _blockBackpack;

    public BlockBackpack BlockBackpack => _blockBackpack;
}
