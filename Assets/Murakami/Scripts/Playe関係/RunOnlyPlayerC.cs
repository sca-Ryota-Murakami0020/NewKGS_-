using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RunOnlyPlayerC : MonoBehaviour
{
    bool goal = false;
    public bool GOAL {
        set {
            this.goal = value;
        }
        get {
            return this.goal;
        }
    }
    [Header("���p�X�N���v�g"),SerializeField]
    private AvoidanceC avoC;
    [SerializeField]
    private ThirdStageGM thirdGM;
    [SerializeField]
    private ParticleSystem shockEffect;

    #region//�X�s�[�h���̃p�����[�^�֌W
    [SerializeField] private Animator anim;
    //[SerializeField] private PlayerInput playerInput;
    [SerializeField] private float _gravity;
    [SerializeField] private float inputFallSpeed;
    [SerializeField] private float fallSpeed;

    private InputAction buttonAction;
    private float jumpPow = 0.0f;
    private float inputJumpVelocity = 0.0f;
    private int jumpCount = 0;
    private bool onGround = false;
    public bool ONGROUND {
        set {
            this.onGround = value;
        }
        get {
            return this.onGround;
        }
    }

    private bool hitGround = false;
    private bool isFall = false;

    private Vector2 inputMoveVelocity = Vector2.zero;
    #endregion

    [SerializeField] Rigidbody rb;
    bool y = false;

    [SerializeField] GameObject goalPos;
    bool warp = false;
    public bool WARP {
        set {
            this.warp = value;
        }
        get {
            return this.warp;
        }
    }    

    [SerializeField] MeshCollider plane;
    [SerializeField] KakoPauseManager pause;
    [SerializeField] ThirdStageGM gameManager;
    // Start is called before the first frame update
    void Start()
    {
        jumpPow = thirdGM.PlayerJumpPow;
    }

    [SerializeField]float jumpForce = 10000.0f;

    bool fall = false;
    public bool FALLING {
        set {
            this.fall = value;
        }
        get {
            return this.fall;
        }
    }

    [SerializeField] GameObject warpObj;

    // Update is called once per frame
    void Update()
    {
       
        //inputMoveVelocity = new Vector2(aa,bb);
       
        /*�f�o�b�N�p
        if(Input.GetKeyDown(KeyCode.I)) {
            StageSelectController.mode = StageSelectController.MODE.CHALLENGE;
        }
        */
        if(!goal && !fall && !pause.PAUSE) {
           MoveObjects();
            if(Gamepad.current.aButton.wasPressedThisFrame) {
                y = true;
                Debug.Log("�Ăяo��");
                anim.SetTrigger("DoJump");
                onGround = false;
            }

            if (y && myPos.y < 5.0f && !isFall) this.rb.AddForce(transform.up * jumpForce * Time.deltaTime);

            if (y && myPos.y > 5.0f)
            {
                isFall = true;
                this.rb.AddForce(transform.up * -jumpForce * 2 * Time.deltaTime) ;
            }


            if (onGround) {
                y = false;
                isFall = false;
            }
        } else if(goal && !fall) {
            Vector3 current = this.transform.position;
            Vector3 target = goalPos.transform.position;
            float step = 10.0f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(current, target, step);
        }
        
       //}
    }


    //�ړ����͏���
    /*
    public void OnMove(InputAction.CallbackContext context)
    {      
        inputMoveVelocity = context.ReadValue<Vector2>();
    }*/

    public void OnMove(InputValue c)
    {
         Debug.Log("yonda");
        inputMoveVelocity = c.Get<Vector2>();
        if(inputMoveVelocity == Vector2.zero) return;
    }

    //�W�����v���͏���
    public void OnJump(InputAction.CallbackContext context)
    {
        if(!context.performed || jumpCount != 0)
            return;
        if(avoC.AvoiP != AvoidanceC.AvoiPlam.Doing
            && jumpCount == 0)
        {
            Debug.Log("�W�����v");
            jumpCount += 1;
            anim.SetTrigger("DoJump");
        }
    }

    //�����͏���
    public void OnAvoidance(InputAction.CallbackContext context)
    {
        if(!context.performed || !onGround) return;
        if(avoC.AvoiP == AvoidanceC.AvoiPlam.CanUse)
        {
            anim.SetTrigger("StartRoll");
            StartAvoidance();
        }
    }

    //��𔻒�̎擾
    public void StartAvoidance()
    {
        avoC.AvoiP = AvoidanceC.AvoiPlam.Doing;
        avoC.UsingAvoidanceGauge();
    }
    //��𔻒�̉���
    public void EndAvoidance()
    {
        avoC.AvoiP = AvoidanceC.AvoiPlam.CoolTime;
        anim.SetTrigger("EndRoll");
    }

    //�W�����v����
    private void JumpRunPlayer()
    {
        inputJumpVelocity = jumpPow;
    }

  
    Vector3 myPos;
    
    [SerializeField] private float speed;
    bool jump = false;
    //�v���C���[�̑O�i����
    private void MoveObjects()
    {
        //buttonAction = playerInput.actions.FindAction("Move");
        /*
        if(onGround && !hitGround)
        {
            inputJumpVelocity = -inputFallSpeed;
            Debug.Log("�����ݒ�");
        }
        if(!onGround)
        {
            inputJumpVelocity = -_gravity * Time.deltaTime;
            if(inputJumpVelocity < -fallSpeed) inputJumpVelocity = -fallSpeed;
            Debug.Log("����");
        }*/
        myPos = this.transform.position;
        //�{�^�����͏���
        if(onGround) { 
        inputMoveVelocity.x = Input.GetAxis("Horizontal");
        if(inputMoveVelocity.x > 0.1 && myPos.x <= 6.0f) {
            myPos.x += 0.08f * Time.deltaTime * speed;
               // Debug.Log("����y" + inputMoveVelocity.x);
            //this.transform.position += new Vector3(inputMoveVelocity.x, inputJumpVelocity, playerSpeed) * playerSpeed;
        }
        else if(inputMoveVelocity.x < -0.1 && myPos.x >= -6.0f) {
            myPos.x -= 0.08f * Time.deltaTime * speed;
               // Debug.Log("����y" + inputMoveVelocity.x);
            }
        }
        myPos.z += 0.1f * Time.deltaTime * speed;

       // if(Input.GetKeyDown("joystick button 0")) {
            //myRigidbody.useGravity = false;
         //   if(onGround) {
           //     jump = true;
                
            //}
            //JumpRunPlayer();
        //}
        if(jump && myPos.y < 5.0f) {
            //this.rb.AddForce(transform.up * jumpForce);
        }
        else if(myPos.y > 5.0f) {
            jump = false;
        }
        else if(!jump) {
            //this.rb.AddForce(transform.up * jumpForce);
        }
        /*
        if(myPos.y > 10.0f && !onGround) {
            jump = false;
            myPos.y -= 10.0f * Time.deltaTime;
        }
        */
        this.transform.position = myPos;
        // var bb = Input.GetAxis("Vertical");
        //if(inputMoveVelocity.x <= 0.9f && inputMoveVelocity.x >= -0.9f) {
        //if(myPos.x > -7.6f && myPos.x < 8.05f) {
        //_velocity = new Vector3(inputMoveVelocity.x, _rigidbody.velocity.y, playerSpeed).normalized;

        //}

        //}

        //Debug.Log(inputMoveVelocity.x);

        if(!onGround)
        {
            inputJumpVelocity -= _gravity * Time.deltaTime;
            if(inputJumpVelocity <= 0.0f) inputJumpVelocity = 0.0f;
        } 
        
    }

    [SerializeField] AudioSource sound;
    [SerializeField] AudioClip damageSound;

    //�ڐG����
    private void OnCollisionEnter(Collision collision)
    {
        //�n��
        if(collision.gameObject.tag == "Ground")
        {
            onGround = true;
            hitGround = true;
            Debug.Log("���n" + onGround);
            anim.SetTrigger("Landing");
            //myRigidbody.useGravity = true;
        }           
        //��������̃M�~�b�N�Ɉ�������������
        if(collision.gameObject.tag == "Trap")
        {

        }

        if(collision.gameObject.tag == "Enemy") {
            sound.PlayOneShot(damageSound);
            SpornShockEffect();
            fall = true;
        }
    }

    //���ꂽ����
    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            onGround = false;
            hitGround = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Goal")
        {
            warpObj.SetActive(true);
            goal = true;
            rb.useGravity = false;
            //Debug.Log("omedetou");
            //thirdGM.StageClear();

        }
        if(other.gameObject.tag == "warp") {
            warp = true;
        }
        if(other.gameObject.tag == "holl") {
            fall = true;
            plane.enabled = false;
        }
    }

    private void SpornShockEffect()
    {
        Vector3 pos = this.gameObject.transform.position;
        ParticleSystem effect = Instantiate(shockEffect);
        effect.transform.position = pos;
        effect.Play();
    }
}