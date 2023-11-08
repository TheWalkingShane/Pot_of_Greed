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
                    if (ST.isTaken(3))
                    {
                        ST.getCard(3).GetComponent<CardInfo>().health -= ST.getCard(0).GetComponent<CardInfo>().damage;
                        if (ST.getCard(3).GetComponent<CardInfo>().health <= 0)
                        {
                            ST.emptySlot(3);
                        }
                    }
                    else
                    {
                        this.enemyHealth -= ST.getCard(0).GetComponent<CardInfo>().damage;
                    }
                };
                if (ST.isTaken(1))
                {
                    if (ST.isTaken(4))
                    {
                        ST.getCard(4).GetComponent<CardInfo>().health -= ST.getCard(1).GetComponent<CardInfo>().damage;
                        if (ST.getCard(4).GetComponent<CardInfo>().health <= 0)
                        {
                            ST.emptySlot(4);
                        }
                    }
                    else
                    {
                        this.enemyHealth -= ST.getCard(1).GetComponent<CardInfo>().damage;
                    }
                };
                if (ST.isTaken(2))
                {
                    if (ST.isTaken(5))
                    {
                        ST.getCard(5).GetComponent<CardInfo>().health -= ST.getCard(2).GetComponent<CardInfo>().damage;
                        if (ST.getCard(5).GetComponent<CardInfo>().health <= 0)
                        {
                            ST.emptySlot(5);
                        }
                    }
                    else
                    {
                        this.enemyHealth -= ST.getCard(2).GetComponent<CardInfo>().damage;
                    }
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
                    if (ST.isTaken(0))
                    {
                        ST.getCard(0).GetComponent<CardInfo>().health -= ST.getCard(3).GetComponent<CardInfo>().damage;
                        if (ST.getCard(0).GetComponent<CardInfo>().health <= 0)
                        {
                            ST.emptySlot(0);
                        }
                    }
                    else
                    {
                        this.playerHealth -= ST.getCard(3).GetComponent<CardInfo>().damage;
                    }
                };
                if (ST.isTaken(4))
                {
                    if (ST.isTaken(1))
                    {
                        ST.getCard(1).GetComponent<CardInfo>().health -= ST.getCard(4).GetComponent<CardInfo>().damage;
                        if (ST.getCard(1).GetComponent<CardInfo>().health <= 0)
                        {
                            ST.emptySlot(1);
                        }
                    }
                    else
                    {
                        this.playerHealth -= ST.getCard(4).GetComponent<CardInfo>().damage;
                    }
                };
                if (ST.isTaken(5))
                {
                    if (ST.isTaken(2))
                    {
                        ST.getCard(2).GetComponent<CardInfo>().health -= ST.getCard(5).GetComponent<CardInfo>().damage;
                        if (ST.getCard(2).GetComponent<CardInfo>().health <= 0)
                        {
                            ST.emptySlot(2);
                        }
                    }
                    else
                    {
                        this.playerHealth -= ST.getCard(5).GetComponent<CardInfo>().damage;
                    }
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
        
        while(!ePressed && turns > 0)
        {
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
