using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public AudioClip ref_audioClip;
    public SpriteRenderer fader_renderer;
    
    [Header("Apple")]
    public CatchableObject apple_prefab;
    public CatchableObject bonus_apple_prefab;

    public float appleMinSpawnInterval = 0.5f;
    public float appleSpawnInterval;
    public float bonusAppleProba;
    private float appleSpawnTimer = 1.5f;


    [Header("Bomb")]
    public CatchableObject bomb_prefab;
    public float bombMinSpawnInterval = 0.5f;
    public float bombSpawnInterval = 5;
    public float bombSpawnProba = 0.5f;
    public float delayWithoutBombs = 4;
    private float bombSpawnTimer = 0f;


    private float mainTimer = 0f;
    protected float startDelay = 3f;
    protected AudioSource ref_audioSource;
    protected float current_alpha = 1;

    // Start is called before the first frame update
    void Start()
    {        
        ref_audioSource = gameObject.AddComponent<AudioSource>();
        ref_audioSource.loop = true;
        ref_audioSource.volume = 0.5f;
        ref_audioSource.clip = ref_audioClip;

         StartCoroutine( FadeOutFromWhite() );
    }

    // Update is called once per frame
    void Update()
    {
        //updates the timers
        mainTimer += Time.deltaTime;
        appleSpawnTimer += Time.deltaTime;

        if (mainTimer > startDelay && GameObject.FindObjectOfType<Player_Script>().GetLives() > 0)
        {
            if (mainTimer > delayWithoutBombs)
            {
                bombSpawnTimer += Time.deltaTime;

            }

            //Spawns an apple or a bonus apple
            if (appleSpawnTimer >= appleSpawnInterval)
            {
                appleSpawnTimer = 0;
                appleSpawnInterval = Mathf.Clamp(appleSpawnInterval - 0.02f, appleMinSpawnInterval, 1000);
                InstantiateCatchableObject(apple_prefab);
                if (Random.Range(0f, 1f) <= bonusAppleProba)
                {
                    InstantiateCatchableObject(bonus_apple_prefab);
                } 
            }

            //tries to spawn a bomb
            if (bombSpawnTimer > bombSpawnInterval)
            {
                bombSpawnTimer = 0;
                bombSpawnInterval = Mathf.Clamp(bombSpawnInterval - 0.4f, bombMinSpawnInterval, 1000);
                if (Random.Range(0f, 1f) < bombSpawnProba)
                {
                    InstantiateCatchableObject(bomb_prefab);
                }
            }
        }
    }

    //Coroutine to fade out from white/launch music with a delay
    IEnumerator FadeOutFromWhite()
    {
        yield return new WaitForSeconds(0.5f);

        ref_audioSource.Play();

        while (current_alpha > 0)
        {
            current_alpha -= Time.deltaTime / 2;
            fader_renderer.color = new Color(1, 1, 1, current_alpha);
            yield return null;
        }

        Destroy(fader_renderer.gameObject);

    }

    private void InstantiateCatchableObject(CatchableObject prefab)
    {
        CatchableObject newObject = Instantiate(prefab);
        newObject.transform.position = new Vector3(Random.Range(-8.5f,8.5f), 6, 0);
    }
}
