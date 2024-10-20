using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Obstacle_spawn : MonoBehaviour
{
    [SerializeField] GameObject pipe_prefab;
    [SerializeField] TextMeshProUGUI SpeedUp_text;

    [SerializeField] private float spawn_time_max = 3f;
    [SerializeField] private float spawn_time_min = 1.5f;

    [SerializeField] private float max_Y = 2.35f;
    [SerializeField] private float min_Y = -2.35f;
    [SerializeField] private float translate_spawn = 10f;

    [SerializeField] private Sprite[] color_pipes;

    [SerializeField] private float Time_SpeedUp = 30f;

    private float speed = 4f;

    private bool isOver = false;

    private float prev_Y = 0;
    private float space_between_pipe = 2.56f;

    private void Start()
    {
        StartCoroutine(WaitUntilSpawn(Random.Range(4f*spawn_time_min/speed, 4f*spawn_time_max/speed)));
        StartCoroutine(SpeedUp());
    }

    private float generateCoordInRange()
    {
        return Mathf.Clamp(Random.Range(prev_Y - space_between_pipe, prev_Y + space_between_pipe), min_Y, max_Y);
    }

    public void GameOver()
    {
        isOver = true;
        StopAllCoroutines();
    }

    IEnumerator WaitUntilSpawn(float time)
    {
        yield return new WaitForSeconds(time);

        float coord = generateCoordInRange();
        GameObject newPipe = Instantiate(pipe_prefab);
        newPipe.GetComponent<PipeObstacle_Script>().setPipeSpeed(speed);

        // Change color of pipes
        int index_color = Random.Range(0, 4);
        newPipe.transform.Find("Pipe Bottom").GetComponent<SpriteRenderer>().sprite = color_pipes[index_color];
        newPipe.transform.Find("Pipe Top").GetComponent<SpriteRenderer>().sprite = color_pipes[index_color];

        // Translate pipes
        newPipe.transform.Translate(new Vector3(translate_spawn,coord));

        if (!isOver)
        {
            StartCoroutine(WaitUntilSpawn(Random.Range(4f * spawn_time_min / speed, 4f * spawn_time_max / speed)));
        }
    }

    IEnumerator SpeedUp()
    {
        float time_left = Time_SpeedUp;
        while (time_left > 0f)
        {
            time_left -= Time.deltaTime;
            SpeedUp_text.SetText("Speed Up : " + Mathf.Ceil(time_left) + "s");
            yield return null;
        }
        foreach (PipeObstacle_Script item in GameObject.FindObjectsOfType<PipeObstacle_Script>())
        {
            item.setPipeSpeed(speed + 1);
        }
        speed += 1;
        time_left = Time_SpeedUp;
        StartCoroutine(SpeedUp());
    }
}
