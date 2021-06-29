using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObject : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject SpawnObject;
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        //Check to see if the tag on the collider is equal to a Tag
        if (other.gameObject.tag == "Egg" || other.gameObject.tag == "Voltorb")
        {
            Vector3 otherPosition = new Vector3 (other.gameObject.transform.position.x, gameObject.transform.position.y, other.gameObject.transform.position.z);
            if(SpawnObject!=null){
                GameObject newGameObject = Instantiate (SpawnObject);
                newGameObject.transform.position = otherPosition;
            }
            other.gameObject.SetActive(false);
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }

    }
}
