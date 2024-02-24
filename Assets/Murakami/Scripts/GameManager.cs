using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region//�^�C���֌W
    [Header("�X�e�[�W�̐�������"), SerializeField]
    private int stageCount;
    [Header("�g�p����摜"), SerializeField]
    private Sprite[] numberImage;
    [Header("�^�C���pImage"), SerializeField]
    private Image[] timeImage;
    #endregion

    #region//�X�R�A�֌W
    [SerializeField]
    private TMP_Text scoreText;
    [Header("�A���l�������e���鎞�ԁiF�j"), SerializeField]
    private float maxGetCount;
    #endregion

    #region//�c�@�֌W
    [SerializeField]
    private TMP_Text remainText;
    //���s���̎c�@
    private int currentRemain = 0;
    //�c�@�p�̃A�j���[�V����
    [SerializeField]
    private Animator remainAni;
    //�X�e�[�W�ԂŎc�@���Ǘ����Ă������
    private PlayerManager playerManager = null;
    #endregion

    #region//�������֌W
    [SerializeField]
    private TMP_Text coinText;
    [Header("�c�@�𑝂₷���̏������z"), SerializeField]
    private int maxCoins;
    //������
    private int getCoins = 0;
    public int GETCOIN {
        set {
            this.getCoins = value;
        }
        get {
            return this.getCoins;
        }
    }
    //�R�C���p�̃A�j���[�V����
    [SerializeField]
    private Animator coinAni;

    //�Ǘ����Ă���R�C���̃I�u�W�F�N�g
    private Queue<GameObject> activeCoins = new Queue<GameObject>();
    #endregion

    //��ʂT�����{���킵���ۂ̃X�R�A
    private int[] rankingScore; 
    //����̓�Փx
    private int difLevel = 0;
    //����̃X�R�A
    private int gameScore;
    //�A���Ŋl�������R�C���̐�
    private int getCoinCount = 0;
    //�A���l���\���Ԃ̉��Z�p�ϐ�
    private float canGetCoinsTime = 0.0f;
    //�A���Ŋl���\�t���O
    private bool canGetCoins = false;
    //�I�����ꂽ����
    private string[] selectBinding;
    //�|�[�Y��
    private bool stopGame = false;
    //�v���C���[�̓����𐧌�����i�J�������ǂ�Ȃ��Ƃ��j
    //private bool dontMove = false;
    //���s���̌o�ߎ���
    private float currentGameTime = 0.0f;
    


    [SerializeField] GameObject playerObject;
    PlayerC player;
    PlayerInput playerInput;

    [SerializeField] Image PausePanel;
    float fadeSpeed = 0.02f;
    float P_red, P_green, P_blue, P_alfa;

    [SerializeField] PauseManager pause;

    [SerializeField] MissionManager mission;

    Vector3 dessPosition;

    public Vector3 DESSPOS {
        set {
            this.dessPosition = value;
        }
        get {
            return this.dessPosition;
        }
    }
    bool fadeIn = false;

    bool GameOver = false;
    public bool GAMEOVER {
        set {
            this.GameOver = value;
        }
        get {
            return this.GameOver;
        }
    }
    [SerializeField] MeshCollider plane;
    [SerializeField] GameObject gameOverThings;
    [SerializeField] GameObject gameClearThings;

    [SerializeField] GameObject zankiIocn;
    Text zankiIconText;
    [SerializeField] GameObject missText;
    CharacterController _characterController;
    
    [SerializeField] GameObject keyActiveObj;
    [SerializeField] GameObject gameOverplayer;
    [SerializeField] Canvas stageCanvas;
    //[SerializeField] CinemachineVirtualCamera cinema;
    #region//�v���p�e�B
    public string[] SelectBinding
    {
        get { return this.selectBinding;}
        set { this.selectBinding = value;}
    }

    public int CurrentRemain
    {
        get { return this.currentRemain;}
        set { this.currentRemain = value;}
    }

    public int StageCount
    {
        get { return this.stageCount;}
    }

    public int GetScore
    {
        get { return this.gameScore;}
    }

    public int DifLevel
    {
        get { return this.difLevel;}
    }

    public Queue<GameObject> ActiveCoinObject
    {
        get { return this.activeCoins;}
        set { this.activeCoins = value;}
    }
    #endregion

    void Start()
    {
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();

        stageCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        keyActiveObj.SetActive(true);
        _characterController = playerObject.GetComponent<CharacterController>();
        zankiIconText = zankiIocn.GetComponentInChildren<Text>();
        gameOverThings.SetActive(false);
        gameClearThings.SetActive(false);
             
        player = playerObject.GetComponent<PlayerC>();
        playerInput = playerObject.GetComponent<PlayerInput>();
        mission = mission.GetComponent<MissionManager>();
        pause = pause.GetComponent<PauseManager>();
        PausePanel = PausePanel.GetComponent<Image>();
        P_red = PausePanel.color.r;
        P_green = PausePanel.color.g;
        P_blue = PausePanel.color.b;
        P_alfa = PausePanel.color.a;
        dessPosition = player.transform.position;
        currentRemain = playerManager.ManagerRemain;

        //�e�L�X�g�֌W�̏�����
        scoreText.SetText("Score:{0}",gameScore);
        remainText.SetText("X {0}",currentRemain);
        coinText.SetText("X {0}", getCoins);

    }

    // Update is called once per frame
    void Update()
    {

        if(player.KEYCOUNT >= 3) {
            keyActiveObj.SetActive(false);
        }

        //Debug.Log(dessPosition);
        if(GameOver) {
            stageCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            //GameOver = true;
            //cinema.enabled = false;
            GameOverActive();
        }
        if(canGetCoins)
        {
            CountSuccession();
        }
        if(!pause.PAUSE && !player.ALLGOAL && !player.FALLING) {
            //cinema.enabled = true;
        } else {
            //cinema.enabled = false;
        }

        if(!pause.PAUSE && !mission.MISSIONFLAG && !player.ALLGOAL && !GameOver)
        {
            CountGameTime();
        }
    }

    void GameClearActive() {
        player.enabled = false;
        
        gameClearThings.SetActive(true);
    }

    IEnumerator WaitGameClearActive() {
        yield return new WaitForSeconds(7.0f);
        GameClearActive();
    }

    void GameOverActive() {
        gameOverThings.SetActive(true);
        gameOverplayer.SetActive(true);
    }

    private void FixedUpdate() {
        if(!fadeIn && (player.FALLING || player.ALLGOAL)) {
            FadeOut();
        }
    }
    public void NameChange() {
        scoreText.SetText("Goal");
    }

    void FadeOut() {
        if(player.FALLING) {
            stageCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            if (!player.ALLGOAL) {
                StartCoroutine(WaitInoti());
            }

            if(P_alfa < 1.0f) {
                P_alfa += fadeSpeed;
                SetAlpha();
            } else if(P_alfa >= 1.0f) {
                fadeIn = true;

            }
        }
        if(player.ALLGOAL) {
            if(P_alfa < 1.0f) {
                P_alfa += fadeSpeed;
                SetAlpha();
            }
            else if(P_alfa >= 1.0f) {
                TitleManager.sceneName = "MojiHyouji";
                SceneManager.LoadScene("LoadScene");
            }
        }

    }

    IEnumerator WaitInoti() {
        if(currentRemain != 0) {
            missText.SetActive(true);
            yield return new WaitForSeconds(2.0f);
            for(int i = 0; i < activeCoins.Count; i++)
            {
                var objects = activeCoins.Dequeue();
                MeshRenderer mesh = objects.GetComponent<MeshRenderer>();
                mesh.enabled = true;
            }
            
            plane.enabled = true;
            
            RevivePlayer();

            missText.SetActive(false);
            zankiIocn.SetActive(true);
            zankiIocn.SetActive(true);
            zankiIconText.text = "�~" + currentRemain;
            List<IEnumerator> ie = new List<IEnumerator>();
            ie.Add(WaitU());
            foreach(IEnumerator item in ie) {
                StartCoroutine(item);

                yield return item.Current;// <===�������d�v
            }
            yield return null;
        } else {
            GameOver = true;
        }
    }

    bool remain = false;

    IEnumerator WaitU() {
        yield return new WaitForSeconds(1.0f);
        if (!remain) {
            //playerManager.ManagerRemain--;
            currentRemain--;
            zankiIconText.text = "�~" + currentRemain;
            remain = true;
        }
        _characterController.enabled = true;
        player.FALLING = false;
        playerInput.enabled = true;

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
            stageCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            fadeIn = false;
            remain = false;
        }
    }

    void SetAlpha() {
        PausePanel.color = new Color(P_red, P_green, P_blue, P_alfa);
    }

    public void AddActiveCoin(GameObject coin)
    {
        activeCoins.Enqueue(coin);
    }

    int rage = 100;
    //�Q�[�����Ԍv�Z
    private void CountGameTime()
    {
        //���Ԍv��
        currentGameTime += Time.deltaTime;
        if(currentGameTime >= 1.0f)
        {
            stageCount -= 1;
            currentGameTime = 0.0f;
        }
        timeImage[0].sprite = numberImage[stageCount / rage];
        timeImage[1].sprite = numberImage[(stageCount % rage) / 10];
        timeImage[2].sprite = numberImage[stageCount % (rage / 10)];
    }

    //�X�R�A�v�Z
    public void AddScore(int score)
    {
        //���ݎ��Ԍv�����s���Ă���Ȃ玞�Ԃ����Z�b�g����
        if(canGetCoins)
        {
            canGetCoinsTime = 0.0f;
        }
        else
        {
            canGetCoins = true;
        }
        //�X�R�A���Z
        var getScore = score * Mathf.Pow(2, getCoinCount);
        gameScore += (int)getScore;
        //�����ŃJ�E���g�𑝉�����i�K���P�{�A�Q�{�A�S�{...�Ƃ��������Ōv�Z����������j
        getCoinCount++;
        scoreText.SetText("Score:{0}", gameScore);
    }

    //�R�C���̉��Z
    public void AddCoins(int price, int score)
    {
        if(!coinAni.GetBool("ShowCoin")) coinAni.SetBool("ShowCoin",true);
        getCoins += price;
        AddScore(score);
        //�����������l�𒴂��Ă����
        if (getCoins >= maxCoins)
        {
            //PlusRemain();
            if(!coinAni.GetBool("ShowRemain"))remainAni.SetBool("ShowRemain", true);
            UpdateRemain(1);
            getCoins -= maxCoins;
        }
        coinText.SetText("X {0}", getCoins);
    }

    //�A���l���\���Ԃ̌v�Z����(�l������X�R�A��2�{�A�S�{�A�W�{...�Ƃ��鏈��)
    private void CountSuccession()
    {
        canGetCoinsTime += 0.02f;
        if(canGetCoinsTime > maxGetCount)
        {
            canGetCoins = false;
            getCoinCount = 0;
            //�A�j���[�V�����Ǘ�
            StartCoroutine(ReSetCoinUI());
        }
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

    //
    public void ClearStage()
    {
        stopGame = true;
    }

    //�c�@�v�Z����
    public void UpdateRemain(int count)
    {
        playerManager.ManagerRemain += count;
        currentRemain += count;
        if (!coinAni.GetBool("ShowRemain")) remainAni.SetBool("ShowRemain", true);
        remainText.SetText("X {0}", currentRemain);
        StartCoroutine("CountCloseRemain");
    }

    //�c�@�������̉��o
    public void Fade()
    {
        //fadeAni.SetTrigger("FadeIn");
        StartCoroutine(CountStartFadeOut());
    }

    //UI��߂�
    private IEnumerator CountCloseRemain()
    {
        yield return new WaitForSeconds(3);
        remainAni.SetBool("ShowRemain",false);
        remainAni.SetTrigger("CloseRemain");
    }

    private IEnumerator ReSetCoinUI()
    {
        yield return new WaitForSeconds(2);
        coinAni.SetBool("ShowCoin", false);
        coinAni.SetTrigger("CloseCoin");
    }

    //�Q�[���ăX�^�[�g
    private IEnumerator CountStartFadeOut()
    {
        yield return new WaitForSeconds(2);
        ///fadeAni.SetTrigger("FadeOut");
        //dontMove = false;
    }
}
