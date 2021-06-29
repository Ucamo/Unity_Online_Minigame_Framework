using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

namespace Photon.Pun
{

public class ScoreController : MonoBehaviour
{
    public GameObject WinCanvas;
    public GameObject objText_P1;
    public GameObject objText_P2;
    public GameObject objText_P3;
    public GameObject objText_P4;
    TextMeshProUGUI txtScore_P1;
    TextMeshProUGUI txtScore_P2;
    TextMeshProUGUI txtScore_P3;
    TextMeshProUGUI txtScore_P4;

    public GameObject objText_Counter;
    TextMeshProUGUI txtCounter;
    SpawnObjects objSpawnObj;
    public int counterLeft;
    public int scoreP1;
    public int scoreP2;
    public int scoreP3;
    public int scoreP4;
    bool GameOver;
    int winnerNumber=0;
    string winnerName="";

    void Start(){
        txtScore_P1 = objText_P1.GetComponent<TextMeshProUGUI>();
        txtScore_P2 = objText_P2.GetComponent<TextMeshProUGUI>();
        txtScore_P3 = objText_P3.GetComponent<TextMeshProUGUI>();
        txtScore_P4 = objText_P4.GetComponent<TextMeshProUGUI>();
        txtCounter = objText_Counter.GetComponent<TextMeshProUGUI>();
        GameObject objSpawn = GameObject.Find("EggSpawner");
        if(objSpawn!=null){
            objSpawnObj= objSpawn.GetComponent<SpawnObjects>();
        }
    }

    void Update()
    {
        txtScore_P1.text=scoreP1.ToString();
        txtScore_P2.text=scoreP2.ToString();
        txtScore_P3.text=scoreP3.ToString();
        txtScore_P4.text=scoreP4.ToString();
        counterLeft=objSpawnObj.TopSpawner-objSpawnObj.countSpawner;
        txtCounter.text = counterLeft.ToString();
        if(counterLeft==0 && !GameOver){
            StopGame();
            GameOver=true;
        }
    }

    IEnumerator WaitToWrapUp()
    {
        //yield on a new YieldInstruction that waits for 1 second.
        yield return new WaitForSeconds(3);
        //Determine winner
        WhoWon();
        FinalAnimations();
        StartCoroutine(ShowWinner(3f));   
    }

    void StopGame(){
        StartCoroutine(WaitToWrapUp());    
    }

    void FinalAnimations(){
        GameObject[] players;
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players){
            CatchController catchCon = player.transform.Find("Body/Root/Top/Hands/CatchCollider").GetComponent<CatchController>();
            Animator chanseyAnimator = player.transform.Find("Body").GetComponent<Animator>();
            if(catchCon.playerNumber==winnerNumber){
                //Get animator of winner.
                chanseyAnimator.SetBool("IsWin", true);
            }else{
                //This one didn't win, set animator to lose.
                 chanseyAnimator.SetBool("IsLose", true);
            }
        }
        if(winnerNumber==0){
            //Draw, everybody loose
            EverybodyLoose();
             
        }
    }

    void EverybodyLoose(){
        GameObject[] players;
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if(player.GetComponent<PhotonView>().IsMine){
                Animator chanseyAnimator = player.transform.Find("Body").GetComponent<Animator>();
                chanseyAnimator.SetBool("IsLose", true);
            }
        } 
    }

    void WhoWon(){
        int[] results=new int[4];
        results[0]=scoreP1;
        results[1]=scoreP2;
        results[2]=scoreP3;
        results[3]=scoreP4;

        int max = results.Max();
        int maxIndex = results.ToList().IndexOf(max);

        int second = 0;
        foreach (int i in results)
        {
            if (i > max)
            {
            second = max;
            max = i;
            }
        }
        Debug.Log("first "+max);
        Debug.Log("second "+second);

        winnerName="P"+(maxIndex+1);
        GameObject[] players;
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players){
            CatchController catchCon = player.transform.Find("Body/Root/Top/Hands/CatchCollider").GetComponent<CatchController>();
            if(catchCon.playerNumber==(maxIndex+1)){
                winnerName=catchCon.playerName;
            }
        }
        winnerNumber=maxIndex+1;
        if(max==second){
            //Draw
            winnerNumber=0;
            winnerName="";
            Transform win = WinCanvas.transform.Find("Win");
            win.gameObject.GetComponent<TextMeshProUGUI>().text="Draw";

        }
        Transform playName = WinCanvas.transform.Find("PlayerName");
        playName.gameObject.GetComponent<TextMeshProUGUI>().text=winnerName;      
    }

    IEnumerator ShowWinner(float time)
    {
        yield return new WaitForSeconds(time);
        WinCanvas.SetActive(true);
    }

    [PunRPC]
    void RPCIncrease(int player, int amount)
    {
        if(player==1){
            scoreP1+=amount;
        }
        if(player==2){
            scoreP2+=amount;
        }
        if(player==3){
            scoreP3+=amount;
        }
        if(player==4){
            scoreP4+=amount;
        }
    }

    public void IncreaseScore(int player, int amount){
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("RPCIncrease", RpcTarget.All, player,amount);
    }

    public void DecreaseScore(int player, int amount){
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("RPCDecrease", RpcTarget.All, player,amount);
    }

    [PunRPC]
    public void RPCDecrease(int player,int amount){
        if(player==1){
            scoreP1-=amount;
            if(scoreP1<0){
                scoreP1=0;
            }
        }
        if(player==2){
            scoreP2-=amount;
            if(scoreP2<0){
                scoreP2=0;
            }
        }
        if(player==3){
            scoreP3-=amount;
            if(scoreP3<0){
                scoreP3=0;
            }
        }
        if(player==4){
            scoreP4-=amount;
            if(scoreP4<0){
                scoreP4=0;
            }
        }
    }

}
}
