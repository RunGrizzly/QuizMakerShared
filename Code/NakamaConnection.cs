using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Nakama;
using UnityEngine;


public class GenericMessage
{
    public string message;

    public GenericMessage(string _message)
    {
        message = _message;
    }
}

public class RandomBool
{
    public bool randomBool;
}

[Serializable]
public class ComplexDataObject
{
    public GenericMessage genericMessage;
    public RandomBool randomBool;
    public int i;

    public float f;

    public ComplexDataObject(GenericMessage _genericMessage, RandomBool _randomBool, int _int, float _float)
    {
        genericMessage = _genericMessage;
        randomBool = _randomBool;
        i = _int;
        f = _float;
    }


}
public class NakamaConnection : MonoBehaviour
{

    Connection connection;

    //Get a new connection and autheticate it with the server
    public async void GetNewConnection()
    {
        Debug.Log("CLIENT: Getting a new connection for the client.");
        connection = await ConnectionConstructor.GetNewConnection("New_client_connection");
        Debug.Log("CLIENT: New connection was established.");
        Debug.Log("CONNECTION - info: " + connection.socket.IsConnected);
    }


    public async void TryJoinRoom(string roomQuery)
    {
        Debug.Log("CLIENT: Attempting to join match");

        if (connection == null)
        {
            Debug.Log("Not authenticated on the server. Please authenticate the device.");
            return;
        }

        Debug.Log("Searching for match with room code");
        Debug.Log("Room code = " + roomQuery);

        IApiMatchList info = null;

        try
        {
            info = await connection.client.ListMatchesAsync(connection.session, 0, 100, 1, true, roomQuery, null);
        }
        catch (Exception e)
        {
            Debug.Log("MATCH EXCEPTION: " + e);
        }



        if (info.Matches.Count() == 1)
        {

            var match = info.Matches.ElementAt(0);

            Debug.Log("Found match with room code " + roomQuery + " , with match ID " + match.MatchId);
            Debug.Log("Joining the matched room");
            await connection.socket.JoinMatchAsync(match.MatchId);



        }

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
