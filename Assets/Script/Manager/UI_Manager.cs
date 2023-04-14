using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    WeaponController weaponController;

    private int _score;
    public int score
    {
        set
        {
            _score = value;
            missileCnt.text = _score.ToString();
        }
        get
        {
            return _score;
        }
    }
    public Text missileCnt;
    public Text bulletCnt;
    public TextMeshPro scoreTxt;


    // Start is called before the first frame update
    void Start()
    {
        weaponController = GetComponent<WeaponController>();
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
