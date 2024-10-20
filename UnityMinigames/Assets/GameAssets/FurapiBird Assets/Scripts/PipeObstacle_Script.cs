using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeObstacle_Script : MonoBehaviour
{
    protected float pipeSpeed = 4f;
    const float despawn_posX = -12f;

    private bool isOver = false;

    // Update is called once per frame
    void Update()
    {
        if (!isOver)
        {
            transform.Translate( -pipeSpeed * Time.deltaTime , 0, 0 );
            if (transform.position.x < despawn_posX)
            {
                Destroy(gameObject);
            }
        }
    }

    public void GameOver()
    {
        isOver = true;
    }

    public void setPipeSpeed(float speed)
    {
        pipeSpeed = speed;
    }
}
