using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip BGM = default;
    [SerializeField] AudioClip GameOverBGM = default;
    bool _isAudioChange = false;
    [SerializeField] bool on;
    [SerializeField] GameManager _gm;
    AudioSource _audio;

    public bool IsAudioChange { get => _isAudioChange; set => _isAudioChange = value; }

    // Start is called before the first frame update
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        _audio.clip = BGM;
        _audio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (on)
        {
            _audio.Play();
        }
         if (_isAudioChange && _gm.IsGameOver)
        {
            _audio.clip = GameOverBGM;
            _audio.Play();
            _audio.loop = false;
            _isAudioChange = false;
        }
        else if (_isAudioChange)
        {
            _audio.clip = BGM;
            _isAudioChange = false;
        }
    }
}
