using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR;
using Valve.VR.InteractionSystem;

public class lookingAt : MonoBehaviour
{

    public Hand hand;

    percentageClock clock;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    bool b_isLookingAt = false;

    // Update is called once per frame
    void Update()
    {
        if(clock==null)
        {
            clock = hand.GetComponentInChildren<percentageClock>();
        }

        /*if (Story.currentState == Story.state.Tutorial)
        {
            GameObject.Find("Canvas").SetActive(true);
        }
        else if(Story.currentState == Story.state.preIntro || Story.currentState == Story.state.Intro)
        {
            return;
        }*/

        if (isLookingAt(hand.transform, this.transform) && !b_isLookingAt)
        {

            b_isLookingAt = true;
            clock.turnOn();
        }
        if(!isLookingAt(hand.transform, this.transform) && b_isLookingAt)
        {
            b_isLookingAt = false;
            clock.turnOff();
        }

    }
    bool isLookingAt(Transform objectToLookAt, Transform player)
    {
        Vector3 dirFromAtoB = (objectToLookAt.transform.position - player.transform.position).normalized;
        float dot = Vector3.Dot(dirFromAtoB, player.transform.forward);
        //Debug.Log("dot is " + dot);
        if (dot > 0.8f) return true;
        return false;
    }
}
