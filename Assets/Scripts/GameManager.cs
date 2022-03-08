using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [SerializeField] bool cursorLocked = true;
    [SerializeField] Transform _resetPos = default;
    bool _ispause = default;
    bool _isGameOver = default;
    string[] _joycon;
    /// <summary>/// 1回だけ実行するためのbool/// </summary>
    bool _isone = true;
    [SerializeField] GameObject _fadePanel = default;
    [SerializeField] GameObject _player = default;
    [SerializeField] GameObject _gameOverText = default;
    [SerializeField] AudioManager _audioManager;
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
        if (_player.transform.position.y < -10f)
        {
            _player.transform.position = _resetPos.position;
        }
        //コントローラーが接続されているか確認してInputを変える
        _joycon = Input.GetJoystickNames();
        if (_joycon.Length == 0)
        {
            Isjoycon = false;
            _cameramove.m_HorizontalAxis.m_InputAxisName = "X Axes";
            _cameramove.m_VerticalAxis.m_InputAxisName = "Y Axes";
        }
        else
        {
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
        }

        if (Input.GetButtonDown("Cancel") && _player)
        {
            _playercon.aim = false;
            _ispause = !_ispause;
            if (!_ispause)
            {
                _audioManager.ButtonCanselSound();
            }
            else
            {
                _audioManager.ButtonPushSound();
            }
        }

        //Playerが死んだとき
        if (!_player && _isone)
        {
            _isGameOver = true;
            _audioManager.IsAudioChange = true;
            _isone = false;
        }
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
