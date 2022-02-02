using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance => _instance;
    private static GameManager _instance;

    [SerializeField] GameObject PausePanel = default;
    [SerializeField] bool cursorLocked = true;
    bool _ispause = default;
    public bool Ispause { get => _ispause; set => _ispause = value; }
    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            SetCursorState();
            PauseResume();
        }
    }
    public void SetCursorState()
    {
        cursorLocked = !cursorLocked;
        Cursor.visible = cursorLocked ? false : true;
        Cursor.lockState = cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
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
