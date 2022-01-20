using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] bool mutekimode = default;
    //最大HPと現在のHP。
    [SerializeField] float maxHp = 200;
    float currentHp = 200;
    //Sliderを入れる
    [SerializeField] public Slider slider;
    /// <summary>入力された方向の XZ 平面でのベクトル</summary>
    [SerializeField] GameObject _corpse = default;
    [SerializeField] GameObject _gard = default;
    PlayerController _playerCon;

    void Start()
    {
        _playerCon = GetComponent<PlayerController>();
        slider.value = 1;
    }

    private void Update()
    {
        if (slider?.value <= 0)
        {
            _playerCon.TargetOff(slider.value);
            Instantiate(_corpse, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }

        if (currentHp >= maxHp)
            currentHp = maxHp;
    }

    //ColliderオブジェクトのIsTriggerにチェック入れること。
    private void OnTriggerEnter(Collider collision)
    {
        if (slider && !mutekimode)
        {
            Vector3 hitPos = collision.bounds.ClosestPoint(this.transform.position);
            if (collision.tag == "EnemyHit")
            {
                Instantiate(_gard, hitPos, Quaternion.identity);
            }
        }
    }
    public void Damage()
    {
        if (slider && !mutekimode)
        {
            float damage = Random.Range(15, 21);
            currentHp = currentHp - damage;
            float value = (float)currentHp / (float)maxHp;
            DOTween.To(() => slider.value, x => slider.value = x, value, 0.5f);
        }
    }
}
