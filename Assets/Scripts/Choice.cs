using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Choice : MonoBehaviour
{
    [SerializeField] Button button;
    void OnEnable()
    {
        if (FindObjectOfType<GameManager>().Isjoycon)
        {
            button.Select();
        }
    }
    void Start()
    {
        //ボタンが選択された状態になる
        if (FindObjectOfType<GameManager>().Isjoycon)
        {
            button.Select();
        }

    }
    public void SlectButton()
    {
        button.Select();
    }
    public void UnSlectButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}