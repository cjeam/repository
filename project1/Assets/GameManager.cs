using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject obje1;
    [SerializeField]
    GameObject obje2;
    [SerializeField]
    GameObject obje3;

    User user = new User();


    public string username;

    public int maxMessages = 10;

    public GameObject chatPanel, textObject;
    public InputField chatBox;
    public Color playerMessage, info;

    [SerializeField]
    List<Message> messageList = new List<Message>();

    void Start()
    {
        user.Id = PlayerPrefs.GetInt("id");
        user.Name = PlayerPrefs.GetString("name");
        username = user.Name;

    }

    void Update()
    {
        string objectName;
        if (chatBox.text != "")
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (FindObjectOfType<EventSystem>().IsTypeFocused<InputField>())
                {
                    SendMessageToChat(username + ": " + chatBox.text, Message.MessageType.playerMessage);
                    chatBox.text = "";
                }

            }
        }
        else
        {
            if (!chatBox.isFocused && Input.GetKeyDown(KeyCode.Return))
            {
                chatBox.ActivateInputField();
            }
        }
        if (!chatBox.isFocused)
        {        
            if (Input.GetKeyDown(KeyCode.Q))
            {
                objectName = "cube";
                SpawnCube(obje1);
                SendMessageToChat("Cube Spawned", Message.MessageType.info);
                PostObjectToServer(objectName);
            }
                
            else if (Input.GetKeyDown(KeyCode.W))
            {
                objectName = "sphere";
                SpawnCube(obje2);
                SendMessageToChat("Sphere Spawned", Message.MessageType.info);
                PostObjectToServer(objectName);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                objectName = "cylinder";
                SpawnCube(obje3);
                SendMessageToChat("Cylinder Spawned", Message.MessageType.info);
                PostObjectToServer(objectName);
            }


            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    SendMessageToChat("You pressed the space bar key!", Message.MessageType.info);
            //    Debug.Log("Space");

            //}
        }


    }

    public void SendMessageToChat(string text, Message.MessageType messageType)
    {

        if (messageList.Count >= maxMessages)
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }
        Message newMessage = new Message();
        newMessage.text = text;
        GameObject newText = Instantiate(textObject, chatPanel.transform);
        newMessage.textObject = newText.GetComponent<Text>();
        newMessage.textObject.text = newMessage.text;
        newMessage.textObject.color = MessageTypeColor(messageType);
        PostDataToServer(newMessage.text);
        messageList.Add(newMessage);
    }

    Color MessageTypeColor(Message.MessageType messageType)
    {
        Color color = info;
        switch (messageType)
        {
            case Message.MessageType.playerMessage:
                color = playerMessage;
                break;
        }
        return color;
    }

    async void PostDataToServer(string newMessageText)
    {
        //Çalýþan
        using (var client = new HttpClient())
        {
            var endpoint = new Uri("https://localhost:7198/api/messages/");
            var newPost = new Messages()
            {
                message = newMessageText,
                userId = user.Id
            };
            var newPostJson = JsonConvert.SerializeObject(newPost);
            var postdata = new StringContent(newPostJson, Encoding.UTF8, "application/json");
            var result = client.PostAsync(endpoint, postdata).Result.Content.ReadAsStringAsync();
            if (result.IsCompletedSuccessfully)
            {
                Debug.Log("Mesaj Baþarý ile kaydedildi.");

            }
        }



        //string url = "https://localhost:7198/api/users/";


        //var postData = new Dictionary<string, string>();
        //postData["name"] = username.text;
        //postData["password"] = password.text;

        //var httpClient = new HttpClient();
        //var response = await httpClient.PostAsync(url, new FormUrlEncodedContent(postData));

        //if (response.IsSuccessStatusCode)
        //{
        //    MessageBox.text = "Giris Basarili.Hosgeldiniz.";
        //}

        //else
        //    MessageBox.text = response.ReasonPhrase;

        //var www = UnityWebRequest.Post(url, postData);
        //www.SetRequestHeader("Content-Type", "application/json");
        //var operation = www.SendWebRequest();
        //while (!operation.isDone)
        //    await Task.Yield();

        //var jsonResponse = www.downloadHandler.text;

        //if (www.result != UnityWebRequest.Result.Success)
        //    MessageBox.text = "Failed:" + www.error;


        //using (var httpClient = new HttpClient())
        //{

        //    var response = await httpClient.PostAsync(url, new FormUrlEncodedContent(postData));
        //    if (response.IsSuccessStatusCode)
        //        MessageBox.text = await response.Content.ReadAsStringAsync();
        //    else
        //        MessageBox.text = response.ReasonPhrase;
        //}

        ////WWWForm form = new WWWForm();
        ////form.AddField("name", username.text);
        ////form.AddField("password", password.text);
        ////UnityWebRequest request = UnityWebRequest.Post(url, form);
        ////request.SetRequestHeader("Content-Type", "application/json");
        ////request.SetRequestHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
        ////request.SetRequestHeader("accept-encoding", "gzip, deflate, br");
        ////request.SetRequestHeader("accept-language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");


        ////yield return request.SendWebRequest();
        ////if (request.isNetworkError || request.isHttpError)
        ////    MessageBox.text = request.error;
        ////else
        ////    MessageBox.text = request.downloadHandler.text;


    }

    async void PostObjectToServer(string objectName)
    {
        //Çalýþan
        using (var client = new HttpClient())
        {
            var endpoint = new Uri("https://localhost:7198/api/spawnedobjects/");
            var newPost = new Objects()
            {
                name = objectName,
                userId = user.Id
            };
            var newPostJson = JsonConvert.SerializeObject(newPost);
            var postdata = new StringContent(newPostJson, Encoding.UTF8, "application/json");
            var result = client.PostAsync(endpoint, postdata).Result.Content.ReadAsStringAsync();
            if (result.IsCompletedSuccessfully)
            {
                Debug.Log("obje Baþarý ile kaydedildi.");

            }
        }
    }

    void SpawnCube(GameObject obje1)
    {
        Instantiate(obje1, obje1.transform.position, Quaternion.identity);
    }
}



[System.Serializable]
public class Message
{
    public string text;
    public Text textObject;
    public MessageType messageType;

    public enum MessageType
    {
        playerMessage,
        info

    }

}

public static class EventSystemExtensions
{
    public static bool IsTypeFocused<T>(this EventSystem self)
    {
        return self.currentSelectedGameObject.GetComponent<T>() != null;
    }
}
