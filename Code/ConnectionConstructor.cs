using System;
using System.Text;
using System.Threading.Tasks;
using Nakama;
using Nakama.TinyJson;
using Newtonsoft.Json;
using UnityEngine;
using Gameshow.Host;

using Random = UnityEngine.Random;


public static class ConnectionConstructor
{
    public static async Task<Connection> GetNewConnection(string displayName, string connectIP)
    {
        //New Client/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        IClient newClient = null; //Create a new client and give it the nakama server credentials
                                  //A new client is a brand new, authenticated client that is stored on the nakama server

        try
        {
            newClient = new Client("http", connectIP, 7350, "defaultkey");
        }
        catch (Exception e)
        {
            Debug.LogWarning("EXCEPTION: A new client could not be created");
            Debug.LogWarning("EXCEPTION: " + e);
        }

        //New Session////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ISession newSession = null;

        try
        {
            newSession = await newClient.AuthenticateDeviceAsync(DeviceID(), SystemInfo.deviceName);
        }
        catch (Exception e)
        {
            Debug.LogWarning("EXCEPTION: Client could not be authenticated. A session was not started.");
            Debug.LogWarning("EXCEPTION: " + e);
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //New Socket/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ISocket newSocket = newClient.NewSocket();

        ConnectionSubscriber.AddToSocket(newSocket);
        newSocket.Connected += () => Debug.Log("Client: " + displayName + " socket connected");
        newSocket.Closed += () => Debug.LogFormat("Client: " + displayName + " socket closed");

        try
        {
            //Attach the socket to the session (the current interface between the client and the server)
            var task = newSocket.ConnectAsync(newSession);
        }
        catch (Exception e)
        {
            Application.Quit();
        }

        //Ammend the new account
        string newDisplayName = string.IsNullOrEmpty(displayName) ? GetSpoofDisplayName() : displayName; //A display name that is assigned to the client on authentication

        await newClient.UpdateAccountAsync(newSession, newSession.Username, newDisplayName/*, "https://hungarytoday.hu/wp-content/uploads/2020/05/84261763_657262308148775_2968667943157628928_o-e1589799341315.jpg", "en"*/);

        return new Connection(newClient, newSession, newSocket);
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }

    public static string GetSpoofDisplayName()
    {
        // string[] stringBases = new string[] { "stegosaurus", "tyrannosaurus", "diplodocus", "anklyosaurus", "dimerotron", "tricerotops" };
        string[] stringRoots = new string[] { "mental", "silly" };
        string[] stringMods = new string[] { "red", "blue", };
        string[] stringBases = new string[] { "max", "aria", "dj", "lubo" };

        return stringRoots[Random.Range(0, stringRoots.Length)] + "_" + stringBases[Random.Range(0, stringBases.Length)] + "_" + stringMods[Random.Range(0, stringMods.Length)];
    }


    public static string DeviceID()
    {

        //return "00020215147461";

        string deviceID = PlayerPrefs.GetString("nakama.deviceid"); //Get the unique device ID from player prefs that will be used to handcuff the device to the new account

        if (string.IsNullOrEmpty(deviceID))
        {
            deviceID = SystemInfo.deviceUniqueIdentifier;
            PlayerPrefs.SetString("nakama.deviceid", deviceID); // Cache device id.
        }
        return deviceID;
    }
}


