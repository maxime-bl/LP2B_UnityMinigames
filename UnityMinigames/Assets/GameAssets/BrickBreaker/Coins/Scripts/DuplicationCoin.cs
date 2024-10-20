using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuplicationCoin : Coin
{
    public Ball ballPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (Ball b in GameObject.FindObjectsOfType<Ball>())
        {
            Ball duplicate = Instantiate(ballPrefab);
            duplicate.delayBeforeMoving = 0.15f;
            duplicate.speedTolerance = b.GetSpeed();
            duplicate.transform.position = b.transform.position;

            Rigidbody2D ballRB = b.GetComponent<Rigidbody2D>();
            duplicate.GetComponent<Rigidbody2D>().velocity = new Vector2(
                ballRB.velocity.y,
                Mathf.Abs(ballRB.velocity.x));
        }
        score.Add(30);
        Destroy(this.gameObject);
    }
}
