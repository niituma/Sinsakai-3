using System.Collections;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip BGM = default;
    [SerializeField] AudioClip _gameOverBGM = default;
    [SerializeField] AudioClip _cancelSE = default;
    [SerializeField] AudioClip _pushSE = default;
    bool _isAudioChange = false;
    [SerializeField] GameManager _gm;
    [SerializeField] AudioSource _audioBGM;
    [SerializeField] AudioSource _audioPlayerSFX;
    [SerializeField] AudioSource _audioSE;

    public bool IsAudioChange { get => _isAudioChange; set => _isAudioChange = value; }

    // Start is called before the first frame update
    void Start()
    {
        _audioBGM.clip = BGM;
        _audioBGM.Play();
    }

    // Update is called once per frame
    void Update()
    {
         if (_isAudioChange && _gm.IsGameOver)
        {
            _audioBGM.clip = _gameOverBGM;
            _audioBGM.Play();
            _audioBGM.loop = false;
            _isAudioChange = false;
        }
        else if (_isAudioChange)
        {
            _audioBGM.clip = BGM;
            _isAudioChange = false;
        }
    }
    public void ButtonPushSound()
    {
        _audioSE.PlayOneShot(_pushSE);
    }
    public void ButtonCanselSound()
    {
        _audioSE.PlayOneShot(_cancelSE);
    }
    public void BGMOff()
    {
        StartCoroutine(DelayMethod(1f,() => _audioBGM.Stop()));
    }
    IEnumerator DelayMethod(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action?.Invoke();
    }
}
