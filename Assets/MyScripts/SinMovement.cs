using UnityEngine;
using System.Collections;

public class SinMovement : MonoBehaviour {

	private Vector3 _startPosition;

	// Use this for initialization
	void Start () {
		_startPosition = transform.position;
	}

	void Update()
	{
		Move();
	}

	void Move()
	{
        int PosModificator=0;
        PosModificator=Random.Range(0,3);
        int mod=0;
        switch(PosModificator){
            case 0: mod=-2;break;
            case 1: mod=0;break;
            case 2: mod=2;break;
            default:mod=0;break;
        }
		transform.position = new Vector3 (_startPosition.x+mod, _startPosition.y, _startPosition.z);
	}
}