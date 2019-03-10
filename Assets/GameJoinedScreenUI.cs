using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameJoinedScreenUI : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI textPlayer;

    protected void Update()
    {
        GameService gameService = GlobalServiceLocator.Instance.Get<GameService>();
        
        string playerList = string.Join("\n", gameService.CurrentGame.Players.Select(x => x.Nickname));

        textPlayer.text = playerList;
    }
}
