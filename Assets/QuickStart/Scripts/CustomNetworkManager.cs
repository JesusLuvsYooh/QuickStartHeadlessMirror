using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System;
using System.Collections;
using kcp2k;

/*
    Documentation: https://mirror-networking.com/docs/Components/NetworkManager.html
    API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkManager.html
*/

public class CustomNetworkManager : NetworkManager
{
    /// <summary>
    /// Runs on both Server and Client
    /// Networking is NOT initialized when this fires
    /// </summary>
    public override void Start()
    {
        base.Start();
        
#if UNITY_SERVER
        StartCoroutine("StartHeadless");
#endif
    }
    
    IEnumerator StartHeadless()
    {
        Debug.Log(" - Type 0 for default - ");
        Debug.Log("Traffic 0=none   1=light (card game)  2=active (social game)  3=heavy (mmo)   4=fruequent (fps)");
        Debug.Log("Server: - file - s - frameRate - 0 - 0 - 0");
        Debug.Log("Client: - file - c - frameRate - hostIP - hostPort - traffic");
       
        
        string[] args = Environment.GetCommandLineArgs();
        
        if (args == null || (args != null && args.Length <= 1))
        {
            Debug.Log("Missing Argsss! - Starting a default server setup.");
            StartServer();
        }
        else
        {
            if (args.Length >= 3 && args[2] != "0")
            {
                serverTickRate = int.Parse(args[2]);
                Application.targetFrameRate = int.Parse(args[2]);
            }

            if (args.Length >= 4 && args[3] != "0")
            {
                networkAddress = args[3];
            }
        
            if (args.Length >= 5 && args[4] != "0")
            {
                //GetComponent<TelepathyTransport>().port = ushort.Parse(args[3]);
                //GetComponent<KcpTransport>().Port = ushort.Parse(args[4]);
                //((KcpTransport)Transport.activeTransport).Port = 1234;
            }

            if (args.Length >= 6 && args[5] != "0")
            {
                staticC.traffic = int.Parse(args[5]);
            }

            yield return new WaitForSeconds(1.0f);

            if (args[1] == "s")
            {
                StartServer();
            }
            else if (args[1] == "c")
            {
                if (args.Length >= 3 && args[2] == "0") { Application.targetFrameRate = 30; }
                yield return new WaitForSeconds(UnityEngine.Random.Range(0.0f, 2.0f));
                StartClient();
            }
        }
    }
}
