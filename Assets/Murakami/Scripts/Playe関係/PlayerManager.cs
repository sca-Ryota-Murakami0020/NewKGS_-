using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//�v���C���[�̏����l��ݒ肷��(���S�ۑ��p�B�֐��͓���Ȃ�)
public class PlayerManager : MonoBehaviour
{
    //�v���C���[�̎c�@�����l
    [SerializeField] private int defPlayerRemain;

    //�v���C���[�̎c�@
    private int currentPlayerRemain;
    //�v���C���[�X�N���v�g
    private PlayerC playerC;
    //�X�e�[�W�R�p�v���C���[�X�N���v�g
    private RunOnlyPlayerC rPlayerC;
    //��Փx�̃��x��
    private int gameLevel = 0;
    //�v���C���[�̃f�t�H���g�̃X�s�[�h�{��
    private float playerSpeedMag = 1.0f;
    //�v���C���[�̃f�t�H���g�̃W�����v��
    private float playerJumpMag = 1.0f;
    //�C���X�^���X
    private PlayerManager instance;

    //�v���p�e�B
    public int ManagerRemain
    {
        get { return this.currentPlayerRemain;}
        set { this.currentPlayerRemain = value;}
    }
    public int GameLevel
    {
        get { return this.gameLevel;}
        set { this.gameLevel = value;}
    }
    public float DefSpeedMag
    {
        get { return this.playerSpeedMag;}
        set { this.playerSpeedMag = value;}
    }
    public float DefJumpMag
    {
        get { return this.playerJumpMag;}
        set { this.playerJumpMag = value;}
    }

    private void Awake()
    {
        currentPlayerRemain = defPlayerRemain;
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
    }
}
