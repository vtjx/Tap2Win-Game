using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;

public class GameMaster : MonoBehaviour
{
    [SerializeField]
    private GameObject[] leftTapObjects;
    [SerializeField]
    private GameObject[] rightTapObjects;
    [SerializeField]
    private GameObject[] showObjects;
    [SerializeField]
    private GameObject[] unshowObjects;
    [SerializeField]
    private GameObject startBtn;
    [SerializeField]
    private GameObject pauseText;
    [SerializeField]
    private GameObject gameOverScreen;
    [SerializeField]
    private GameObject cloud;
    private Image leftTapImg;
    private Image rightTapImg;
    [SerializeField]
    private Sprite ogSprite;
    [SerializeField]
    private Sprite tapSprite;
    private int randomPick;
    private float timer;
    [SerializeField]
    private TMP_Text timerTxt;
    private int multiplier;
    [SerializeField]
    private TMP_Text multiplierTxt;
    private int score;
    [SerializeField]
    private TMP_Text scoreTxt;
    private int highscore;
    [SerializeField]
    private TMP_Text highscoreTxt;
    [SerializeField]
    private TMP_Text postScoreTxt;
    private bool leftTapped;
    private bool rightTapped;
    private bool start;
    private bool gameOver;
    private AudioSource aS;
    [SerializeField]
    private AudioClip scoreSnd;
    [SerializeField]
    private AudioClip missSnd;

    // Start is called before the first frame update
    void Start()
    {
        aS = GetComponent<AudioSource>();
        highscore = PlayerPrefs.GetInt("Highscore");
        timer = 60;
        StartCoroutine("cloudSystem");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }


        timeCtd();
        if (leftTapped)
        {
            leftGroupSystem();
            leftTapped = false;
        }

        if (rightTapped)
        {
            rightGroupSystem();
            rightTapped = false;
        }

        cloudSystem();
        toString();
    }

    private void timeCtd()
    {
        if (start)
        {
            timer -= Time.deltaTime;
            timerTxt.text = Mathf.RoundToInt(timer).ToString() + "s";
            if (timer <= 0)
            {
                gameOver = true;
                GameOverScreen();
            }
        }
    }

    private void leftGroupSystem()
    {
        randomPick = Random.Range(0, leftTapObjects.Length);
        leftTapImg = leftTapObjects[randomPick].GetComponent<Image>();
        leftTapImg.sprite = tapSprite;
    }

    private void rightGroupSystem()
    {
        randomPick = Random.Range(0, rightTapObjects.Length);
        rightTapImg = rightTapObjects[randomPick].GetComponent<Image>();
        rightTapImg.sprite = tapSprite;
    }

    public void leftTap()
    {
        if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite == tapSprite)
        {
            multiplier += 1;
            score += 10 * multiplier;
            UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite = ogSprite;
            leftTapped = true;
            aS.PlayOneShot(scoreSnd);
        }
        else
        {
            multiplier = 0;
            score -= 10;
            foreach (GameObject tap in leftTapObjects)
            {
                tap.GetComponent<Image>().sprite = ogSprite;
            }
            leftTapped = true;
            aS.PlayOneShot(missSnd);
        }
    }

    public void rightTap()
    {
        if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite == tapSprite)
        {
            multiplier += 1;
            score += 10;
            UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite = ogSprite;
            rightTapped = true;
            aS.PlayOneShot(scoreSnd);
        }
        else
        {
            multiplier = 0;
            score -= 10;
            foreach (GameObject tap in rightTapObjects)
            {
                tap.GetComponent<Image>().sprite = ogSprite;
            }
            rightTapped = true;
            aS.PlayOneShot(missSnd);
        }
    }

    private IEnumerator cloudSystem()
    {
        while (!gameOver)
        {
            Instantiate(cloud, new Vector2(13, Random.Range(3, 5)), Quaternion.identity);
            yield return new WaitForSeconds(4);
        }
    }

    private void toString()
    {
        if (score <= 0)
        {
            score = 0;
        }

        if (score > highscore)
        {
            highscore = score;
            PlayerPrefs.SetInt("Highscore", highscore);
        }
        multiplierTxt.text = "x" + multiplier.ToString();
        scoreTxt.text = score.ToString();
        highscoreTxt.text = "HIGHSCORE\n" + highscore.ToString();
        postScoreTxt.text = "SCORE\n" + score.ToString();
    }

    private void GameOverScreen()
    {
        foreach (GameObject obj in unshowObjects)
        {
            obj.SetActive(false);
        }
        gameOverScreen.SetActive(true);
    }

    public void StartBtn()
    {
        start = true;
        startBtn.SetActive(false);
        foreach (GameObject obj in showObjects)
        {
            obj.SetActive(true);
        }
        leftGroupSystem();
        rightGroupSystem();
    }

    public void PauseBtn()
    {
        if (Time.timeScale != 0)
        {
            Time.timeScale = 0;
            pauseText.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pauseText.SetActive(false);
        }
    }

    public void RetryBtn()
    {
        SceneManager.LoadScene("Game");
    }

    public void ExitBtn()
    {
        Application.Quit();
    }
}
