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

        //TODO:Android devices cannot complete this session activation
        // //create a session between the created client and the server
        // ISession newSession = await newClient.AuthenticateDeviceAsync(newID, _username + (Random.Range(0, 999).ToString()));

        var deviceId = PlayerPrefs.GetString("nakama.deviceid");

        if (string.IsNullOrEmpty(deviceId))
        {
            deviceId = SystemInfo.deviceUniqueIdentifier;
            PlayerPrefs.SetString("nakama.deviceid", deviceId); // cache device id.
        }



        ISession newSession = null;

        try
        {
            newSession = await newClient.AuthenticateDeviceAsync(deviceId);
            // Debug.LogFormat("New user: {0}, {1}", session.Created, newSession);
            // newSession = await newClient.AuthenticateDeviceAsync(newID, _username + (Random.Range(0, 999).ToString()), false);
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
        Brain.ins.eventManager.appQuitEvent.AddListener
        (() =>
        {
            newSocket.CloseAsync();
        }
        );

        newSocket.ReceivedMatchState += (matchState) =>
        {
            Debug.Log("Client: " + _username + " received a message");

            if (matchState.OpCode == OpCodes.generic)
            {
                string messageJson = System.Text.Encoding.UTF8.GetString(matchState.State);
                Debug.Log(messageJson);
            }

            else if (matchState.OpCode == OpCodes.boolean)
            {
                // Debug.Log(matchState);
                // Debug.Log(matchState.State);

                string boolJson = System.Text.Encoding.UTF8.GetString(matchState.State);
                bool randBool = JsonParser.FromJson<bool>(boolJson);
                Debug.Log(randBool);
            }

            else if (matchState.OpCode == OpCodes.complex)
            {
                // Debug.Log(matchState);
                // Debug.Log(matchState.State);

                string complexJson = System.Text.Encoding.UTF7.GetString(matchState.State);

                Debug.Log("A complex string was created from match state broadcast");
                Debug.Log("The string looks like this: " + complexJson);




                ComplexDataObject root = JsonConvert.DeserializeObject<ComplexDataObject>(complexJson);

                string message = root.genericMessage.message.ToString();
                bool randBool = root.randomBool.randomBool;
                int i = root.i;
                float f = root.f;


                Debug.Log("A generic message was bundled with the complex data: " + message + ", " + randBool + ", " + i + ", " + f);

                // GenericMessage complexString = JsonParser.FromJson<GenericMessage>(complexJson);
                // RandomBool complexBool = JsonParser.FromJson<RandomBool>(complexJson);
                // Vector3 complexV3 = JsonParser.FromJson<Vector3>(complexJson);
                // GameObject complexGO = JsonParser.FromJson<GameObject>(complexJson);

                // Debug.Log("Attempting to parse a complex data object...");
                // Debug.Log("Complex string = " + complexString);
                // Debug.Log("Complex bool = " + complexBool);
                // // Debug.Log("Complex vector3 = " + complexV3);
                // // Debug.Log("Complex gameobject = " + complexGO);
            }


        };




        newSocket.Connected += () => Debug.Log("Client: " + _username + " socket connected");
        newSocket.Closed += () => Debug.LogFormat("Client: " + _username + " socket closed");



        try
        {
            //Attach the socket to the session (the current interface between the client and the server)
            Debug.Log("SOCKET: Attempting socket connection.");

            await newSocket.ConnectAsync(newSession);
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


