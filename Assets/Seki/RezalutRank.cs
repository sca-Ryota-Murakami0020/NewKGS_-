using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RezalutRank : MonoBehaviour
{
    //�󂯎��X�R�A
    int point;

    //�󂯎�閼�O
    string myName;

    [SerializeField, Header("�����N���O")]
    Text[] rankText = new Text[5];

    [SerializeField, Header("�\��������e�L�X�g")]
    Text[] rankingText = new Text[5];

    string[] ranking = { "1��", "2��", "3��", "4��", "5��" };

    string[] king = { "1", "2", "3", "4", "5" };
    string[] rankName = new string[5];
    int[] rankingValue = new int[5];
    PlayerManager playerManager;

    bool rank = false;

    

    bool go = false;
    public bool GO {
        set {
            this.go = value;
        }
        get {
            return this.go;
        }
    }

    [SerializeField] GameObject zannen;
    [SerializeField] NameScripts userName;
    [SerializeField] LetterPanel letter;

    [SerializeField] GameObject inputName;
    [SerializeField] GameObject rankIn;

    [SerializeField] RezalutC rezalutC;

    void Start()
    {
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        point = playerManager.GameLevel;
        GetRanking();
        Ranking(point);
    }

    /// <summary>
    /// �����L���O�Ăяo��
    /// </summary>
    void GetRanking() {
        //�����L���O�Ăяo��
        for(int i = 0; i < ranking.Length; i++) {
            rankingValue[i] = PlayerPrefs.GetInt(ranking[i]);
            rankName[i] = PlayerPrefs.GetString(king[i]);
        }
    }

    /// <summary>
    /// �����L���O��������
    /// </summary>
    void SetRanking(int _value, string namae) {
        //rankingValue[0] = 100;
        //rankName[0] = "moriya";
        //�������ݗp
        for(int i = 0; i < ranking.Length; i++) {
            //�擾�����l��Ranking�̒l���r���ē���ւ�
            if(_value >= rankingValue[i]) {
                var change = rankingValue[i];
                rankingValue[i] = _value;
                _value = change;
                var name = rankName[i];
                rankName[i] = namae;
                namae = name;
            }
            rankingText[i].text = rankingValue[i].ToString();
            rankText[i].text = rankName[i];
        }

        //����ւ����l��ۑ�
        for(int i = 0; i < ranking.Length; i++) {
            PlayerPrefs.SetInt(ranking[i], rankingValue[i]);
            PlayerPrefs.SetString(king[i], rankName[i]);
        }
        go = true;
    }

    void Ranking(int _value) {
        for(int i = 0; i < ranking.Length; i++) {
            //�擾�����l��Ranking�̒l���r���ē���ւ�
            if(_value >= rankingValue[i]) {
                rank = true;
            } else {
                rank = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(rank);
        if(rank && rezalutC.PUSH) {
            StartCoroutine(waitActive(rank, rezalutC.PUSH));
        } else if(!rank && rezalutC.PUSH){
            StartCoroutine(waitActive(rank, rezalutC.PUSH));
        }

        if(letter.NEXT)
        {
            rankIn.SetActive(true);
            name = userName.MYNAME;
            SetRanking(point, name);
        }
    }

    IEnumerator waitActive(bool rank,bool push) {
        yield return new WaitForSeconds(1.0f);
        if(rank && push) {
            inputName.SetActive(true);
        } else if(!rank && push) {
            zannen.SetActive(true);
        }
    }
}
