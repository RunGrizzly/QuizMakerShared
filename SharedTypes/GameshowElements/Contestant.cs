
using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Contestant
{

    public string name;
    public Device device;

    public float points;


    public Contestant(Device _device, string _name)
    {
        name = _name;
        device = _device;
        points = 0;
    }
}
