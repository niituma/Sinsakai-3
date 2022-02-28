using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTimeLineStart : MonoBehaviour
{
    [SerializeField] FadeOutIn _fadePanel = default;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _fadePanel.gameObject.SetActive(true);
            _fadePanel.IsTimeLineFadeOut();
        }
    }
}
