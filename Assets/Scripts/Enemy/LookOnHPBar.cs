using UnityEngine;

public class LookOnHPBar : MonoBehaviour
{
    [SerializeField] Canvas _canvas = default;
    GameObject lookTarget = default;
    void Start()
    {
        lookTarget = Camera.main.gameObject;
    }

    void Update()
    {
        var direction = lookTarget.transform.position - _canvas.transform.position;
        direction.y = 0;

        var lookRotation = Quaternion.LookRotation(direction, Vector3.up);
        _canvas.transform.rotation = Quaternion.Lerp(_canvas.transform.rotation, lookRotation, 0.1f);
    }
}
