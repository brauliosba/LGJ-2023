using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputArrowCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.name.Contains("Destroy"))
        {
            GameManager.instance.battleSeriesManager.spawnManager.CheckIsCurrentPrefabIsTheLastOne(this.GetComponent<InputArrowPrefab>());
            Destroy(this.gameObject);
            
        }
        if (collision.name.Contains("Init"))
        {
            GameManager.instance.battleSeriesManager.spawnManager.currentInputArrowPrefab = this.gameObject.GetComponent<InputArrowPrefab>();
        }
    }

}
