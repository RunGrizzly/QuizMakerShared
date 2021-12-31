using System;

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