using System;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutIn : MonoBehaviour
{
    [SerializeField] float _fadeInSpeed = 0.8f;
    [SerializeField] float _fadeOutSpeed = 0.01f;
    [SerializeField] float _timeLineFadeOutSpeed = 0.01f;
    [Tooltip("画面が真っ暗になってからシーン遷移を待つ時間"), SerializeField]
    float _sceneDlaytime = 3f;
    float red, green, blue;
    float alfa;
    [SerializeField] bool _isFadeOut = default;
    [SerializeField] bool _isFadeIn = default;
    [SerializeField] bool _isTimeLineFadeOut = default;
    [SerializeField] GameObject _bossTimeLine = default;
    int _scenenum = 0;
    Image fadeImage;

    private void Start()
    {
        fadeImage = GetComponent<Image>();
        red = fadeImage.color.r;
        green = fadeImage.color.g;
        blue = fadeImage.color.b;
        alfa = fadeImage.color.a;
    }
    private void Update()
    {
        if (_isFadeOut)
        {
            StartFadeOut();
        }
        else if (_isFadeIn)
        {
            StartFadeIn();
        }
        else if (_isTimeLineFadeOut)
        {
            StartTimeLineFadeOut();
        }
    }
    public void IsFadeIn()
    {
        this.gameObject.SetActive(true);
        _isFadeIn = true;
    }
    public void IsFadeOut(int num)
    {
        this.gameObject.SetActive(true);  // a)パネルの表示をオンにする
        _isFadeOut = true;
        _scenenum = num;
    }
    public void IsTimeLineFadeOut()
    {
        this.gameObject.SetActive(true);  // a)パネルの表示をオンにする
        _isTimeLineFadeOut = true;
    }
    void StartFadeIn()
    {
        alfa -= _fadeInSpeed * Time.deltaTime;                //a)不透明度を徐々に下げる
        fadeImage.color = new Color(red, green, blue, alfa);    //b)変更した不透明度パネルに反映する
        if (alfa <= 0)
        {
            _isFadeIn = false;
            this.gameObject.SetActive(false);
        }
    }
    void StartFadeOut()
    {
        alfa += _fadeOutSpeed * Time.deltaTime;         // b)不透明度を徐々にあげる
        fadeImage.color = new Color(red, green, blue, alfa);    // c)変更した透明度をパネルに反映する
        if (alfa >= 1)
        {
            StartCoroutine(DelayMethod(_sceneDlaytime, () =>
            {
                _isFadeOut = false;  //d)パネルの表示をオフにする
                SceneManager.LoadScene(_scenenum);
            }));
        }
    }
    void StartTimeLineFadeOut()
    {
        alfa += _timeLineFadeOutSpeed * Time.deltaTime;         // b)不透明度を徐々にあげる
        fadeImage.color = new Color(red, green, blue, alfa);    // c)変更した透明度をパネルに反映する
        if (alfa >= 1)
        {
            _bossTimeLine.SetActive(true);
            _isFadeOut = false;
        }
    }
    IEnumerator DelayMethod(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action?.Invoke();
    }
}
