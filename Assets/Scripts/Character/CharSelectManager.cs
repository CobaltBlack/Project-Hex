using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class CharSelectManager : MonoBehaviour
{
    // assign in inspector
    public Animator animColorFade;                  //Reference to animator which will fade to and from black when starting game.
    public AnimationClip fadeColorAnimationClip;    //Animation clip fading to color (black default) when changing scenes

    public Character characterData;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void LoadGame(Character chosenCharacterData)
    {
        characterData = chosenCharacterData;

        //Use invoke to delay calling of LoadDelayed by half the length of fadeColorAnimationClip
        Invoke("LoadDelayed", fadeColorAnimationClip.length * .5f);

        //Set the trigger of Animator animColorFade to start transition to the FadeToOpaque state.
        animColorFade.SetTrigger("fade");
    }

    public void LoadDelayed()
    {
        //Load the selected scene, by scene index number in build settings
        SceneManager.LoadScene("Map");
    }
}