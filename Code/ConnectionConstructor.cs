using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Nakama;

using UnityEngine;


using Random = UnityEngine.Random;


public static class ConnectionConstructor
{
    public static async Task<Connection> GetNewConnection(string connectIP = null)
    {
        //New Client/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        IClient newClient = null; //Create a new client and give it the nakama server credentials
                                  //A new client is a brand new, authenticated client that is stored on the nakama server

        string username = GetSpoofUsername();

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
            newSession = await newClient.AuthenticateDeviceAsync(DeviceID(), null, true);
        }
        catch (Exception e)
        {
            Debug.LogWarning("EXCEPTION: Client could not be authenticated. A session was not started.");
            Debug.LogWarning("EXCEPTION: " + e);
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //New Socket/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ISocket newSocket = null;

        try
        {
            //Attach the socket to the session (the current interface between the client and the server)
            newSocket = newClient.NewSocket();
            ConnectionSubscriber.AddToSocket(newSocket);
            var task = newSocket.ConnectAsync(newSession);
        }
        catch (Exception e)
        {
            Application.Quit();
        }

        //Create new connection
        Connection newConnection = new Connection(newClient, newSession, newSocket);

        newSocket.Connected += () => Debug.Log("Client: " + newSession.Username + " socket connected");
        newSocket.Closed += () => Debug.LogFormat("Client: " + newSession.Username + " socket closed");

        Brain.ins.Connection.ServerConnectionEvent.Invoke(newConnection);

        return newConnection;
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }

    public static string GetSpoofUsername()
    {
        // string[] stringBases = new string[] { "stegosaurus", "tyrannosaurus", "diplodocus", "anklyosaurus", "dimerotron", "tricerotops" };
        string[] stringRoots = new string[] { "mental", "silly" };
        string[] stringMods = new string[] { "red", "blue", };
        string[] stringBases = new string[] { "max", "aria", "dj", "lubo" };

        return stringRoots[Random.Range(0, stringRoots.Length)] + "_" + stringBases[Random.Range(0, stringBases.Length)] + "_" + stringMods[Random.Range(0, stringMods.Length)];
    }


    public static string DeviceID()
    {
        string deviceID = PlayerPrefs.GetString("nakama.deviceid"); //Get the unique device ID from player prefs that will be used to handcuff the device to the new account

        if (string.IsNullOrEmpty(deviceID))
        {
            deviceID = SystemInfo.deviceUniqueIdentifier;
            PlayerPrefs.SetString("nakama.deviceid", deviceID); // Cache device id.
        }
        return deviceID;
    }
}


