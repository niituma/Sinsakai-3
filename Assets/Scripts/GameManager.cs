using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] bool cursorLocked = true;
    bool _ispause = default;
    public bool Ispause { get => _ispause; set => _ispause = value; }
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
            _ispause = !_ispause;
        }
    }
    public void SetCursorState()
    {
        cursorLocked = !cursorLocked;
        Cursor.visible = cursorLocked ? false : true;
        Cursor.lockState = cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
    }
    public void ClosePauseUI()
    {
        _ispause = false;
    }
    public void LoadScene(int num)
    {
        SceneManager.LoadScene(num);
    }
}
