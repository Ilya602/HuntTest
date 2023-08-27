using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Merge;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.General
{
    public static class SaveManager
    {
        public static void Save(int currFood, ref List<MergeObject> objects, ref Cell[] cells)
        {
            List<ObjectData> objectDataList = new List<ObjectData>();
            List<CellData> cellDataList = new List<CellData>();
            List<string> contents = new List<string>();

            foreach (var obj in objects)
            {
                ObjectData objectData = new ObjectData(obj.EvoLevel, System.Array.IndexOf(cells, obj.NewCell));
                objectDataList.Add(objectData);
            }

            foreach (var cell in cells)
            {
                CellData cellData = new CellData(Array.IndexOf(cells, cell), cell.IsLocked);
                cellDataList.Add(cellData);
            }

            string generalData = JsonConvert.SerializeObject(new GeneralData(currFood, objectDataList.ToArray(), cellDataList.ToArray()));
            contents.Add(generalData);

            File.WriteAllLines($"{Application.persistentDataPath}/GameData.json", contents);
        }

        public static GeneralData Load()
        {
            string path = $"{Application.persistentDataPath}/GameData.json";

            if (File.Exists(path))
            {
                string[] data = File.ReadAllLines(path);

                return JsonConvert.DeserializeObject<GeneralData>(data[0]);
            }

            return null;
        }
    }
}