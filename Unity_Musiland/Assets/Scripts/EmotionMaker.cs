using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionMaker : MonoBehaviour {

    [SerializeField]
    GameObject PositionSpawn;

    [SerializeField]
    GameObject[] prefablist;

    GameObject goemotion;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    

    public void MakeEmotion(EnumList.Emotion emotion)
    {
        if (!goemotion)
        {
            int id = 0;

            switch (emotion)
            {
                case EnumList.Emotion.Angry:
                    id = 0;
                    break;

                case EnumList.Emotion.Laugh:
                    id = 1;
                    break;

                case EnumList.Emotion.Panic:
                    id = 2;
                    break;
            }

            GameObject prefab = prefablist[id];

            goemotion = Instantiate(prefab, PositionSpawn.transform.position, Quaternion.identity) as GameObject;
            goemotion.transform.parent = PositionSpawn.transform;
        }
       
    }

    public void stopEmotion()
    {
        if (goemotion)
        {
            Destroy(goemotion);
        }
    }
}
