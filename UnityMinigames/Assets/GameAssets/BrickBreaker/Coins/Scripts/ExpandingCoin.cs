using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandingCoin : Coin
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        score.Add(20);
        GameObject.FindObjectOfType<Paddle>().ApplyWidthBonus(1);
        Destroy(this.gameObject);
    }
}
