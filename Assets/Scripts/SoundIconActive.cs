using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundIconActive : MonoBehaviour
{
    [SerializeField] GameObject SoundIcon = default;
    [SerializeField] GameObject MuteIcon = default;
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
            SoundIcon.SetActive(true);
            MuteIcon.SetActive(false);
        }
        else
        {
            SoundIcon.SetActive(false);
            MuteIcon.SetActive(true);
        }
    }
}
