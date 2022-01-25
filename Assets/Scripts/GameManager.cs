using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance => _instance;
    private static GameManager _instance;

    [SerializeField] GameObject PausePanel = default;
    bool _ispause = default;
    public bool Ispause { get => _ispause; set => _ispause = value; }
    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            PauseResume();
        }
    }
    public void PauseResume()
    {
        Ispause = !Ispause;

        if (Ispause)
        {
            PausePanel.SetActive(true);
            Cursor.visible = true;
        }
        else
        {
            PausePanel.SetActive(false);
            Cursor.visible = false;
        }

    }
}
