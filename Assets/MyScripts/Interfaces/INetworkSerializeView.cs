using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface INetworkSerializeView 
{
    void SerializeView(object stream, object info);
}
