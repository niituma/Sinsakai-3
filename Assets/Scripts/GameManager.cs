using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] bool cursorLocked = true;
    bool _ispause = default;
    bool _isGameOver = default;
    /// <summary>/// 1回だけ実行するためのbool/// </summary>
    bool _isone = true;
    [SerializeField] GameObject _fadePanel = default;
    [SerializeField] GameObject _player = default;
    [SerializeField] GameObject _gameOverText = default;
    [SerializeField] AudioManager _audioManager;
    ControllerSystem _playercon;
    FadeOutIn _fade;
    public bool Ispause { get => _ispause; set => _ispause = value; }
    public bool IsGameOver { get => _isGameOver; set => _isGameOver = value; }

    private void Start()
    {
        _playercon = _player.GetComponent<ControllerSystem>();
        _fade = _fadePanel.GetComponent<FadeOutIn>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        if (Input.GetButtonDown("Cancel") && _player)
        {
            if (_ispause)
            {
                _audioManager.ButtonCanselSound();
            }
            else
            {
                _audioManager.ButtonPushSound();
            }
            SetCursorState();
            _playercon.aim = false;
            _ispause = !_ispause;
        }

        //Playerが死んだとき
        if (!_player && _isone)
        {
            _isGameOver = true;
            _audioManager.IsAudioChange = true;
            _isone = false;
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
