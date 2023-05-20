using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class MainMenuManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private TextMeshProUGUI startText;

    private void Awake()
    {
        startButton.onClick.AddListener(() =>
        {
            EventManager.instance.PostEvent("OpenSettingRoomScene", null);
        });
        
        StartCoroutine(FadeTextToFullAlpha());

    }
    void Update()
    {

    }
    public IEnumerator FadeTextToFullAlpha() // 알파값 0에서 1로 전환
    {
        startText.color = new Color(startText.color.r, startText.color.g, startText.color.b, 0);
        while (startText.color.a < 1.0f)
        {
            startText.color = new Color(startText.color.r, startText.color.g, startText.color.b, startText.color.a + (Time.deltaTime / 2.0f));
            yield return null;
        }
        StartCoroutine(FadeTextToZero());
    }

    public IEnumerator FadeTextToZero()  // 알파값 1에서 0으로 전환
    {
        startText.color = new Color(startText.color.r, startText.color.g, startText.color.b, 1);
        while (startText.color.a > 0.0f)
        {
            startText.color = new Color(startText.color.r, startText.color.g, startText.color.b, startText.color.a - (Time.deltaTime / 2.0f));
            yield return null;
        }
        StartCoroutine(FadeTextToFullAlpha());
    }
}
