using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroy : MonoBehaviour
{
    [SerializeField]
    private GameObject _destroyObj;

    private void OnBecameInvisible() {
        _destroyObj.SetActive(false);
    }
}
