using UnityEngine;
using System;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RingCommand : UIBehaviour, ILayoutGroup
{
    public float _radius = 100;
    public float _offsetAngle;
    float _value = 0;
    float _scroll = 0;
    float _padscroll = 0;
    int _magicModeIndex = 0;
    bool _isuseWheel = true;
    bool _isrotation = true;
    [SerializeField] PlayerMagic _magic;
    private void Update()
    {
        if (_isuseWheel)
        {
            _scroll = Input.GetAxis("Mouse ScrollWheel");
            _padscroll = Input.GetAxis("ChangeMagic");


            if (_scroll != 0 || _padscroll != 0)
            {
                _isuseWheel = false;
            }
        }
        if (!_isuseWheel && _isrotation)
        {
            _isrotation = false;
            if (_scroll > 0 || _padscroll > 0)
            {
                _value = _offsetAngle + 120;
                _magicModeIndex++;
                MagicChange();
            }
            else if (_scroll < 0 || _padscroll < 0)
            {
                _value = _offsetAngle - 120;
                _magicModeIndex--;
                MagicChange();
            }

            DOTween.To(() => _offsetAngle, x => _offsetAngle = x, _value, 0.5f).OnComplete(() =>
            {
                _isuseWheel = true;
                _isrotation = true;
            });
        }

        Arrange();
    }
    // 要素数が変わると自動的に呼ばれるコールバック
    #region ILayoutController implementation
    public void SetLayoutHorizontal() { }
    public void SetLayoutVertical()
    {
        Arrange();
    }
    #endregion
    void MagicChange()
    {
        if (_magicModeIndex > 2)
        {
            _magicModeIndex = 0;
        }
        else if (_magicModeIndex < 0)
        {
            _magicModeIndex = 2;
        }
        _magic.MagicMode = (PlayerMagic.Action)(_magicModeIndex % Enum.GetNames(typeof(PlayerMagic.Action)).Length);
    }

    void Arrange()
    {
        float splitAngle = 360 / transform.childCount;
        var rect = transform as RectTransform;

        for (int elementId = 0; elementId < transform.childCount; elementId++)
        {
            var child = transform.GetChild(elementId) as RectTransform;
            float currentAngle = splitAngle * elementId + _offsetAngle;
            child.anchoredPosition = new Vector2(
                Mathf.Cos(currentAngle * Mathf.Deg2Rad),
                Mathf.Sin(currentAngle * Mathf.Deg2Rad)) * _radius;
        }
        if (_offsetAngle < 0)
        {
            _offsetAngle = 330;
        }
        else if (_offsetAngle > 330)
        {
            _offsetAngle = 90;
        }
    }
}
