using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class NumberImageChange : MonoBehaviour
{
    [System.Serializable]
    public class ScoreRow
    {
        public Image[] scoreDigits;
        public string[] playerName;
    }



    [SerializeField] Sprite[] numberSprites;// 0����9�܂ł̐����摜��ێ�����z��
    [SerializeField] ScoreRow[] ScoreRows;  // �X�R�A�̊e���̉摜��\������Image�R���|�[�l���g�̔z��
    [SerializeField] Text[] nameText;       //PlayerName��text�R���|�[�l���g�̔z��
    int count = 0;
    // �����L���O�f�[�^�̃X�R�A��UI�ŕ\������
    public void ShowScoreImages(int score, int c)
    {

        string scoreString = score.ToString();// �X�R�A�𕶎���ɕϊ�
        for (int j = 0; j < 7; j++)
        {
            if (j < scoreString.Length)
            {
                int digit = score % 10; // �e���̐������擾
                score = score / 10;
                ScoreRows[c].scoreDigits[j].sprite = numberSprites[digit]; // �Ή�����摜��ݒ�
                ScoreRows[c].scoreDigits[j].gameObject.SetActive(true); // �摜��\��
            }
            else
            {
                ScoreRows[c].scoreDigits[j].gameObject.SetActive(false); // ��������Ȃ��ꍇ�͔�\���ɂ���
            }
        }

    }
    //�����L���O��playername�̕\��
    public void ShowName(string name, int i)
    {
        nameText[i].text = name;

    }
}
