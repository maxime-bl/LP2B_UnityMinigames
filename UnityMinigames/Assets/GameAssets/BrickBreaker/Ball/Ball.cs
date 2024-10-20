using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Spawning")]
    public Vector2 spawningPosition = new Vector2(0, -3.8f);
    public float delayBeforeMoving = 2f;
    private bool isMoving = false;
    private float spawningTime;

    [Header("Movement")]
    [SerializeField] private float speed = 4f;
    [Header("Trajectory correction")]
    public float minVerticalAngle = 10;
    public float minHorizontalAngle = 15;
    public float xWallProximity = 4.6f;
    public float speedTolerance = 0.05f;

    public float maxBouncingAngle = 50f;
    public float minimumHeight = -6;

    [Header("Sounds")]
    public AudioClip wallBumpSound;
    public AudioClip actionBumpSound;

    private AudioSource wallBumpAS;
    private AudioSource actionBumpAS;
    private Animator animator;
    private Rigidbody2D rb;
    private GameMaster gameMaster;


    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        animator.SetBool("IsBlinking", true);
        spawningTime = Time.time;
        rb = gameObject.GetComponent<Rigidbody2D>();
        gameMaster = GameObject.FindObjectOfType<GameMaster>();

        //initializes all audio sources (AS)
        wallBumpAS = gameObject.AddComponent<AudioSource>();
        wallBumpAS.clip = wallBumpSound;
        wallBumpAS.playOnAwake = false;
        wallBumpAS.volume = 0.5f;

        actionBumpAS = gameObject.AddComponent<AudioSource>();
        actionBumpAS.clip = actionBumpSound;
        actionBumpAS.playOnAwake = false;
        actionBumpAS.volume = 0.5f;

    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving && Time.time > delayBeforeMoving + spawningTime)
        {
            animator.SetBool("IsBlinking", false);
            rb.velocity = new Vector2(0, speed);
            isMoving = true;
        }

        if (isMoving)
        {
            //checks if the trajectory is to horizontal and corrects it
            if (Mathf.Abs(rb.velocity.normalized.y) < Mathf.Sin(Mathf.Deg2Rad * minHorizontalAngle))
            {
                rb.velocity = speed * new Vector2(
                    Mathf.Sign(rb.velocity.x) * Mathf.Cos((minHorizontalAngle + 1) * Mathf.Deg2Rad),
                    Mathf.Sign(rb.velocity.y) * Mathf.Sin((minHorizontalAngle + 1) * Mathf.Deg2Rad));
            }
            //checks if the ball is against a wall and its trajectory is too vertical
            else if (Mathf.Abs(transform.position.x) > xWallProximity && Mathf.Abs(rb.velocity.normalized.x) < Mathf.Sin(minVerticalAngle * Mathf.Deg2Rad))
            {
                rb.velocity = speed * new Vector2(
                    -Mathf.Sign(transform.position.x) * Mathf.Sin((minVerticalAngle+1) * Mathf.Deg2Rad),
                    Mathf.Sign(rb.velocity.y) * Mathf.Cos((minVerticalAngle+1) * Mathf.Deg2Rad));
            }

            //cheks if the ball is at the right speed
            if (rb.velocity.magnitude < speed * (1-speedTolerance) || rb.velocity.magnitude > speed * (1 + speedTolerance))
            {
                rb.velocity = rb.velocity.normalized * speed;
            }
        }

        // if the balls falls under the screen
        if (transform.position.y < minimumHeight)
        {
            gameMaster.ReportBallDeath();
            Destroy(gameObject);
        }
    }


    public void rotateBall(float paddleX, float paddleWidth)
    {
        float angle = Mathf.Clamp(((transform.position.x - paddleX)/(paddleWidth/2)) * maxBouncingAngle, -maxBouncingAngle, maxBouncingAngle);
        rb.velocity = new Vector2(speed * Mathf.Sin(angle * Mathf.Deg2Rad), speed * Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Ball")
        {
            wallBumpAS.Play();
        } else if (collision.gameObject.tag == "Brick")
        {
            actionBumpAS.Play();
        }
    }

    public void IncrementSpeed(float speedIncrement)
    {
        speed += speedIncrement;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public float GetSpeed()
    {
        return speed;
    }
}
