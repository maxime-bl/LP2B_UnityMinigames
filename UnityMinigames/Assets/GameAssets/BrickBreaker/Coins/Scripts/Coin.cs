using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Coin : MonoBehaviour
{
    public float minHeight = -6f;
    public float speed = 2f;
    protected Score score;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreLayerCollision(0, 6);
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -speed);
        score = GameObject.FindObjectOfType<Score>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < minHeight)
        {
            Destroy(this.gameObject);
        }
    }

}
