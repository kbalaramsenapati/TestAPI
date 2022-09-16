using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.Networking;
public class GameManager : MonoBehaviour
{
    #region Serializable Class
    [Serializable]
    public class CustomerstateButton
    {
        public string string_customerstate;
        public Button button_customerstate;
    }
    #endregion
    #region Decleration
    public static GameManager Insta_gameManager;
    public CustomerstateButton[] class_customerstateButton;

    [SerializeField]
    private TMP_Text _Text;
    [SerializeField]
    private AudioClip _audioClip;
    [SerializeField]
    private AudioSource _audioSource;

    public float float_CountTimer = 0;
    public GameObject gameObject_LoadingPanel;
    public Slider slider_Loading;
    public TMP_Text slider_Text;
    #endregion


    #region System Define Function
    private void Awake()
    {
        if (Insta_gameManager != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Insta_gameManager = this;
        }
    }
    private void Start()
    {
        for (int i = 0; i < class_customerstateButton.Length; i++)
        {
            int tempNo = i;
            class_customerstateButton[tempNo].button_customerstate.onClick.AddListener(() => getData(class_customerstateButton[tempNo].string_customerstate));
        }
    }
    #endregion

    #region User Define Function
    void getData(string tempData)
    {
        _audioSource.Stop();
        _Text.text = "";
        StartCoroutine(APIController.Insta_aPIController.WaitFor_SendData(tempData));

        Onclick_LoadingPanel(false);
    }


    public IEnumerator WaitForShowData(string tempText, string tempAudioURL)
    {
        Onclick_LoadingPanel(true);
       
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(tempAudioURL, AudioType.WAV))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                _audioClip = DownloadHandlerAudioClip.GetContent(www);
                _audioSource.clip = _audioClip;
                _audioSource.Play();
                _Text.text = tempText;
                Debug.Log("Audio clip Is Playing");
            }
        }
    }

    public void Onclick_LoadingPanel(bool tempCondition)
    {
        InvokeRepeating(nameof(_CountTimer), 0, 0.1f);
        float_CountTimer = 0;
        gameObject_LoadingPanel.SetActive(!tempCondition);
        if(!tempCondition)
        {
            InvokeRepeating(nameof(_CountTimer), 0, 0.1f);
        }
        else
        {
            CancelInvoke(nameof(_CountTimer));
        }

        for (int i = 0; i < class_customerstateButton.Length; i++)
        {
            int tempNo = i;
            class_customerstateButton[tempNo].button_customerstate.interactable = tempCondition;
        }
    }
    void _CountTimer()
    {
        if (float_CountTimer < 99)
        {
            float_CountTimer = float_CountTimer + 1f;
        }
        slider_Loading.value = float_CountTimer;
        slider_Text.text = float_CountTimer.ToString()+"%";
    }
    #endregion
}
