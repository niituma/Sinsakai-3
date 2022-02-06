using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    GameObject _gameManeger = default;
    GameManager _gm;
    private void Des()
    {
        Destroy(gameObject);
    }
    void FadeOut()
    {
        _gameManeger = GameObject.FindGameObjectWithTag("GameManager");
        _gm = _gameManeger.GetComponent<GameManager>();
        _gm.GameOver();
    }
}
