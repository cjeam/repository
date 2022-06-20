using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Text;
using UnityEngine.SceneManagement;

public class InputSystem : MonoBehaviour

{
    EventSystem system;
    public Selectable firstInput;
    public Button submitButton;
    public Button registerButton;
    public InputField username;
    public InputField password;
    public Text MessageBox;
  //  void PostData() => StartCoroutine(PostDataToServer());
 //   void GetData() => StartCoroutine(GetDataFromServer());

    // Start is called before the first frame update
    void Start()
    {
        system = EventSystem.current;
        firstInput.Select();
        GameObject.Find("registerButton").GetComponent<Button>().onClick.AddListener(PostDataToServer);
        GameObject.Find("loginButton").GetComponent<Button>().onClick.AddListener(GetDataFromServer);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
        {
            Selectable previous = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
            if (previous != null)
            {
                previous.Select();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            if (next != null)
            {
                next.Select();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            submitButton.onClick.Invoke();
            Debug.Log("Button Pressed");
            LoginButtonSubmitted();

        }
    }


    public void sceneSwitch()
    {
        SceneManager.LoadScene("GameScene");
    }
    void LoginButtonSubmitted()
    {
        Debug.Log(username.text);
        Debug.Log(password.ToString());
    }

    void RegisterButtonSubmitted()
    {
        PostDataToServer();
    }

    public async void GetDataFromServer()
    {
        string url = "https://localhost:7198/api/users/" + username.text + "/user";
        using var www = UnityWebRequest.Get(url);
        www.SetRequestHeader("Content-Type", "application/json");
        var operation = www.SendWebRequest();
        while(!operation.isDone)
            await Task.Yield();

        var jsonResponse = www.downloadHandler.text;

        if (www.result != UnityWebRequest.Result.Success)
            MessageBox.text = "Failed: User Could not found." + www.error;

        try
        {
            var result = JsonConvert.DeserializeObject<User>(jsonResponse);
            if (result.Password == password.text)
            {
                MessageBox.text = "Giris Basarili.Sifre Eslesti";
                PlayerPrefs.SetString("name", result.Name);
                PlayerPrefs.SetInt("id", result.Id);
                Debug.Log(PlayerPrefs.GetInt("id"));
                Debug.Log(PlayerPrefs.GetString("name"));
                sceneSwitch();
            }
            else if (result.Password != password.text)
            {
                MessageBox.text = "Sifre Hatali";
            }

            //MessageBox.text = "Success: " + result.Name;
        }
        catch (System.Exception)
        {
            MessageBox.text = "Failed: " + www.error;

        }
        

    }

    async void PostDataToServer()
    {
        //Çalýþan
        using (var client = new HttpClient())
        {
            var endpoint = new Uri("https://localhost:7198/api/users/");
            var newPost = new Post()
            {

                name = username.text,
                password = password.text,
                status = 1
            };
            var newPostJson = JsonConvert.SerializeObject(newPost);
            var postdata = new StringContent(newPostJson, Encoding.UTF8, "application/json");
            var result = client.PostAsync(endpoint, postdata).Result.Content.ReadAsStringAsync();
            var jsonObject = JsonConvert.DeserializeObject<User>(await result);
            if (result.IsCompletedSuccessfully)
            {
                MessageBox.text = "Giris Basarili";
                PlayerPrefs.SetString("name", jsonObject.Name);
                PlayerPrefs.SetInt("id", jsonObject.Id);
                result.ToString();
                sceneSwitch();

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
}
