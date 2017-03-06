using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionMaker : MonoBehaviour {

    [SerializeField]
    GameObject PositionSpawn;

    [SerializeField]
    GameObject[] prefablist;

    GameObject goemotion;

    [SerializeField]
    public float scaleparticle=1;

   public EnumList.Emotion currentemotion;

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
                    currentemotion = EnumList.Emotion.Angry;
                    break;

                case EnumList.Emotion.Laugh:
                    id = 1;
                    currentemotion = EnumList.Emotion.Laugh;
                    break;

                case EnumList.Emotion.Panic:
                    id = 2;
                    currentemotion = EnumList.Emotion.Panic;
                    break;
            }

            GameObject prefab = prefablist[id];

            goemotion = Instantiate(prefab, PositionSpawn.transform.position, prefab.transform.rotation) as GameObject;
            goemotion.transform.parent = PositionSpawn.transform;
            goemotion.transform.localScale = new Vector3 (scaleparticle, scaleparticle, scaleparticle);
        }
       
    }

    public void stopEmotion()
    {
        if (goemotion)
        {
            currentemotion = EnumList.Emotion.Null;
            Destroy(goemotion);
        }
    }
}
