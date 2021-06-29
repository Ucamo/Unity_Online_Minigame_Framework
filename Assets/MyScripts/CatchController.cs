using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

namespace Photon.Pun
{

public class CatchController : MonoBehaviour
{
    public Animator chanseyAnimator;

    public int playerNumber=1;
    public string playerName;
    ScoreController objScore;
    public GameObject MultiEgg;
    PhotonView pView;
    void Start(){
        //Get player number
        //Get score controller reference
        GameObject obj = GameObject.Find("Scores");
        if(obj!=null){
            objScore = obj.GetComponent<ScoreController>();
        }
        pView =  gameObject.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<PhotonView>();

    }

    void IncreaseScore(){
        if(pView.IsMine){
        if(objScore!=null){
            objScore.IncreaseScore(playerNumber,1);
        }
        }
    }

    void DecreaseScore(){
        if(pView.IsMine){
        if(objScore!=null){
            objScore.DecreaseScore(playerNumber,5);
        }
        }
    }


    private void OnCollisionEnter(Collision other)
    {
        //Check to see if the tag on the collider is equal to a Tag
        if (other.gameObject.tag == "Egg")
        {
            if(other.gameObject.name.Contains("Left")){
                chanseyAnimator.SetBool("IsLeftCatch", true);
                StartCoroutine(ResetValues(0.2f));
                IncreaseScore();
            }

            if(other.gameObject.name.Contains("Right")){
                chanseyAnimator.SetBool("IsRightCatch", true);
                StartCoroutine(ResetValues(0.2f));
                 IncreaseScore();
            }

            if(other.gameObject.name.Contains("Middle")){
                chanseyAnimator.SetBool("IsIdleCatch", true);
                StartCoroutine(ResetValues(0.2f));
                 IncreaseScore();
            }

            other.gameObject.SetActive(false);
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        if(other.gameObject.tag == "Voltorb"){
            if(other.gameObject.name.Contains("Voltorb")){
                chanseyAnimator.SetBool("IsHurt", true);
                 StartCoroutine(ResetValues(0.8f));
                 DecreaseScore();
                 GameObject newGameObject = Instantiate (MultiEgg);
		         newGameObject.transform.position = gameObject.transform.position;
                 Destroy(newGameObject,4f);
            }
            other.gameObject.SetActive(false);
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }

    IEnumerator ResetValues(float time)
    {
        yield return new WaitForSeconds(time);
        chanseyAnimator.SetBool("IsHurt", false);
        chanseyAnimator.SetBool("IsLeftCatch", false);
        chanseyAnimator.SetBool("IsRightCatch", false);
        chanseyAnimator.SetBool("IsIdleCatch", false);        
    }

}
}
