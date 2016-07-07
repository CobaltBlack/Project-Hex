using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.Events;

// Example of creating an instance
public class ExampleInstance : Instance
{
    public override string instanceName
    {
        get { return "Example Instance"; }
    }

    public override Prompt instancePrompt
    {
        // IntroPrompt() is a prompt defined below
        get { return new IntroPrompt(); }
    }

    public override InstanceType instanceType
    {
        get { return InstanceType.DIALOGUE; }
    }

    // Defining the first prompt
    private class IntroPrompt : Prompt
    {
        public override string promptText
        {
            get
            {
                return "Has anyone really been far as decided to use even go want to do look more like?";
            }
        }

        public override List<PromptAnswer> promptAnswers
        {
            get
            {
                // Define answers below (within the same prompt class to avoid naming conflicts)
                List<PromptAnswer> answers = new List<PromptAnswer>();
                answers.Add(new Answer_1());
                answers.Add(new Answer_2());
                answers.Add(new Answer_3());
                answers.Add(new Answer_4());
                return answers;
            }
        }

        // Defining a PromptAnswer
        class Answer_1 : PromptAnswer
        {
            public override string answerText
            {
                get
                {
                    return "(Go to next prompt) You've got to be kidding me. I've been further even more decided to use even go need to do look more as anyone can.";
                }
            }

            // This action will be performed when clicked.
            // You can use it to start another prompt.
            public override void answerAction()
            {
                Debug.Log("Omg this did something ### 1");
                OpenPrompt(new Prompt2());
            }

            public override int requiredItemId
            {
                get
                {
                    return -1;
                }
            }
        }

        class Answer_2 : PromptAnswer
        {
            public override string answerText
            {
                get
                {
                    return "Can you really be far even as decided half as much to use go wish for that?";
                }
            }

            public override int requiredItemId
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public override void answerAction()
            {
                Debug.Log("Omg this did something ### 2");
                ClosePrompt();
            }
        }

        class Answer_3 : PromptAnswer
        {
            public override string answerText
            {
                get
                {
                    return "My guess is that when one really been far even as decided once to use even go want, it is then that he has really been far even as decided to use even go want to do look more like.";
                }
            }

            public override int requiredItemId
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public override void answerAction()
            {
                Debug.Log("Omg this did something ### 3");
                ClosePrompt();
            }
        }

        class Answer_4 : PromptAnswer
        {
            public override string answerText
            {
                get
                {
                    return "It's just common sense.";
                }
            }

            public override int requiredItemId
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public override void answerAction()
            {
                Debug.Log("Omg this did something ### 4");
                ClosePrompt();
            }
        }
    }

    private class Prompt2 : Prompt
    {
        public override List<PromptAnswer> promptAnswers
        {
            get
            {
                List<PromptAnswer> answers = new List<PromptAnswer>();
                answers.Add(new Answer1());
                answers.Add(new Answer2());
                answers.Add(new Answer3());
                answers.Add(new Answer4());
                return answers;
            }
        }

        public override string promptText
        {
            get
            {
                return "What the fuck did you just fucking say about me, you little bitch?";
            }
        }

        class Answer1 : PromptAnswer
        {
            public override string answerText
            {
                get
                {
                    return "I’ll have you know I graduated top of my class in the Navy Seals";
                }
            }

            public override int requiredItemId
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public override void answerAction()
            {
                ClosePrompt();
            }
        }

        class Answer2 : PromptAnswer
        {
            public override string answerText
            {
                get
                {
                    return "I’ve been involved in numerous secret raids on Al-Quaeda";
                }
            }

            public override int requiredItemId
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public override void answerAction()
            {
                ClosePrompt();
            }
        }

        class Answer3 : PromptAnswer
        {
            public override string answerText
            {
                get
                {
                    return "I have over 300 confirmed kills";
                }
            }

            public override int requiredItemId
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public override void answerAction()
            {
                ClosePrompt();
            }
        }
        class Answer4 : PromptAnswer
        {
            public override string answerText
            {
                get
                {
                    return "I am trained in gorilla warfare";
                }
            }

            public override int requiredItemId
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public override void answerAction()
            {
                ClosePrompt();
            }
        }
    }
}
