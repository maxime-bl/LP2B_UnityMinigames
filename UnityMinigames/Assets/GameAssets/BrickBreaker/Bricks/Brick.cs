using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Brick : MonoBehaviour
{
    public BrickEffect effectScript;
    public GameMaster gameMaster;
    public int points = 10;
    public bool alternateColor;
    private SpriteRenderer ref_sr;
    // Start is called before the first frame update
    void Start()
    {
        gameMaster = GameObject.FindObjectOfType<GameMaster>();
        effectScript = GetComponent<BrickEffect>();
        ref_sr = GetComponent<SpriteRenderer>();

        if(alternateColor == true)
        {
            float hue = (gameMaster.GetLevel()%5f) / 5;
            ref_sr.color = Color.HSVToRGB(hue, 0.5f, 1f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            if (effectScript != null)
            {
                effectScript.Effect(collision);
            }
            gameMaster.ReportBrickDeath(points);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            
            gameMaster.ReportBrickDeath(points);
            Destroy(this.gameObject);
        }
    }
}
