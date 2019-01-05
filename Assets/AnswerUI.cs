using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswerUI : MonoBehaviour
{
    public delegate void AnswerClickedHandler(int id);
    public event AnswerClickedHandler AnswerClickedEvent;

    [SerializeField] protected Button button;
    [SerializeField] protected TextMeshProUGUI text;

    private int id;

    public void Initialize(Data.Question.Answer answer)
    {
        id = answer.Id;
        text.text = answer.Text;
        button.onClick.AddListener(OnButtonClicked);
    }

    protected void OnDestroy()
    {
        button.onClick.RemoveListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        AnswerClickedEvent.Invoke(id);
    }
}
