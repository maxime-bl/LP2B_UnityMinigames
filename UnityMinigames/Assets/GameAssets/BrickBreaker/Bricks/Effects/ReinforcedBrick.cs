using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReinforcedBrick : BrickEffect
{
    public Brick defaultBrickPrefab;
    public override void Effect(Collision2D ball)
    {
        //replaces itself with a regular brick
        Brick newBrick = Instantiate(defaultBrickPrefab);
        newBrick.transform.position = this.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
