using UnityEngine;
using System;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RingCommand : UIBehaviour, ILayoutGroup
{
    public float radius = 100;
    public float offsetAngle;
    float value = 0;
    float scroll = 0;
    int _magicModeIndex = 0;
    bool _isuseWheel = true;
    bool _isrotation = true;
    [SerializeField] PlayerMagic _magic;
    private void Update()
    {
        if (_isuseWheel)
        {
            scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
            {
                _isuseWheel = false;
            }
        }
        if (!_isuseWheel && _isrotation)
        {
            _isrotation = false;
            if (scroll > 0)
            {
                value = offsetAngle + 120;
                _magicModeIndex++;
                MagicChange();
            }
            else if (scroll < 0)
            {
                value = offsetAngle - 120;
                _magicModeIndex--;
                MagicChange();
            }

            DOTween.To(() => offsetAngle, x => offsetAngle = x, value, 0.5f).OnComplete(() =>
            {
                _isuseWheel = true;
                _isrotation = true;
            });
        }

        Arrange();
    }
    protected override void OnValidate()
    {
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
            float currentAngle = splitAngle * elementId + offsetAngle;
            child.anchoredPosition = new Vector2(
                Mathf.Cos(currentAngle * Mathf.Deg2Rad),
                Mathf.Sin(currentAngle * Mathf.Deg2Rad)) * radius;
        }
        if (offsetAngle < 0)
        {
            offsetAngle = 330;
        }
        else if (offsetAngle > 330)
        {
            offsetAngle = 90;
        }
    }
}
