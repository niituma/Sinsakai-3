using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundIconActive : MonoBehaviour
{
    [SerializeField] GameObject _soundIcon = default;
    [SerializeField] GameObject _muteIcon = default;
    [SerializeField] Slider _slider;
    [SerializeField] AudioSource BGM;

    private void Start()
    {
        BGM.volume = _slider.value;
    }
    public void ChangeVolume()
    {
        BGM.volume = _slider.value;

        if (_slider.value > 0)
        {
            _soundIcon.SetActive(true);
            _muteIcon.SetActive(false);
        }
        else
        {
            _soundIcon.SetActive(false);
            _muteIcon.SetActive(true);
        }
    }
}
