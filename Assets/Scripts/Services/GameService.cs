using Message;
using UnityEngine;

public class GameService : IService
{
    private ClientService clientService;
    private PlayerService playerService;
    private ProtoMessageCallbackService protoMessageCallbackService;

    public GameService(PlayerService playerService, ClientService clientService, ProtoMessageCallbackService protoMessageCallbackService)
    {
        this.playerService = playerService;
        this.clientService = clientService;
        this.protoMessageCallbackService = protoMessageCallbackService;

        this.protoMessageCallbackService.Subscribe<GameJoined>(OnGameJoined);
    }

    public class Game
    {

    }

    public void FindGame()
    {
        JoinGame joinGame = new JoinGame();
        joinGame.Player = playerService.LocalPlayer;

        clientService.WriteAsync(joinGame);
    }

    private void OnGameJoined(GameJoined gameJoined)
    {
        Debug.Log("HERE");
    }
}
