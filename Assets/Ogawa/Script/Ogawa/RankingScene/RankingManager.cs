using UnityEngine;
using System;
using System.IO;

public class RankingManager : MonoBehaviour
{
    public JsonReader jsonReader;
    public NumberImageChange numberImageChange; // NumberImageChange�̎Q�Ƃ�ǉ�

    private int[] scoreToShow;
    private string[] nameToShow;

    private void Start()
    {
        //test
        /*string testName = "Murakami";
        int testScore = 1000000;*/

        scoreToShow = new int[5];//������
        nameToShow = new string[5];//������

        // JsonReader���g���ă����L���O�f�[�^��ǂݍ���
        PlayerScoresData playerScoresData = ReadJsonFile();
        if (playerScoresData != null)
        {
            // �X�R�A�f�[�^��\������
            DisplayScoreImages(playerScoresData);
            //�����L���O�̃A�b�v�f�[�g
            //test
            //UpdateRanking(testName,testScore);
        }
        else
        {
            Debug.LogWarning("Failed to load ranking data!");
        }
    }

    private PlayerScoresData ReadJsonFile()
    {
        string filePath = "Assets/Resources/Ranking.json"; // �p�X���w��

        if (System.IO.File.Exists(filePath))
        {
            string jsonContent = System.IO.File.ReadAllText(filePath);

            Debug.Log("Loaded JSON data: " + jsonContent);

            PlayerScoresData playerScoresData = JsonUtility.FromJson<PlayerScoresData>(jsonContent);

            if (playerScoresData != null)
            {
                // �f�o�b�O���O��playerScoresData�̓��e��\��
                //Debug.Log("Loaded player score data: "+jsonContent);
                for (int i = 0; i < playerScoresData.playerScores.Length; i++)
                {
                    //Debug.Log("Player Name: " + playerScoresData.playerScores[i].PlayerName + ", Score: " + playerScoresData.playerScores[i].score);

                }
                return playerScoresData;
            }
            else
            {
                Debug.LogWarning("Failed to parse JSON data!");
                return null;
            }
        }
        else
        {
            Debug.LogWarning("Json file does not exist at path: " + filePath);
            return null;
        }
    }

    // �����L���O�̍X�V����
    public void UpdateRanking(string playerName, int score)
    {
        // �V�����X�R�A������PlayerScore�I�u�W�F�N�g���쐬
        PlayerScore newScore = new PlayerScore();
        newScore.PlayerName = playerName;
        newScore.score = score;

        // �����L���O�f�[�^��ǂݍ���
        PlayerScoresData playerScoresData = ReadJsonFile();

        if (playerScoresData != null)
        {
            // �V�����X�R�A�����5�ʂɓ��邩�ǂ������m�F
            bool scoreAdded = false;

            // �V�����X�R�A�����5�ʂɓ���ꍇ�̏���
            if (playerScoresData.playerScores.Length < 5)
            {
                // �����L���O��5�l�����̏ꍇ�͏�ɒǉ�
                Array.Resize(ref playerScoresData.playerScores, playerScoresData.playerScores.Length + 1);
                playerScoresData.playerScores[playerScoresData.playerScores.Length - 1] = newScore;
                scoreAdded = true;
            }
            else
            {
                // �V�����X�R�A�����5�ʂɓ��邩�`�F�b�N
                for (int i = 0; i < playerScoresData.playerScores.Length; i++)
                {
                    if (newScore.score > playerScoresData.playerScores[i].score)
                    {
                        // �����L���O�ɒǉ�
                        InsertScore(playerScoresData.playerScores, newScore, i);
                        scoreAdded = true;
                        break;
                    }
                }
            }

            // �V�����X�R�A�������L���O���ɓ������ꍇ�̏���
            if (scoreAdded)
            {
                // �����L���O���\�[�g
                Array.Sort(playerScoresData.playerScores, (x, y) => y.score.CompareTo(x.score));

                // 6�Ԗڈȍ~�̃f�[�^���폜
                if (playerScoresData.playerScores.Length > 5)
                {
                    Array.Resize(ref playerScoresData.playerScores, 5);
                }

                // �����L���O���X�V����JSON�t�@�C���ɏ�������
                SaveUpdatedRanking(playerScoresData);

                //gamemanager�Ƀ����N�C���������Ƃ�`����

            }
        }
        else
        {
            Debug.LogWarning("Failed to load ranking data!");
        }
    }

    // �X�V���ꂽ�����L���O��JSON�t�@�C���ɕۑ�����
    private void SaveUpdatedRanking(PlayerScoresData playerScoresData)
    {
        string filePath = "Assets/Resources/Ranking.json"; // �p�X���w��

        // JSON�`���ɕϊ����ăt�@�C���ɏ�������
        string json = JsonUtility.ToJson(playerScoresData);
        File.WriteAllText(filePath, json);

        Debug.Log("Ranking update");
    }

    private void InsertScore(PlayerScore[] scores, PlayerScore newScore, int index)
    {
        for (int i = scores.Length - 1; i > index; i--)
        {
            scores[i] = scores[i - 1];
        }
        scores[index] = newScore;
    }

    private void DisplayScoreImages(PlayerScoresData playerScoresData)
    {
        if (numberImageChange != null)
        {
            if (playerScoresData != null && playerScoresData.playerScores.Length > 0)
            {

                // ��ʂT�l�̃X�R�A���擾���ANumberImageChange�ɓn��
                for (int i = 0; i < 5; i++)
                {
                    scoreToShow[i] = playerScoresData.playerScores[i].score;
                    nameToShow[i] = playerScoresData.playerScores[i].PlayerName;
                    Debug.Log($"Rank {i + 1}: Player Name: {nameToShow[i]}, Score: {scoreToShow[i]}");
                    //Debug.Log(nameToShow[i]);
                    numberImageChange.ShowScoreImages(scoreToShow[i], i);
                    numberImageChange.ShowName(nameToShow[i], i);
                }


            }
            else
            {
                Debug.LogWarning("PlayerScoresData is null or empty!");
            }
        }
        else
        {
            Debug.LogWarning("NumberImageChange reference is missing!");
        }
    }
}
