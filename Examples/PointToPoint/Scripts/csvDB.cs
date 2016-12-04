using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Text.RegularExpressions;

public struct FurnitureData
{
    public Vector3 dims;
    //public float length;
    //public float width;
    //public float height;
    public float scale;
    public string filepath;
    public string name;
    public string tag;
    //public Vector3 centeroffset;
    
    public FurnitureData(string row)
    {
        string[] entries = row.Split(',');
        filepath = entries[0];
        name = entries[1];
        scale = float.Parse(entries[2]);
        dims = new Vector3 (scale * float.Parse(entries[3]), scale * float.Parse(entries[4]), scale * float.Parse(entries[5]));
        tag = entries[6];
    }
}

public class csvDB : MonoBehaviour {

    private Object x;
    private DirectoryInfo assetPath;
    private int pathlength;
    private string s;
    public List<FurnitureData> furniture;

    // Use this for initialization
    void Start()
    {
        assetPath = new DirectoryInfo("Assets/");
        DirectoryInfo dir = new DirectoryInfo("Assets/Resources/");


        //pathlength = assetPath.GetDirectories("Resources")[0].ToString().Length;
        //Debug.Log(assetPath.GetDirectories("Resources")[0].ToString().Length);


        loadData("Assets/Resources/furnituredata.csv");
        string s2 = Resources.Load("filename").ToString();
        //Instantiate(Resources.Load(s2));
        //Debug.Log(furniture);
        loadModels(furniture);
        /*
        Dictionary<string,List<FurnitureData>> X = new Dictionary<string, List<FurnitureData>>();
        string tag = "chair";
        X.Add(tag, furniture);
        Debug.Log(X[tag][0].name);
        Debug.Log(X.ContainsKey("chair"));
        Debug.Log(X.ContainsKey("bed"));
        */
        Dictionary<string, List<FurnitureData>> X = filterBySize(furniture,new Vector3(6f,6f,1.5f));
        //Debug.Log(X["Chair"].Count);
        List<FurnitureData> temp = new List<FurnitureData>();
        Debug.Log(X.TryGetValue("Bed", out temp));
        //Debug.Log(temp.Count);
        //temp = X["Chair"];
        List<string> keys2 = new List<string>(X.Keys);
        Debug.Log(keys2);
        foreach(string s in keys2)
        {
            Debug.Log(s);
        }



    }
    /*
    string[] getFilteredFurniture(Dictionary<string, List<FurnitureData>> d)
    {
        
    }*/

    Dictionary<string, List<FurnitureData>> filterBySize(List<FurnitureData> fd, Vector3 box)
    {
        int l = fd.Count;
        Dictionary<string, List<FurnitureData>> X = new Dictionary<string, List<FurnitureData>>();
        for (int i =0; i<l;i++)
        {
            Debug.Log(i);
            FurnitureData f = fd[i];
            Debug.Log(f.name);
            //List<FurnitureData> temp = new List<FurnitureData>();
            Vector3 scaledDims = f.dims * f.scale;
            Debug.Log(scaledDims);
            if (box.x > scaledDims.x && box.y > scaledDims.y && box.z > scaledDims.z)
            {
                Debug.Log("added");

                if (X.ContainsKey(f.tag))
                {
                    //Debug.Log("tag exits:");
                    //temp = X[f.tag];
                    //temp.Add(f);
                    //X[f.tag] = temp;
                    X[f.tag].Add(f);
                    //Debug.Log(X[f.tag].Count + 1);
                    //Debug.Log(f.tag);
                }
                else
                {
                    //Debug.Log("Creating new tag:");
                    //Debug.Log(X);
                    X.Add(f.tag, new List<FurnitureData>());
                    //X.Add(f.tag, fd);
                    X[f.tag].Add(f);
                    //Debug.Log(X.ContainsKey(f.tag));
                   // Debug.Log(X[f.tag].Count + 1);
                }
            }

        }
        return X;
    }

    //public List<string> categories()

    void loadData(string path)
    {
        string data = System.IO.File.ReadAllText(path);
        string[] lines = Regex.Split(data,"\r\n");
        furniture = new List<FurnitureData>();
        //Debug.Log(furniture);

        foreach(string row in lines)
        {
            furniture.Add(new FurnitureData(row));
        }
    }

    //void prefabdwalk(DirectoryInfo dir)
    //{
    //    FileInfo[] fileinfo = dir.GetFiles("*.prefab");
    //    DirectoryInfo[] dirinfo = dir.GetDirectories("*");
    //    if (dirinfo != null)
    //    {
    //        foreach (FileInfo f in fileinfo)
    //        {
    //            processprefab(f);
    //        }
    //        foreach (DirectoryInfo d in dirinfo)
    //        {
    //            prefabdwalk(d);
    //        }
    //    }
    //}


    void loadModels(List<FurnitureData> FD)
    {
        int i = -5;
        foreach(FurnitureData f in FD)
        {
            //Debug.Log(f.filepath);
            Object piece = Resources.Load(f.filepath);
            //Debug.Log(piece);
            GameObject piece2 = (GameObject) Instantiate(piece);
            Debug.Log(piece2);
            piece2.transform.localScale = new Vector3(f.scale,f.scale,f.scale);
            piece2.transform.position = new Vector3(3 * i++, 0, 0);

        }
    }


    void processprefab(FileInfo f)
    {
        string fstr = f.ToString();
        
        string frel = fstr.Substring(pathlength + 1,fstr.Length-6-pathlength-2);
        //Debug.Log(frel);
        Instantiate(Resources.Load(frel));
    }

    // Update is called once per frame
    void Update () {
		
	}
}
