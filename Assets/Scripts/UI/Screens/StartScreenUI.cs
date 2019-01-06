using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreenUI : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_InputField input;
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;

    protected void Awake()
    {
        startButton.onClick.AddListener(OnStartButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
    }
    
    private void OnStartButtonClicked()
    {
        if(input.text.Length > 0)
        {
            GlobalServiceLocator.Instance.Get<PlayerService>().Nickname = input.text;
        }

        FindObjectOfType<SImpleMainFlow>().SwitchScreen(ScreenStates.Connecting);
    }

    private void OnQuitButtonClicked()
    {
        Application.Quit();
    }
}
