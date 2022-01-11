using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneActive : MonoBehaviour
{
    [SerializeField] GameObject _stone = default;
    [SerializeField] GameObject _eff = default;
    [SerializeField] GameObject _eff2 = default;
    [SerializeField] string _attackname = "PMagicBall";
    ObjSpin _spin = default;
    Animator _anim = default;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _spin = _stone.GetComponent<ObjSpin>();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == _attackname) {
            _eff.SetActive(true);
            _eff2.SetActive(true);
            _spin.enabled = true;
            _anim.SetTrigger("Active");
            FindObjectOfType<OpenGade>().OpenCount();
            Destroy(this.GetComponent<StoneActive>());
        }
    }
}
