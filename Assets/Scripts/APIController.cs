using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using SimpleHTTP;
using System.IO;
using System;
public class APIController : MonoBehaviour
{
    #region Decleration
    public static APIController Insta_aPIController;


    public string string_baseURL;
    public Bodydata class_bodydata;
    [SerializeField]
    private string string_SendjsonData;

    public RecivedData class_recivedDatas;

    
    #endregion

    #region System Define Function
    private void Awake()
    {
        if (Insta_aPIController != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Insta_aPIController = this;
        }
    }

    private void Start()
    {
        //StartCoroutine(WaitFor_SendData("cs_men_products"));
    }
    #endregion

    #region User Define Function

    public IEnumerator WaitFor_SendData(string tempcustomer_state)
    {
        
        

        class_bodydata.customer_state = tempcustomer_state;
        //recivedDatas.Clear();
        yield return new WaitForSeconds(0.1f);
        string_SendjsonData = JsonUtility.ToJson(class_bodydata);
        yield return new WaitForSeconds(0.5f);

        Request request = new Request(string_baseURL)
            .AddHeader("X-I2CE-ENTERPRISE-ID", "dave_expo")
            .AddHeader("X-I2CE-USER-ID", "74710c52-42a5-3e65-b1f0-2dc39ebe42c2")
            .AddHeader("X-I2CE-API-KEY", "NzQ3MTBjNTItNDJhNS0zZTY1LWIxZjAtMmRjMzllYmU0MmMyMTYwNzIyMDY2NiAzNw__")
            .AddHeader("Content-Type", "application/json")
            .Post(RequestBody.From(string_SendjsonData));
        Client http = new Client();
        yield return http.Send(request);
        string tempfeedbackRespond = "";
        if (http.IsSuccessful())
        {
            Response resp = http.Response();
            tempfeedbackRespond = "status: " + resp.Status().ToString() + "\nbody: " + resp.Body();
            class_recivedDatas = JsonUtility.FromJson<RecivedData>(resp.Body().ToString());
            //RecivedData tempData = new RecivedData();
            
            string tempplaceholder = class_recivedDatas.placeholder;
            string tempvoice = class_recivedDatas.response_channels.voice;
            StartCoroutine(GameManager.Insta_gameManager.WaitForShowData(tempplaceholder, tempvoice));

            Debug.Log(tempfeedbackRespond);
        }
        else
        {
            tempfeedbackRespond = "error: " + http.Error();
            Debug.Log(tempfeedbackRespond);
        }
        
    }
    
    #endregion

}
[Serializable]
public class Bodydata
{
    public string system_response;
    public string engagement_id;
    public string customer_state;
}
[Serializable]
public class RecivedData
{
    public bool hide_in_customer_history;
    public string registered_entities;
    public string whiteboard_template;
    public string customer_state;
    public _placeholder_aliases placeholder_aliases;//Class1
    public string show_feedback;
    public _tostatefunction to_state_function;//class2
    public string placeholder;
    public bool show_in_history;
    public _data data;//class3
    public bool overwrite_whiteboard;
    public string start_timestamp;
    public string console;
    public string name;
    public string title;
    public _response_channels response_channels;//class4
    public string whiteboard;
    public _state_options state_options;//class5
    public string response_id;
    public string whiteboard_title;
    public string timestamp;
    public string maintain_whiteboard;
    public string wait;
    public string type;
    public string options;
    public string engagement_id;

}
[Serializable]
public class _tostatefunction
{
    public string function;
}
[Serializable]
public class _placeholder_aliases
{

}
[Serializable]
public class _data
{
    public _slideshow[] slideshow;
}
[Serializable]
public class _slideshow
{
    public string image;//Image
    public string caption;
}
[Serializable]
public class _response_channels
{
    public string voice;//AudioClip
    public string frames;
    public string shapes;
}
[Serializable]
public class _state_options
{
    public string cs_top_three;
    public string cs_must_have;
    public string cs_enquiry;
    public string cs_mt1;
    public string cs_mt2;
    public string cs_mt3;
}