﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagic : MonoBehaviour
{
    [Header("攻撃")]
    [SerializeField] GameObject _magicoverparticle = default;
    ParticleSystem _overparticle = default;
    [SerializeField] GameObject _magicMuzzle = default;
    [SerializeField] GameObject _magiceff = default;
    [SerializeField] GameObject _impactEff = default;
    [SerializeField] GameObject _FireSEff = default;
    [SerializeField] GameObject _IceBommerEff = default;
    [SerializeField] GameObject _EarthSpikeEff = default;
    [SerializeField] GameObject _rightattackmuzzle = default;
    [SerializeField] GameObject _leftattackmuzzle = default;
    [SerializeField] float _magicCoolDownSpeed = 2f;
    [SerializeField] float _magiclimiter = 0f;
    [SerializeField] float _magiclimit = 100f;
    bool _iscombo = default;
    public bool Iscombo { get => _iscombo; set => _iscombo = value; }
    public enum Action
    {
        Fire,
        Ice,
        Earth
    }
    public Action _magicMode;

    Animator _anim = default;
    ControllerSystem _input;


    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _input = GetComponent<ControllerSystem>();
        _overparticle = _magicoverparticle.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        AttackMotion();
        MagicOverFlow();
    }
    void AttackMotion()
    {
        if (_magicMode == Action.Fire)
        {
            _anim.SetBool("Punch", _input.attack);
            _input.attack = false;
        }

        if (_magiclimiter < _magiclimit)
        {
            _anim.SetBool("Magic", _input.fire);
        }
        _input.fire = false;
    }
    void MagicOverFlow()
    {
        var emission = _overparticle.emission;

        if (_magiclimiter > 0)
        {
            _magiclimiter -= _magicCoolDownSpeed * Time.deltaTime;
        }
        else if (_magiclimiter < 0)
        {
            _magiclimiter = 0;
        }

        if (_magiclimiter >= 100f)
        {
            emission.rateOverTime = 300f;
        }
        else if (_magiclimiter >= 60f)
        {
            emission.rateOverTime = 100f;
        }
        else if (_magiclimiter >= 30f)
        {
            emission.rateOverTime = 10f;
        }
        else
        {
            emission.rateOverTime = 0f;
        }
    }
    void Magic(int magics)
    {
        switch (magics)
        {
            case 0:
                Instantiate(_impactEff, _leftattackmuzzle.transform.position, this.transform.rotation);
                break;
            case 1:
                Instantiate(_impactEff, _rightattackmuzzle.transform.position, this.transform.rotation);
                break;
            case 2:
                Instantiate(_magiceff, _rightattackmuzzle.transform.position, this.transform.rotation);
                _magiclimiter += 13f;
                break;
            case 3:
                Instantiate(_FireSEff, transform.position, Quaternion.identity);
                break;
            case 4:
                Instantiate(_IceBommerEff, _rightattackmuzzle.transform.position, this.transform.rotation);
                break;
            case 5:
                Instantiate(_EarthSpikeEff, _magicMuzzle.transform.position, this.transform.rotation);
                break;
            default:
                break;
        }
    }
    void ConboSwith(int OnOff)
    {
        switch (OnOff)
        {
            case 1:
                _iscombo = true;
                break;
            case 2:
                _iscombo = false;
                break;
            default:
                Debug.LogWarning("指定の範囲外です。Animationのイベントから指定してください。");
                break;
        }
    }
}
