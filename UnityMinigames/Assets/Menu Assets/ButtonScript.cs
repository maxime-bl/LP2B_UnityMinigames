using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] protected Animator anim_ref;
    [SerializeField] protected GameObject animatedObject;
    [SerializeField] protected string SceneName;
    protected bool IsHighlighted = false;

    void Awake()
    {
        animatedObject.SetActive(true);
    }

    // Function used to run the highlight animation when the cursor is over the game button
    public void OnMouseEnter()
    {
        if (IsHighlighted == false)
        {
            IsHighlighted = true;
            anim_ref.SetBool("IsHighlighted", true);
        }
    }

    // Function used to stop the highlight animation when the cursor exit the game button
    public void OnMouseExit()
    {
        if (IsHighlighted == true)
        {
            IsHighlighted = false;
            anim_ref.SetBool("IsHighlighted", false);
        }
    }

    // Function used to run the click animation and sound effect
    public void OnMouseUpAsButton()
    {
        animatedObject.GetComponent<AudioSource>().Play();
        anim_ref.SetTrigger("OnClick");
        StartCoroutine(LoadScene_Game());
    }

    IEnumerator LoadScene_Game()
    {
        yield return new WaitForSeconds(1.3f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
