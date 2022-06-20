using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dataTransfer : MonoBehaviour
{
    User user = new User();
    // Start is called before the first frame update
    void Start()
    {
        user.Id = PlayerPrefs.GetInt("id");
        user.Name = PlayerPrefs.GetString("name");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
