using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputArrowCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.name.Contains("Destroy"))
        {
            AudioManager.instance.PlaySFX("miss");
            GameManager.instance.battleSeriesManager.spawnManager.CheckIsCurrentPrefabIsTheLastOne(this.GetComponent<InputArrowPrefab>());
            this.GetComponent<InputArrowPrefab>().Destroy();
            
        }
        if (collision.name.Contains("Init"))
        {
            GameManager.instance.battleSeriesManager.spawnManager.UpdateCurrentArrow(this.gameObject.GetComponent<InputArrowPrefab>());
        }
    }

}
