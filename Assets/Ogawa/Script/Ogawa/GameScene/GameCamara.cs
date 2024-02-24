using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameCamara : MonoBehaviour
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

    // InputActionAsset�ւ̎Q��
    [SerializeField] private InputActionReference _moveAction;

    // �R�[���o�b�N�̓o�^�E����
    private void Awake()
    {
        // ���͒l��0�ȊO�̒l�ɕω������Ƃ��ɌĂяo�����R�[���o�b�N
        _moveAction.action.performed += OnMove;

        // ���͒l��0�ɖ߂����Ƃ��ɌĂяo�����R�[���o�b�N
        _moveAction.action.canceled += OnMove;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (followObject == null) return;

        //UpdateLookPosition();
        //UpdateCameraPosition();
        CameraMove();

        transform.LookAt(lookPos);
    }

    private void OnDestroy()
    {
        _moveAction.action.performed -= OnMove;
        _moveAction.action.canceled -= OnMove;
    }

    // InputAction�̗L�����E������
    private void OnEnable() => _moveAction.action.Enable();
    private void OnDisable() => _moveAction.action.Disable();

    // �R�[���o�b�N�̎���
    private void OnMove(InputAction.CallbackContext context)
    {
        // 2�����͂��󂯎��
        var move = context.ReadValue<Vector2>();

        // 2�����͂̒l��\��
        Debug.Log($"move: {move}");
    }

    /*void UpdateLookPosition()
    {
        // �ڕW�̎��_�ƌ��݂̎��_�̋��������߂�
        Vector3 vec = followObject.transform.position - lookPos;
        float distance = vec.magnitude;

        if (distance > lookPlayDistance)
        {   // �V�т̋����𒴂��Ă�����ڕW�̎��_�ɋ߂Â���
            float move_distance = (distance - lookPlayDistance) * (Time.deltaTime * followSmooth);
            lookPos += vec.normalized * move_distance;
        }
    }

    void UpdateCameraPosition()
    {
        // XZ���ʂɂ�����J�����Ǝ��_�̋������擾����
        Vector3 xz_vec = followObject.transform.position - transform.position;
        xz_vec.y = 0;
        float distance = xz_vec.magnitude;

        // �J�����̈ړ����������߂�
        float move_distance = 0;
        if (distance > cameraDistance + cameraPlayDiatance)
        {   // �J�������V�т𒴂��ė��ꂽ��ǂ�������
            move_distance = distance - (cameraDistance + cameraPlayDiatance);
            move_distance *= Time.deltaTime * followSmooth;
        }
        else if (distance < cameraDistance - cameraPlayDiatance)
        {   // �J�������V�т𒴂��ċ߂Â����痣���
            move_distance = distance - (cameraDistance - cameraPlayDiatance);
            move_distance *= Time.deltaTime * leaveSmooth;
        }

        // �V�����J�����̈ʒu�����߂�
        Vector3 camera_pos = transform.position + (xz_vec.normalized * move_distance);

        // �����͏�Ɍ��݂̎��_����̈��̍������ێ�����
        camera_pos.y = lookPos.y + currentCameraHeight;

        transform.position = camera_pos;
    }*/

    void CameraMove()
    {
        // �Q�[���p�b�h�i�f�o�C�X�擾�j
        var gamepad = Gamepad.current;
        if (gamepad == null) return;

        // �Q�[���p�b�h�̍��E�̃X�e�B�b�N�̓��͒l���擾
        var x = gamepad.leftStick.x.ReadValue();
        var y = gamepad.leftStick.y.ReadValue();
        var up = gamepad.rightStick.up.ReadValue();
        var down = gamepad.rightStick.down.ReadValue();
        var left = gamepad.rightStick.left.ReadValue();
        var right = gamepad.rightStick.right.ReadValue();

        // �S�Ă̓��͂����O�o��
        print($"x: {x}, y: {y}, up: {up}, down: {down}, left: {left}, right: {right}");


        // �Q�[���p�b�h�̉E�̃X�e�B�b�N�̓��͒l���擾
        var rightStick = gamepad.rightStick.ReadValue();



        transform.LookAt(lookPos);

    }

}
