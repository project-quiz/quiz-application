using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectingScreenUI : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI text;

    private ClientService clientService;

    protected IEnumerator Start()
    {
        text.text = "Connecting to server...";
        yield return new WaitForSeconds(0.5f);

        clientService = GlobalServiceLocator.Instance.Get<ClientService>();

        clientService.ConnectAndListen();
    }
}

