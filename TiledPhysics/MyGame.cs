using System;
using GXPEngine;
using Physics;
using Visuals;

public class MyGame : Game
{
    public bool _paused = false;

    Scene currentScene;
    UI ui;
    Cursor cursor;

    string _startSceneName = "maps/Main Menu.tmx";
    string ColliderFileName = "Colliders.txt";
    bool levelLoad = false;
    float gravityStrength = 1f;

    public MyGame() : base(1920, 1080, false, false, pPixelArt:true)
    //public MyGame() : base(1920, 1080, true, false)
    {
        cursor = new Cursor();
        AddChild(cursor);
        
        targetFps = 60;

        ColliderLoader.main.loadFile(ColliderFileName);

        createLevel(_startSceneName);

        PrintInfo();
        Mover.gravity = new Vec2(0, gravityStrength);

    }

    void addPropperLine(Vec2 start, Vec2 end)
    {
        LineCollider l = new LineCollider(start, end);
        AddChild(l);
    }

    void AddCollider(Collider c, Vec2 position, GameObject visual)
    {
        ColliderManager.main.AddSolidCollider(c);
        c.position = position;
        c.owner = visual;
        AddChild(visual);
    }

    /// <summary>
	/// Will create a new level and UI based on the name given
	/// </summary>
	/// <param name="mapName">fileName of the new level</param>
	void createLevel(String mapName)
    {
        createUI();
        currentScene = new Scene(ui);
        AddChild(currentScene);
        currentScene.loadMapFile(mapName);
        currentScene.createLevel();

        //set the index of the ui to be in front of the level
        SetChildIndex(ui, GetChildCount());
        SetChildIndex(cursor, GetChildCount());
    }

    /// <summary>
	/// Will create a new UI
	/// </summary>
	void createUI()
    {
        ui = new UI(width, height);
        AddChild(ui);
    }

    /// <summary>
	/// Will initialize the loading of a new level, actual level will be created next frame
	/// </summary>
	/// <param name="newMapName"></param>
	public void loadNewLevel(string newMapName)
    {
        currentScene.LateDestroy();
        ui.LateDestroy();
        _startSceneName = newMapName;
        levelLoad = true;
        ColliderManager.main.ClearSolidColliders();
        ColliderManager.main.ClearTriggerCollider();
    }

    /****************************************************************************************/

    void PrintInfo()
    {
        Console.WriteLine("Hold spacebar to slow down the frame rate.");
        Console.WriteLine("Press P to toggle pause.");
    }

    void HandleInput()
    {
        targetFps = Input.GetKey(Key.SPACE) ? 5 : 60;
        if (Input.GetKeyDown(Key.UP))
        {
            Mover.gravity = new Vec2(0, -gravityStrength);
        }
        if (Input.GetKeyDown(Key.RIGHT))
        {
            Mover.gravity = new Vec2(gravityStrength, 0);
        }
        if (Input.GetKeyDown(Key.DOWN))
        {
            Mover.gravity = new Vec2(0, gravityStrength);
        }
        if (Input.GetKeyDown(Key.LEFT))
        {
            Mover.gravity = new Vec2(-gravityStrength, 0);
        }
        if (Input.GetKeyDown(Key.BACKSPACE))
        {
            Mover.gravity = new Vec2(0, 0);
        }
        if (Input.GetKeyDown(Key.P))
        {
            _paused ^= true;
        }
        //DEBUG reload level
        if (Input.GetKeyDown(Key.R))
        {
            loadNewLevel(_startSceneName);
        }
    }

    public void Update()
    {
        //if we are currently loading a new level, actually do it
        if (levelLoad)
        {
            createLevel(_startSceneName);
            levelLoad = false;
            Console.WriteLine("new Level loaded");
        }
        
        ui.clearText(); //since this is always the first update called, clear the ui text here
        
        HandleInput();
    }

    [STAThread]
    static void Main()
    {
        new MyGame().Start();
    }
}