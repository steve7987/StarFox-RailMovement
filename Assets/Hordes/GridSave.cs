using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


[System.Serializable]
public class GridSave
{

    public List<string> buildingNames;
    public List<Vector3> buildingLocs;

    public GridSave(HashSet<BuildingController> buildings)
    {
        buildingNames = new List<string>();
        buildingLocs = new List<Vector3>();

        foreach (var b in buildings)
        {
            buildingNames.Add(b.data.name);
            buildingLocs.Add(b.transform.position);
        }
    }

    public void SaveData(string name)
    {
        string json = JsonUtility.ToJson(this);

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + '/' + name + ".grid";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, json);
        stream.Close();

        Debug.Log("Data saved to " + path);
    }

    public static GridSave LoadData(string filename)
    {
        string path = Application.persistentDataPath + '/' + filename + ".grid";

        return LoadDataFullPath(path);
    }

    static GridSave LoadDataFullPath(string path)
    {
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            string json;

            json = formatter.Deserialize(stream) as string;
            stream.Close();

            GridSave gsave = JsonUtility.FromJson<GridSave>(json);
            Debug.Log("Data loaded from " + path);
            return gsave;
        }
        else
        {
            Debug.LogError("No data found at: " + path);
            return null;
        }
    }
}
