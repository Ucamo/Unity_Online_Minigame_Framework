using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using TMPro;

namespace Photon.Pun
{
public class SetUpLocalReferences : MonoBehaviour
{
    ChanseyController chController;
    public GameObject textObjectP1;
    public GameObject textObjectP2;
    public GameObject textObjectP3;
    public GameObject textObjectP4;
    void Start(){
        StartCoroutine(WaitToSetup());
    }

    IEnumerator WaitToSetup()
    {
        //yield on a new YieldInstruction that waits for 1 second.
        yield return new WaitForSeconds(1);
        chController = gameObject.GetComponent<ChanseyController>();
        SetUpLocalAnimator();
        SetUpPlayerNumber();
    }

    void SetUpLocalAnimator(){
        GameObject[] players;
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if(player.GetComponent<PhotonView>().IsMine){
                Animator chanseyAnimator = player.transform.Find("Body").GetComponent<Animator>();
                chController.chanseyAnimator=chanseyAnimator;
            }
        } 
    }

    void SetUpPlayerNumber(){
        GameObject[] players;
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players){
            int OwnerActorNr = player.GetComponent<PhotonView>().OwnerActorNr;
            CatchController catchCon = player.transform.Find("Body/Root/Top/Hands/CatchCollider").GetComponent<CatchController>();
            catchCon.playerNumber=OwnerActorNr;
            catchCon.playerName=player.GetComponent<PhotonView>().Owner.NickName;
            if(OwnerActorNr==1){
                textObjectP1.GetComponent<TextMeshProUGUI>().text=player.GetComponent<PhotonView>().Owner.NickName;
                
            }
            if(OwnerActorNr==2){
                textObjectP2.GetComponent<TextMeshProUGUI>().text=player.GetComponent<PhotonView>().Owner.NickName;
            }
            if(OwnerActorNr==3){
                textObjectP3.GetComponent<TextMeshProUGUI>().text=player.GetComponent<PhotonView>().Owner.NickName;
            }
            if(OwnerActorNr==4){
                textObjectP4.GetComponent<TextMeshProUGUI>().text=player.GetComponent<PhotonView>().Owner.NickName;
            }
            
        }
    }
}
}
