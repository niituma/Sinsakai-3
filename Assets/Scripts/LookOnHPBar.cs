using UnityEngine;

public class LookOnHPBar : MonoBehaviour
{
    [SerializeField] Canvas _hpcanvas = default;
    // Update is called once per frame
    void Update()
    {
        _hpcanvas.transform.LookAt(GameObject.Find("Player").transform);
    }
}
