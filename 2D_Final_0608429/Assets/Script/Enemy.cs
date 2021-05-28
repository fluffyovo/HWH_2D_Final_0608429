using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region 欄位
    [Header("追蹤範圍"), Range(0, 50)]
    public float rangeTrack = 3;
    [Header("攻擊範圍"), Range(0, 50)]
    public float rangeAttack = 1;
    [Header("移動速度"), Range(0, 50)]
    public float speed = 2;
    [Header("攻擊特效")]
    public ParticleSystem psAttack;
    [Header("攻擊冷卻時間"), Range(0, 50)]
    public float cdAttack = 3;
    [Header("攻擊力"), Range(0, 100)]
    public float attack = 10;
    [Header("血量"), Range(0, 1000)]
    public float hp = 200f;
    [Header("血條系統")]
    public HpManager hpManager;
    [Header("動畫元件")]
    public Animator aniCat;
    [Header("音效來源")]
    public AudioSource aud;
    [Header("攻擊音效")]
    public AudioClip soundAttack;

    private float hpMax;
    private bool isDead = false;

    private Transform player;   //玩家位置
    private Player _player;     //玩家腳本        

    private float timer;
    #endregion


    #region 事件
    private void Start()
    {
        hpMax = hp;
        player = GameObject.Find("玩家").transform;
        _player = player.GetComponent<Player>();
    }

    private void Update()
    {
        Track();
    }

    private void OnDrawGizmos()
    {
        // 圖形的顏色
        Gizmos.color = new Color(0, 0, 1, 0.3f);
        // 繪製圓形範圍(中心點 ，半徑)
        Gizmos.DrawSphere(transform.position, rangeTrack);

        // 攻擊偵測範圍
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, rangeAttack);
    }
    #endregion


    #region 方法
    private void Track()
    {
        if (isDead) return;

        float dis = Vector3.Distance(transform.position, player.position);

        if (dis <= rangeAttack)
        {
            Attack();
        }
        else if (dis <= rangeTrack)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    private void Attack()
    {
        timer += Time.deltaTime;

        if (timer >= cdAttack)
        {
            timer = 0;

            // 2D 碰撞 = 2D 物理.覆蓋圓形範圍 (中心點，半徑，圖層)
            Collider2D hit = Physics2D.OverlapCircle(transform.position, rangeAttack, 1 << 9);
            aud.PlayOneShot(soundAttack, 0.6f);
            // 碰到的物件 取得元件<玩家>().受傷(攻擊力)
            hit.GetComponent<Player>().Hit(attack);
        }
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
        if (isDead) return;
        hp = 0;
        isDead = true;
        aniCat.Play("貓_死亡");
        Destroy(gameObject, 1.5f);

    }
    #endregion

}
