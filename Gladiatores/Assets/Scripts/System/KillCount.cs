using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillCount : MonoBehaviour {

    private int _now;

    [SerializeField]
    private int killNumber = 0;
    [SerializeField]
    private int maxKillCount = 100;

    [SerializeField]
    private Slider slider;
    [SerializeField]
    private Text killText;

    [SerializeField]
    private bool isInverted;//ゲージの反転処理

    // Use this for initialization
    void Start () {
        _now = 1;
        slider.maxValue = maxKillCount;
        if (isInverted)
        {
            slider.value = 100;
        }
        else
        {
            slider.value = 0;
        }
    }

    // Update is called once per frame
    void Update () {

        killNumber = Mathf.Clamp(killNumber, 0, maxKillCount);
        //指定した番号ごとにゲージをリセット
        if (isInverted)
        {
            slider.value = 100 - killNumber;
        }
        else
        {
            slider.value = killNumber;
        }

        //アイコン上に討伐数を表示
        killText.text = killNumber.ToString();

    }

    public void AddKillCount()
    {
        killNumber++;
    }

    public int GetKillNumber()
    {
        return killNumber;
    }
}
