using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDeal : MonoBehaviour {

    public CardStack dealer;
    public CardStack player;

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 256, 28), "Hit Me!"))
        {
            player.Push(dealer.Pop());
        }
    }

    // Test Deal
    //int count = 0;
    //int[] cards = new int[] {10, 13}; //Jack, Ace
    //void OnGUI()
    //{
       // if (GUI.Button(new Rect(10, 10, 256, 28), "Hit Me!"))
        //{
            //player.Push(cards[count++]);
        //}
    //}
}
