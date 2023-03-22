using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Input Boxes")]
    public Image[] inputBoxes;

    private int[] boxValues = new int[9];
    private int[] rowSums = new int[3];
    private int[] colSums = new int[3];
    private int[] digonalSums = new int[2];

    private int counter = 0;
    private int playerColorChoice = 1;

    [Header("Colors")]
    private Color colorCircle = new Color(255, 0, 0);
    private Color colorCross = new Color(255, 255, 0);

    [Header("GameOver Scene Components")]
    public Text playerNameLabel;
    public Text winLabel;
    public Image retry;

    [Header("Current Playing Label")]
    public Text playerPlaying;
    public Text opponentPlaying;

    [Header("Opponent Thinking Times")]
    public float minThinkingTime = 1;
    public float maxThinkingTime = 3f;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else DestroyImmediate(this);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    private void Start()
    {
        NewGame();
    }

    private void NewGame()
    {
        playerNameLabel.gameObject.SetActive(false);
        winLabel.gameObject.SetActive(false);
        retry.gameObject.SetActive(false);
        PlayerPlayingTextUpdates();
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PlayerPlay(int choice)
    {
        if (boxValues[choice] != 0)
            return;

        this.counter++;
        BoxColouring(choice, playerColorChoice);

        if (CheckForWin(choice, playerColorChoice))
            PlayerWin();
        else if (counter == 9) Tie();
        else OpponentPlay();
    }

    private void OpponentPlay()
    {
        this.counter++;

        StartCoroutine(OpponentPlaying());

    }

    IEnumerator OpponentPlaying()
    {
        OpponentPlayingTextUpdates();

        yield return new WaitForSeconds(Random.Range(minThinkingTime, maxThinkingTime));
        while (true)
        {
            int choice = Random.Range(0, inputBoxes.Length);
            if (boxValues[choice] == 0)
            {
                BoxColouring(choice, playerColorChoice * -1);
                if (CheckForWin(choice, playerColorChoice * -1))
                    OpponentWin();
                else if (counter == 9) Tie();
                break;
            }
        }

        PlayerPlayingTextUpdates();
    }

    private void BoxColouring(int choice, int currentSelection)
    {
        if (currentSelection == 1)
        {
            inputBoxes[choice].GetComponent<Image>().color = colorCross;
            boxValues[choice] = 1;
        }
        else
        {
            inputBoxes[choice].GetComponent<Image>().color = colorCircle;
            boxValues[choice] = -1;
        }
    }


    private bool CheckForWin(int choice, int currentSelection)
    {
        colSums[choice % 3] += currentSelection;
        if (colSums[choice % 3] == 3 || colSums[choice % 3] == -3)
            return true;

        rowSums[(int)(choice / 3)] += currentSelection;
        if (rowSums[(int)(choice / 3)] == 3 || rowSums[(int)(choice / 3)] == -3)
            return true;

        if (choice == 0 || choice == 4 || choice == 8)
        {
            digonalSums[0] += currentSelection;
            if (digonalSums[0] == 3 || digonalSums[0] == -3)
                return true;
        }
        if (choice == 2 || choice == 4 || choice == 6)
        {
            digonalSums[1] += currentSelection;
            if (digonalSums[1] == 3 || digonalSums[1] == -3)
                return true;
        }
        return false;
    }

    private void PlayerWin()
    {
        GameOverComponentsActivation();
        playerNameLabel.text = "PLAYER";
        winLabel.gameObject.SetActive(true);
    }


    private void OpponentWin()
    {
        GameOverComponentsActivation();
        playerNameLabel.text = "OPPONENT";
        winLabel.gameObject.SetActive(true);
    }

    private void Tie()
    {
        GameOverComponentsActivation();
        playerNameLabel.text = "TIE";
    }

    private void GameOverComponentsActivation()
    {
        playerNameLabel.gameObject.SetActive(true);
        retry.gameObject.SetActive(true);

        playerPlaying.gameObject.SetActive(false);
        opponentPlaying.gameObject.SetActive(false);
    }

    private void PlayerPlayingTextUpdates()
    {
        playerPlaying.gameObject.SetActive(true);
        opponentPlaying.gameObject.SetActive(false);
    }

    private void OpponentPlayingTextUpdates()
    {
        playerPlaying.gameObject.SetActive(false);
        opponentPlaying.gameObject.SetActive(true);
    }

}
