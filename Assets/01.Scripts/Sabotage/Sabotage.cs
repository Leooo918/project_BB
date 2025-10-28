using System;

[Serializable]
public abstract class Sabotage
{
    public abstract void DoSabotage(BlockMap map);

    public virtual void OnDestroy() { }
}
