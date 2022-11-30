using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolCtrl : MonoBehaviour
{
    //弾のプレファブ
    [Header("生成するオブジェクト")]
    public GameObject _poolObj;
    //弾の配列
    [Header("生成しているオブジェクト")]
    public List<GameObject> _listOfPooledObjects = new List<GameObject>();
    //生成する球の数
    [Header("生成する個数")]
    public int _instantiateCount;

    private void Awake()
    {
        /*ゲーム開始時にカウントの数だけ弾を生成し、
         * 非表示にした後リストの中に追加する。
         */
        for (int i = 0; i < _instantiateCount; i++)
        {
            GameObject obj = Instantiate(_poolObj,this.transform);
            obj.SetActive(false);
            _listOfPooledObjects.Add(obj);
        }
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
