using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolCtrl : MonoBehaviour
{
    //弾のプレファブ
    public GameObject _bulletObj;
    public List<GameObject> _listOfPooledObjects = new List<GameObject>();
    public int _instantiateCount;

    private void Awake()
    {
        /*ゲーム開始時にカウントの数だけ弾を生成し、
         * 非表示にした後リストの中に追加する。
         */
        for (int i = 0; i < _instantiateCount; i++)
        {
            GameObject obj = Instantiate(_bulletObj,this.transform);
            obj.SetActive(false);
            _listOfPooledObjects.Add(obj);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    //GameObject型のデータを返す
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < _listOfPooledObjects.Count; i++)
        {
            //生成したオブジェクトがHierarchy上で非アクティブならば
            if(_listOfPooledObjects[i].activeInHierarchy == false)
            {
                //Listにデータを返す
                return _listOfPooledObjects[i];
            }
        }
        return null;
    }
}
