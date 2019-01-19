using Model;
using Google.Protobuf.Collections;
using System.Collections.Generic;
using System.Linq;

public class QuestionService : IService
{
    private List<Question> questions = new List<Question>();

    public QuestionService()
    {
        RepeatedField<Answer> repeatedField = new RepeatedField<Answer>();

        Answer answer1 = new Answer { Guid = "1", Text = "Answer 1" };
        Answer answer2 = new Answer { Guid = "2", Text = "Answer 2" };
        Answer answer3 = new Answer { Guid = "3", Text = "Answer 3" };
        Answer answer4 = new Answer { Guid = "4", Text = "Answer 4" };

        Question question = new Question();
        question.Guid = "Question 1";

        question.Answers.Add(answer1);
        question.Answers.Add(answer2);
        question.Answers.Add(answer3);
        question.Answers.Add(answer4);

        questions.Add(question);
    }

    public Question GetCurrentQuestion()
    {
        return questions.FirstOrDefault();
    }
}
