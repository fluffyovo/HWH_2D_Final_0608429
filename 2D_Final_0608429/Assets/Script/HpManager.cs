using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HpManager : MonoBehaviour
{
    [Header("血條")]
    public Image bar;
    [Header("傷害數值")]
    public RectTransform rectDamage;

    public void UpdateHpBar(float hp, float hpMax)
    {
        // 血條.填滿數值 = 當前血量 / 血量最大值
        bar.fillAmount = hp / hpMax;
    }

    public IEnumerator ShowDamage(float damage)
    {
        RectTransform rect = Instantiate(rectDamage, transform);   // 生成傷害數值在血條系統內
        rect.anchoredPosition = new Vector2(0, 25);                // 指定座標
        rect.GetComponent<Text>().text = "-" + damage.ToString();        // 更新數值

        float y = rect.anchoredPosition.y;                         // 取得原始 Y 軸

        while (y < 55)                                             // 當 Y 小於 400 時持續執行
        {
            y += 5;                                                // 每次上升的單位 20
            rect.anchoredPosition = new Vector2(0, y);             // 更新座標
            yield return new WaitForSeconds(0.025f);               // 等待幾秒(秒數)
        }

        Destroy(rect.gameObject, 0.3f);                            // 0.3秒後刪除傷害數值物件
    }

}
