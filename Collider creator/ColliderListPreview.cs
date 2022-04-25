using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using Visuals;
using Physics;
using UIElements;

public class ColliderListPreview : Pivot
{
    public ColliderLoader _colliderLoader;
    int _totalWidth;
    int _itemWidth;
    int _rowHeight;
    bool hasVisual = false;

    public ColliderListPreview(ColliderLoader loader)
    {
        _colliderLoader = loader;
    }

    public void CreatePreview(int totalWidth, int itemWidth, int rowHeight)
    {
        hasVisual = true;
        _totalWidth = totalWidth;
        _itemWidth = itemWidth;
        _rowHeight = rowHeight;


        List<string> colliderNames = _colliderLoader.GetColliderNames();
        int colliderCount = colliderNames.Count;
        int collumns = totalWidth / itemWidth;
        int rows = Mathf.Ceiling((float)colliderCount / (float)collumns);
        int colliderIndex = 0;

        Console.WriteLine("creating a preview with " + colliderCount + " colliders");
        Console.WriteLine("collumns: " + collumns + " rows: " + rows);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < collumns; j++)
            {
                if (colliderIndex < colliderCount)
                {
                    string colliderName = colliderNames[colliderIndex];
                    ColliderPreview colliderPreview = new ColliderPreview(itemWidth, rowHeight, _colliderLoader, colliderName);
                    AddChild(colliderPreview);
                    colliderPreview.x = Mathf.Map(j, 0, collumns - 1, 0, totalWidth - itemWidth);
                    colliderPreview.y = i * (rowHeight + 5);
                    colliderPreview.preview = this;
                    colliderIndex++;
                }
            }
        }
    }

    public void RegeneratePreview()
    {
        foreach (GameObject child in GetChildren())
        {
            child.Destroy();
        }
        CreatePreview(_totalWidth, _itemWidth, _rowHeight);
    }
}
