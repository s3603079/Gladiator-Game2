using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : SingletonMonoBehaviour<ScoreManager>
{
    //経過時間
    private float _timer;

    //キルカウント
    [SerializeField]
    KillCount _killCount;

    //スコア(８桁)
    int _score;

    //動きをつけた表示用
    private int _displayScore;

    //スコアのテキスト
    [SerializeField]
    private Text _scoreText;

    //スコア加算のタイミングを知るため(死亡時)
    [SerializeField]
    private VirtualChatactor _chara;

    protected override void Awake()
    {
        base.Awake();
    }

    // Use this for initialization
    void Start()
    {
        _score = 0;
        _displayScore = _score;
        _timer = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
#if true
        //デバッグ用---------------------------------------------------
        Debug.Log("スコアの加算");
        if (!_chara.IsLiving())//敵が死んだら
        {
            AddScore();//スコアを入手
        }
        //-------------------------------------------------------------
#endif
    }

    public void AddScore()
    {
        /*長時間生き残ると、スコアが高い*/
        _score = (int)(Time.time - _timer) * 10 + _killCount.GetKillNumber() * 100;
    }

    public int GetScore()
    {
        return _score;
    }

    public void DrawScore()
    {
        //スコアの表示
        if (_displayScore != _score)
        {
            _displayScore = (int)Mathf.Lerp(_displayScore, _score, 0.1f);
        }

        _scoreText.text = string.Format("{0:00000000}", _displayScore);
    }
}