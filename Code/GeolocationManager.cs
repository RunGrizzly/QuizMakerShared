using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;

public class GeolocationManager : MonoBehaviour
{
    //Get a Google API Key from https://developers.google.com/maps/documentation/geocoding/get-api-key
    public string GoogleAPIKey;
    public string latitude;
    public string longitude;
    public string countryLocation;

    void Start()
    {
        // longitude = Input.location.lastData.longitude.ToString();
        // latitude = Input.location.lastData.latitude.ToString();

        longitude = "-54.19156";
        latitude = "1.00080";

        StartCoroutine(GetLocation(latitude, longitude));
    }

    IEnumerator GetLocation(string latitude, string longitude)
    {

        Debug.Log("Getting location");

        UnityWebRequest www = null;

        //Sends the coordinates to Google Maps API to request information
        try
        {
            www = new UnityWebRequest("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + latitude + "," + longitude + "&key=" + GoogleAPIKey);
        }
        catch (Exception e)
        {
            Debug.Log("WWW faild " + e);
            yield break;
        }

        float requestTime = 0;

        while (!www.isDone)
        {
            requestTime += Time.deltaTime;
            if (requestTime > 5f)
            {
                Debug.LogWarning("Web request timed out");
                yield break;
            }
            yield return null;
        }

        Debug.Log("Successfully sent a web request");

        //Deserialize the JSON file
        var location = JsonUtility.FromJson<Dictionary<string, object>>(www.ToString());
        var locationList = location["results"] as List<object>;
        var locationList2 = locationList[0] as Dictionary<string, object>;


        //Extract the substring information at the end of the locationList2 string
        countryLocation = locationList2["formatted_address"].ToString().Substring(locationList2["formatted_address"].ToString().LastIndexOf(",") + 2);

    }

    // IEnumerator Start()
    // {
    //     // First, check if user has location service enabled
    //     if (!Input.location.isEnabledByUser)
    //     {
    //         Debug.LogWarning("User does not have location services enabled");
    //         yield break;
    //     }

    //     // Start service before querying location
    //     Input.location.Start();

    //     // Wait until service initializes
    //     int maxWait = 20;

    //     while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
    //     {
    //         yield return new WaitForSeconds(1);
    //         maxWait--;
    //     }

    //     // Service didn't initialize in 20 seconds
    //     if (maxWait < 1)
    //     {
    //         yield break;
    //     }

    //     // Connection has failed
    //     if (Input.location.status == LocationServiceStatus.Failed)
    //     {
    //         Debug.Log("Unable to determine device location");
    //         yield break;
    //     }
    //     else
    //     {
    //         // Access granted and location value could be retrieve
    //         longitude = Input.location.lastData.longitude.ToString();
    //         latitude = Input.location.lastData.latitude.ToString();
    //     }

    //     //Stop retrieving location
    //     Input.location.Stop();

    //     //Sends the coordinates to Google Maps API to request information
    //     UnityWebRequest www = new UnityWebRequest("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + latitude + "," + longitude + "&key=" + GoogleAPIKey);

    //     yield return www;

    //     //if request was successfully
    //     if (www.error == null)
    //     {
    //         //Deserialize the JSON file
    //         var location = JsonUtility.FromJson<Dictionary<string, object>>(www.ToString());
    //         var locationList = location["results"] as List<object>;
    //         var locationList2 = locationList[0] as Dictionary<string, object>;

    //         //Extract the substring information at the end of the locationList2 string
    //         countryLocation = locationList2["formatted_address"].ToString().Substring(locationList2["formatted_address"].ToString().LastIndexOf(",") + 2);
    //         //This will return the country information
    //         Debug.LogAssertion(countryLocation);
    //     }

    // }
}