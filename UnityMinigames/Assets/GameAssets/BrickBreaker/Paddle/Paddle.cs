using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    [Header("Movement")]
    public float SPEED;
    public float xWall= 4.8f;
    private float xMax;

    [Header("Prefabs")]
    public Ball ballPrefab;

    [Header("Bonus")]
    public float defaultWidth = 3.5f;
    public float widthIncrement = 0.7f; 
    public int minWidthBonusLevel = -1;
    public int maxWidthBonusLevel = 3;

    [Header("Sound")]
    public AudioClip pickupSound;
    public AudioClip bouncingSound;

    private AudioSource pickupAS;
    private AudioSource bouncingAS;

    private int widthBonusLevel = 0;
    private BoxCollider2D bc;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        bc = gameObject.GetComponent<BoxCollider2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        bc.size = new Vector2(defaultWidth, bc.size.y);
        sr.size = new Vector2(defaultWidth, sr.size.y);
        xMax = xWall - (bc.size.x / 2) * transform.localScale.x;

        pickupAS = gameObject.AddComponent<AudioSource>();
        pickupAS.clip = pickupSound;
        pickupAS.playOnAwake = false;
        pickupAS.volume = 1f;

        bouncingAS = gameObject.AddComponent<AudioSource>();
        bouncingAS.clip = bouncingSound;
        bouncingAS.playOnAwake = false;
        bouncingAS.volume = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Input.GetAxis("Horizontal") * SPEED * Time.deltaTime, 0f,0f);

        //checks that the paddle doesn't cross a wall
        if (Mathf.Abs(transform.position.x) > xMax)
        {
            transform.position = new Vector3((float)(xMax * Mathf.Sign(transform.position.x) - 0.01f), transform.position.y, transform.position.z);
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            bouncingAS.Play();
            collision.GetComponent<Ball>().rotateBall(transform.position.x, sr.size.x * transform.localScale.x);
        } else if (collision.gameObject.tag == "Coin")
        {
            pickupAS.Play();
        }
    }  
    
    public void ApplyWidthBonus(int value)
    {
        if (widthBonusLevel+value >= minWidthBonusLevel && widthBonusLevel+value <= maxWidthBonusLevel)
        {
            widthBonusLevel += value;
            bc.size = new Vector2(defaultWidth + widthBonusLevel * widthIncrement, bc.size.y);
            sr.size = new Vector2(defaultWidth + widthBonusLevel * widthIncrement, sr.size.y);
            xMax = xWall - (bc.size.x / 2) * transform.localScale.x;
        }

    }

    public void ResetWidth()
    {
        widthBonusLevel = 0;
        bc.size = new Vector2(defaultWidth, bc.size.y);
        sr.size = new Vector2(defaultWidth, sr.size.y);
        xMax = xWall - (bc.size.x / 2) * transform.localScale.x;
    }
}
