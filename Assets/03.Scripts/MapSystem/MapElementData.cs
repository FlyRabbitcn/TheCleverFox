using System;

[Serializable]
public class MapElementData
{
    public int X;
    public int Y;
    public string Name;
    public MapElementData(int x, int y, string name)
    {
        this.X = x;
        this.Y = y;
        this.Name = name;
    }
    public MapElementData()
    {

    }
}
