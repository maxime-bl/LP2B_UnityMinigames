using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BirdController : MonoBehaviour
{
    protected Animator Bird_Animator;
    protected Rigidbody2D Bird_Rb2d;
    protected AudioSource Bird_Sound;
    [SerializeField] protected AudioSource Music_player;
    [SerializeField] protected AudioClip[] Jingles;
    [SerializeField] protected float Speed = 4f;
    [SerializeField] protected Obstacle_spawn spawner;
    [SerializeField] protected TextMeshProUGUI score;

    [SerializeField] protected Canvas GameOverMenu;
    [SerializeField] protected Canvas StartMenu;
    [SerializeField] protected TextMeshProUGUI Final_Score;
    [SerializeField] protected Parallax parallax;

    private float jump_timer = 0;
    [SerializeField] protected float Time_before_speed_jump = 0.4f;

    private int player_score = 0;

    private bool isOver = false;
    private bool hasStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        Bird_Animator = this.GetComponent<Animator>();
        Bird_Rb2d = this.GetComponent<Rigidbody2D>();
        Bird_Sound = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasStarted == true)
        {
            // Jump
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space) && isOver == false)
            {
                Bird_Rb2d.velocity = new Vector2(Bird_Rb2d.velocity[0], Speed);
            }
            else if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space)) && jump_timer > Time_before_speed_jump && isOver == false)
            {
                Bird_Rb2d.velocity = new Vector2(Bird_Rb2d.velocity[0], Speed);
            }


            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space))
            {
                jump_timer += Time.deltaTime;
            }

            if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.Space))
            {
                jump_timer = 0;
            }

            Vector3 velocity = Bird_Rb2d.velocity;
            Bird_Animator.SetFloat("VerticalSpeed", velocity[1]);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space))
            {
                StartMenu.gameObject.SetActive(false);
                Time.timeScale = 1;
                hasStarted = true;
            }
        }
    }

    // Function that detect the collision between the bird and the pipes / bottom hitbox
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Pipe"))
        {
            Bird_Animator.SetTrigger("Dead");
            Bird_Rb2d.gravityScale = 3f;
            if (isOver == false)
            {
                StartCoroutine(DeathSounds());
            }
            isOver = true;
            spawner.GameOver();
            parallax.GameOver();
            foreach (PipeObstacle_Script item in GameObject.FindObjectsOfType<PipeObstacle_Script>())
            {
                item.GameOver();
            }
            GameOverMenu.gameObject.SetActive(true);
            Final_Score.SetText("Score : " + player_score);
        }
    }

    // Function to update the score
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Point"))
        {
            player_score++;
            score.SetText("Score : " + player_score);
        }
    }

    IEnumerator DeathSounds()
    {
        Music_player.Stop();
        Bird_Sound.Play();
        yield return new WaitForSeconds(1f);
        Music_player.PlayOneShot(Jingles[1]);
    }
}
