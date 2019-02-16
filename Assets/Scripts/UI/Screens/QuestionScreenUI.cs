using Message;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestionScreenUI : MonoBehaviour
{
    [Header("Question")]
    [SerializeField] protected TextMeshProUGUI questionText;
    [Header("Answers")]
    [SerializeField] protected RectTransform answerParent;
    [SerializeField] protected AnswerViewUI answerPrefeb;

    private List<AnswerViewUI> answers = new List<AnswerViewUI>();

    protected void OnEnable()
    {
        QuestionService questionService = GlobalServiceLocator.Instance.Get<QuestionService>();

        Question question = questionService.GetCurrentQuestion();
        questionText.text = question.Guid;

        foreach (var item in question.Answers)
        {
            AnswerViewUI instance = Instantiate(answerPrefeb, answerParent);
            instance.Initialize(item);
            instance.AnswerClickedEvent += OnAnswerClicked;
            answers.Add(instance);
        }
    }

    protected void OnDisable()
    {
        for (int i = 0; i < answers.Count; i++)
        {
            Destroy(answers[i].gameObject);
        }

        answers.Clear();
    }

    protected void OnAnswerClicked(string guid)
    {
        Debug.Log($"Sent message to server that guid: '{guid}' was pressed");
    }
}