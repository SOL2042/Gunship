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
    public Text missileCnt;
    public Text bulletCnt;
    public TextMeshProUGUI scoreTxt;
    public TextMeshProUGUI creditTxt;
    public TextMeshProUGUI ThrottleTxt;


    // Start is called before the first frame update
    void Start()
    {
        missileCnt.color = Color.green;
        missileCnt.text = "Hellfire Missile : 8";
        bulletCnt.color = Color.green;
        bulletCnt.text = "30mm Chain Gun : 150";
    }
    // Update is called once per frame
    void Update()
    {
        missileCnt.text = $"Hellfire Missile : {WeaponController.instance.GetComponent<WeaponController>().missileCnt}" ;
        bulletCnt.text = $"30mm Chain Gun : {WeaponController.instance.GetComponent<WeaponController>().bulletCnt}" ;
        scoreTxt.text = $"Score : {score}";
        creditTxt.text = $"Credit : {credit}";
        ThrottleTxt.text = $"Throttle : {((int)Player.instance.throttle)}%";
    }
}
