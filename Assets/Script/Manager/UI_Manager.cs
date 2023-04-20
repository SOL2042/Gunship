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
    public Text missileCnt;
    public Text bulletCnt;
    public TextMeshProUGUI scoreTxt;


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
    }
}
