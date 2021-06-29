using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Photon.Pun
{
public class ChanseyController : MonoBehaviour
{
    public Animator chanseyAnimator;

    // Update is called once per frame
    void Update()
    {
        if(chanseyAnimator!=null){
        if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)){
            chanseyAnimator.SetBool("IsLeft", true);
        }
        else{
            chanseyAnimator.SetBool("IsLeft", false);
        }
        if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)){
            chanseyAnimator.SetBool("IsRight", true);
        }
        else{
            chanseyAnimator.SetBool("IsRight", false);
        }
        }

    }
}

}
