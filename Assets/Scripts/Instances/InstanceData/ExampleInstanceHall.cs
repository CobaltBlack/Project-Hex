using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.Events;

public class ExampleInstanceHall : Instance
{
    protected override string GetInstanceName()
    {
        return "Example Instance HALL";
    }

    protected override string GetInstanceDescription()
    {
        return "Example Instance HALL description ehre blah blahblahblahblahblahblahblahblahblahblah";
    }

    protected override Prompt GetInitialPrompt()
    {
        return new IntroPrompt();
    }

    protected override InstanceType GetInstanceType()
    {
        return InstanceType.Dialogue;
    }

    protected override List<Tileset> GetInstanceTileset()
    {
        return new List<Tileset> { Tileset.Building };
    }

    protected override List<Layout> GetInstanceLayout()
    {
        return new List<Layout> { Layout.Hall };
    }

    // Define the first prompt
    class IntroPrompt : Prompt
    {

        protected override string GetPromptText()
        {
            return "You find yourself in an open atrium - you could almost hear the faint laughter and music which once filled the hall.";
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
                return "Investigate the hall.";
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
                return "Quickly leave.";
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
                return "Use skill: Illumination.";
            }

            protected override void GetAnswerAction()
            {
                Debug.Log("Answer_3 : PromptAnswer");
                ClosePrompt();
            }
        }
    }
}
