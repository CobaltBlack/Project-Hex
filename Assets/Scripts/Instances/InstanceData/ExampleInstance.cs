using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.Events;

// Example of creating an instance
public class ExampleInstance : Instance
{
    protected override string GetInstanceName()
    {
        return "Example Instance";
    }

    protected override Prompt GetInitialPrompt()
    {
        return new IntroPrompt();
    }

    protected override InstanceType GetInstanceType()
    {
        return InstanceType.Dialogue;
    }

    // Define the first prompt
    class IntroPrompt : Prompt
    {

        protected override string GetPromptText()
        {
            return "Has anyone really been far as decided to use even go want to do look more like?";
        }

        protected override List<PromptAnswer> GetPromptAnswers()
        {
            // Define answers below (within the same prompt class to avoid naming conflicts)
            List<PromptAnswer> answers = new List<PromptAnswer>();
            answers.Add(new Answer_1());
            answers.Add(new Answer_2());
            answers.Add(new Answer_3());
            answers.Add(new Answer_4());
            return answers;
        }

        // Define a PromptAnswer
        class Answer_1 : PromptAnswer
        {
            // This action will be performed when clicked.
            // You can use it to start another prompt.
            protected override void GetAnswerAction()
            {
                Debug.Log("Omg this did something ### 1");
                OpenPrompt(new Prompt2());
            }

            protected override string GetAnswerText()
            {
                return "(Go to next prompt) You've got to be kidding me. I've been further even more decided to use even go need to do look more as anyone can.";
            }
        }

        class Answer_2 : PromptAnswer
        {
            protected override string GetAnswerText()
            {
                return "Can you really be far even as decided half as much to use go wish for that?";
            }

            protected override void GetAnswerAction()
            {
                Debug.Log("Omg this did something ### 2");
                ClosePrompt();
            }
        }

        class Answer_3 : PromptAnswer
        {
            protected override string GetAnswerText()
            {
                return "My guess is that when one really been far even as decided once to use even go want, it is then that he has really been far even as decided to use even go want to do look more like.";
            }

            protected override void GetAnswerAction()
            {
                Debug.Log("Omg this did something ### 3");
                ClosePrompt();
            }
        }

        class Answer_4 : PromptAnswer
        {
            protected override string GetAnswerText()
            {
                return "It's just common sense.";
            }

            protected override void GetAnswerAction()
            {
                Debug.Log("Omg this did something ### 4");
                ClosePrompt();
            }
        }
    }

    // Define the second prompt
    class Prompt2 : Prompt
    {

        protected override string GetPromptText()
        {
            return "What the fuck did you just fucking say about me, you little bitch?";
        }

        protected override List<PromptAnswer> GetPromptAnswers()
        {
            List<PromptAnswer> answers = new List<PromptAnswer>();
            answers.Add(new Answer1());
            answers.Add(new Answer2());
            answers.Add(new Answer3());
            answers.Add(new Answer4());
            return answers;
        }

        class Answer1 : PromptAnswer
        {
            protected override string GetAnswerText()
            {
                return "I’ll have you know I graduated top of my class in the Navy Seals";
            }

            protected override void GetAnswerAction()
            {
                ClosePrompt();
            }
        }

        class Answer2 : PromptAnswer
        {
            protected override string GetAnswerText()
            {
                return "I’ve been involved in numerous secret raids on Al-Quaeda";
            }

            protected override void GetAnswerAction()
            {
                ClosePrompt();
            }
        }

        class Answer3 : PromptAnswer
        {
            protected override string GetAnswerText()
            {
                return "I have over 300 confirmed kills";
            }

            protected override void GetAnswerAction()
            {
                ClosePrompt();
            }
        }
        class Answer4 : PromptAnswer
        {
            protected override string GetAnswerText()
            {
                return "I am trained in gorilla warfare";
            }

            protected override void GetAnswerAction()
            {
                ClosePrompt();
            }
        }
    }
}
