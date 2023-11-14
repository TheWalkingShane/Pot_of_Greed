using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


public class Gameplay : MonoBehaviour
{
    public static Gameplay Instance;
    public GameState State;
    private int playerHealth = 10;
    private int enemyHealth = 10;
    private int turns = 2;
    public CardPlacement placement;
    public SlotTracking ST;
    public CardLookUp CLU;
    public GameObject card;
    private GameObject currCard;
    public Transform storage;
    public GameObject[] Eslots = new GameObject[3];
    private bool ePressed = false;
    public TextMeshProUGUI health;
    public TextMeshProUGUI eHealth;
    
    public static event Action<GameState> OnGameStateChanged;
    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        UpdateGameState(GameState.PlayerTurn);
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;
        switch (newState)
        {
            case GameState.PlayerTurn:
                placement.cardInput(true);
                ePressed = false;
                StartCoroutine(playerSelection());
                break;
            case GameState.PlayerAttack:
                if (ST.isTaken(0))
                {
                    cardAttack(0,3, true);
                };
                if (ST.isTaken(1))
                {
                    cardAttack(1,4, true);
                };
                if (ST.isTaken(2))
                {
                    cardAttack(2,5, true);
                };
                if (this.enemyHealth <= 0)
                {
                    Debug.Log("Moving to Victory");
                    updateHealth();
                    UpdateGameState(GameState.Victory);
                    break;
                }
                updateHealth();
                Debug.Log("Moving to EnemyTurn");
                StartCoroutine(turnWait(GameState.EnemyTurn));
                break;
            case GameState.EnemyTurn:
                if (!ST.isEslotsEmpty())
                {
                    StartCoroutine(turnWait(GameState.EnemyAttack));
                    break;
                }
                int sel1 = Random.Range(1, 3); //Decides basekit or specialty
                int sel2 = Random.Range(3, 6); // Decides virtual slot num
                if (sel1 == 1)
                {
                    int sel3 = Random.Range(0, 4); //Decides card
                    while (ST.isTaken(sel2))
                    {
                        sel2 = Random.Range(3, 6);
                    }
                    currCard = Instantiate(card, storage.transform);
                    currCard.GetComponent<CardInfo>().setCard(CLU.cardList[sel3].health, CLU.cardList[sel3].damage);
                    currCard.transform.position = Eslots[sel2 - 3].transform.GetChild(5).position;
                    currCard.transform.rotation = Quaternion.Euler(0,0,0);
                    currCard.GetComponent<MeshRenderer>().material.mainTexture = CLU.cardList[sel3].cardImage; //Use card texture
                    ST.fillSlot(sel2, currCard);
                }
                if (sel1 == 2)
                {
                    int sel3 = Random.Range(0, 4); //Decides card
                    while (ST.isTaken(sel2))
                    {
                        sel2 = Random.Range(3, 6);
                    }
                    currCard = Instantiate(card, storage.transform);
                    currCard.GetComponent<CardInfo>().setCard(CLU.specialCardList[sel3].health, CLU.specialCardList[sel3].damage);
                    currCard.transform.position = Eslots[sel2 - 3].transform.GetChild(5).position;
                    currCard.transform.rotation = Quaternion.Euler(0,0,0);
                    currCard.GetComponent<MeshRenderer>().material.mainTexture = CLU.specialCardList[sel3].cardImage; //Use card texture
                    ST.fillSlot(sel2, currCard);
                }
                
                Debug.Log("EnemyAttack");
                StartCoroutine(turnWait(GameState.EnemyAttack));
                break;
            case GameState.EnemyAttack:
                if (ST.isTaken(3))
                {
                    cardAttack(3, 0, false);
                };
                if (ST.isTaken(4))
                {
                    cardAttack(4, 1, false);
                };
                if (ST.isTaken(5))
                {
                    cardAttack(5, 2,false);
                };
                if (this.playerHealth <= 0)
                {
                    Debug.Log("Moving to Lose");
                    updateHealth();
                    UpdateGameState(GameState.Lose);
                    break;
                }
                
                updateHealth();
                Debug.Log("Moving to PlayerTurn");
                StartCoroutine(turnWait(GameState.PlayerTurn));
                break;
            case GameState.Lose:
                //Play animation (possibly) and end game / go back to main menu
                break;
            case GameState.Victory:
                //Go back to maze
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        if (OnGameStateChanged != null) OnGameStateChanged(newState);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ePressed = true;
        }
    }

    public void cardPlaced()
    {
        turns--;
    }
    private IEnumerator playerSelection()
    {
        Debug.Log("Waiting...");
        
        while(!ePressed /*&& turns > 0*/)
        {
            if (turns <= 0)
            {
                placement.cardInput(false);
            }
            yield return null;
        }
        placement.cardInput(false);
        turns = 2;
        
        yield return new WaitForSeconds(2f);
        Debug.Log("Player Attack");
        UpdateGameState(GameState.PlayerAttack);
    }
    private IEnumerator turnWait(GameState newState) {

        yield return new WaitForSeconds(3f);
        UpdateGameState(newState);
    }

    private void updateHealth()
    {
        health.text = $"Player HP: {this.playerHealth}";
        eHealth.text = $"Enemy HP: {this.enemyHealth}";
    }

    private void cardAttack(int offensiveSlot, int defensiveSlot, bool playerTurn)
    {
        if (ST.isTaken(defensiveSlot))
        {
            ST.getCard(defensiveSlot).GetComponent<CardInfo>().dealDamage(ST.getCard(offensiveSlot).GetComponent<CardInfo>().damage);
            if (ST.getCard(defensiveSlot).GetComponent<CardInfo>().health <= 0)
            {
                ST.emptySlot(defensiveSlot);
            }
        }
        else if(playerTurn)
        {
            this.enemyHealth -= ST.getCard(offensiveSlot).GetComponent<CardInfo>().damage;
        }
        else
        {
            this.playerHealth -= ST.getCard(offensiveSlot).GetComponent<CardInfo>().damage;
        }
    }
}

public enum GameState
{
    PlayerTurn,
    PlayerAttack,
    EnemyTurn,
    EnemyAttack,
    Victory,
    Lose
}
