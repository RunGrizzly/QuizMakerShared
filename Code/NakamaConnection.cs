using System;
using System.Linq;
using Nakama;
using UnityEngine;
using UnityEngine.Events;

public class ServerConnectionEvent : UnityEvent<Connection> { };
public class ServerDisconnectionEvent : UnityEvent { };

public enum ConnectionType { Host, Client };


public class NakamaConnection : MonoBehaviour
{

    [SerializeField] private bool m_debugServerStatus = false;
    [SerializeField] private ConnectionType m_connectAs;

    [field: SerializeField] public string ConnectIP;

    public Connection Connection { get; private set; } = null; //The instance of this connection containing client, session and socket data
    public ServerConnectionEvent ServerConnectionEvent { get; private set; } = null; //Transparency to the client whenever a new connection is made
    public ServerDisconnectionEvent ServerDisconnectionEvent { get; private set; } = null; //Transparency to the client whenever a connection is dropped

    private void OnEnable()
    {
        ServerConnectionEvent = new ServerConnectionEvent();
        ServerDisconnectionEvent = new ServerDisconnectionEvent();
    }

    private void OnDisable()
    {
        ServerConnectionEvent.RemoveAllListeners();
        ServerDisconnectionEvent.RemoveAllListeners();
    }

    //Immediately get a connection to the nakama server on start
    private void Start()
    {
        GetNewConnection();
    }

    private void Update()
    {

        if (!m_debugServerStatus) return;

        if (Connection != null)
        {
            ////////////////////////////
            if (Connection.client != null)
            {
                Debug.Log("CLIENT: " + Connection.client);
            }
            else
            {
                Debug.Log("CLIENT: the nakama connection has not established a client");
            }
            ////////////////////////////
            if (Connection.session != null)
            {
                Debug.Log("SESSION: " + Connection.session);
            }
            else
            {
                Debug.Log("SESSION: the nakama connection does not have an active session");
            }
            ////////////////////////////
            if (Connection.socket != null)
            {
                Debug.Log("SOCKET: " + Connection.socket);
                Debug.Log("SOCKET: The nakama connection socket has connected state: " + Connection.socket.IsConnected);
            }
            else
            {
                Debug.Log("SESSION: the nakama connection does not have an active socket");
            }
            ////////////////////////////
        }
        else Debug.Log("The nakama connection is null");
    }


    //Get a new connection and autheticate it with the server
    public async void GetNewConnection()
    {
        Connection = null;

        Debug.Log("CLIENT: Getting a new connection for the client.");
        Connection = await ConnectionConstructor.GetNewConnection(ConnectIP, m_connectAs);
        Debug.Log("CLIENT: New connection was established.");
        Debug.Log("CONNECTION - info: " + Connection.socket.IsConnected);
    }

    public async void KillConnection()
    {
        if (Connection == null)
        {
            return;
        }

        await Connection.socket.CloseAsync();
        Connection = null;
    }

    public async void TryJoinRoom(string roomQuery)
    {
        Debug.Log("CLIENT: Attempting to join match");

        //If we have not yet established a connection with GetNewConnection
        if (Connection == null)
        {
            Debug.Log("Not authenticated on the server. Please authenticate the device.");
            return;
        }

        Debug.Log("Searching for match with room code");
        Debug.Log("Room code = " + roomQuery);

        IApiMatchList info = null;

        try
        {
            //List matches available to the client using the room query to match roomcode label
            //Make non case sensitive with to upper
            info = await Connection.client.ListMatchesAsync(Connection.session, 0, 100, 1, true, roomQuery.ToUpper(), null);
        }
        catch (Exception e)
        {
            Debug.Log("MATCH EXCEPTION: " + e);
        }


        //If we get a hit (1 is returned)
        if (info.Matches.Count() == 1)
        {
            //Join the hit (the only returned match will be at element 0)
            var match = info.Matches.ElementAt(0);

            Debug.Log("Found match with room code " + roomQuery + " , with match ID " + match.MatchId);
            Debug.Log("Joining the matched room");
            await Connection.socket.JoinMatchAsync(match.MatchId);
        }

        //If we get more than one hit (duplicate room codes)
        //Exit out
        //TODO: Look at maintaining unique room codes so there can't be duplicates
        else if (info.Matches.Count() > 1)
        {
            Debug.Log("Conflict: Found multiple matches with room code " + roomQuery + " , please provide unique room code");
        }

        else
        {
            Debug.Log("No matches were found with " + roomQuery);
        }
    }


}
