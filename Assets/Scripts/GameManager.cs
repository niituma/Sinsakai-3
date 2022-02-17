using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] bool cursorLocked = true;
    bool _ispause = default;
    bool _isGameOver = default;
    string[] _joycon;
    /// <summary>/// 1回だけ実行するためのbool/// </summary>
    bool _isone = true;
    [SerializeField] GameObject _fadePanel = default;
    [SerializeField] GameObject _player = default;
    [SerializeField] GameObject _gameOverText = default;
    [SerializeField] AudioManager _audioManager;
    [SerializeField] PlayableDirector _playableDirector;
    [SerializeField] CinemachineVirtualCamera _camera;
    bool _isjoycon = false;
    CinemachinePOV _cameramove;
    ControllerSystem _playercon;
    FadeOutIn _fade;
    public bool Ispause { get => _ispause; set => _ispause = value; }
    public bool IsGameOver { get => _isGameOver; set => _isGameOver = value; }
    public bool Isjoycon { get => _isjoycon; set => _isjoycon = value; }

    private void Start()
    {
        _cameramove = _camera.GetCinemachineComponent<CinemachinePOV>();
        _playercon = _player.GetComponent<ControllerSystem>();
        _fade = _fadePanel.GetComponent<FadeOutIn>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        //コントローラーが接続されているか確認してInputを変える
        _joycon = Input.GetJoystickNames();
        if (_joycon[0] == "")
        {
            Isjoycon = false;
            _cameramove.m_HorizontalAxis.m_InputAxisName = "X Axes";
            _cameramove.m_VerticalAxis.m_InputAxisName = "Y Axes";
        }
        else
        {
            Isjoycon = true;
            _cameramove.m_HorizontalAxis.m_InputAxisName = "X PadAxes";
            _cameramove.m_VerticalAxis.m_InputAxisName = "Y PadAxes";
        }

        if (Input.GetButtonDown("Cancel") && _player)
        {
            if (_ispause)
            {
                if (_playableDirector)
                {
                    _playableDirector.Resume();
                }
                _audioManager.ButtonCanselSound();
            }
            else
            {
                if (_playableDirector)
                {
                    _playableDirector.Pause();
                }
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
