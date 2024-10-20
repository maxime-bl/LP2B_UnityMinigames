using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class Button_Next : MonoBehaviour
{
    private int actual_index;
    [SerializeField] protected GameObject Next_Button_Ref;
    [SerializeField] protected GameObject Prev_Button_Ref;
    [SerializeField] protected GameObject ActualGameButton_Ref;
    [SerializeField] protected Button_Prev Prev_Button_script_ref;
    [SerializeField] protected GameObject[] GameButton_Prefab;
    
    // Start is called before the first frame update
    void Start()
    {
        // Set the value of the index to the first button appearing on the screen
        // 0 : Apple catcher
        // 1 : Brick breaker
        // 2 : Furapi bird
        actual_index = 0;
    }

    // Fuction that switches the current game button to the next one
    public void SwitchGame()
    {
        // Unable the user to spam on the switch buttons
        Next_Button_Ref.GetComponent<Button>().interactable = false;
        Prev_Button_Ref.GetComponent<Button>().interactable = false;

        // Create the new game button
        GameObject NewGameButton_Ref = Instantiate(GameButton_Prefab[(actual_index + 1) % 3]);
        GameObject ToDestroy = ActualGameButton_Ref;

        // Desactivate the game buttons colliders
        ActualGameButton_Ref.GetComponent<Collider2D>().isTrigger = false;
        NewGameButton_Ref.GetComponent<Collider2D>().isTrigger = false;

        // Run the animations
        ActualGameButton_Ref.GetComponent<Animator>().SetTrigger("SlideOutRight");
        NewGameButton_Ref.GetComponent<Animator>().SetTrigger("SlideInLeft");

        // Refresh and destroy the game buttons
        StartCoroutine(WaitBeforeDestroying(ToDestroy, NewGameButton_Ref));
    }

    // Function to set the actual_index to a new value
    public void SetIndex(int index)
    {
        this.actual_index = index;
    }

    // Function to  set the Game button reference to a new one
    public void SetGameRef(GameObject ObjRef)
    {
        ActualGameButton_Ref = ObjRef;
    }

    // Coroutine used to wait before the end of the animation to update the game button (destroy ...)
    IEnumerator WaitBeforeDestroying(GameObject ToDestroy, GameObject NewGameButton_Ref)
    {
        // Wait 0.8s
        yield return new WaitForSecondsRealtime(0.8f);
        Destroy(ToDestroy);

        // Refresh the game button informations
        actual_index++;
        actual_index %= 3;
        Prev_Button_script_ref.SetIndex(actual_index);
        ActualGameButton_Ref = NewGameButton_Ref;
        Prev_Button_script_ref.SetGameRef(NewGameButton_Ref);

        // Activate the collider
        NewGameButton_Ref.GetComponent<Collider2D>().isTrigger = true;

        // Enable the user to click on the switch buttons
        Next_Button_Ref.GetComponent<Button>().interactable = true;
        Prev_Button_Ref.GetComponent<Button>().interactable = true;
    }
}
