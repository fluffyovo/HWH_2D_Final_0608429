using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public void NextSc()
    {
        SceneManager.LoadScene("遊戲場景");
    }
}
