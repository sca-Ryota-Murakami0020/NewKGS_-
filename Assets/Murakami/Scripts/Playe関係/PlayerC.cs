using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;
using PathCreation;
using UnityEngine.SceneManagement;
using UnityEngine.ParticleSystemJobs;

//[RequireComponent(typeof(CharacterController))]
public class PlayerC : MonoBehaviour
{
    #region//�ҏW�\�ϐ�
    [Header("�ʏ�̑��x"), SerializeField]
    private float playerSpeed;
    [Header("������̑��x"), SerializeField]
    private float avoidanceSpeed;
    [Header("�_�b�V�����̑��x"), SerializeField]
    private float dashSpeed;   
    [Header("���Ⴊ�ݒ��̑��x"), SerializeField] 
    private float shitSpeed;
    [Header("�����̏���"), SerializeField]
    private float _initFallSpeed;
    [Header("�������̑��������iInfinity�Ŗ������j"), SerializeField]
    private float _fallSpeed;
    [Header("�d�͉����x"), SerializeField]
    private float _gravity;
    [Header("�W�����v����u�Ԃ̑���(�W�����v��)"), SerializeField]
    private float defaultJumpSpeed;
    [Header("�W�����v����g�������̃W�����v��"),SerializeField]
    private float addJumpSpeed;
    [Header("�X���C�_�[���g�������̃W�����v��"),SerializeField]
    private  float sliderSpeed;

    [Header("�Q�[�W�����ڂ���Canvas"),SerializeField]
    private Canvas canvas;

    [Header("�Q�[�}�l"),SerializeField]
    private GameManager gameManager;
    [Header("�A�j���[�V����"),SerializeField]
    private Animator anim = null;
    [Header("�G�t�F�N�g�Ăяo���ꏊ�̃I�u�W�F�N�g"),SerializeField]
    private GameObject popEffectObject;
    [Header("�f�o�t�G�t�F�N�g�̌Ăяo���ʒu"),SerializeField]
    private GameObject effectObject;
    [Header("�Փˎ��̏Ռ��G�t�F�N�g"),SerializeField]
    private ParticleSystem shockEffect;
    [Header("�f�o�t�p�̏Ռ��G�t�F�N�g"),SerializeField]
    private ParticleSystem shockDebufEffect;
    [SerializeField]
    private GameObject shotRayPosition;
    [SerializeField]
    private CharacterController cc;
    #endregion

    #region//�Q�Ƃ���X�N���v�g
    [SerializeField] private GutsGaugeC gutsGaugeC;
    [SerializeField] private AvoidanceC avoidanceC;
    [SerializeField] private CurrentDebufC debufC;
    private PlayerManager playerManager;
    #endregion

    #region//�ϐ�
    //���͂��ꂽ�x�N�g��
    private Vector2 _inputMove;
    //���݂̌����ƍ��W
    private Transform _transform;
    //������̃x�N�g��
    private float _verticalVelocity;
    //���s�p�̃X�s�[�h�̕ϐ�
    private float currentSpeed = 0.0f;
    //���s�p�̃W�����v��
    private float currentJumpPower = 0.0f;
    //��]
    private float _turnVelocity;
    //�؋󎞊�
    private float onAirTime = 0.0f;
    //�������p�̃X�s�[�h�{��
    private float spliteSpeed = 1.0f;
    //�������p�̃W�����v��
    private float spliteJumpSpeed;
    //�������ŃW�����v�����Ƃ��̏d�͂̔{��
    private float spliteGravity = 1.0f;
    //�J�~���Ă��锻��
    //private bool isRain = true;
    //�ڒn����
    private bool _isGroundedPrev;
    //�g�����|�����̐ڐG����
    private bool onTramporin = false;
    //�W�����v�������̔���(�������ɑ؋󃂁[�V�����ɉf���)
    private bool doJump = false;
    //�������ɏ���Ă���
    private bool onSplite = false;
    //�������W�����v�̗������J�n����
    private bool fallStartSplite = false;
    //�������̗̈���ɂ��锻��
    private bool staySplite = false;

    //�X�e�B�b�N�̓���
    [SerializeField] private PlayerInput _playerInput;
    private InputAction _buttonAction;
    private CharacterController _characterController;
    #endregion

    #region//enum�^
    //���[�v�p�̓���
    public enum WarpPlam
    {
        Can,
        Finish,
        StayPortal
    }
    private WarpPlam playerWarpP;
    #endregion

    #region//�v���p�e�B
    public bool IsGrand
    {
        get { return this.isGrounded;}
    }

    public GameObject PopObject
    {
        get { return this.popEffectObject;}
    }

    public WarpPlam PlayerWarpP
    {
        get { return this.playerWarpP;}
        set { this.playerWarpP = value;}
    }
    #endregion

    //�S�[���\���
    private bool canGoal = false;
    //�������������Ă�����
    private bool getedGuardian = false;
    public bool GetedGuardian {
        get { return this.getedGuardian; }
        set { this.getedGuardian = value; }
    }

    public bool CanGoal {
        get { return this.canGoal; }
    }

    [SerializeField] MeshCollider planeCol;


    [SerializeField] PathCreator Path;

    [SerializeField] MissionManager mission;

    bool missio = false;
    public bool MISSIO {
        set {
            missio = value;
        }
        get {
            return missio;
        }
    }
    float missioTime;

    [SerializeField] Animator tranporin;
    [SerializeField] GameObject DamagePanel;

    //���S�ȃS�[������t���O
    bool allGoal = false;

    public bool ALLGOAL {
        set {
            this.allGoal = value;
        }
        get {
            return this.allGoal;
        }
    }

    bool falling = false;
    public bool FALLING {
        set {
            this.falling = value;
        }
        get {
            return falling;
        }
    }

    int KeyCount = 0;
    public int KEYCOUNT {
        set {
            this.KeyCount = value;
        }
        get {
            return this.KeyCount;
        }
    }
    bool silde = false;
    bool isGrounded;
    bool bigJump = false;
    bool getKey = false;
    bool slideFlag = false;
    public bool GETKEY {
        set {
            this.getKey = value; ;
        }
        get {
            return this.getKey;
        }
    }

    [SerializeField] GameObject slopeObj;
    Animator myAnim;
    private void Awake()
    {
        myAnim = this.GetComponent<Animator>();
        myAnim.enabled = true;
        mission = mission.GetComponent<MissionManager>();
        _transform = transform;
        _characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        DamagePanel.SetActive(false);       
        planeCol.enabled = true;
        effectObject.SetActive(false);

        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        //����̉e���𔽉f������
        currentSpeed = playerSpeed * playerManager.DefSpeedMag;
        currentJumpPower = defaultJumpSpeed * playerManager.DefJumpMag;
        //Debug.Log(currentJumpPower);
    }

    float d;
    float P_speed = 15.0f;
    void Update()
    {
        //�؋󎞊Ԍv�Z
        if (!isGrounded) CountOnAir();
        //Debug.Log("�ڒn����" + isGrounded);

        #region//�֒S��
        /*
        if (bigJump) {
            anim.SetTrigger("TrnJump");
            currentJumpPower = addJumpSpeed;
            _fallSpeed = 50f;
            isGrounded = false;
            _verticalVelocity = currentJumpPower;
            anim.SetTrigger("HighJumpAri");
            _verticalVelocity -= _gravity * Time.deltaTime;

            // �������鑬���ȏ�ɂȂ�Ȃ��悤�ɕ␳
            if(_verticalVelocity < -_fallSpeed)
                _verticalVelocity = -_fallSpeed;
            bigJump = false;
        } else {
            _fallSpeed = 30f;
        }
        */
        if (onTramporin) {
            bigJump = true;

            if(bigJump) {
                Debug.Log("tonnda");
                anim.SetTrigger("TrnJump");
                currentJumpPower = addJumpSpeed;
                _fallSpeed = 50f;
                isGrounded = false;
                _verticalVelocity = currentJumpPower;
                anim.SetTrigger("HighJumpAri");
                _verticalVelocity -= _gravity * Time.deltaTime;

                // �������鑬���ȏ�ɂȂ�Ȃ��悤�ɕ␳
                if(_verticalVelocity < -_fallSpeed)
                    _verticalVelocity = -_fallSpeed;
                bigJump = false;
            }

            isGrounded = _characterController.isGrounded;

            if(isGrounded) {
                // ���n����u�Ԃɗ����̏������w�肵�Ă���
                _verticalVelocity = -_initFallSpeed;
            } else if(!isGrounded) {
                // �󒆂ɂ���Ƃ��́A�������ɏd�͉����x��^���ė���������
                _verticalVelocity -= _gravity * Time.deltaTime;

                // �������鑬���ȏ�ɂȂ�Ȃ��悤�ɕ␳
                if(_verticalVelocity < -_fallSpeed)
                    _verticalVelocity = -_fallSpeed;
            }

            _isGroundedPrev = isGrounded;
        }

        if(_characterController.isGrounded && silde) {
            playerSpeed = 6f;
            StartCoroutine(WaitSpeed());
        }

        if(missio) {
            missioTime += Time.deltaTime;
        }
        if(missioTime >= 3.0f) {
            mission.MiSSIONCOUNT++;
            missio = false;
            missioTime = 0;
        }
        if(allGoal && !gameManager.GAMEOVER) {
            _playerInput.enabled = false;

        }
        #endregion
        

        else if(slideFlag) {
            d += P_speed * Time.deltaTime;
            _transform.position = Path.path.GetPointAtDistance(d);
            
        } else {
            //�v���C���[�̏�ԊǗ�
            MovePlayer();
        }
    }

    //�֒S��
    IEnumerator WaitSpeed() {
        yield return new WaitForSeconds(3.0f);
        playerSpeed = 4f;
    }

    //�A�C�e���l��
    public void GetItem(int getScore, string itemName) {
        //�X�R�A���Z
        gameManager.AddScore(getScore);
        switch(itemName) {
            case "Key":
                getKey = true;
                if(mission.MiSSIONCOUNT < 2) {
                    StartCoroutine(WaitKeyFlag());
                }
                if(KeyCount < 4) {
                    KeyCount++;

                } else {

                    KeyCount = 0;
                }

                if(KeyCount == 3) {
                    canGoal = true;
                }
                break;
            case "Gard":
                getedGuardian = true;
                break;
        }
    }

    //�֒S��
    IEnumerator WaitKeyFlag() {
        yield return new WaitForSeconds(3.0f);
        mission.STATMISSION = false;
        mission.YBUTTON = false;
    }

    //�̗̓Q�[�W�̕\�������������Ȃ�Ȃ��l�ɂ��邽�߂ɕK�v
    private void LateUpdate()
    {
        canvas.transform.LookAt(Camera.main.transform.position);
    }

    #region//�{�^������
    //�ړ��A�N�V����
    public void OnMove(InputAction.CallbackContext context)
    {
        _inputMove = context.ReadValue<Vector2>();
        //�x�N�g���̓���
        if(_inputMove != Vector2.zero)
        {
            //�n��ɂ���Ȃ�
            if(isGrounded) anim.SetBool("InputVec", true);
            //�󒆂ɂ���Ƃ�or�������Ȃ�
            else anim.SetTrigger("OutGround");
        }
        //��~
        else anim.SetBool("InputVec", false);
    }

    //�W�����v�A�N�V����
    public void OnJump(InputAction.CallbackContext context)
    {       
        if(silde)
        {
            anim.SetTrigger("TrnJump");
            DOTween.KillAll();
            currentJumpPower = sliderSpeed;
            playerSpeed = 10.0f;
            _verticalVelocity = currentJumpPower;         
            
            silde = false;
        }

        //�n��or�g�����|�����ɏ���Ă��鎞�܂��͉�𒆈ȊO������������!context.performed || 
        if (!_characterController.isGrounded)
        {          
            //Debug.Log("��ׂȂ���");
            return;
        }

        //�n�ʂł̃W�����v
        if (avoidanceC.AvoiP != AvoidanceC.AvoiPlam.Doing && jumpCount == 0)
        {
            //��������ł̃W�����v
            if(onSplite && _inputMove != Vector2.zero)
            {
                Debug.Log("�������ɂ��W�����v");
                anim.SetTrigger("PanelJump");
            }
            //�g�����|������̃W�����v��
            else if (onTramporin) anim.SetTrigger("TrnJump");
            //�ʏ�̃W�����v
            else anim.SetTrigger("InputJump");
            doJump = true;
            jumpCount = 1;
        }
    }

    //���
    public void OnAvoidance(InputAction.CallbackContext context)
    {
        if(!context.performed || !_characterController.isGrounded) return;
        if(avoidanceC.AvoiP == AvoidanceC.AvoiPlam.CanUse)
        {
            anim.SetTrigger("StartRoll");
            StartAvoidance();
        }
    }
 
    //�_�b�V���J�n
    public void OnDash(InputAction.CallbackContext context)
    {
        if (!context.performed || 
            !_characterController.isGrounded || 
            gutsGaugeC.GPlam == GutsGaugeC.GutsPlam.CoolTime)
            return;
        StartDash();
    }

    //�_�b�V���I��
    public void EndDash(InputAction.CallbackContext context)
    {
        if(!context.performed || gutsGaugeC.GPlam != GutsGaugeC.GutsPlam.Doing) return;
        EndDash();
    }
    #endregion


    #region//�s������

    //�s������
    private void MovePlayer()
    {
        _buttonAction = _playerInput.actions.FindAction("Dash");
        _buttonAction = _playerInput.actions.FindAction("Jump");
        var isGrounded = _characterController.isGrounded;

        #region//��������
        if (!onSplite)
        {
            // ���n����u�Ԃɗ����̏������w�肵�Ă���  
            if (isGrounded && !_isGroundedPrev)
            {
                _verticalVelocity = -_initFallSpeed;
            }

            //�ʏ�̗���&& !onSplite && !fallStartSplite
            else
            {
                // �󒆂ɂ���Ƃ��́A�������ɏd�͉����x��^���ė���������
                _verticalVelocity -= _gravity * Time.deltaTime;

                // �������鑬���ȏ�ɂȂ�Ȃ��悤�ɕ␳
                if (_verticalVelocity < -_fallSpeed) _verticalVelocity = -_fallSpeed;
            }
        }

        //���������g�p�����Ƃ��̃W�����v�������Ƃ� if(!isGrounded && onSplite && jumpCount == 1 && fallStartSplite)
        else
        {
            // �󒆂ɂ���Ƃ��́A�������ɏd�͉����x��^���ė���������
            _verticalVelocity -= (_gravity * Time.deltaTime) * spliteGravity;
            var maxGravity = _fallSpeed * spliteGravity;

            // �������鑬���ȏ�ɂȂ�Ȃ��悤�ɕ␳
            if (_verticalVelocity < -maxGravity) _verticalVelocity = -maxGravity;
        }
        #endregion

        #region//�s������
        //�ʏ�̈ړ�
        if (gutsGaugeC.GPlam != GutsGaugeC.GutsPlam.Doing)
        {
            //�������ɏ���Ă��鎞
            currentSpeed = playerSpeed * debufC.MoveDebufMag;
            PlayerMove(currentSpeed);
        }

        //�_�b�V���� && canMovePlayer == MovePlayerPlam.Can
        else if (gutsGaugeC.GPlam == GutsGaugeC.GutsPlam.Doing)
        {
            //�X�e�B�b�N�̓��͂�����Ƃ��̂ݏ������s��
            if (_inputMove != Vector2.zero)
            {
                if(!onSplite) currentSpeed = (dashSpeed * playerManager.DefSpeedMag) * spliteSpeed;
                else currentSpeed = dashSpeed * debufC.MoveDebufMag;
                PlayerMove(currentSpeed);
            }
            //�{�^������ON�A�X�e�B�b�N����NO�����X�^�~�i�������������ă_�b�V�����I������
            else EndDash();
        }

        //��������͂��ꂽ��
        else if (avoidanceC.AvoiP == AvoidanceC.AvoiPlam.Doing
            && _inputMove != Vector2.zero)
        {
            currentSpeed = avoidanceSpeed; //* debufC.MoveDebufMag;
            PlayerMove(currentSpeed);
        }            
        #endregion

        _isGroundedPrev = isGrounded; 
    }

    //�W�����v����
    public void JumpPlayer()
    {
        _verticalVelocity = currentJumpPower * debufC.JumpDebufMag;
        //Debug.Log("�W�����v�����̒l�F" + _verticalVelocity);
        //_characterController.height = 0.5f;
    }

    //�g�����|�����W�����v
    public void TrnJumpPlayer() 
        => _verticalVelocity = (currentJumpPower * 2.0f) * debufC.JumpDebufMag;

    #region//�������W�����v
    public void SpliteJump()
    {
        float pow = 0.0f;
        if (jumpCount == 0)
        {
            pow = currentJumpPower * spliteJumpSpeed;
            _verticalVelocity = pow;
        }
        StartCoroutine(StartFallSplite());
    }

    private IEnumerator StartFallSplite()
    {
        yield return new WaitForSeconds(10.0f);
        fallStartSplite = true;
    }
    #endregion

    //�ړ�����
    private void PlayerMove(float speed)//, Vector3 cameraVec
    {
        //�J�����̊p�x���擾����
        var cameraAngleY = Camera.main.transform.eulerAngles.y;

        Vector3 moveVelocity = new Vector3(
               _inputMove.x * speed,
               _verticalVelocity,
               _inputMove.y * speed
           );

        moveVelocity = Quaternion.Euler(0,cameraAngleY,0) * moveVelocity;
        // ���݃t���[���̈ړ��ʂ��ړ����x����v�Z
        var moveDelta = moveVelocity * Time.deltaTime;//* cameraVecF 
        //Debug.DrawRay(this.transform.position, moveDelta, Color.red, 1.0f);
        //�A�j���[�V����
        anim.SetFloat("InputSpeed", _inputMove.magnitude, 0.1f, Time.deltaTime);
        //Debug.Log("�W�����v�����̒l�F" + _verticalVelocity);
        // CharacterController�Ɉړ��ʂ��w�肵�A�I�u�W�F�N�g�𓮂���
        _characterController.Move(moveDelta);

        //��]�̍X�V
        if (_inputMove != Vector2.zero)
        {
            // �ړ����͂�����ꍇ�́A�U�����������s��
            // ������͂���y������̖ڕW�p�x[deg]���v�Z
            var targetAngleY = 0.0f;
            targetAngleY = -Mathf.Atan2(_inputMove.y, _inputMove.x) * Mathf.Rad2Deg + 90;
            targetAngleY += cameraAngleY;
            // �C�[�W���O���Ȃ��玟�̉�]�p�x[deg]���v�Z
            var angleY = Mathf.SmoothDampAngle(
                _transform.eulerAngles.y,
                targetAngleY,
                ref _turnVelocity,
                0.1f
            );

            // �I�u�W�F�N�g�̉�]���X�V
            _transform.rotation = Quaternion.Euler(0, angleY, 0);
        }
    }

    #region//��𔻒�
    //���G���������
    public void StartAvoidance()
    {
        avoidanceC.AvoiP = AvoidanceC.AvoiPlam.Doing;
        avoidanceC.UsingAvoidanceGauge();
    }

    //���G�������������
    public void EndAvoidance()
    {
        avoidanceC.AvoiP = AvoidanceC.AvoiPlam.CoolTime;
        anim.SetTrigger("EndRoll");
    }
    #endregion

    #region//�_�b�V������
    //�_�b�V���̊J�n
    public void StartDash()
    {
        gutsGaugeC.GPlam = GutsGaugeC.GutsPlam.Doing;
        anim.SetBool("OnDash", true);
    }

    //�_�b�V���̏I��
    public void EndDash()
    {
        gutsGaugeC.JugeStamina();
        anim.SetBool("OnDash",false);
    }
    #endregion

    int jumpCount = 0;
    
    int co = 0;

    [SerializeField] AudioSource sound;
    [SerializeField] AudioClip DamageSound;

    //�ڐG����
    private void OnTriggerEnter(Collider col)
    {       
        if(col.tag == "Ice") {
            slideFlag = true;
            _playerInput.enabled = false;
            myAnim.enabled = false;
            slopeObj.SetActive(true);
            this.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }

        //�g�����|����
        if(col.tag == "Tramprin")
        {
            tranporin.SetBool("Tranporin", true);
            isGrounded = false;
            onTramporin = true;
        }

        //�g���b�v
        if(col.tag == "Trap" && avoidanceC.AvoiP != AvoidanceC.AvoiPlam.Doing)
        {
            TrapC trapC = col.gameObject.GetComponent<TrapC>();
            switch (trapC.DebufKinds)
            {
                //�ړ��p�f�o�t
                case TrapC.DebufKind.DefMove:
                    debufC.ActiveMoveDebuf(trapC.DebufTime, trapC.DebufMag);
                    break;
                //�W�����v�p�f�o�t
                case TrapC.DebufKind.DefJump:
                    debufC.ActiveJumpDebuf(trapC.DebufTime, trapC.DebufMag);
                    break;
            }
        }

        //���Ƃ���
        if (col.tag == "holl")
        {
            falling = true;
            
            planeCol.enabled = false;
            
            _playerInput.enabled = false;
            StartCoroutine(WaitChara());
        }

        if(col.tag == "mission") {

            mission.KeyActive(mission.RADOMMISSIONCOUNT);
            if(mission.RADOMMISSIONCOUNT != 2) {
                mission.MISSIONVALUE[mission.RADOMMISSIONCOUNT]++;
            }
            if(mission.MiSSIONCOUNT != 3) {
                missio = true;
            }

            col.gameObject.SetActive(false);
        }

        if(col.tag == "Enemy") {
            co = 1;
            sound.PlayOneShot(DamageSound);
            falling = true;
            //StartCoroutine(WaitFall());
            SpornShockEffect();
            _playerInput.enabled = false;
            StartCoroutine(WaitChara());
        }

        if(col.tag == "Car") {
            if(gameManager.CurrentRemain != 0) {
                falling = true;
                SpornShockEffect();
                _playerInput.enabled = false;
            }
        }

        if(col.tag == "slope") {
            myAnim.enabled = true;
            _playerInput.enabled = true;
            slopeObj.SetActive(false);
            this.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            slideFlag = false;          
        }

        //������
        if(col.tag == "SpritPanel")
        {
            onSplite = true;
            SpliteC spliteC = col.gameObject.GetComponent<SpliteC>();
            spliteSpeed = spliteC.AddSpeedMag;
            spliteJumpSpeed = spliteC.AddJumpMag;
            spliteGravity = spliteC.SubGravity;
            Debug.Log("�������ɐG�ꂽ");
        }

        //���[�v�|�[�^��(�o���Ƃ���͔͈͓��Ȃ̂ŏo��܂ł̓��[�v�ł��Ȃ��l�ɂ���)
        if(col.tag == "warp" && playerWarpP == WarpPlam.Can)
        {
            WarpC warpC = col.gameObject.GetComponent<WarpC>();
            Vector3 portalPos = warpC.RollsignPortal.transform.position;
            Quaternion portalRot = warpC.RollsignPortal.transform.rotation;
            //Debug.Log($"�n�_�F{portalPos},��]�F{portalRot}");
            _playerInput.enabled = false;
            cc.enabled = false;
            WarpPlayerEffect(this.popEffectObject.transform.position);
            StartCoroutine(WarpPlayer(portalPos, portalRot));
        }
    }

    //�s��̃��[�v�|�[�^���ɔ�΂�
    private IEnumerator WarpPlayer(Vector3 pos, Quaternion rot)
    {
        yield return new WaitForSeconds(1.0f);
        playerWarpP = WarpPlam.Finish;
        this.transform.position = pos;
        this.transform.rotation = rot;
        cc.enabled = true;
        _playerInput.enabled = true;
        yield return new WaitForSeconds(0.3f);
        WarpPlayerEffect(this.popEffectObject.transform.position);

        playerWarpP = WarpPlam.StayPortal;
    }

    [Header("���[�v�p�G�t�F�N�g"),SerializeField]
    private ParticleSystem warpEffect;
    [Header("�\���Ǘ�����Ώۂ̃��f��"),SerializeField]
    private GameObject _model;
    //���[�v�p�̃G�t�F�N�g��\���B�v���C���[�̃��f�����\��or�\������
    private void WarpPlayerEffect(Vector3 pos)
    {
        ParticleSystem effect = Instantiate(warpEffect);
        effect.transform.position = pos;
        effect.Play();
        if(this._model.activeSelf) this._model.SetActive(false);
        else this._model.SetActive(true);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "SpritPanel")
            if(!staySplite) staySplite = true;
    }

    IEnumerator WaitChara() {
        yield return new WaitForSeconds(1.0f);
        _characterController.enabled = false;
    }

    IEnumerator WaitFall() {
        yield return null;

        while(co != 0) {

            DamagePanel.SetActive(false);

            yield return new WaitForSeconds(0.15f);

            DamagePanel.SetActive(true);

            yield return new WaitForSeconds(0.15f);
            co--;
        }
        DamagePanel.SetActive(false);
        falling = true;
        yield break;
    }

    private void OnTriggerExit(Collider col)
    {
        if(col.tag == "Tramprin") {
            StartCoroutine(WaitJump());
            onTramporin = false;
        }
        //������
        if(col.tag == "SpritPanel")
            staySplite = false;

        if(col.tag == "warp" && playerWarpP == WarpPlam.StayPortal)
            StartCoroutine(ChangeWarpPlam());
    }

    //���[�v�|�[�^�����痣�ꂽ���Ԃ̌v�Z
    private IEnumerator ChangeWarpPlam()
    {
        yield return null;
        playerWarpP = WarpPlam.Can;
        //Debug.Log("�S�Ẵ��[�v�H���̊���");
    }

    IEnumerator WaitJump() {
        yield return new WaitForSeconds(1f);
        tranporin.SetBool("Tranporin", false);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //�n�ʂւ̒��n�A�j���[�V�������Ăяo�� && !_characterController.isGrounded
        if (hit.gameObject.tag == "Ground" || hit.gameObject.tag == "SpritPanel")
        {
            isGrounded = _characterController.isGrounded;
            //�W�����v���ė������ꍇ
            if(jumpCount != 0)
            {
                _characterController.height = 0.95f;
                //�󒆑ҋ@���Ԃ����l�ȉ��Ȃ�
                if (onAirTime <= 2.0f && !onSplite)
                    anim.SetTrigger("Landing");
                //���l�ȏ�A�܂��͉������W�����v�ł̒��n�Ȃ�
                else if (onAirTime > 2.0f || onSplite)
                    anim.SetTrigger("HighLanding");

                if (onSplite)
                {
                    onSplite = false;
                    StartCoroutine(ResetMag());
                }
            }

            else
            {
                if(hit.gameObject.tag == "Ground" && !staySplite)
                {
                    _characterController.height = 0.95f;
                    //�󒆑ҋ@���Ԃ����l�ȉ��Ȃ�
                    if (onAirTime <= 2.0f && !onSplite)
                        anim.SetTrigger("Landing");
                    //���l�ȏ�A�܂��͉������W�����v�ł̒��n�Ȃ�
                    else if (onAirTime > 2.0f || onSplite)
                        anim.SetTrigger("HighLanding");

                    if (onSplite)
                    {
                        onSplite = false;
                        StartCoroutine(ResetMag());
                    }
                }
            }
            if(onTramporin) onTramporin =false;
            if(currentJumpPower != defaultJumpSpeed * playerManager.DefJumpMag)
            {
                currentJumpPower = defaultJumpSpeed * playerManager.DefJumpMag;
            }
            onAirTime = 0.0f;
            doJump = false;
            jumpCount = 0;
        }

        //�S�[��
        if (hit.gameObject.tag == "Goal" && canGoal)
        {
            SousaUIContorller.stageClear++;
            allGoal = true;
            gameManager.NameChange();
        }
    }

    //�G�t�F�N�g�̌Ăяo��
    public void PopDebufEffect()
        =>effectObject.SetActive(true);

    //�G�t�F�N�g�̍폜
    public void DeleteDebufEffect()
        =>effectObject.SetActive(false);

    //�Ռ��G�t�F�N�g�̌Ăяo��
    private void SpornShockEffect()
    {
        Vector3 pos = this.popEffectObject.transform.position;
        ParticleSystem effect = Instantiate(shockEffect);
        effect.transform.position = pos;
        effect.Play();
    }

    //�f�o�t�p�Փ˃G�t�F�N�g�̌Ăяo��
    private void SpornShockDebufEffect()
    {
        Vector3 pos = this.popEffectObject.transform.position;
        ParticleSystem effect = Instantiate(shockDebufEffect);
        effect.transform.position = pos;
        effect.Play();
    }

    private IEnumerator ResetMag()
    {
        yield return new WaitForSeconds(2);
        spliteJumpSpeed = 1.0f;
        spliteSpeed = 1.0f;
        spliteGravity = 1.0f;
    }
    #endregion
   
    //�؋󎞊Ԍv�Z
    private void CountOnAir()
        => onAirTime += Time.deltaTime;
}
