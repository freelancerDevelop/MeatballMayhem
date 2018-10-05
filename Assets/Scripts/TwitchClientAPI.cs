using System.Collections;
using System.Collections.Generic;
// If type or namespace TwitchLib could not be found. Make sure you add the latest TwitchLib.Unity.dll to your project folder
// Download it here: https://github.com/TwitchLib/TwitchLib.Unity/releases
// Or download the repository at https://github.com/TwitchLib/TwitchLib.Unity, build it, and copy the TwitchLib.Unity.dll from the output directory
using TwitchLib.Unity;
using UnityEngine;

public class TwitchClientAPI : MonoBehaviour
{
    [SerializeField] //[SerializeField] Allows the private field to show up in Unity's inspector. Way better than just making it public
    private string _usernameToGetChannelVideosFrom = "meatballmayhem";

    private Api _api;

    private void Start()
    {
        // To keep the Unity application active in the background, you can enable "Run In Background" in the player settings:
        // Unity Editor --> Edit --> Project Settings --> Player --> Resolution and Presentation --> Resolution --> Run In Background
        // This option seems to be enabled by default in more recent versions of Unity. An aditional, less recommended option is to set it in code:
        // Application.runInBackground = true;

        // Create new instance of Api
        _api = new Api();

        // The api needs a ClientID or an OAuth token to start making calls to the api.

        // Set the client id
        _api.Settings.ClientId = Secrets.CLIENT_ID;

        // Set the oauth token.
        // Most requests don't require an OAuth token, in which case setting a client id would be sufficient.
        // Some requests require an OAuth token with certain scopes. Make sure your OAuth token has these scopes or the request will fail.
        _api.Settings.AccessToken = Secrets.OAUTH_TOKEN;
    }

    private void Update()
    {
        // Don't call the Api on every Update, this is sample on how to call the Api,
        // This is not an example on how to code.
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Alpha1 = The number 1 key on your keyboard.
        {
            // Do what you want here, however if you want to call the twitch API this can be done as follows.
            // The following example is the GetChannelVideos if you want to call any TwitchLib.Api
            // endpoint replace the the following with your method call "_api.Channels.v5.GetChannelVideosAsync("{{CHANNEL_ID}}");"
            _api.Invoke(_api.Channels.v5.GetChannelVideosAsync("14900522"),
                        GetChannelVideosCallback);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) // Alpha2 = The number 2 key on your keyboard.
        {
            if (string.IsNullOrWhiteSpace(_usernameToGetChannelVideosFrom))
                throw new System.Exception($"The provided channel doesn't exist: {_usernameToGetChannelVideosFrom}");

            StartCoroutine(GetChannelVideosByUsername(_usernameToGetChannelVideosFrom));
        }
    }

    private IEnumerator GetChannelVideosByUsername(string usernameToGetChannelVideosFrom)
    {
        // Lets get Lucky's id first
        TwitchLib.Api.Models.Helix.Users.GetUsers.GetUsersResponse getUsersResponse = null;
        yield return _api.InvokeAsync(_api.Users.helix.GetUsersAsync(logins: new List<string> { usernameToGetChannelVideosFrom }),
                                      (response) => getUsersResponse = response);
        // We won't reach this point until the api request is completed, and the getUsersResponse is set.

        // We'll assume the request went well and that we made no typo's, meaning we should have 1 user at index 0, which is LuckyNoS7evin
        string luckyId = getUsersResponse.Users[0].Id;

        // Now that we have lucky's id, lets get his videos!
        TwitchLib.Api.Models.v5.Channels.ChannelVideos channelVideos = null;
        yield return _api.InvokeAsync(_api.Channels.v5.GetChannelVideosAsync(luckyId),
                                      (response) => channelVideos = response);
        // Again, we won't reach this point until the request is completed!

        // Handle user's ChannelVideos
        // Using this way of calling the api, we still have access to usernameToGetChannelVideosFrom!

        var listOfVideoTitles = GetListOfVideoTitles(channelVideos);
        var printableListOfVideoTitles = string.Join("  |  ", listOfVideoTitles);

        Debug.Log($"Videos from user {usernameToGetChannelVideosFrom}: {printableListOfVideoTitles}");
    }

    private void GetChannelVideosCallback(TwitchLib.Api.Models.v5.Channels.ChannelVideos e)
    {
        var listOfVideoTitles = GetListOfVideoTitles(e);
        var printableListOfVideoTitles = string.Join("  |  ", listOfVideoTitles);

        Debug.Log($"Videos from 14900522: {printableListOfVideoTitles}");
    }

    private List<string> GetListOfVideoTitles(TwitchLib.Api.Models.v5.Channels.ChannelVideos channelVideos)
    {
        List<string> videoTitles = new List<string>();

        foreach (var video in channelVideos.Videos)
            videoTitles.Add(video.Title);

        return videoTitles;
    }
}