using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//augmenter le score lorsqu'une brique décède

public class GameMaster : MonoBehaviour
{
    [Header("Balls")]
    public Ball ballPrefab;
    public float initialBallSpeed = 4f;
    [SerializeField] private float ballSpeed;
    public float speedIncrement = 2f;

    [Header("Object References")]
    public Score score;
    public Paddle paddle;
    public TMP_Text levelCompleteText;
    public TMP_Text gameOverText;
    public HealthBar healthBar;
    public TMP_Text quitRestartText;

    [Header("Bricks")]
    public Brick defaultBrick;
    public Brick reinforcedBrick;
    public float reinforcedBrickRatio = 0.15f;
    private Vector2 brickSize;//new Vector2(1.76f, 0.92f);

    [Header("Grid")]
    public List<Vector2Int> gridSizes;
    public Vector2Int finalGridSize = new Vector2Int(11,9);
    public Vector2 gridCenter = new Vector2(0f, 2f);
    public float brickRatio = 0.5f;
    private Vector2Int currentGridSize;


    [Header("Sounds")]
    public AudioClip introSound;
    public AudioClip levelCompleteSound;
    public AudioClip ballDeathSound;
    public AudioClip gameOverSound;

    private AudioSource introAS;
    private AudioSource levelCompleteAS;
    private AudioSource ballDeathAS;
    private AudioSource gameOverAS;

    private int levelNumber = 1;
    private int lives = 3;


    // Start is called before the first frame update
    void Start()
    {
        brickSize = defaultBrick.transform.localScale * defaultBrick.GetComponent<BoxCollider2D>().size;
        ballSpeed = initialBallSpeed;
        currentGridSize = gridSizes[0];
        healthBar.SetLifeNumber(lives);

        introAS = gameObject.AddComponent<AudioSource>();
        introAS.clip = introSound;
        introAS.playOnAwake = false;
        introAS.volume = 0.85f;

        levelCompleteAS = gameObject.AddComponent<AudioSource>();
        levelCompleteAS.clip = levelCompleteSound;
        levelCompleteAS.playOnAwake = false;
        levelCompleteAS.volume = 0.85f;

        ballDeathAS = gameObject.AddComponent<AudioSource>();
        ballDeathAS.clip = ballDeathSound;
        ballDeathAS.playOnAwake = false;
        ballDeathAS.volume = 0.7f;


        gameOverAS = gameObject.AddComponent<AudioSource>();
        gameOverAS.clip = gameOverSound;
        gameOverAS.playOnAwake = false;
        gameOverAS.volume = 0.85f;


        generateBricks();
        introAS.Play();

        this.SummonBall(2f);
    }


    public void generateBricks() {
        foreach (Brick brick in Object.FindObjectsOfType<Brick>())
        {
            Destroy(brick.gameObject);
        }

        float x;
        bool isWidthOdd;
        if (currentGridSize.x%2 != 0)
        {
            isWidthOdd = true;
            x = gridCenter.x;
        } else
        {
            isWidthOdd = false;
            x = gridCenter.x + brickSize.x / 2;
        }


        for (int i = 0; i < Mathf.Ceil((float)currentGridSize.x / 2); i++)
        {
            float y = gridCenter.y + brickSize.y * currentGridSize.y / 2;
            for (int j = 0; j<currentGridSize.y; j++)
            {
                if (Random.Range(0f, 1f) < brickRatio)
                {
                    //picks a brick type randomly
                    float random = Random.Range(0f, 1f);
                    Brick brickToInstantiate;
                             
                    if (random > reinforcedBrickRatio)
                    {
                        brickToInstantiate = defaultBrick;
                    } else
                    {
                        brickToInstantiate = reinforcedBrick;
                    }
              
                    Brick newBrick = Instantiate(brickToInstantiate);
                    newBrick.transform.position = new Vector2(x, y);

                    //for every column except the middle one, when the width is odd
                    if (!(isWidthOdd && i == 0))
                    {
                        Brick newBrick2 = Instantiate(brickToInstantiate);
                        newBrick2.transform.position = new Vector2(-x, y);
                    }
                }
                y -= brickSize.y;
            }
            x += brickSize.x;
        }
    }

    public void ReportBrickDeath(int points)
    {
        score.Add(points);

        if (GameObject.FindObjectsOfType<Brick>().Length <= 1)
        {
            levelCompleteAS.Play();
            //remove all the remaining balls
            foreach (Ball b in Object.FindObjectsOfType<Ball>())
            {
                Destroy(b.gameObject);
            }
            StartCoroutine(LevelComplete());
        }
    }

    public void ReportBallDeath()
    {
        // if these is was the last ball in the scene
        if (GameObject.FindObjectsOfType<Ball>().Length <= 1)
        {
            //remove buffs
            lives--;
            healthBar.SetLifeNumber(lives);

            if (lives > 0)
            {
                //respawns a ball and remove width bonus/malus
                paddle.ResetWidth();
                paddle.transform.position = new Vector2(0, paddle.transform.position.y);
                ballDeathAS.Play();
                SummonBall(2f);
            } else
            {
                //game over
                foreach (Brick b in Object.FindObjectsOfType<Brick>())
                {
                    Destroy(b.gameObject);
                }
                foreach (Coin c in Object.FindObjectsOfType<Coin>())
                {
                    Destroy(c.gameObject);
                }
                paddle.gameObject.SetActive(false);
                score.gameObject.SetActive(false);
                healthBar.gameObject.SetActive(false);

                gameOverText.text = "Game Over !" + "\n\n" +
                    "Level: " + levelNumber +
                    "\nScore: " + score.GetScore();
                gameOverText.GetComponent<Animator>().SetTrigger("FadeIn");
                quitRestartText.gameObject.GetComponent<Animator>().SetTrigger("FadeIn");

                //play game over sound
                gameOverAS.Play();

            }
        }
    }


    public void SummonBall(float delayBeforeMoving)
    {
        Ball newBall = Instantiate(ballPrefab);
        newBall.transform.position = paddle.transform.position + new Vector3(0, 0.5f, 0);
        newBall.delayBeforeMoving = delayBeforeMoving;
        newBall.SetSpeed(this.ballSpeed);
    }


    public void GenerateLevel()
    {
        generateBricks();
        paddle.transform.position = new Vector2(0, paddle.transform.position.y);
        this.SummonBall(2f);
    }



    IEnumerator LevelComplete()
    {
        foreach (Coin c in Object.FindObjectsOfType<Coin>())
        {
            Destroy(c.gameObject);
        }
        levelCompleteText.text = "Level " + levelNumber + "\ncomplete";
        levelNumber++;
        levelCompleteText.GetComponent<Animator>().SetTrigger("FadeIn");
        ballSpeed += speedIncrement;

        if (levelNumber <= gridSizes.Count)
        {
            currentGridSize = gridSizes[levelNumber - 1];
        } else
        {
            currentGridSize = finalGridSize;
        }
        
        yield return new WaitForSeconds(2f);
        levelCompleteText.GetComponent<Animator>().SetTrigger("FadeOut");

        yield return new WaitForSeconds(0.75f);
        paddle.ResetWidth();
        GenerateLevel();
    }

   public int GetLevel()
    {
        return levelNumber;
    }
}
