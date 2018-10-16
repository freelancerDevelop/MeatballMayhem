using TwitchLib.Client.Models;
using TwitchLib.Unity;
using UnityEngine;

public class TwitchClient : MonoBehaviour
{
    //public static TwitchClient Instance;
    private Client _client;
    public PlayerManager pm;
    public RoundManager rm;
    public AnnounceWinner aw;
    public Aim aim;
    public Bomb bomb;
    public Fired fired;
    public bool debugMode = false;
    public GameObject Explosive;
    public Camera MainCamera;

    private void Start()
    {
        // Make the application run in the background so we don't disconnect from Twitch.
        Application.runInBackground = true;

        string _channelToConnectTo = PlayerPrefs.GetString("CHANNEL_NAME"); //SetSecrets.Instance.USERNAME_FROM_OAUTH_TOKEN;

        // Create Credentials instance
        ConnectionCredentials credentials = new ConnectionCredentials(PlayerPrefs.GetString("CHANNEL_NAME"), PlayerPrefs.GetString("OAUTH_TOKEN"));//SetSecrets.Instance.USERNAME_FROM_OAUTH_TOKEN, SetSecrets.Instance.OAUTH_TOKEN

        // Create new instance of Chat Client
        _client = new Client();

        // Initialize the client with the credentials instance, and setting a default channel to connect to.
        _client.Initialize(credentials, _channelToConnectTo);

        // Bind callbacks to events
        _client.OnConnected += OnConnected;
        _client.OnJoinedChannel += OnJoinedChannel;
        _client.OnMessageReceived += OnMessageReceived;
        //_client.OnChatCommandReceived += OnChatCommandReceived;
        _client.OnWhisperCommandReceived += OnWhisperCommandReceived;

        // Connect
        _client.Connect();
    }

    private void Update()
    {
        //Check to see if the game is joinable.
        StartCoroutine(pm.CheckForMaxPlayers(pm.atMaxPlayers));
    }

    private void OnConnected(object sender, TwitchLib.Client.Events.OnConnectedArgs e)
    {
        Debug.Log($"The bot {e.BotUsername} succesfully connected to Twitch.");

        if (!string.IsNullOrWhiteSpace(e.AutoJoinChannel))
            Debug.Log($"The bot will now attempt to automatically join the channel provided when the Initialize method was called: {e.AutoJoinChannel}");
    }

    private void OnJoinedChannel(object sender, TwitchLib.Client.Events.OnJoinedChannelArgs e)
    {
        Debug.Log($"The bot {e.BotUsername} just joined the channel: {e.Channel}");
        _client.SendMessage(e.Channel, $"It's time for Meatball Mayhem! PogChamp /w meatballmayhem !join to get your own Meatball!");
    }

    private void OnMessageReceived(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
    {
        Debug.Log($"Message received from {e.ChatMessage.Username}: {e.ChatMessage.Message}");
    }

    private void OnWhisperCommandReceived(object sender, TwitchLib.Client.Events.OnWhisperCommandReceivedArgs e)
    {
        string name = e.Command.WhisperMessage.Username;

        switch (e.Command.CommandText)
        {
            case "hello":
                _client.SendMessage(e.Command.WhisperMessage.DisplayName, $"Whale hello there, {e.Command.WhisperMessage.DisplayName}!");
                break;

            case "about":
                _client.SendMessage(e.Command.WhisperMessage.DisplayName, $"Running Meatball Mayhem v0.9a! Whisper !commands to view the game commands or download the app!");
                break;

            default:
                _client.SendMessage(e.Command.WhisperMessage.DisplayName, $"Unknown chat command: {e.Command.CommandIdentifier}{e.Command.CommandText}");
                break;

            case "commands":
                _client.SendMessage(e.Command.WhisperMessage.DisplayName, $"Commands: !hello, !about, !join, !moveleft, !moveright, !jumpleft, !jumpright, !aim, !fire");
                break;

            case "join":

                Debug.Log("Checking for max players...");

                if (debugMode == false && pm.playerList.Contains(name))
                {
                    _client.SendMessage(e.Command.WhisperMessage.DisplayName, $"You've already joined and can't join again!");
                    break;
                }

                if (pm.atMaxPlayers)
                {
                    _client.SendMessage(e.Command.WhisperMessage.DisplayName, $"Game is now full! Try joining after the game is over!");
                    Debug.Log("A chatter tried to join the game but game is full...");
                    break;
                }
                _client.SendMessage(e.Command.WhisperMessage.DisplayName, $"Joining the game!");
                AddNewPlayer(name);
                AddToFired(gameObject);

                pm.playerName = name;
                pm.SpawnPlayer();
                AddToAlive(name);

                Debug.Log("Spawning " + name + "!");

                break;
            
            // GAME COMMANDS
            case "movel":
            case "mover":

                // If we get a command in chat to move a Meatball player left, do a check to see if that message username 
                // matches a prefab name. Otherwise, ignore it
                if (GameObject.Find(name))
                {
                    // Check if the player is alive first
                    bool isPlayerAlive = GameObject.Find(name).GetComponentInChildren<Health>().alive;
                    // First we check if we're in the move round
                    if (rm.phase == "Move" && isPlayerAlive)
                    {
                        Rigidbody2D storedRB = GameObject.Find(name).GetComponentInChildren<Rigidbody2D>();

                        string replace = e.Command.CommandText == "movel" ? "!movel " : "!mover ";
                        string forceValue = e.Command.WhisperMessage.Message;

                        forceValue = forceValue.Replace(replace, "");
                        int forceValueInt;
                        int.TryParse(forceValue, out forceValueInt);

                        if (forceValueInt > 0 && forceValueInt <= 5)
                        {
                            if (e.Command.CommandText == "movel")
                            {
                                storedRB.AddForce(Vector2.left * forceValueInt, ForceMode2D.Impulse);
                                _client.SendWhisper(e.Command.WhisperMessage.DisplayName, $"Moving you left...");
                            }
                            else
                            {
                                _client.SendWhisper(e.Command.WhisperMessage.DisplayName, $"Moving you right");
                                storedRB.AddForce(Vector2.right * forceValueInt, ForceMode2D.Impulse);
                            }
                        }
                        else
                        {
                            // The value specified was either too low or too high. Ignore the command.
                        }
                    }
                    else
                    {
                        // Do nothing, because we're in the attack round.
                        _client.SendMessage(e.Command.WhisperMessage.DisplayName, $"You can't do this in the attack round.");
                    }
                }
                else
                {
                    // The person typing the command is either dead or isn't a current player.
                }
                break;

            case "jumpl":
            case "jumpr":

                if (GameObject.Find(name))
                {
                    // Check if the player is alive first
                    bool isPlayerAlive = GameObject.Find(name).GetComponentInChildren<Health>().alive;
                    //First we check if we're in the move round.
                    if (rm.phase == "Move" && isPlayerAlive)
                    {
                        Rigidbody2D storedRB = GameObject.Find(name).GetComponentInChildren<Rigidbody2D>();

                        string replace = e.Command.CommandText == "jumpl" ? "!jumpl " : "!jumpr ";
                        string forceValue = e.Command.WhisperMessage.Message;

                        forceValue = forceValue.Replace(replace, "");
                        int forceValueInt;
                        int.TryParse(forceValue, out forceValueInt);

                        if (forceValueInt > 0 && forceValueInt <= 5)
                        {
                            if (e.Command.CommandText == "jumpl")
                            {
                                _client.SendWhisper(e.Command.WhisperMessage.DisplayName, $"Jumping you left...");
                                storedRB.AddForce(Vector2.up * forceValueInt, ForceMode2D.Impulse);
                                storedRB.AddForce(Vector2.left * forceValueInt, ForceMode2D.Impulse);
                            }
                            else
                            {
                                _client.SendWhisper(e.Command.WhisperMessage.DisplayName, $"Jumping you right...");
                                storedRB.AddForce(Vector2.up * forceValueInt, ForceMode2D.Impulse);
                                storedRB.AddForce(Vector2.right * forceValueInt, ForceMode2D.Impulse);
                            }

                        }
                        else
                        {
                            // The value specified was either too low or too high. Ignore the command.
                            _client.SendWhisper(e.Command.WhisperMessage.DisplayName, $"Did you mistype a command?");
                        }
                    }
                    else
                    {
                        // Do nothing, because we're in the attack round
                        _client.SendWhisper(e.Command.WhisperMessage.DisplayName, $"You can't do this in the attack round.");
                    }
                }
                else
                {
                    // Do nothing because the player is dead or this whisper isn't from a current player
                }
                break;

            case "aim":

                if (GameObject.Find(name))
                {
                    bool isPlayerAlive = GameObject.Find(name).GetComponent<Health>().alive;

                    // First we check if we're in the attack round.
                    if (rm.phase == "Attack" && isPlayerAlive)
                    {
                        GameObject playerGO = GameObject.Find(name);
                        string aimAngle = e.Command.WhisperMessage.Message;
                        aimAngle = aimAngle.Replace("!aim ", "");
                        int aimAngleInt;
                        int.TryParse(aimAngle, out aimAngleInt);

                        // Do a check to see if the angle is within physics boundaries. If not, break.
                        if (aimAngleInt >= 0 && aimAngleInt <= 360)
                        {
                            // If the angle is acceptable, set the aim angle for the player.
                            // Get the player's instantiated prefab, and set angle on the attached aim script.
                            _client.SendWhisper(e.Command.WhisperMessage.DisplayName, $"Aiming...");
                            playerGO.GetComponent<Aim>().SetAimToAngle(aimAngleInt);
                        }
                        else
                        {
                            // The value specified was either too low or too high. Ignore the command.
                            _client.SendWhisper(e.Command.WhisperMessage.DisplayName, $"Did you mistype a command?");
                        }
                    }
                    else
                    {
                        // Do nothing, because we're in the move round.
                        _client.SendWhisper(e.Command.WhisperMessage.DisplayName, $"You can't aim in the move round.");
                    }
                }
                break;

            case "fire":

                if (GameObject.Find(name))
                {
                    bool isPlayerAlive = GameObject.Find(name).GetComponent<Health>().alive;
                    if (rm.phase == "Attack" && isPlayerAlive)
                    {
                        string forceValue = e.Command.WhisperMessage.Message;
                        forceValue = forceValue.Replace("!fire ", "");
                        int forceValueInt;
                        int.TryParse(forceValue, out forceValueInt);

                        // Check the player's gameobject script called Fired to see if they fired already.

                        // THIS IS CAUSING A BUG FOR PLAYERS! THEY CAN'T FIRE IF SOMEONE ELSE HAS FIRED ALREADY!!!
                        // var playerFiredScript = GameObject.Find(name).GetComponent<Fired>();
                        // Try it this way instead...
                        bool playerFiredBool = GameObject.Find(name).GetComponent<Fired>().fired;

                        // Do a check to see if the player already fired
                        if (playerFiredBool == false)
                        {
                            // Do a check to see if the value is within reasonable bounds. If not, break.
                            if (forceValueInt > 0 && forceValueInt <= 5)
                            {
                                _client.SendWhisper(e.Command.WhisperMessage.DisplayName, $"Firing!");
                                // Grab the Bomb component from the player's prefab
                                bomb = GameObject.Find(name).GetComponentInChildren<Bomb>();
                                // Fire the explosive at the forceValue specified
                                bomb.ThrowBomb(forceValueInt);

                                // Set the fired bool to true so they can't fire again until the next round".
                                playerFiredBool = true;
                            }
                            else
                            {
                                // If the user made a typo we should ignore the command and break
                            }
                        }
                        else if (playerFiredBool == true)
                        {
                            // Player already fired. Ignore the command.
                            Debug.Log("Player already fired!");
                        }

                    }
                    else
                    {
                        // Do nothing, because we're in the move round or the player already fired.
                    }
                }
                break;
        }
    }

    private void AddNewPlayer(string playerToAdd)
    {
        pm.playerList.Add(playerToAdd);
        pm.currentPlayers++;
    }

    private void AddToFired(GameObject gameObject)
    {
        rm.playerList.Add(gameObject);
    }

    private void AddToAlive(string player)
    {
        aw.playersAlive.Add(player);
    }
}