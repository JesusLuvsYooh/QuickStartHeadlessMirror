using UnityEngine;
using Mirror;
using System;
using System.Net;
using System.Net.Sockets;
using System.Linq;

// www.StephenAllenGames.co.uk  JesusLuvsYooh

namespace QuickStart
{

    public class PlayerScript : NetworkBehaviour
    {
        private SceneScript sceneScript;
        private GameObject sceneScriptObj;
        
        public TextMesh playerNameText;
        public GameObject floatingInfo;
        private Material playerMaterialClone;
        private Weapon activeWeapon;
        private float weaponCooldownTime;
        
        private float autoTurnAmount = 0.0f;
        private float autoMoveAmount = 0.0f;
        public string trafficType = "none";

        [SyncVar(hook = nameof(OnNameChanged))]
        public string playerName;
        void OnNameChanged(string _Old, string _New)
        {
            playerNameText.text = playerName;
        }

        [SyncVar(hook = nameof(OnColorChanged))]
        public Color playerColor = Color.white;
        void OnColorChanged(Color _Old, Color _New)
        {
            //Debug.Log(gameObject.name + " HOOK OnColorChanged");
            playerNameText.color = _New;
            playerMaterialClone = new Material(this.GetComponent<Renderer>().material);
            playerMaterialClone.color = _New;
            this.GetComponent<Renderer>().material = playerMaterialClone;
        }

        private int selectedWeaponLocal = 1;
        public GameObject[] weaponArray;
        
        [SyncVar(hook = nameof(OnWeaponChanged))]
        public int activeWeaponSynced = 1;
        void OnWeaponChanged(int _Old, int _New)
        {
            // disable old weapon
            // in range and not null
            if (0 < _Old && _Old < weaponArray.Length && weaponArray[_Old] != null)
            {
                weaponArray[_Old].SetActive(false);
            }

            // enable new weapon
            // in range and not null
            if (0 < _New && _New < weaponArray.Length && weaponArray[_New] != null)
            {
                weaponArray[_New].SetActive(true);
                activeWeapon = weaponArray[activeWeaponSynced].GetComponent<Weapon>();
                if (isLocalPlayer) { sceneScript.UIAmmo(activeWeapon.weaponAmmo); }
            }
        }
        
        
        public override void OnStartLocalPlayer()
        {
            sceneScript.playerScript = this;

            Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = new Vector3(0, 0, 0);

            floatingInfo.transform.localPosition = new Vector3(0, -0.3f, 0.6f);
            floatingInfo.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            CmdSetupPlayer("Player" + UnityEngine.Random.Range(100, 999), new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f)));

            SetupAutoTraffic();
        }
        
        
        
        
        void Awake()
        {
            //allow all players to run this
            sceneScript = GameObject.Find("SceneReference").GetComponent<SceneReference>().sceneScript;
            if (selectedWeaponLocal < weaponArray.Length && weaponArray[selectedWeaponLocal] != null)
            { activeWeapon = weaponArray[selectedWeaponLocal].GetComponent<Weapon>(); sceneScript.UIAmmo(activeWeapon.weaponAmmo); }
        }


        [Command]
        public void CmdSendPlayerMessage()
        {
            if (sceneScript) { sceneScript.statusText = playerName + " says hello " + UnityEngine.Random.Range(10, 99); }
        }
        

        [Command]
        public void CmdSetupPlayer(string _name, Color _col)
        {
            //player info sent to server, then server updates sync vars which handles it on all clients
            playerName = _name;
            playerColor = _col;
            sceneScript.statusText = playerName + " joined.";
        }
        

        void Update()
        {
            //FindSceneScript();
            //allow all players to run this
            if (isLocalPlayer == false)
            {
                floatingInfo.transform.LookAt(Camera.main.transform);
            }

            //only our own player runs below here
            if (!isLocalPlayer) { return; }

            float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * 110.0f;
            float moveZ = Input.GetAxis("Vertical") * Time.deltaTime * 4f;

            transform.Rotate(0, moveX, 0);
            transform.Translate(0, 0, moveZ);

            if (Input.GetButtonDown("Fire2")) //Fire2 is mouse 2nd click and left alt
            {
                activeWeapon = null;
                selectedWeaponLocal += 1;
                if (selectedWeaponLocal > weaponArray.Length) { selectedWeaponLocal = 1; }
                CmdChangeActiveWeapon(selectedWeaponLocal);
               // GetLocalIPAddress(); GetIPs();
            }

            if (Input.GetButtonDown("Fire1")) //Fire1 is mouse 1st click
            {
                if (activeWeapon && Time.time > weaponCooldownTime && activeWeapon.weaponAmmo > 0)
                {
                    weaponCooldownTime = Time.time + activeWeapon.weaponCooldown;
                    activeWeapon.weaponAmmo -= 1;
                    sceneScript.UIAmmo(activeWeapon.weaponAmmo);
                    CmdShootRay();
                }
            }

            if (staticC.traffic > 1)
            {
                if (autoTurnAmount > 0) { transform.Rotate(0, autoTurnAmount, 0); }
                if (autoMoveAmount > 0) { transform.Translate(0, 0, autoMoveAmount); }
            }
        }


        [Command]
        void CmdShootRay()
        {
            RpcFireWeapon();
        }


        [ClientRpc]
        void RpcFireWeapon()
        {
            //bulletAudio.Play(); muzzleflash  etc
            var bullet = (GameObject)Instantiate(activeWeapon.weaponBullet, activeWeapon.weaponFirePosition.position, activeWeapon.weaponFirePosition.rotation);
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * activeWeapon.weaponSpeed;
            if (bullet) { Destroy(bullet, activeWeapon.weaponLife); }
        }
        
        
        [Command]
        public void CmdChangeActiveWeapon(int _activeWeaponSynced)
        {
            activeWeaponSynced = _activeWeaponSynced;
        }
        
        
        void AutoRepeatingShoot()
        {
            if (activeWeapon && Time.time > weaponCooldownTime && activeWeapon.weaponAmmo > 0)
            {
                weaponCooldownTime = Time.time + activeWeapon.weaponCooldown;
                //activeWeapon.weaponAmmo -= 1;
                sceneScript.UIAmmo(activeWeapon.weaponAmmo);
                CmdShootRay();
            }
        }
        
        
        void AutoRepeatingMessage()
        {
            CmdSendPlayerMessage();
        }
        
        
        public void SetupAutoTraffic()
        {

            //Debug.Log("Traffic 0=none   1=light (card game)  2=active (social game)  3=heavy (mmo)   4=fruequent (fps)");
            CancelInvoke("AutoRepeatingMessage");
            CancelInvoke("AutoRepeatingShoot");
            
            if (staticC.traffic == 0)
            {
                trafficType = "None";
            }
            else if (staticC.traffic == 1)
            {
                trafficType = "Cards";
                InvokeRepeating(nameof(AutoRepeatingMessage), 1, 3f);
            }
            else if (staticC.traffic == 2)
            {
                trafficType = "Social";
                autoTurnAmount = UnityEngine.Random.Range(0.1f, 1.5f);
                InvokeRepeating(nameof(AutoRepeatingMessage), 1, 2f);
                InvokeRepeating(nameof(AutoRepeatingShoot), 1, 6);
            }
            else if (staticC.traffic == 3)
            {
                trafficType = "MMO";
                autoTurnAmount = UnityEngine.Random.Range(0.1f, 1.5f);
                autoMoveAmount = UnityEngine.Random.Range(0.05f, 0.1f);
                InvokeRepeating(nameof(AutoRepeatingMessage), 1, 1.0f);
                InvokeRepeating(nameof(AutoRepeatingShoot), 1, 3f);
            }
            else if (staticC.traffic == 4)
            {
                trafficType = "FPS";
                autoTurnAmount = UnityEngine.Random.Range(0.1f, 1.5f);
                autoMoveAmount = UnityEngine.Random.Range(0.1f, 0.2f);
                InvokeRepeating(nameof(AutoRepeatingMessage), 1, 0.75f);
                InvokeRepeating(nameof(AutoRepeatingShoot), 1, 0.6f);
            }
        }
    }
}
