public interface IUIElement
{
    public void EnableFor();
    public void Disable();
}


public interface IUIElement<T>
{
    public void EnableFor(T data);
    public void Disable();
}


public interface IUIElement<T1, T2>
{
    public void EnableFor(T1 data, T2 callbackAction);
    public void Disable();
}