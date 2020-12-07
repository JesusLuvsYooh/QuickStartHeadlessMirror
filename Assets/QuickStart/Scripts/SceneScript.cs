using Mirror;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// www.StephenAllenGames.co.uk  JesusLuvsYooh

namespace QuickStart
{
    public class SceneScript : NetworkBehaviour
    {
        public SceneReference sceneReference;
        public Text canvasStatusText;
        public Text canvasAutoTraffic;
        public Text canvasPlayerCount;
        public GameObject[] playersArray;
        public PlayerScript playerScript;

        [SyncVar(hook = nameof(OnStatusTextChanged))]
        public string statusText;
        void OnStatusTextChanged(string _Old, string _New)
        {
            //called from sync var hook, to update info on screen for all players
            canvasStatusText.text = statusText;
        }

        public void ButtonSendMessage()
        {
            if (playerScript) { playerScript.CmdSendPlayerMessage(); }
        }

        public void ButtonChangeScene()
        {
            if (isServer)
            {
                Scene scene = SceneManager.GetActiveScene();
                if (scene.name == "MyScene") { NetworkManager.singleton.ServerChangeScene("MyOtherScene"); }
                else { NetworkManager.singleton.ServerChangeScene("MyScene"); }
            }
            else
            {
                Debug.Log("You are not Host.");
            }
        }

        public Text canvasAmmoText;

        public void UIAmmo(int _value)
        {
            canvasAmmoText.text = "Ammo: " + _value;
        }

        
        public void ButtonSetupAutoTraffic()
        {
            if (playerScript)
            {
                staticC.traffic += 1; if (staticC.traffic > 4) { staticC.traffic = 0; }
                playerScript.SetupAutoTraffic();
                canvasAutoTraffic.text = "Traffic: " + playerScript.trafficType;
            }
        }
        
        public void ButtonFindPlayers()
        {
            playersArray = GameObject.FindGameObjectsWithTag("Player");
            canvasPlayerCount.text = "Players: " + playersArray.Length;
        }
    }
}
