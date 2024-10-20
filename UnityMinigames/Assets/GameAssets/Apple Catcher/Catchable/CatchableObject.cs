using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchableObject : MonoBehaviour
{
    public float gravity = 1f;
    public int points;
    public float despawnHeight = -10f;
    public float angularVelocity = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravity;
        rb.angularVelocity = angularVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < despawnHeight)
        {
            Destroy(gameObject);
        }
    }

    //React to a collision (collision start)
    void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(gameObject);
    }

    public int GetPoints()
    {
        return points;
    }
}
