using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject lifeIconPrefab;
    public float iconOffset = 1f;
    public List<GameObject> iconList;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLifeNumber(int lives)
    {
        foreach (GameObject icon in iconList)
        {
            Destroy(icon.gameObject);
        }

        for (int i=0; i<lives; i++)
        {
            GameObject newIcon = Instantiate(lifeIconPrefab);
            newIcon.transform.parent = this.transform;
            newIcon.transform.localPosition = /*this.transform.localPosition +*/ new Vector3(i * iconOffset, 0, 0);
            iconList.Add(newIcon);

        }
    }
}
