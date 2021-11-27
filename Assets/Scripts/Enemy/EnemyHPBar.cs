using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    [SerializeField] bool mutekimode = default;
    //最大HPと現在のHP。
    [SerializeField] float maxHp = 200;
    float currentHp = 200;
    //Sliderを入れる
    [SerializeField] Slider slider;
    /// <summary>入力された方向の XZ 平面でのベクトル</summary>
    [SerializeField] GameObject _corpse = default;

    void Start()
    {
        slider.value = 1;
    }

    private void Update()
    {
        if (slider?.value <= 0)
        {
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
            //Enemyタグのオブジェクトに触れると発動
            if (collision.gameObject.tag == "PMagicBall")
            {
                //ダメージはこの中でランダムに決める。
                int damage = Random.Range(15, 21);

                //現在のHPからダメージを引く
                currentHp = currentHp - damage;

                //最大HPにおける現在のHPをSliderに反映。
                //int同士の割り算は小数点以下は0になるので、
                //(float)をつけてfloatの変数として振舞わせる。
                slider.value = (float)currentHp / (float)maxHp;
            }
        }
    }
    public void Damage()
    {
        if (slider && !mutekimode)
        {
            int damage = Random.Range(15, 21);
            currentHp = currentHp - damage;
            slider.value = (float)currentHp / (float)maxHp;
        }
    }
}