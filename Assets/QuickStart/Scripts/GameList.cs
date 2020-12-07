using UnityEngine;
using UnityEngine.SceneManagement;

namespace QuickStart
{
    public class GameList : MonoBehaviour
    {
        public void LoadScene()
        {
            SceneManager.LoadScene("Menu");
        }
    }
}