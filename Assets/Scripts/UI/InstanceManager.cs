using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class InstanceManager : MonoBehaviour
{

    private ModalPanelInstance modalPanel;
    private DisplayManager displayManager; // this manager is currently used to display visual debug messages, but may later be repurposed for in game announcements.

    void Awake()
    {
        modalPanel = ModalPanelInstance.Instance();
        displayManager = DisplayManager.Instance();
    }

    // ==================== LIST OF INSTANCES ====================

    private string question;

    private string answerOne;
    private string answerTwo;
    private string answerThree;
    private string answerFour;

    private UnityAction answerOneAction;
    private UnityAction answerTwoAction;
    private UnityAction answerThreeAction;
    private UnityAction answerFourAction;

    // actions level 0
    void instanceTestQuest_0_choiceOne()
    {
        instanceTestQuest_1();
    }

    // prompt level 0
    public void instanceTestQuest_0()
    {
        string question = " Welcome to Project Hex prototype dialogue system. Please press Continue to prompt the dialogue tree.";
        string answerOne = " Continue...";
        answerOneAction = new UnityAction(instanceTestQuest_0_choiceOne);

        modalPanel.TurnButtonsOff();
        modalPanel.ChoiceOne(question, answerOne, answerOneAction);

    }

    // actions level 1
    void instanceTestQuest_1_ChoiceOne()
    {
        instanceTestQuest_2a();
    }

    void instanceTestQuest_1_ChoiceTwo()
    {
        instanceTestQuest_2b();
    }

    void instanceTestQuest_1_ChoiceThree()
    {
        modalPanel.ClosePanel();
        displayManager.DisplayMessage("Choice 3 is chosen. No change is made in game in this iteration.");
    }

    void instanceTestQuest_1_ChoiceFour()
    {
        modalPanel.ClosePanel();
    }

    // prompt level 1
    public void instanceTestQuest_1()
    {
        string question = " This should be the second window. Please make your decision.";

        string answerOne = " 1. This choice will take you to another 4 choice window.";
        string answerTwo = " 2. This choice will take you to a 2 choice window.";
        string answerThree = " 3. This choice will exit the dialogue tree, and will make a change to the game.";
        string answerFour = " 4. This choice will exit the dialogue tree.";

        answerOneAction = new UnityAction(instanceTestQuest_1_ChoiceOne);
        answerTwoAction = new UnityAction(instanceTestQuest_1_ChoiceTwo);
        answerThreeAction = new UnityAction(instanceTestQuest_1_ChoiceThree);
        answerFourAction = new UnityAction(instanceTestQuest_1_ChoiceFour);

        modalPanel.TurnButtonsOff();
        modalPanel.ChoiceFour(question, answerOne, answerTwo, answerThree, answerFour, answerOneAction, answerTwoAction, answerThreeAction, answerFourAction);
    }

    // prompt level 2a
    public void instanceTestQuest_2a()
    {
        string question = " This should be the final window. Please make your decision.";

        string answerOne = " 1. This choice will exit the dialogue tree.";
        string answerTwo = " 2. This choice will exit the dialogue tree.";
        string answerThree = " 3. This choice will exit the dialogue tree.";
        string answerFour = " 4. This choice will exit the dialogue tree.";

        answerOneAction = new UnityAction(instanceTestQuest_1_ChoiceFour); // reuses instanceTestQuest_1_ChoiceFour UnityAction for simply exiting dialogue
        answerTwoAction = new UnityAction(instanceTestQuest_1_ChoiceFour);
        answerThreeAction = new UnityAction(instanceTestQuest_1_ChoiceFour);
        answerFourAction = new UnityAction(instanceTestQuest_1_ChoiceFour);

        modalPanel.TurnButtonsOff();
        modalPanel.ChoiceFour(question, answerOne, answerTwo, answerThree, answerFour, answerOneAction, answerTwoAction, answerThreeAction, answerFourAction);
    }

    // prompt level 2b
    public void instanceTestQuest_2b()
    {
        string question = " This should be the final window. Please make your decision.";

        string answerOne = " 1. This choice will exit the dialogue tree.";
        string answerTwo = " 2. This choice will exit the dialogue tree.";

        answerOneAction = new UnityAction(instanceTestQuest_1_ChoiceFour); // reuses instanceTestQuest_1_ChoiceFour UnityAction for simply exiting dialogue
        answerTwoAction = new UnityAction(instanceTestQuest_1_ChoiceFour);

        modalPanel.TurnButtonsOff();
        modalPanel.ChoiceTwo(question, answerOne, answerTwo, answerOneAction, answerTwoAction);
    }










    // CURRENTLY NOT IMPLEMENTED!!!

    class AnswerAction // Button text + Button action pair 
    {
        string answer;
        UnityAction action;

        AnswerAction(string answer, UnityAction action) // Constructor
        {
            this.answer = answer;
            this.action = action;
        }
    }

    class Prompt // single "prompt" of an instance dialogue tree
    {
        string question;

        string answerOne;
        string answerTwo;
        string answerThree;
        string answerFour;

        UnityAction answerOneAction;
        UnityAction answerTwoAction;
        UnityAction answerThreeAction;
        UnityAction answerFourAction;

        Prompt(string qestion, string answerOne, string answerTwo, string answerThree, string answerFour, 
            UnityAction answerOneAction, UnityAction answerTwoAction, UnityAction answerThreeAction, UnityAction answerFourAction) // Constructor
        {
            this.question = question;

            this.answerOne = answerOne;
            this.answerTwo = answerTwo;
            this.answerThree = answerThree;
            this.answerFour = answerFour;

            this.answerOneAction = answerOneAction;
            this.answerTwoAction = answerTwoAction;
            this.answerThreeAction = answerThreeAction;
            this.answerFourAction = answerFourAction;
        }
    }
}
