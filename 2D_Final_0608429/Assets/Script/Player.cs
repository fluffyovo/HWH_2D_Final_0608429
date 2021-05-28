using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    #region 欄位
    [Header("移動速度"), Range(1, 100)]
    public float speed = 10.5f;
    [Header("血量"), Range(0, 1000)]
    public float hp = 200f;
    [Header("攻擊"), Range(1, 100)]
    public float attack = 20f;
    [Header("虛擬搖桿")]
    public FixedJoystick joystick;
    [Header("變形元件")]
    public Transform tra;
    [Header("動畫元件")]
    public Animator ani;
    [Header("偵測攻擊範圍")]
    public float rangeAttack = 1.2f;
    [Header("音效來源")]
    public AudioSource aud;
    [Header("攻擊音效")]
    public AudioClip soundAttack;
    [Header("血條系統")]
    public HpManager hpManager;
    //[Header("死亡動畫")]
    //public Animation aniDie;

    public float attackWeapon;
    private float hpMax;
    private bool isDead = false;
    #endregion


    #region 事件
    private void Start()
    {

        hpMax = hp;

    }

    private void Update()
    {
        Move();
    }

    private void OnDrawGizmos()
    {
        //指定圖示顏色 (紅，綠，藍，透明）
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        //繪製圖示 球體(中心點，半徑)
        Gizmos.DrawSphere(transform.position, rangeAttack);
    }
    #endregion


    #region 方法
    private void Move()
    {
        if (isDead) return; 

        float h = joystick.Horizontal;
        float v = joystick.Vertical;

        tra.Translate(h * speed * Time.deltaTime, v * speed * Time.deltaTime, 0);
        if (Mathf.Abs(h) > 0.1f)
        {
            ani.SetFloat("水平", h);
        }
    }

    public void Attack()
    {
        if (isDead) return;

        aud.PlayOneShot(soundAttack, 0.6f);
        //ani.Play("狗_攻擊");
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, rangeAttack, -transform.up, 0, 1 << 8);

        if (hit && hit.collider.tag == "敵人") hit.collider.GetComponent<Enemy>().Hit(attack + attackWeapon);

    }

    public void Hit(float Pdamage)
    {
        if (isDead) return;

        hp -= Pdamage;                                    // 扣除傷害值
        hpManager.UpdateHpBar(hp, hpMax);                 // 更新血條
        StartCoroutine(hpManager.ShowDamage(Pdamage));    // 啟動協同程序(顯示傷害值())

        if (hp <= 0) Dead();                              // 如果血量 <= 0 就死亡
    }

    private void Dead()
    {
        hp = 0;
        isDead = true;
        ani.Play("狗_死亡");
        Invoke("Replay", 2);          // 延遲呼叫("方法名稱"，延遲時間)
    }

    private void Replay()
    {
        SceneManager.LoadScene("遊戲場景");
    }
    #endregion

}
