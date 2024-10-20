using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] protected float Speed = 2f;
    private const float MOUNTAIN_OFFSET_X = 18f;
    private const float MOUNTAIN_OFFSET_Y = -3f;
    private const float CLOUD_OFFSET_X = 18f;
    private bool isOver = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOver)
        {
        // Translate the moutains
            foreach (GameObject item in GameObject.FindGameObjectsWithTag("Mountains"))
            {
                item.transform.Translate(new Vector3(-Speed * Time.deltaTime, 0));
                if (item.transform.position.x < -MOUNTAIN_OFFSET_X)
                {
                    item.transform.position = new Vector3(MOUNTAIN_OFFSET_X,MOUNTAIN_OFFSET_Y);
                }
            }
        }
        

        // Translate the clouds
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Clouds"))
        {
            item.transform.Translate(new Vector3((float)- 0.25f * Speed * Time.deltaTime, 0));
            if (item.transform.position.x < -CLOUD_OFFSET_X)
            {
                item.transform.position = new Vector3(CLOUD_OFFSET_X,item.transform.position.y);
            }
        }
    }

    public void GameOver()
    {
        isOver = true;
    }
}
