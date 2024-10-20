using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCoin : BrickEffect
{
    public List<CoinListElement> coinList;
    public override void Effect(Collision2D ball)
    {
        float random = Random.Range(0f, 1f);
        float probSum = 0f;
        Coin coinToInstantiate = null;

        foreach (CoinListElement coin in coinList)
        {
            probSum += coin.probability;
            if (random < probSum)
            {
                coinToInstantiate = coin.prefab;
                break;
            }
        }

        if (coinToInstantiate != null)
        {
            Coin newCoin = Instantiate(coinToInstantiate);
            newCoin.transform.position = this.transform.position;
        }
    }

    [System.Serializable]
    public class CoinListElement
    {
        public Coin prefab;
        public float probability;
    }
}
