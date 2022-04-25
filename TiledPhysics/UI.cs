using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

//UI is an easyDraw to enable screenFading
/// <summary>
/// Container for all elements that should stick to the screen regardless of scene movement.
/// e.g. health bar, coin count, etc.
/// </summary>
public class UI : EasyDraw
{
    public Dictionary<String, GameObject> elements; // a way for other parts to easily search for UI elements based on their given name
    private string text;
    private int textPriority = 0;

    public UI(int width, int height) : base(width, height, false)
    {
        elements = new Dictionary<string, GameObject>();
        TextAlign(CenterMode.Center, CenterMode.Center);
        ShapeAlign(CenterMode.Center, CenterMode.Center);
        TextSize(30);
        Console.WriteLine("ui initialized");
    }

    /// <summary>
    /// Adds the given Easydraw surface
    /// </summary>
    /// <param name="element"> Object to place on the ui</param>
    /// <param name="name">Name of the object to add</param>
    /// <param name="x">desired x position of the origin, leave empty for current position</param>
    /// <param name="y">desired y position of the origin, leave empty for current position</param>
    public void addElement(GameObject element, String name, float? x = null, float? y = null)
    {
        Console.WriteLine("adding: " + element);
        elements.Add(name, element);
        this.AddChild(element);
        element.x = x.GetValueOrDefault(element.x);
        element.y = y.GetValueOrDefault(element.y);

    }

    /// <summary>
    /// Try to show the given text if it has the highest priority, should be called every frame you want to show the text
    /// </summary>
    /// <param name="text">the text you want to show</param>
    /// <param name="priority">the priority at which you want to show the text</param>
    public void showText(string text, int priority)
    {
        //if there currently is no text or the given priority is higher, overwrite the text and priority
        if (this.text.Length == 0 || priority > this.textPriority)
        {
            Clear(0, 0, 0, 0);
            this.text = text;
            this.textPriority = priority;
            Fill(255);
            Text(this.text, width / 2 + 5, height - 30);
        }
    }

    /// <summary>
    /// reset the text and its priority
    /// </summary>
    public void clearText()
    {
        Clear(0, 0, 0, 0);

        this.text = "";
        this.textPriority = 0;
    }

    /// <summary>
    /// tries to remove the given object from the ui
    /// </summary>
    /// <param name="elementName">name of the element to remove</param>
    /// <returns>True if an element was removed</returns>
    public bool removeElement(string elementName)
    {
        if (elements.ContainsKey(elementName))
        {
            GameObject element;
            elements.TryGetValue(elementName, out element);
            element.Destroy();
            elements.Remove(elementName);
            return true;
        }
        return false;
    }
}