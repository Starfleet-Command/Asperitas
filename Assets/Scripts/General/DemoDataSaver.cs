using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoDataSaver : MonoBehaviour
{
    public SaveLoad saveScript;
    public Button saveButton;
    public Button loadButton;
    public List<GameObject> itemsToStore;
    
    // Start is called before the first frame update
    void Start()
    {
        saveButton.onClick.AddListener(delegate {saveScript.SaveData(GameObjectInfo.FromGameObject(itemsToStore),"testStore");});
        loadButton.onClick.AddListener(delegate {saveScript.LoadData("testStore");});
    }


}

[System.Serializable]
public class GameObjectInfo
{
    public int objectId;
    public float posX;
    public float posY;
    public float posZ;
    public float rotationX;
    public float rotationY;
    public float rotationZ;

    public static GameObjectInfo[] FromGameObject(List<GameObject> items)
    {
        List<GameObjectInfo> info = new List<GameObjectInfo>();
        foreach (GameObject obj in items)
        {
            GameObjectInfo objectData = new GameObjectInfo();
            objectData.objectId = 1;
            objectData.posX = obj.transform.position.x;
            objectData.posY = obj.transform.position.y;
            objectData.posZ = obj.transform.position.z;
            objectData.rotationX = obj.transform.rotation.x;
            objectData.rotationY = obj.transform.rotation.y;
            objectData.rotationZ = obj.transform.rotation.z;
            info.Add(objectData);
        }

        return info.ToArray();
    }
}


