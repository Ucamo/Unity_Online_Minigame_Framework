using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface INetworkInstance 
{
    GameObject Instantiate(GameObject objToSpawn, Vector3 position);
}
