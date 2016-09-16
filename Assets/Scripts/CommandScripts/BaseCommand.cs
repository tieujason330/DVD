using UnityEngine;
using System.Collections;

public abstract class BaseCommand
{
    public CommandType _commandType;

    public BaseCommand()
    {
        
    }

    //// Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    public abstract void Execute();
}
