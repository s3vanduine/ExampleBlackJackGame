using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    int dealersFirstCard = -1;

    public CardStack player;
    public CardStack dealer;
    public CardStack deck;

    public Button hitButton;
    public Button stickButton;
    public Button playAgainButton;

    public Text winnerText;

    /*
     * Cards delt to each player
     * First hist/stick/best
     * Dealer's turn: must have minimum of 17 score for hand before they can stick
     * Dealer's cards: first card is hidde, subsequent cards are face up
     * 
     */

    #region public methods

    public void Hit()
    {
        player.Push(deck.Pop());
        if (player.HandValue() > 21)
        {
            hitButton.interactable = false;
            stickButton.interactable = false;
            StartCoroutine(DealersTurn());
        }
    }

    public void Stick()
    {
        hitButton.interactable = false;
        stickButton.interactable = false;

        StartCoroutine(DealersTurn());
    }

    public void PlayAgain()
    {
        playAgainButton.interactable = false;

        //Remove visual component as well as data behind the scenes
        player.GetComponent<CardStackView>().Clear();
        dealer.GetComponent<CardStackView>().Clear();
        deck.GetComponent<CardStackView>().Clear();
        deck.CreateDeck();
        winnerText.text = "";

        dealersFirstCard = -1;

        hitButton.interactable = true;
        stickButton.interactable = true;

        StartGame();
    }

    #endregion

    #region Unity messages

    void Start()
    {
        StartGame();
    }

    #endregion

    void StartGame()
    {
        for (int i = 0; i < 2; i++)
        {
            player.Push(deck.Pop());
            HitDealer();
        }
    }

    void HitDealer()
    {
        int card = deck.Pop();

        if (dealersFirstCard < 0)
        {
            dealersFirstCard = card;
        }

        dealer.Push(card);
        if (dealer.CardCount >= 2)
        {
            CardStackView view = dealer.GetComponent<CardStackView>();
            view.Toggle(card, true);
        }
    }

    IEnumerator DealersTurn()
    {
        hitButton.interactable = false;
        stickButton.interactable = false;

        CardStackView view = dealer.GetComponent<CardStackView>();
        view.Toggle(dealersFirstCard, true);
        view.ShowCards();
        while (dealer.HandValue() < 17 || (dealer.HandValue() < player.HandValue() && player.HandValue() <= 21))
        {
            HitDealer();
            yield return new WaitForSeconds(2f);
        }

        //if player is bust or,
        //if player is less than dealer or equal but on an odd number, player loses
        if (player.HandValue() > 21 ||
            (dealer.HandValue() > player.HandValue() && dealer.HandValue() <= 21) ||
            (dealer.HandValue() == player.HandValue() && dealer.HandValue() % 2 == 1))
        {
            winnerText.text = "You've lost";
        }
        //if player is under 22, and has a hand value greater than the dealer's, or the dealer is bust but the player is not,
        //or the player and dealer have the same score but the number is even, the player wins.
        else if ((player.HandValue() <= 21 && player.HandValue() > dealer.HandValue()) ||
            (dealer.HandValue() > 21 && player.HandValue() <= 21) ||
            (dealer.HandValue() == player.HandValue() && dealer.HandValue() % 2 == 0))
        {
            winnerText.text = "Winner!";
        }
        //in case I missed any scenarios
        else 
        {
            winnerText.text = "It's a tie?";
        }
        yield return new WaitForSeconds(2f);
        playAgainButton.interactable = true;
    }

}
