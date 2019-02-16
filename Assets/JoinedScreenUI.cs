using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinedScreenUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Button findGameButton;

    protected void Start()
    {
        PlayerService playerService = GlobalServiceLocator.Instance.Get<PlayerService>();

        text.text = $"Connected to: {playerService.LocalPlayer.Guid} + {playerService.LocalPlayer.Nickname}\n";

        findGameButton.onClick.AddListener(OnFindGameButtonClicked);
    }

    private void OnFindGameButtonClicked()
    {
        GlobalServiceLocator.Instance.Get<GameService>().FindGame();
    }
}
