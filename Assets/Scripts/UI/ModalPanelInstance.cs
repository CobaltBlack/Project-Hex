using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class ModalPanelInstance : MonoBehaviour
{

    public Text question;
    public Text answerOne;
    public Text answerTwo;
    public Text answerThree;
    public Text answerFour;
    public Text answerFive;
    public Text answerSix;
    //public Image iconImage; // no images in current iteration
    public Button choiceOneButton;
    public Button choiceTwoBouton;
    public Button choiceThreeButton;
    public Button choiceFourButton;
    public Button choiceFiveButton;
    public Button choiceSixButton;
    public GameObject modalPanelObject; // assign "Modal Panel - Instance" in inspector. Also, remember to tick off "Modal Panel - Instance" before running game.

    private static ModalPanelInstance modalPanel;

    public static ModalPanelInstance Instance()
    {
        if (!modalPanel)
        {
            modalPanel = FindObjectOfType(typeof(ModalPanelInstance)) as ModalPanelInstance;
            if (!modalPanel)
            {
                Debug.LogError("There needs to be one active ModalPanel script on a GameObject in your scene.");
            }
        }
        return modalPanel;
    }

    // 4 button default
    public void ChoiceFour(string question, string answerOne, string answerTwo, string answerThree, string answerFour, UnityAction choiceOneEvent, UnityAction choiceTwoEvent, UnityAction choiceThreeEvent, UnityAction choiceFourEvent)
    {
        modalPanelObject.SetActive(true); // turn modal panel ON

        choiceOneButton.onClick.RemoveAllListeners(); // we might be listening to something, so we dont call something from last time
        choiceOneButton.onClick.AddListener(choiceOneEvent);
        //choiceOneButton.onClick.AddListener(ClosePanel);

        choiceTwoBouton.onClick.RemoveAllListeners();
        choiceTwoBouton.onClick.AddListener(choiceTwoEvent);
        //choiceTwoBouton.onClick.AddListener(ClosePanel);

        choiceThreeButton.onClick.RemoveAllListeners();
        choiceThreeButton.onClick.AddListener(choiceThreeEvent);
        //choiceThreeButton.onClick.AddListener(ClosePanel);

        choiceFourButton.onClick.RemoveAllListeners();
        choiceFourButton.onClick.AddListener(choiceFourEvent);
        //choiceFourButton.onClick.AddListener(ClosePanel);

        this.question.text = question; // set question
        this.answerOne.text = answerOne; // set answers
        this.answerTwo.text = answerTwo;
        this.answerThree.text = answerThree;
        this.answerFour.text = answerFour;

        //this.iconImage.gameObject.SetActive(false); // turn icon off

        choiceOneButton.gameObject.SetActive(true); // turn buttons on
        choiceTwoBouton.gameObject.SetActive(true);
        choiceThreeButton.gameObject.SetActive(true);
        choiceFourButton.gameObject.SetActive(true);
    }

    // 5 button variation
    public void ChoiceFive(string question, UnityAction choiceOneEvent, UnityAction choiceTwoEvent, UnityAction choiceThreeEvent, UnityAction choiceFourEvent, UnityAction choiceFiveEvent)
    {
        modalPanelObject.SetActive(true);

        choiceOneButton.onClick.RemoveAllListeners(); // we might be listening to something, so we dont call something from last time
        choiceOneButton.onClick.AddListener(choiceOneEvent);
        choiceOneButton.onClick.AddListener(ClosePanel);

        choiceTwoBouton.onClick.RemoveAllListeners();
        choiceTwoBouton.onClick.AddListener(choiceTwoEvent);
        choiceTwoBouton.onClick.AddListener(ClosePanel);

        choiceThreeButton.onClick.RemoveAllListeners();
        choiceThreeButton.onClick.AddListener(choiceThreeEvent);
        choiceThreeButton.onClick.AddListener(ClosePanel);

        choiceFourButton.onClick.RemoveAllListeners();
        choiceFourButton.onClick.AddListener(choiceFourEvent);
        choiceFourButton.onClick.AddListener(ClosePanel);

        choiceFiveButton.onClick.RemoveAllListeners();
        choiceFiveButton.onClick.AddListener(choiceFiveEvent);
        choiceFiveButton.onClick.AddListener(ClosePanel);

        this.question.text = question; // set question

        //this.iconImage.gameObject.SetActive(false); // turn icon off

        choiceOneButton.gameObject.SetActive(true); // turn buttons on
        choiceTwoBouton.gameObject.SetActive(true);
        choiceThreeButton.gameObject.SetActive(true);
        choiceFourButton.gameObject.SetActive(true);
        choiceFiveButton.gameObject.SetActive(true);
    }

    // 6 button variation
    public void ChoiceSix(string question, UnityAction choiceOneEvent, UnityAction choiceTwoEvent, UnityAction choiceThreeEvent, UnityAction choiceFourEvent, UnityAction choiceFiveEvent, UnityAction choiceSixEvent)
    {
        modalPanelObject.SetActive(true);

        choiceOneButton.onClick.RemoveAllListeners(); // we might be listening to something, so we dont call something from last time
        choiceOneButton.onClick.AddListener(choiceOneEvent);
        choiceOneButton.onClick.AddListener(ClosePanel);

        choiceTwoBouton.onClick.RemoveAllListeners();
        choiceTwoBouton.onClick.AddListener(choiceTwoEvent);
        choiceTwoBouton.onClick.AddListener(ClosePanel);

        choiceThreeButton.onClick.RemoveAllListeners();
        choiceThreeButton.onClick.AddListener(choiceThreeEvent);
        choiceThreeButton.onClick.AddListener(ClosePanel);

        choiceFourButton.onClick.RemoveAllListeners();
        choiceFourButton.onClick.AddListener(choiceFourEvent);
        choiceFourButton.onClick.AddListener(ClosePanel);

        choiceFiveButton.onClick.RemoveAllListeners();
        choiceFiveButton.onClick.AddListener(choiceFiveEvent);
        choiceFiveButton.onClick.AddListener(ClosePanel);

        choiceSixButton.onClick.RemoveAllListeners();
        choiceSixButton.onClick.AddListener(choiceSixEvent);
        choiceSixButton.onClick.AddListener(ClosePanel);

        this.question.text = question; // set question

        //this.iconImage.gameObject.SetActive(false); // turn icon off

        choiceOneButton.gameObject.SetActive(true); // turn buttons on
        choiceTwoBouton.gameObject.SetActive(true);
        choiceThreeButton.gameObject.SetActive(true);
        choiceFourButton.gameObject.SetActive(true);
        choiceFiveButton.gameObject.SetActive(true);
        choiceSixButton.gameObject.SetActive(true);
    }

    // 1 Button special
    public void ChoiceOne(string question, string answerOne, UnityAction choiceOneEvent)
    {
        modalPanelObject.SetActive(true); // turn modal panel ON

        choiceOneButton.onClick.RemoveAllListeners(); // we might be listening to something, so we dont call something from last time
        choiceOneButton.onClick.AddListener(choiceOneEvent);
        //choiceOneButton.onClick.AddListener(ClosePanel);

        this.question.text = question; // set question
        this.answerOne.text = answerOne; // set answers

        //this.iconImage.gameObject.SetActive(false); // turn icon off

        choiceOneButton.gameObject.SetActive(true); // turn buttons on
    }

    // 4 button default
    public void ChoiceTwo(string question, string answerOne, string answerTwo, UnityAction choiceOneEvent, UnityAction choiceTwoEvent)
    { 
        modalPanelObject.SetActive(true); // turn modal panel ON

        choiceOneButton.onClick.RemoveAllListeners(); // we might be listening to something, so we dont call something from last time
        choiceOneButton.onClick.AddListener(choiceOneEvent);
        //choiceOneButton.onClick.AddListener(ClosePanel);

        choiceTwoBouton.onClick.RemoveAllListeners();
        choiceTwoBouton.onClick.AddListener(choiceTwoEvent);
        //choiceTwoBouton.onClick.AddListener(ClosePanel);

        this.question.text = question; // set question
        this.answerOne.text = answerOne; // set answers
        this.answerTwo.text = answerTwo;

        //this.iconImage.gameObject.SetActive(false); // turn icon off

        choiceOneButton.gameObject.SetActive(true); // turn buttons on
        choiceTwoBouton.gameObject.SetActive(true);
    }

    public void TurnButtonsOff()
    {
        choiceOneButton.gameObject.SetActive(false); // turn all buttons off
        choiceTwoBouton.gameObject.SetActive(false);
        choiceThreeButton.gameObject.SetActive(false);
        choiceFourButton.gameObject.SetActive(false);
        choiceFiveButton.gameObject.SetActive(false);
        choiceSixButton.gameObject.SetActive(false);
    }

    public void ClosePanel()
    {
        modalPanelObject.SetActive(false);
    }







   

    // TESTING - THIS IS INCOMPLETE / MIGHT BE BROKEN
    public void Choice(string question, string answerOne, string answerTwo, string answerThree, string answerFour, 
        UnityAction answerOneAction, UnityAction answerTwoAction, UnityAction answerThreeAction, UnityAction answerFourAction)
    {
        modalPanelObject.SetActive(true); // turn modal panel ON

        choiceOneButton.gameObject.SetActive(false); // turn buttons off
        choiceTwoBouton.gameObject.SetActive(false);
        choiceThreeButton.gameObject.SetActive(false);
        choiceFourButton.gameObject.SetActive(false);

        this.question.text = question; // set question

        this.answerOne.text = answerOne; // set answers
        this.answerTwo.text = answerTwo;
        this.answerThree.text = answerThree;
        this.answerFour.text = answerFour;

        if (answerOne != null)
        {
            choiceOneButton.onClick.RemoveAllListeners(); // set actions
            choiceOneButton.onClick.AddListener(answerOneAction);
            choiceOneButton.gameObject.SetActive(true); // turn buttons on
        }

        if (answerTwo != null)
        {
            choiceTwoBouton.onClick.RemoveAllListeners();
            choiceTwoBouton.onClick.AddListener(answerTwoAction);
            choiceTwoBouton.gameObject.SetActive(true);
        }

        if (answerThree != null)
        {
            choiceThreeButton.onClick.RemoveAllListeners();
            choiceThreeButton.onClick.AddListener(answerThreeAction);
            choiceThreeButton.gameObject.SetActive(true);
        }

        if (answerFour != null)
        {
            choiceFourButton.onClick.RemoveAllListeners();
            choiceFourButton.onClick.AddListener(answerFourAction);
            choiceFourButton.gameObject.SetActive(true);
        }
    }




    }
