using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] AudioClip _fireRightPanch = default;
    [SerializeField] AudioClip _fireLeftPanch = default;
    [SerializeField] AudioClip _fireKick = default;
    [SerializeField] AudioClip _fireBurst = default;
    [SerializeField] AudioClip _iceLance = default;
    [SerializeField] AudioClip _rockGun = default;
    [SerializeField] AudioClip _earthRock = default;
    [SerializeField] AudioClip _touch = default;
    [SerializeField] AudioClip _climb = default;

    [SerializeField] AudioSource _playerSFX;
    void HandleTouch()
    {
        _playerSFX.PlayOneShot(_touch);
    }
    void ClimbUp()
    {
        _playerSFX.PlayOneShot(_climb);
    }
    void FireRightPanch()
    {
        _playerSFX.PlayOneShot(_fireRightPanch);
    }
    void FireLeftPanch()
    {
        _playerSFX.PlayOneShot(_fireLeftPanch);
    }
    void FireKick()
    {
        _playerSFX.PlayOneShot(_fireKick);
    }
    void FireBurst()
    {
        _playerSFX.PlayOneShot(_fireBurst);
    }
    void IceLance()
    {
        _playerSFX.PlayOneShot(_iceLance);
    }
    public void RockGun()
    {
        _playerSFX.PlayOneShot(_rockGun);
    }
    void EarthRock()
    {
        _playerSFX.PlayOneShot(_earthRock);
    }
}