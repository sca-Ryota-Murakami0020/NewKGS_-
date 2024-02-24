using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using static UnityEditor.PlayerSettings;
using TMPro;

public class ThirdStageGM : MonoBehaviour
{
    [SerializeField]
    private RunOnlyPlayerC runPlayerC;
    //[SerializeField]
    //private TMP_Text remainText;
    //���s���̎c�@
    private int currentRemain = 0;
    
    public int CURRENTREMAIN {
        set {
            this.currentRemain = value;
        }
        get {
            return this.currentRemain;
        }
    }
    
    //�c�@�p�̃A�j���[�V����
    [SerializeField]
    private Animator remainAni;
    //�X�e�[�W�ԂŎc�@���Ǘ����Ă������
    private PlayerManager playerManager = null;

    //�v���C���[�̃X�s�[�h�֌W
    [SerializeField]
    private float playerSpeed;
    [SerializeField]
    private float playerJumpPow;
    //�f�t�H�̃X�s�[�h
    private bool debufMove = false;
    private float defMove = 0.0f;
    private bool debufJump = false;
    private float defJump = 0.0f;

    //�X�V�p
    private float oldSpeedMag = 0.0f;
    private float oldJumpMag = 0.0f;

    //���Ԍv�Z�p
    private float moveTime = 0.0f;
    private float jumpTime = 0.0f;

    public float PlayerSpeed
    {
        get { return this.playerSpeed;}
    }

    public float PlayerJumpPow
    {
        get { return this.playerJumpPow;}
    }

    [SerializeField] GameObject gameClear;
    [SerializeField] GameObject gameOverthing;

    [SerializeField] Image divedPanel;
    float fadeSpeed = 0.02f;
    float P_red, P_green, P_blue, P_alfa;

    
    Vector3 dessPosition;
    [SerializeField] GameObject zankiIocn;
    [SerializeField]Text zankiIconText;
    [SerializeField] GameObject missText;
    [SerializeField] GameObject playerObject;
    bool fadeIn = false;
    [SerializeField] MeshCollider plane;
    bool GameOver = false;
    public bool GAMEOVER {
        set {
            this.GameOver = value;
        }
        get {
            return this.GameOver;
        }
    }
    [SerializeField] GameObject overPlayer;
    [SerializeField] RezalutC rezalut;

    // Start is called before the first frame update
    void Start()
    {
        dessPosition = playerObject.transform.position;
        //�����l�̋L��
        defMove = playerSpeed;
        defJump = playerJumpPow;

        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        playerSpeed = playerSpeed * playerManager.DefSpeedMag;
        playerJumpPow = playerJumpPow * playerManager.DefJumpMag;

        currentRemain = playerManager.ManagerRemain;
    }

    private void FixedUpdate() {
        if(!fadeIn && (runPlayerC.FALLING || runPlayerC.WARP)) {
            
            FadeOut();
        }
        if(GameOver) {
            overPlayer.SetActive(true);
            gameOverthing.SetActive(true);
        }

        if(runPlayerC.WARP && StageSelectController.mode == StageSelectController.MODE.CHALLENGE) {
            gameClear.SetActive(true);
        }
    }

    void FadeOut() {
        if(runPlayerC.FALLING) {
            runPlayerC.enabled = false;
            if(!runPlayerC.WARP) {
                StartCoroutine(WaitInoti());
            }

            if(P_alfa < 1.0f) {
                P_alfa += fadeSpeed;
                SetAlpha();
            } else if(P_alfa >= 1.0f) {
                fadeIn = true;

            }
        }
        if(runPlayerC.WARP) {
            if(P_alfa < 1.0f) {
                P_alfa += fadeSpeed;
                SetAlpha();
            } else if(P_alfa >= 1.0f) {
                if(StageSelectController.mode == StageSelectController.MODE.STORY) {
                    TitleManager.sceneName = "MojiHyouji";
                    SceneManager.LoadScene("LoadScene");
                }
                else if(StageSelectController.mode == StageSelectController.MODE.CHALLENGE && rezalut.YES) {
                    TitleManager.sceneName = "Masaki";
                    SceneManager.LoadScene("LoadScene");
                }
            }
        }

    }

    IEnumerator WaitInoti() {
        if(currentRemain != 0) {
            missText.SetActive(true);
            yield return new WaitForSeconds(2.0f);          

            plane.enabled = true;

            RevivePlayer();

            missText.SetActive(false);
            zankiIocn.SetActive(true);
            zankiIocn.SetActive(true);
            runPlayerC.enabled = true;
            zankiIconText.text = "�~" + currentRemain;
            List<IEnumerator> ie = new List<IEnumerator>();
            ie.Add(WaitU());
            foreach(IEnumerator item in ie) {
                StartCoroutine(item);

                yield return item.Current;// <===�������d�v
            }
            yield return null;
        } else {
            //Debug.Log("dd");
            GameOver = true;
        }
    }

    bool remain = false;

    IEnumerator WaitU() {
        yield return new WaitForSeconds(1.0f);
        if(!remain) {
            //playerManager.ManagerRemain--;
            currentRemain--;
            Debug.Log("�����Z��");
            zankiIconText.text = "�~" + currentRemain;
            remain = true;
        }
        
        runPlayerC.FALLING = false;
        
        if(!GameOver) {
            List<IEnumerator> ie = new List<IEnumerator>();
            ie.Add(WaitFadeIn());
            foreach(IEnumerator item in ie) {
                StartCoroutine(item);
                yield return item.Current;// <===�������d�v
            }
            yield return null;
        } else {
            //GameOverActive();
            //g = true;
            yield break;
        }
    }

    IEnumerator WaitFadeIn() {
        yield return new WaitForSeconds(1.0f);

        FadeIn();
    }

    void FadeIn() {
        zankiIocn.SetActive(false);

        if(P_alfa > 0.0f) {
            P_alfa -= fadeSpeed;
            SetAlpha();
        }

        if(P_alfa <= 0.0f) {
            fadeIn = false;
            remain = false;
        }
    }

    void SetAlpha() {
        divedPanel.color = new Color(P_red, P_green, P_blue, P_alfa);
    }

    /// <summary>
    /// �c�@���������ꍇ�̐����Ԃ�֐�
    /// </summary>
    /// <param name="recount">�`�F�b�N�|�C���g�̐�</param>
    public void RevivePlayer() {
        if(currentRemain != 0 && playerManager.ManagerRemain != 0) {
            playerObject.transform.position = dessPosition;
        }
    }


    //�J�n�i�����j
    public void DebufMoveSpeed(float mag)
    {
        if(oldSpeedMag < mag) oldSpeedMag = mag;
        else mag = oldSpeedMag;

        playerSpeed = defMove;
        //�Ē�`
        playerSpeed = playerSpeed * (playerManager.DefSpeedMag - mag);
        if(!debufMove) debufMove = true;
        else moveTime = 0.0f;
    }

    public void DebufJumpSpeed(float mag)
    {
        if(oldJumpMag < mag) oldJumpMag = mag;
        else mag = oldJumpMag;

         playerJumpPow = defJump;
        //�Ē�`
        playerJumpPow = playerJumpPow * (playerManager.DefJumpMag - mag);
        if(!debufJump) debufJump = true;
        else jumpTime = 0.0f;
    }

    //���Ԍv�Z
    private void CountMoveDebuf()
    {
        moveTime += 0.01f;
        if(moveTime >= 10.0f)
        {
            return;
        }
    }

    private void CountJumpDebuf()
    {
        jumpTime += 0.01f;
        if(jumpTime >= 10.0f)
        {
            jumpTime = 0.0f;
            return;
        }
    }

}
