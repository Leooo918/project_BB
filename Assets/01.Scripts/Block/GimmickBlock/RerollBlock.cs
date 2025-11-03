public class RerollBlock : ItemBlock
{
    protected override void GiveItem()
    {
        PlayerActionController.Instance.AddCount(PlayerActionEnum.Reroll, 1);
    }
}
