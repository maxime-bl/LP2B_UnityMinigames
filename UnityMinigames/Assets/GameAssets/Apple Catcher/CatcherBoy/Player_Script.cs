using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player_Script : MonoBehaviour
{

    //---------------------------------------------------------------------------------
    // ATTRIBUTES
    //---------------------------------------------------------------------------------
    public TextMeshPro score_text;
    public TextMeshPro lives_text;
    public float speed;
    public float xMax;
    public AudioClip appleSound;
    public AudioClip bonusAppleSound;
    public AudioClip bombSound;
    public AudioClip endSound;
    public List<GameObject> gameOverMenuElements;

    protected int score = 0;
    protected int lives = 3;
    protected AudioSource apple_as;
    protected AudioSource bonus_apple_as;
    protected AudioSource bomb_as;
    protected AudioSource end_as;
    protected Animator ref_animator;
    protected Rigidbody2D ref_rb;

    //---------------------------------------------------------------------------------
    // METHODS
    //---------------------------------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        apple_as = gameObject.AddComponent<AudioSource>();
        apple_as.playOnAwake = false;
        apple_as.volume = 0.7f;
        apple_as.clip = appleSound;

        bonus_apple_as = gameObject.AddComponent<AudioSource>();
        bonus_apple_as.playOnAwake = false;
        bonus_apple_as.volume = 0.7f;
        bonus_apple_as.clip = bonusAppleSound;

        bomb_as = gameObject.AddComponent<AudioSource>();
        bomb_as.playOnAwake = false;
        bomb_as.volume = 1f;
        bomb_as.clip = bombSound;

        end_as = gameObject.AddComponent<AudioSource>();
        end_as.playOnAwake = false;
        end_as.volume = 1f;
        end_as.clip = endSound;

        ref_animator = GetComponent<Animator>();
        ref_rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Manage movement speed and animations
        if (lives > 0)
        {
            Debug.Log(Input.GetAxis("Horizontal"));
            transform.Translate(Time.deltaTime * new Vector3(speed * Input.GetAxis("Horizontal"), 0, 0));

            if (Mathf.Abs(transform.position.x) > xMax)
            {
                transform.position = new Vector3((xMax - 0.01f) * Mathf.Sign(transform.position.x), transform.position.y, transform.position.z);
            }

            //Inform animator : are we going forward/backward ?
            if (Input.GetAxis("Horizontal") < 0)
            {
                ref_animator.SetBool("isForwards", false);
            }
            else if (Input.GetAxis("Horizontal") > 0)
            {
                ref_animator.SetBool("isForwards", true);
            }

            //Inform animator : Are we moving?
            ref_animator.SetBool("isMoving", Input.GetAxis("Horizontal") != 0);
        }
    }

    //React to a collision (collision start)
    void OnCollisionEnter2D(Collision2D col)
    {
        //if catches an apple
        if (col.gameObject.tag == "Apple" || col.gameObject.tag == "BonusApple") 
        {
            score += col.gameObject.GetComponent<CatchableObject>().GetPoints();
            score_text.SetText("Score : " + score);

            if (col.gameObject.tag == "Apple")
            {
                apple_as.Play();
            } else {
                bonus_apple_as.Play();
            }
        }

        //if catches a bomb
        else if (col.gameObject.tag == "Bomb")
        {
            bomb_as.Play();
            lives--;
            lives_text.SetText("Lives : " + lives);
            if (lives <= 0)
            {
                //game over
                foreach (CatchableObject obj in GameObject.FindObjectsOfType<CatchableObject>())
                {
                    Destroy(obj.gameObject);
                }
                end_as.Play();
                ref_animator.SetBool("isMoving", false);
                foreach (GameObject obj in gameOverMenuElements)
                {
                    obj.GetComponent<Animator>().SetTrigger("FadeIn");
                }
            }
        }
    }

    public int GetLives()
    {
        return lives;
    }
}
