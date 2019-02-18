using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinedScreenUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Button findGameButton;

    private PlayerService playerService;

    protected void Start()
    {
        playerService = GlobalServiceLocator.Instance.Get<PlayerService>();

        text.text = $"Connected to: {playerService.LocalPlayer.Guid}\n";

        nameInputField.onValueChanged.AddListener(OnNameInputChanged);
        findGameButton.onClick.AddListener(OnFindGameButtonClicked);
    }

    private void OnNameInputChanged(string name)
    {
        playerService.UpdateNickName(name);
    }

    private void OnFindGameButtonClicked()
    {
        GlobalServiceLocator.Instance.Get<GameService>().FindGame();
    }
}
