using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkingCoin : Coin
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        score.Remove(30);
        GameObject.FindObjectOfType<Paddle>().ApplyWidthBonus(-1);
        Destroy(this.gameObject);
    }
}
