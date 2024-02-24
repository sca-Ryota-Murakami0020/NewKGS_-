using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Example : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;
    public GameObject followObject = null;          // ���_�ƂȂ�I�u�W�F�N�g
    private Vector3 lookPos = Vector3.zero;         // ���ۂɃJ��������������W
    public float lookPlayDistance;                  // ���_�̗V��
    public float followSmooth;                      // �ǂ�������Ƃ��̑��x
    public float cameraDistance;                    // ���_����J�����܂ł̋���
    public float cameraHeight;                      // �f�t�H���g�̃J�����̍���
    public float currentCameraHeight;               // ���݂̃J�����̍���

    public float cameraPlayDiatance;                // ���_����J�����܂ł̋����̗V��
    public float leaveSmooth;                       // �����Ƃ��̑��x

    [SerializeField] private InputActionReference _stickChildAction;    // InputActionAsset�ւ̎Q��
    private bool isCameraAuto = false;              //�E�X�e�B�b�N��G���Ă��邩�ǂ����̃t���O

    // �R�[���o�b�N�̓o�^�E����
    private void Awake()
    {
        _stickChildAction.action.performed += OnMove;
        _stickChildAction.action.canceled += OnMove;
    }

    private void OnDestroy()
    {
        _stickChildAction.action.performed -= OnMove;
        _stickChildAction.action.canceled -= OnMove;
    }

    // InputAction�̗L�����E������
    private void OnEnable() => _stickChildAction.action.Enable();
    private void OnDisable() => _stickChildAction.action.Disable();

    // �R�[���o�b�N�̎���
    private void OnMove(InputAction.CallbackContext context)
    {
        // �X�e�B�b�N�̎qControl�̓��͂��󂯎��
        var childValue = context.ReadValue<float>();

        // �l�����O�o��
        Debug.Log($"childValue: {childValue}");
        //�E�X�e�B�b�N��G���Ă��Ȃ�
        isCameraAuto = true;
    }

    //�E�X�e�B�b�N��G���Ă��Ȃ����ɓ���̓����������J�����������Œ�������
    private void CameraAutoMove()
    {
        if(isCameraAuto)
        {
            //�܂��G��Ȃ�
        }
    }

    void FixedUpdate()
    {
        var gamepad = Gamepad.current;
        if (gamepad == null) return;
        if (followObject == null) return;

        // �Q�[���p�b�h�̍��E�̃X�e�B�b�N�̓��͒l���擾
        var x = gamepad.leftStick.x.ReadValue();
        var y = gamepad.leftStick.y.ReadValue();
        var up = gamepad.rightStick.up.ReadValue();
        var down = gamepad.rightStick.down.ReadValue();
        var left = gamepad.rightStick.left.ReadValue();
        var right = gamepad.rightStick.right.ReadValue();

        // �S�Ă̓��͂����O�o��
        Debug.Log($"x: {x}, y: {y}, up: {up}, down: {down}, left: {left}, right: {right}");
        
        // �Q�[���p�b�h�̍��E�̃X�e�B�b�N�̓��͒l���擾
        var rightStick = gamepad.rightStick.ReadValue();
        var leftStick = gamepad.leftStick.ReadValue();
        
        float rotationX = rightStick.x * followSmooth;
        float rotationY = rightStick.y * followSmooth;
        CameraMove(rotationX,rotationY);

        transform.LookAt(lookPos);
    }

    //�E�X�e�B�b�N�ŃJ�����̓����𑀍삷��
    void CameraMove(float x,float y)
    {
        Vector3 rotation = transform.eulerAngles;

        rotation.y += x;
        rotation.x -= y;

        // ���������̉�]�����͈͓��ɐ�������i�Ⴆ�΁A-90�x����90�x�̊ԁj
        rotation.x = Mathf.Clamp(rotation.x, -90f, 90f);

        // �J�����̉�]��K�p����
        transform.eulerAngles = rotation;
    }
}