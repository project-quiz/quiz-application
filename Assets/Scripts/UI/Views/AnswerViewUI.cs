using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswerViewUI : MonoBehaviour
{
    public delegate void AnswerClickedHandler(string guid);
    public event AnswerClickedHandler AnswerClickedEvent;

    [SerializeField] protected Button button;
    [SerializeField] protected TextMeshProUGUI text;

    private string guid;

    public void Initialize(Data.Question.Answer answer)
    {
        guid = answer.Guid;
        text.text = answer.Text;
        button.onClick.AddListener(OnButtonClicked);
    }

    protected void OnDestroy()
    {
        button.onClick.RemoveListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        AnswerClickedEvent.Invoke(guid);
    }
}
