using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JoinedScreenUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    protected void Start()
    {
        PlayerService playerService = GlobalServiceLocator.Instance.Get<PlayerService>();

        text.text = $"Connected to: {playerService.LocalPlayer.Guid} + {playerService.LocalPlayer.Nickname}\n";
    }
}
