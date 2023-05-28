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
    public TextMeshProUGUI ejectCntTxt;
    public TextMeshProUGUI WaveTxt;
    public TextMeshProUGUI FlyModeTxt;

    public Slider USBaseSlider;
    public float USbaseHPReciprocal;

    [SerializeField]
    private GameObject playerAim;

    public GameObject defeatUI;
    public GameObject pauseUI;
    public GameObject sucideUI;
    public GameObject AAMissileRadarUI;

    [SerializeField]
    private Button defeatRestartButton;
    [SerializeField]
    private Button defeatMainMenuButton;
    
    [SerializeField]
    private Button pauseRestartButton;
    [SerializeField]
    private Button pauseMainMenuButton;

    void Start()
    {
        defeatRestartButton.onClick.AddListener(() =>
        {
            EventManager.instance.PostEvent("OpenInGameScene", null);
        });
        defeatMainMenuButton.onClick.AddListener(() =>
        {
            EventManager.instance.PostEvent("OpenMainMenuScene", null);
        });
        pauseRestartButton.onClick.AddListener(() =>
        {
            EventManager.instance.PostEvent("OpenInGameScene", null);
        });
        pauseMainMenuButton.onClick.AddListener(() =>
        {
            EventManager.instance.PostEvent("OpenMainMenuScene", null);
        });
        USbaseHPReciprocal = 1 / USBase.instance.totalData.maxHp;
        missileCnt.text = "Hellfire Missile : 8";
        rocketCnt.text = "Rocket : 38";
        bulletCnt.text = "30mm Chain Gun : 150";
    }
    void Update()
    {
        missileCnt.text = $"Hellfire Missile : {WeaponController.instance.missileCnt}";
        rocketCnt.text = $"Rocket : {WeaponController.instance.rocketCnt}";
        bulletCnt.text = $"30mm Chain Gun : {WeaponController.instance.bulletCnt}";
        scoreTxt.text = $"Score : {score}";
        creditTxt.text = $"Credit : {credit}";
        throttleTxt.text = $"Throttle : {((int)Player.instance.throttle)}%";
        USBaseHPTxt.text = $"Base HP : {(USBase.instance.totalData.currentHp * USbaseHPReciprocal * 100)}%";
        BaseHP();
        ejectCntTxt.text = $"Eject : {(int)WeaponController.instance.suicideTimer}";
        WaveTxt.text = $"Wave : {EnemyController.instance.waveRound}";
        FlyModeTxt.text = $"Mode : {WeaponController.instance.totalData.flyMode}";
    }

    private void OnEnable()
    {
        Reload();
    }
    
    private void Reload()
    {
        //StopAllCoroutines();
        //if(WeaponController.instance.missileCnt <= 0)
        //{
        //    StartCoroutine(FadeTextToFullAlpha(missileCnt));
        //}
        //else
        //{
        //    StopCoroutine(FadeTextToZero(missileCnt));
        //    StopCoroutine(FadeTextToFullAlpha(missileCnt));
        //}
        //if (WeaponController.instance.rocketCnt <= 0)
        //{
        //    StartCoroutine(FadeTextToFullAlpha(rocketCnt));
        //}
        //else
        //{
        //    StopCoroutine(FadeTextToZero(rocketCnt));
        //    StopCoroutine(FadeTextToFullAlpha(rocketCnt));
        //}
        //if (WeaponController.instance.bulletCnt <= 0)
        //{
        //    StartCoroutine(FadeTextToFullAlpha(bulletCnt));
        //}
        //else
        //{
        //    StopCoroutine(FadeTextToZero(bulletCnt));
        //    StopCoroutine(FadeTextToFullAlpha(bulletCnt));
        //}
    }

    private void BaseHP()
    {
       USBaseSlider.value = 100 - (USBase.instance.totalData.currentHp * USbaseHPReciprocal * 100);
    }
    public void Sucide()
    {
        sucideUI.SetActive(true);
    }
    public void Defeat()
    {
        if (defeatUI.activeInHierarchy != true)
        {
            defeatUI.SetActive(true);
        }
       
    }
    public void Pause()
    {
        if (pauseUI.activeInHierarchy != true)
        {
            pauseUI.SetActive(true);
        }
        else
        {
            pauseUI.SetActive(false);
        }
    }
    public void AAMissileRadar()
    {
        if (AAMissileRadarUI.activeInHierarchy == false)
        {
            AAMissileRadarUI.SetActive(true);
        }
        else
        {
            AAMissileRadarUI.SetActive(false);
        }
    }
    private IEnumerator FadeTextToFullAlpha(TextMeshProUGUI text) // 알파값 0에서 1로 전환
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        while (text.color.a < 1.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime / 2.0f));
            yield return null;
        }
        StartCoroutine(FadeTextToZero(text));
    }

    private IEnumerator FadeTextToZero(TextMeshProUGUI text)  // 알파값 1에서 0으로 전환
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
