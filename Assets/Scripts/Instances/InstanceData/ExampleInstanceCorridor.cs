using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.Events;

public class ExampleInstanceCorridor : Instance
{
    protected override string GetInstanceName()
    {
        return "Example Instance CORRIDOR / ROOM";
    }

    protected override string GetInstanceDescription()
    {
        return "Example Instance CORRIDOR / ROOM description blah blha";
    }

    protected override Prompt GetInitialPrompt()
    {
        return new IntroPrompt();
    }

    protected override InstanceType GetInstanceType()
    {
        return InstanceType.Dialogue;
    }

    // ========================= TEST =========================
    protected override List<Tileset> GetInstanceTileset()
    {
        return new List<Tileset> { Tileset.Building };
    }

    protected override List<Layout> GetInstanceLayout()
    {
        return new List<Layout> { Layout.Corridor, Layout.Room };
    }
    // ========================= TEST =========================
}

// Define the first prompt
class IntroPrompt : Prompt
{

    protected override string GetPromptText()
    {
        return "You find what appears to be a shape of a man on the ground, leaning against the wall. The corridor is dimly lit, and you cannot make out the face.";
    }

    protected override List<PromptAnswer> GetPromptAnswers()
    {
        // Define answers below (within the same prompt class to avoid naming conflicts)
        List<PromptAnswer> answers = new List<PromptAnswer>();
        answers.Add(new Answer_1());
        answers.Add(new Answer_2());
        answers.Add(new Answer_3());
        return answers;
    }

    // Define a PromptAnswer
    class Answer_1 : PromptAnswer
    {
        protected override string GetAnswerText()
        {
            return "Approach carefully.";
        }
        protected override void GetAnswerAction()
        {
            Debug.Log("Answer_1 : PromptAnswer");
            ClosePrompt();
        }
    }

    class Answer_2 : PromptAnswer
    {
        protected override string GetAnswerText()
        {
            return "Alive or dead, there is no time to lose over such petty curiousity";
        }

        protected override void GetAnswerAction()
        {
            Debug.Log("Answer_2 : PromptAnswer");
            ClosePrompt();
        }
    }

    class Answer_3 : PromptAnswer
    {
        protected override string GetAnswerText()
        {
            return "Light your torch.";
        }

        protected override void GetAnswerAction()
        {
            Debug.Log("Answer_3 : PromptAnswer");
            ClosePrompt();
        }
    }
}
