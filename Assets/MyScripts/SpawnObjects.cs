using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
using TMPro;

namespace Photon.Pun
{
public class SpawnObjects :  MonoBehaviour, IPunObservable {

	public GameObject[] objetoASpawnear;
	public int poolSize;
	public float frecuenciaDeSpawneo;
	private Vector3 _startPosition;
	public int countSpawner;
	public int TopSpawner;
	bool hasTopSpawner;
	int index;
	float x;
	float y;
	float z;

 	public GameObject objInfo;
	TextMeshProUGUI txtInfo;
	bool gameStarted;
	public int WaitSecondsBeforeStart;

	List<GameObject> objectPool;
	void Start () {
		txtInfo = objInfo.GetComponent<TextMeshProUGUI>();
		_startPosition = transform.position;
		objectPool = new List<GameObject>();
		//Create a pool and fill it with inactive GameObjects.
		foreach(GameObject objectToSpawn in objetoASpawnear){
			for (int i = 0; i < poolSize; i++) {
            	GameObject newGameObject = Instantiate(objectToSpawn);
            	newGameObject.SetActive(false);
            	objectPool.Add(newGameObject);
        	}
		}

		if(TopSpawner>0){
			hasTopSpawner=true;
		}
		StartCoroutine(StartCountDown());
	}
	IEnumerator StartCountDown()
    {
        while(WaitSecondsBeforeStart>0){
			txtInfo.text=WaitSecondsBeforeStart.ToString();
			int playerNumber= PhotonNetwork.LocalPlayer.ActorNumber;
			if(playerNumber==1){
				WaitSecondsBeforeStart--;
			}   
			yield return new WaitForSeconds(1);
			
		}
		txtInfo.text = "GO!";
		yield return new WaitForSeconds(1);
		txtInfo.text="";
		StartGame();
    }

	void StartGame(){
		InvokeRepeating ("SpawnObject", frecuenciaDeSpawneo, frecuenciaDeSpawneo);
		//SpawnAllObjects();
		gameStarted=true;
	}

	GameObject GetObjectFromPool(string objToSpawn,string newName=""){
        //Get object from the pool
        foreach(GameObject objInPool in objectPool){
            //check if the object is inactive
            if(!objInPool.activeInHierarchy){
				if(objInPool.name.Contains(objToSpawn)){
					objInPool.SetActive(true);
					objInPool.transform.position= new Vector3(0,0,0);
					objInPool.name=newName;
                	return objInPool;	
				}
				
            }
        }
        //No inactive objects found, instantiate a new one and add it to the pool.
        GameObject newGameObject = PhotonNetwork.Instantiate(objToSpawn,new Vector3(0, 0, 0),Quaternion.identity, 0);
        newGameObject.SetActive(false);
        objectPool.Add(newGameObject);
        return GetObjectFromPool(newGameObject.name,newGameObject.name);
    }

	void SpawnObject()
	{	
		if((hasTopSpawner && countSpawner<TopSpawner)){
		if(PhotonNetwork.IsMasterClient){
			index = Random.Range (0, objetoASpawnear.Length);
			x = gameObject.transform.position.x;
			y = gameObject.transform.position.y;
			z = gameObject.transform.position.z;
		}
		
		Vector3 position;
		float modifier=0;
		GameObject objFromPool= objetoASpawnear[index];
		if(objFromPool.name.Contains("Egg")){
			modifier=-2f;
			countSpawner++;
			objFromPool.name="";
			objFromPool.name="Egg";
		}else{
			objFromPool.name="";
			objFromPool.name="Voltorb";
		}

		position = new Vector3 (x, y, z+modifier);

		GameObject newGameObject = objFromPool;
		newGameObject.transform.position = position;		 



		StartCoroutine(ReplicateCoroutine(newGameObject));
		}
	}

	IEnumerator ReplicateCoroutine(GameObject newObject)
    {
		if(PhotonNetwork.IsMasterClient){
			yield return new WaitForSeconds(0.5f);
		}else{
			yield return new WaitForSeconds(0);
		}
		
		ReplicateObjectsInAllPositions(newObject);
    }

	void ReplicateObjectsInAllPositions(GameObject  newObject){	
		string originalName=newObject.name;
		if(_startPosition.x==newObject.transform.position.x){
			newObject.name +="_Middle";
		}
		if(_startPosition.x>newObject.transform.position.x){
			newObject.name +="_Right";
		}
		if(_startPosition.x<newObject.transform.position.x){
			newObject.name +="_Left";
		}

		float modififier=0;
		if(newObject.name.Contains("_Left")){
			modififier=2;
		}
		if(newObject.name.Contains("_Right")){
			modififier=-2;
		}
		float posY = newObject.transform.position.y;
		float posZ = newObject.transform.position.z;
		if(newObject.name.Contains("Egg")){
			posZ= 15.95f;
		}	

		for(int x=1;x<5;x++){
			//Spawn for each position
			GameObject newGameObject = GetObjectFromPool(originalName, newObject.name);
			//Spawn positions base
			Vector3 basePosition = GameObject.Find("Position_P"+x).transform.position;
			newGameObject.transform.position = new Vector3(basePosition.x+modififier,posY,posZ);
		}
	}

   void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
                // We own this player: send the others our data
                //This script is local, you write to stream
				stream.SendNext(WaitSecondsBeforeStart);
                stream.SendNext(index);
				stream.SendNext(x);
				stream.SendNext(y);
				stream.SendNext(z);
                
        }
        else
        {
                // Network player, receive data
                //This script is receiving data from remote players script
				this.WaitSecondsBeforeStart=(int)stream.ReceiveNext();
                this.index = (int)stream.ReceiveNext();
                this.x = (float)stream.ReceiveNext();
				this.y = (float)stream.ReceiveNext();
				this.z = (float)stream.ReceiveNext();
        }
    }
}
}