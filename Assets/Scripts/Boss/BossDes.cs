using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDes : MonoBehaviour
{
    public float spawnEffectTime = 2;
    public float pause = 1;
    public AnimationCurve fadeIn;
    GameObject _boss = default;
    ParticleSystem ps;
    float timer = 0;
    Renderer _renderer;

    int shaderProperty;

    void Start()
    {
        shaderProperty = Shader.PropertyToID("_cutoff");
        _boss = GameObject.FindGameObjectWithTag("DeadBoss");
        _renderer = GetComponent<Renderer>();
        ps = GetComponentInChildren<ParticleSystem>();

        var main = ps.main;
        main.duration = spawnEffectTime;

        ps.Play();

    }

    void Update()
    {

        timer += Time.deltaTime;
        if (timer >= spawnEffectTime + pause)
        {
            Destroy(_boss);
        }


        _renderer.material.SetFloat(shaderProperty, fadeIn.Evaluate(Mathf.InverseLerp(0, spawnEffectTime, timer)));

    }
}
