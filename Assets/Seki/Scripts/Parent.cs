using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parent : MonoBehaviour
{
    //�����Ƃ��Ă�
    //�@�ǂɂԂ��������ɂԂ������I�u�W�F�N�g�̎q�Ƀv���C���[��ݒ肷��
    //�A�Ԃ������e�̎q���擾����player���q�Ɠ�����������

   

    [SerializeField] CharacterMove2 characterMove2;

    public Transform target;
    [SerializeField] EmptyHit emptyHit;
    void Start() {
        
        characterMove2.enabled = false;
        //CharacterMove2.target = player.transform.GetChild(0).transform;
    }
    private void Update() {

        if(emptyHit.WALLFLAG) {
            characterMove2.enabled = true;
            //this.gameObject.transform.parent = target.gameObject.transform;
            //Vector3 direction = new Vector3(180f, 0f, 0f);
            //this.transform.localRotation = Quaternion.Euler(direction);
        }

        else {
            characterMove2.enabled = false;
            //this.gameObject.transform.parent = null;
        }
        //player = wallHit.WALL;
        //SetParent(player);
    }

    /*Invoked when a button is pressed.
    public void SetParent(GameObject newParent) {
        //Makes the GameObject "newParent" the parent of the GameObject "player".
        player.transform.parent = newParent.transform;
        Vector3 direction = new Vector3(0f,0f, 90f);
        player.transform.localRotation =Quaternion.Euler(direction);
        characterMove2.enabled = true;//���ƂŃR�����g�A�E�g�O��
        //Display the parent's name in the console.
        //Debug.Log("Player's Parent: " + player.transform.parent.name);

        // Check if the new parent has a parent GameObject.

    }
    */
    
}
