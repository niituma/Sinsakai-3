using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] bool cursorLocked = true;
    bool _ispause = default;
    [SerializeField] GameObject _fadePanel = default;
    [SerializeField] GameObject _player = default;
    [SerializeField] GameObject _gameOverText = default;
    ControllerSystem _playercon;
    FadeOutIn _fade;
    public bool Ispause { get => _ispause; set => _ispause = value; }
    private void Start()
    {
        _playercon = _player.GetComponent<ControllerSystem>();
        _fade = _fadePanel.GetComponent<FadeOutIn>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            SetCursorState();
            _playercon.aim = false;
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
    public void GameOver()
    {
        _gameOverText.SetActive(true);
        _fade.IsFadeOut(1);
    }
}
