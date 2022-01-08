using System;
using System.Text;
using System.Threading.Tasks;
using Nakama;
using Nakama.TinyJson;
using Newtonsoft.Json;
using UnityEngine;

using Random = UnityEngine.Random;


public static class ConnectionConstructor
{

    public static async Task<Connection> GetNewConnection(string _username)
    {
        Debug.Log("Activating a new client");

        //Create a new client and give it the nakama server credentials
        IClient newClient = new Client("http", "192.168.8.111", 7350, "defaultkey");

        Debug.Log("COMPLETE:Activated a new client");

        string newID = SpoofID(); //Unique to the "session user" - so a device basically

        //Create a session between the created client and the server
        var deviceId = PlayerPrefs.GetString("nakama.deviceid");

        if (string.IsNullOrEmpty(deviceId))
        {
            deviceId = SystemInfo.deviceUniqueIdentifier;
            PlayerPrefs.SetString("nakama.deviceid", deviceId); // cache device id.
        }

        ISession newSession = null;

        try
        {
            newSession = await newClient.AuthenticateDeviceAsync(deviceId, newID);
        }
        catch (Exception e)
        {
            Debug.Log("EXCEPTION: " + e);
        }


        Debug.Log("COMPLETE: A new session was started");

        Debug.Log("Client: " + _username + " was authenticated.");

        //Open up a socket on the new client
        ISocket newSocket = newClient.NewSocket();

        Debug.Log("SOCKET: New socket variable created.");
        Debug.Log("SOCKET: Socket info: " + newSocket.ToJson());

        // Add a function that closes the new socket on appQuit
        Brain.ins.eventManager.e_appQuit.AddListener
        (() =>
        {
            newSocket.CloseAsync();
        }
        );

        ConnectionSubscriber.AddToSocket(newSocket);

        newSocket.Connected += () => Debug.Log("Client: " + _username + " socket connected");
        newSocket.Closed += () => Debug.LogFormat("Client: " + _username + " socket closed");

        try
        {
            //Attach the socket to the session (the current interface between the client and the server)
            Debug.Log("SOCKET: Attempting socket connection.");
            //Make sure we don't return until the socket is connected

            var task = newSocket.ConnectAsync(newSession);

        }
        catch (Exception e)
        {
            Debug.Log("SOCKET_EXCEPTION: " + e);
        }


        Debug.Log("SOCKET: Socket connection completed.");

        return new Connection(newClient, newSession, newSocket);

    }

    public static string SpoofID()
    {
        // string[] stringBases = new string[] { "stegosaurus", "tyrannosaurus", "diplodocus", "anklyosaurus", "dimerotron", "tricerotops" };
        string[] stringRoots = new string[] { "mental", "silly" };
        string[] stringMods = new string[] { "red", "blue", };
        string[] stringBases = new string[] { "max", "aria", "dj", "lubo" };

        return stringRoots[Random.Range(0, stringRoots.Length)] + "_" + stringBases[Random.Range(0, stringBases.Length)] + "_" + stringMods[Random.Range(0, stringMods.Length)];
    }


}


