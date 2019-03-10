using Message;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GameService : IService
{
    private ClientService clientService;
    private PlayerService playerService;
    private ProtoMessageCallbackService protoMessageCallbackService;

    public Game CurrentGame { get; private set; }

    public GameService(PlayerService playerService, ClientService clientService, ProtoMessageCallbackService protoMessageCallbackService)
    {
        this.playerService = playerService;
        this.clientService = clientService;
        this.protoMessageCallbackService = protoMessageCallbackService;

        this.protoMessageCallbackService.Subscribe<GameJoined>(OnGameJoined);
        this.protoMessageCallbackService.Subscribe<PlayerJoined>(OnPlayerJoined);
        this.protoMessageCallbackService.Subscribe<PlayerLeft>(OnPlayerLeft);
    }

    public class Game
    {
        public IReadOnlyList<Player> Players => players;
        private List<Player> players;

        public Game(IEnumerable<Player> players)
        {
            this.players = new List<Player>(players);
        }

        public void AddPlayer(Player player)
        {
            if(players.Any(x => x.Guid == player.Guid))
            {
                Debug.Log("Help I am already in this game? Handle this correctly for the player.");
                return;
            }

            players.Add(player);
        }

        public void RemovePlayer(Player player)
        {
            var indexOf = players.FindIndex(x => x.Guid == player.Guid);
            if(indexOf > -1)
            {
                players.RemoveAt(indexOf);
            }
        }
    }

    public void FindGame()
    {
        JoinGame joinGame = new JoinGame();
        joinGame.Player = playerService.LocalPlayer;

        clientService.WriteAsync(joinGame);
    }
    
    private void OnPlayerJoined(PlayerJoined playerJoined)
    {
        Debug.Log("OnPlayerJoined: " + playerJoined.Guid);

        CurrentGame.AddPlayer(playerJoined.Player);
    }

    private void OnPlayerLeft(PlayerLeft playerLeft)
    {
        Debug.Log("OnPlayerLeft: " + playerLeft.Guid);

        CurrentGame.RemovePlayer(playerLeft.Player);
    }

    private void OnGameJoined(GameJoined gameJoined)
    {
        Debug.Log("OnGameJoined: " + gameJoined.GUID);
        CurrentGame = new Game(gameJoined.Players);

        foreach (var player in gameJoined.Players)
        {
            Debug.Log($"OnGameJoined Player {player.Guid}: {player.Nickname}");
        }
    }
}
