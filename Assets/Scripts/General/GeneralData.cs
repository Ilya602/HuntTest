using System;

namespace Assets.Scripts.General
{
    [Serializable]
    public class GeneralData
    {
        public int FoodValue { get; set; }
        public ObjectData[] ObjectsData { get; set; }
        public CellData[] CellsData { get; set; }


        public GeneralData() { }
        public GeneralData(int foodValue, ObjectData[] objectsData, CellData[] cellsData)
        {
            FoodValue = foodValue;
            ObjectsData = objectsData;
            CellsData = cellsData;
        }
    }

    [Serializable]
    public struct ObjectData
    {
        public int EvoLevel { get; set; }
        public int CellIndex { get; set; }


        public ObjectData(int evoLevel, int cellIndex)
        {
            EvoLevel = evoLevel;
            CellIndex = cellIndex;
        }
    }

    [Serializable]
    public struct CellData
    {
        public int Index { get; set; }
        public bool IsLocked { get; set; }


        public CellData(int index, bool isLocked)
        {
            Index = index;
            IsLocked = isLocked;
        }
    }
}