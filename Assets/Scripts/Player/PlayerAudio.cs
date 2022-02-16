using System.Collections;
using System;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header("攻撃用サウンド")]
    [SerializeField] AudioClip _fireRightPanch = default;
    [SerializeField] AudioClip _fireLeftPanch = default;
    [SerializeField] AudioClip _fireKick = default;
    [SerializeField] AudioClip _fireBurst = default;
    [SerializeField] AudioClip _iceLance = default;
    [SerializeField] AudioClip _rockGun = default;
    [SerializeField] AudioClip _earthRock = default;
    [SerializeField] AudioClip _notBullet = default;
    [Header("行動サウンド")]
    [SerializeField] AudioClip _grapple = default;
    [SerializeField] AudioClip _touch = default;
    [SerializeField] AudioClip _climb = default;
    [SerializeField] AudioClip _jump = default;
    [SerializeField] AudioClip _land = default;

    [SerializeField] AudioSource _playerSFX;

    //行動サウンド
    void HandleTouch()
    {
        _playerSFX.PlayOneShot(_touch);
    }
    void ClimbUp()
    {
        _playerSFX.PlayOneShot(_climb);
    }
    void JumpSound()
    {
        _playerSFX.PlayOneShot(_jump);
    }
    void LandSound()
    {
        _playerSFX.volume = 0.2f;
        _playerSFX.PlayOneShot(_land);
        StartCoroutine(DelayMethod(0.2f,() =>_playerSFX.volume = 1f));
    }


    //攻撃用サウンド
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
    public void NotGunBullet()
    {
        _playerSFX.PlayOneShot(_notBullet);
    }
    void EarthRock()
    {
        _playerSFX.PlayOneShot(_earthRock);
    }
    void Grapple()
    {
        _playerSFX.PlayOneShot(_grapple);
    }
    IEnumerator DelayMethod(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action?.Invoke();
    }
}