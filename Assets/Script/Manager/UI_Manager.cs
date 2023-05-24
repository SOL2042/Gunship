using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    private static UI_Manager _instance;
    public static UI_Manager instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<UI_Manager>();
            return _instance;
        }
    }
    private int _score;
    public int score
    {
        set
        {
            _score = value;
            scoreTxt.text = _score.ToString();
        }
        get
        {
            return _score;
        }
    }

    private int _credit;
    public int credit
    {
        set
        {
            _credit = value;
            creditTxt.text = _credit.ToString();
        }
        get
        {
            return _credit;
        }
    }


    public TextMeshProUGUI missileCnt;
    public TextMeshProUGUI rocketCnt;
    public TextMeshProUGUI bulletCnt;
    public TextMeshProUGUI scoreTxt;
    public TextMeshProUGUI creditTxt;
    public TextMeshProUGUI throttleTxt;
    public TextMeshProUGUI USBaseHPTxt;

    public Slider USBaseSlider;
    public float USbaseHPReciprocal;

    public GameObject defeatUI;

    [SerializeField]
    private Button restartButton;
    [SerializeField]
    private Button MainMenuButton;

    // Start is called before the first frame update
    void Start()
    {
        restartButton.onClick.AddListener(() =>
        {
            EventManager.instance.PostEvent("OpenInGameScene", null);
        });
        MainMenuButton.onClick.AddListener(() =>
        {
            EventManager.instance.PostEvent("OpenMainMenuScene", null);
        });
        USbaseHPReciprocal = 1 / USBase.instance.totalData.maxHp;
        missileCnt.text = "Hellfire Missile : 8";
        rocketCnt.text = "Rocket : 38";
        bulletCnt.text = "30mm Chain Gun : 150";
    }
    // Update is called once per frame
    void Update()
    {
        missileCnt.text = $"Hellfire Missile : {WeaponController.instance.GetComponent<WeaponController>().missileCnt}";
        rocketCnt.text = $"Rocket : {WeaponController.instance.GetComponent<WeaponController>().rocketCnt}";
        bulletCnt.text = $"30mm Chain Gun : {WeaponController.instance.GetComponent<WeaponController>().bulletCnt}";
        scoreTxt.text = $"Score : {score}";
        creditTxt.text = $"Credit : {credit}";
        throttleTxt.text = $"Throttle : {((int)Player.instance.throttle)}%";
        USBaseHPTxt.text = $"Base HP : {(USBase.instance.totalData.currentHp * USbaseHPReciprocal * 100)}%";
        BaseHP();

    }

    private void Reload()
    {

    }

    private void BaseHP()
    {
       USBaseSlider.value = 100 - (USBase.instance.totalData.currentHp * USbaseHPReciprocal * 100);
    }
    // USBase.instance.totalData.currentHp = 0;

    public void Defeat()
    {
        defeatUI.SetActive(true);
    }

    public IEnumerator FadeTextToFullAlpha(TextMeshProUGUI text) // 알파값 0에서 1로 전환
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        while (text.color.a < 1.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime / 2.0f));
            yield return null;
        }
        StartCoroutine(FadeTextToZero(text));
    }

    public IEnumerator FadeTextToZero(TextMeshProUGUI text)  // 알파값 1에서 0으로 전환
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        while (missileCnt.color.a > 0.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime / 2.0f));
            yield return null;
        }
        StartCoroutine(FadeTextToFullAlpha(text));
    }

}
