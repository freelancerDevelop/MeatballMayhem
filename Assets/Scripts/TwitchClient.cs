using TwitchLib.Client.Models;
using TwitchLib.Unity;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TwitchClient : MonoBehaviour
{

    private Client _client;
    public PlayerManager pm;
    public RoundManager rm;
    public Aim aim;
    public Bomb bomb;
    public Fired Fired;
    public bool debugMode = true;

    [SerializeField] //[SerializeField] Allows the private field to show up in Unity's inspector. Way better than just making it public
    private string _channelToConnectTo = Secrets.USERNAME_FROM_OAUTH_TOKEN;

    private void Start()
    {
        // To keep the Unity application active in the background, you can enable "Run In Background" in the player settings:
        // Unity Editor --> Edit --> Project Settings --> Player --> Resolution and Presentation --> Resolution --> Run In Background
        // This option seems to be enabled by default in more recent versions of Unity. An aditional, less recommended option is to set it in code:
        Application.runInBackground = true;

        //Create Credentials instance
        ConnectionCredentials credentials = new ConnectionCredentials(Secrets.USERNAME_FROM_OAUTH_TOKEN, Secrets.OAUTH_TOKEN);

        // Create new instance of Chat Client
        _client = new Client();

        // Initialize the client with the credentials instance, and setting a default channel to connect to.
        _client.Initialize(credentials, _channelToConnectTo);

        // Bind callbacks to events
        _client.OnConnected += OnConnected;
        _client.OnJoinedChannel += OnJoinedChannel;
        _client.OnMessageReceived += OnMessageReceived;
        _client.OnChatCommandReceived += OnChatCommandReceived;

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
        _client.SendMessage(e.Channel, "Successfully connected to Twitch chat! PogChamp");
    }

    private void OnMessageReceived(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
    {
        Debug.Log($"Message received from {e.ChatMessage.Username}: {e.ChatMessage.Message}");
    }

    private void OnChatCommandReceived(object sender, TwitchLib.Client.Events.OnChatCommandReceivedArgs e)
    {
        string name = e.Command.ChatMessage.Username;

        switch (e.Command.CommandText)
        {
            case "hello":
                _client.SendMessage(e.Command.ChatMessage.Channel, $"Whale hello there, {e.Command.ChatMessage.DisplayName}!");
                break;

            case "about":
                _client.SendMessage(e.Command.ChatMessage.Channel, $"Running Meatball Mayhem v0.1a! Type !commands to view the game commands or download the app!");
                break;

            default:
                _client.SendMessage(e.Command.ChatMessage.Channel, $"Unknown chat command: {e.Command.CommandIdentifier}{e.Command.CommandText}");
                break;

            case "commands":
                _client.SendMessage(e.Command.ChatMessage.Channel, $"Commands: !hello, !about, !join, !moveleft, !moveright, !jumpleft, !jumpright, !aim, !fire");
                break;

            case "join":

                _client.SendMessage(e.Command.ChatMessage.Channel, $"Attempting to join the game...");
                Debug.Log("Checking for max players...");

                if (debugMode == false && pm.playerList.Contains(name))
                {
                    AlreadyJoinedMessage();
                    break;
                }

                if (pm.atMaxPlayers)
                {
                    GameFullMessage();
                    Debug.Log("A chatter tried to join the game but game is full...");
                    break;
                }

                AddNewPlayer(name);

                pm.playerName = name;
                pm.SpawnPlayer();

                Debug.Log("Spawning " + name + "!");

                break;


            //GAME COMMANDS
            //-----------------------------------------------------------------------------
            //Once we find the player's name in the player list, we set their Rigidbody2D as
            //as var, process their full command from chat, remove the command text from the
            //string, and parse the remaining text in the string as an int. Then we use the int
            //value to move the stored Rigidbody2D.
            //-----------------------------------------------------------------------------

            case "movel":
            case "mover":

                //If we get a command in chat to move a Meatball player left, do a check to see if that message username 
                //matches a prefab name. Otherwise, ignore it.
                if (GameObject.Find(name))
                {
                    //First we check if we're in the move round.
                    if (rm.phase == "Move")
                    {
                        Rigidbody2D storedRB = GameObject.Find(name).GetComponentInChildren<Rigidbody2D>();

                        string replace = e.Command.CommandText == "movel" ? "!movel " : "!mover ";
                        string forceValue = e.Command.ChatMessage.Message;

                        forceValue = forceValue.Replace(replace, "");
                        int forceValueInt;
                        int.TryParse(forceValue, out forceValueInt);

                        if (forceValueInt > 0 && forceValueInt <= 5)
                        {
                            if (e.Command.CommandText == "movel")
                            {

                                _client.SendMessage(e.Command.ChatMessage.Channel, $"Trying to move you left...");

                                storedRB.AddForce(Vector2.left * forceValueInt, ForceMode2D.Impulse);

                            }
                            else
                            {

                                _client.SendMessage(e.Command.ChatMessage.Channel, $"Trying to move you right...");

                                storedRB.AddForce(Vector2.right * forceValueInt, ForceMode2D.Impulse);

                            }
                        }
                        else
                        {
                            //The value specified was either too low or too high. Ignore the command.
                        }
                    }
                    else
                    {
                        //Do nothing, because we're in the attack round.
                    }
                }
                else
                {
                    //If they made a typo, or didn't put a value in after command, ignore it.
                }
                break;

            case "jumpl":
            case "jumpr":

                //First we check if we're in the move round.
                if (rm.phase == "Move")
                {
                    Rigidbody2D storedRB = GameObject.Find(name).GetComponentInChildren<Rigidbody2D>();

                    string replace = e.Command.CommandText == "jumpl" ? "!jumpl " : "!jumpr ";
                    string forceValue = e.Command.ChatMessage.Message;

                    forceValue = forceValue.Replace(replace, "");
                    int forceValueInt;
                    int.TryParse(forceValue, out forceValueInt);

                    if (forceValueInt > 0 && forceValueInt <= 5)
                    {
                        if (e.Command.CommandText == "jumpl")
                        {

                            _client.SendMessage(e.Command.ChatMessage.Channel, $"Trying to jump you left...");

                            storedRB.AddForce(Vector2.up * forceValueInt, ForceMode2D.Impulse);
                            storedRB.AddForce(Vector2.left * forceValueInt, ForceMode2D.Impulse);

                        }
                        else
                        {

                            _client.SendMessage(e.Command.ChatMessage.Channel, $"Trying to jump you right...");

                            storedRB.AddForce(Vector2.up * forceValueInt, ForceMode2D.Impulse);
                            storedRB.AddForce(Vector2.right * forceValueInt, ForceMode2D.Impulse);
                        }

                    }
                    else
                    {
                        //The value specified was either too low or too high. Ignore the command.
                    }
                }
                else
                {
                    //Do nothing, because we're in the attack round.
                }
                break;

            case "aim":
                _client.SendMessage(e.Command.ChatMessage.Channel, $"Attempting to aim at location...");
                //First we check if we're in the attack round.
                if (rm.phase == "Attack")
                {
                    GameObject playerGO = GameObject.Find(name);
                    string aimAngle = e.Command.ChatMessage.Message;
                    aimAngle = aimAngle.Replace("!aim ", "");
                    int aimAngleInt;
                    int.TryParse(aimAngle, out aimAngleInt);

                    //Do a check to see if the angle is within physics boundaries. If not, break.
                    if (aimAngleInt >= 0 && aimAngleInt <= 360)
                    {
                        //If the angle is acceptable, set the aim angle for the player.
                        //Get the player's instantiated prefab, and set angle on the attached aim script.
                        playerGO.GetComponent<Aim>().SetAimToAngle(aimAngleInt);
                    }
                    else
                    {
                        //The value specified was either too low or too high. Ignore the command.
                    }
                }
                else
                {
                    //Do nothing, because we're in the attack round.
                }
                break;

            case "fire":
                _client.SendMessage(e.Command.ChatMessage.Channel, $"Firing!");
                if (rm.phase == "Attack")
                {
                    string forceValue = e.Command.ChatMessage.Message;
                    forceValue = forceValue.Replace("!attack ", "");
                    int forceValueInt;
                    int.TryParse(forceValue, out forceValueInt);
                    bool alreadyFired = Fired.alreadyFired;

                    //Do a check to see if the player already fired
                    if (alreadyFired == false)
                    {
                        //Do a check to see if the value is within reasonable bounds. If not, break.
                        if (forceValueInt > 0 && forceValueInt <= 5)
                        {
                            //Fire the explosive at the forceValue specified
                            bomb.ThrowBomb(forceValueInt);

                            //Set the fired bool to true so they can't fire again until the next round".
                            Fired.fired = true;
                        }
                        else
                        {
                            //Player already fired. Ignore the command.
                            Debug.Log("Player already fired, but tried firing again");
                        }
                    }
                }
                else
                {
                    //Do nothing, because we're in the attack round or the player already fired.
                }
                break;
        }
    }

    private void AddNewPlayer(string playerToAdd)
    {
        pm.playerList.Add(playerToAdd);
        pm.currentPlayers++;
    }

    private void AlreadyJoinedMessage()
    {
        _client.SendMessage(_channelToConnectTo, "You've already joined and can't join again!");
    }

    public void GameFullMessage()
    {
        _client.SendMessage(_channelToConnectTo, "Game is now full! Try joining after the game is over!");
    }
}