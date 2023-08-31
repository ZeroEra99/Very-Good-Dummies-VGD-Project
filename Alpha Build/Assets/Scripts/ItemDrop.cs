using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemDrop : MonoBehaviour
{
    [SerializeField]
    private GameObject[] itemList;
    private int itemNum;
    private int randNum;
    private Transform EnemyPosition;
    private Vector3 itemPosition;


    private void Start()
    {
        EnemyPosition = GetComponent<Transform>();
        
    }

    public void DropItem()
    {
        itemPosition = new Vector3(EnemyPosition.position.x, 21.3f, EnemyPosition.position.z);
        randNum = Random.Range(0, 101);
        Debug.Log("Random Number is " + randNum);


        if (randNum >= 75)
        {
            itemNum = 2; //drop GodModeCollectible
            Instantiate(itemList[itemNum], itemPosition, Quaternion.identity);
        }
        else if (randNum > 50 && randNum < 75)
        {
            itemNum = 1; //drop SpeedHacCollectible
            Instantiate(itemList[itemNum], itemPosition, Quaternion.identity);
        }
        else if (randNum > 25 && randNum <= 50)
        {
            itemNum = 0;//drop FireFistCollectible
            Instantiate(itemList[itemNum], itemPosition, Quaternion.identity);


        }

    }
}
