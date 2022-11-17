using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolCtrl : MonoBehaviour
{
    //�e�̃v���t�@�u
    public GameObject _poolObj;
    //�e�̔z��
    public List<GameObject> _listOfPooledObjects = new List<GameObject>();
    //�������鋅�̐�
    public int _instantiateCount;

    private void Awake()
    {
        /*�Q�[���J�n���ɃJ�E���g�̐������e�𐶐����A
         * ��\���ɂ����ナ�X�g�̒��ɒǉ�����B
         */
        for (int i = 0; i < _instantiateCount; i++)
        {
            GameObject obj = Instantiate(_poolObj,this.transform);
            obj.SetActive(false);
            _listOfPooledObjects.Add(obj);
        }
    }

    //GameObject�^�̃f�[�^��Ԃ�
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < _listOfPooledObjects.Count; i++)
        {
            //���������I�u�W�F�N�g��Hierarchy��Ŕ�A�N�e�B�u�Ȃ��
            if(_listOfPooledObjects[i].activeInHierarchy == false)
            {
                //List�Ƀf�[�^��Ԃ�
                return _listOfPooledObjects[i];
            }
        }
        return null;
    }
}
