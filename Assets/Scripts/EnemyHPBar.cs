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

    void Start()
    {
        slider.value = 1;

        if (slider)
            Debug.Log("Start currentHp : " + currentHp);

    }

    private void Update()
    {
        if (slider?.value <= 0)
            Destroy(this.gameObject);

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
                Debug.Log("damage : " + damage);

                //現在のHPからダメージを引く
                currentHp = currentHp - damage;
                Debug.Log("After currentHp : " + currentHp);

                //最大HPにおける現在のHPをSliderに反映。
                //int同士の割り算は小数点以下は0になるので、
                //(float)をつけてfloatの変数として振舞わせる。
                slider.value = (float)currentHp / (float)maxHp;
                Debug.Log("slider.value : " + slider.value);
            }
        }
    }
    public void Damage()
    {
        int damage = Random.Range(15, 21);
        currentHp = currentHp - damage;
        slider.value = (float)currentHp / (float)maxHp;
        Debug.Log("aaa");
    }
}