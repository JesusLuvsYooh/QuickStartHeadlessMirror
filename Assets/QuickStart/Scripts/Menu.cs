using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QuickStart
{
    public class Menu : MonoBehaviour
    {
        public void LoadScene()
        {
            SceneManager.LoadScene("GameList");
        }

        void Start()
        {
            if (NetworkManager.singleton != null) { Destroy(NetworkManager.singleton.gameObject); }
        }
    }
}