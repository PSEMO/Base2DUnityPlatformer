using UnityEngine;

public class MenuScreen : MonoBehaviour
{
    public MenuType Type;

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
