// 2021.02 K1Togami Follow Camera
// Base on CameraControllercsv2.1.cs in https://ruhrnuklear.de/fcc/

using UnityEngine;

public class FollowCamera : MonoBehaviour
{

    public Transform target; // �^�[�Q�b�g
    private float targetHeight = 5.0f;

    public float distance = 10.0f;
    public float horizontalAngle = 0.0f;
    public float verticalAngle = 10.0f;

    // �J�����̈ړ����E
    public float verticalAngleMinLimit = -30f; // ���グ���E�p�x
    public float verticalAngleMaxLimit = 80f; // �����낵���E�p�x 
    public float maxDistance = 20f; // �ő�Y�[������ 
    public float minDistance = 0.6f; // �ŏ��Y�[������ 

    //public Vector3 offset = Vector3.zero; // �^�[�Q�b�g�ƃJ�����̃I�t�Z�b�g

    public float rotationSpeed = 180.0f; // ��ʂ̉������J�[�\�����ړ��������Ƃ����x��]���邩.
    public float rotationDampening = 0.5f; // ��]�̌������x (higher = faster) 
    public float zoomDampening = 5.0f; // Auto Zoom speed (Higher = faster) 

    // �Փˌ��m�p
    public LayerMask collisionLayers = -1; // What the camera will collide with 
    public float offsetFromWall = 0.1f; // �Փ˂��镨�̂���J��������������Ƃ��̃I�t�Z�b�g 


    private float currentDistance; // ���݂̃J��������
    private float desiredDistance; // �ڕW�Ƃ���J��������
    private float correctedDistance; // ������̃J��������

    private float CameraFollowDelay; // �J������]��̃J�����t�H���[�܂ł̒x��

    // ���[�U�ɂ���]�̋���
    public bool allowMouseInput = true; // �J�����̕������}�E�X�ŃR���g���[�����邱�Ƃ������邩�B

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        horizontalAngle = angles.x;
        verticalAngle = angles.y;

        currentDistance = distance;
        desiredDistance = distance;
        correctedDistance = distance;

        CameraFollowDelay = 0f;
    }

    void LateUpdate()
    {
        // �^�[�Q�b�g����`����Ă��Ȃ��ꍇ�͉������Ȃ�
        if (target == null)
            return;

        Vector3 vTargetOffset; // �^�[�Q�b�g����̃I�t�Z�b�g

        if (GUIUtility.hotControl == 0)
        {
            // �}�E�X���͂�������Ă��邩�ǂ������m�F����
            if (allowMouseInput)
            {
                // �}�E�X�̉E�{�^���������Ȃ���}�E�X�𓮂����ƁA���_��ύX�ł���B
                if (Input.GetMouseButton(1))
                {
                    horizontalAngle += Input.GetAxis("Mouse X") * rotationSpeed * 0.02f;
                    verticalAngle -= Input.GetAxis("Mouse Y") * rotationSpeed * 0.02f;
                    CameraFollowDelay = 1.0f;
                }
            }

            // �R���g���[���̉E�X�e�B�b�N�ɂ���]������ꍇ�́A�����ɓ����B
            // ��]���x�͂��D�݂ɒ������Ă��������B
            //var controllerHorizonal = Input.GetAxis("Horizontal_RS");
            //horizontalAngle += controllerHorizonal * rotationSpeed * 0.001f;
            //verticalAngle += Input.GetAxis("Vertical_RS") * rotationSpeed * 0.001f;
            //if (controllerHorizonal != 0f ) CameraFollowDelay = 1.0f;


            if (CameraFollowDelay > 0f)
            {
                CameraFollowDelay -= Time.deltaTime;
            } else
            {
                // �}�E�X�ɂ���]�������̏ꍇ�A�J�����������^�[�Q�b�g�̎����ɂ��킶�킠�킹��
                RotateBehindTarget();
            }

            verticalAngle = ClampAngle(verticalAngle, verticalAngleMinLimit, verticalAngleMaxLimit);

            // �J�����̌�����ݒ�
            Quaternion rotation = Quaternion.Euler( verticalAngle,horizontalAngle, 0);

            // ��]�̃J�����ʒu���v�Z
            vTargetOffset = new Vector3(0, -targetHeight, 0);
            Vector3 position = target.transform.position - (rotation * Vector3.forward * desiredDistance + vTargetOffset);

            // �������g���ă��[�U�[���ݒ肵���^�̃^�[�Q�b�g�̊�]�̓o�^�_���g���ďՓ˂��`�F�b�N
            RaycastHit collisionHit;
            Vector3 trueTargetPosition = new Vector3(target.transform.position.x,
                target.transform.position.y + targetHeight, target.transform.position.z);

            // �Փ˂��������ꍇ�́A�J�����ʒu��␳���A�␳��̋������v�Z
            var isCorrected = false;
            if (Physics.Linecast(trueTargetPosition, position, out collisionHit, collisionLayers))
            {
                // ���̐���ʒu����Փˈʒu�܂ł̋������v�Z���A�Փ˂������̂�����S�ȁu�I�t�Z�b�g�v��������������
                // ���̃I�t�Z�b�g�́A�J�������q�b�g�����ʂ̐^��ɂ��Ȃ��悤����������
                correctedDistance = Vector3.Distance(trueTargetPosition, collisionHit.point) - offsetFromWall;
                isCorrected = true;
            }

            // �X���[�W���O�̂��߂ɁA�������␳����Ă��Ȃ����A�܂��͕␳���ꂽ���������݂̋������
            // ���傫���ꍇ�ɂ̂݁A������Ԃ��B
            currentDistance = !isCorrected || correctedDistance > currentDistance
                ? Mathf.Lerp(currentDistance, correctedDistance, Time.deltaTime * zoomDampening)
                : correctedDistance;

            // ���E�𒴂��Ȃ��悤�ɂ���
            currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);

            // �V���� currentDistance �Ɋ�Â��Ĉʒu���Čv�Z����B
            position = target.transform.position - (rotation * Vector3.forward * currentDistance + vTargetOffset);

            // �Ō�ɃJ�����̉�]�ƈʒu��ݒ�B
            transform.rotation = rotation;
            transform.position = position;

        }

    }


    // �J������w��ɂ܂킷�B
    private void RotateBehindTarget()
    {
        float targetRotationAngle = target.transform.eulerAngles.y;
        float currentRotationAngle = transform.eulerAngles.y;
        horizontalAngle = Mathf.LerpAngle(currentRotationAngle, targetRotationAngle, rotationDampening * Time.deltaTime);
    }

    // �p�x�N���b�s���O
    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f)
            angle += 360f;
        if (angle > 360f)
            angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }

}
