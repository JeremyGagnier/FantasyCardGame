using UnityEngine;

abstract public class Menu : MonoBehaviour
{
    protected bool isOnTop = true;

    abstract public void Open(MenuState state);

    virtual public void Close()
    {
        Destroy(this.gameObject);
    }

    virtual public void Show(MenuState state)
    {
        isOnTop = true;
    }

    virtual public void Hide()
    {
        isOnTop = false;
    }
}
