using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroy : MonoBehaviour
{
    public GameObject _enemyObj = default;

    private void OnBecameInvisible() {
        _enemyObj.SetActive(false);
    }
}
