using System;
using GXPEngine;
using System.Drawing;
using System.Collections.Generic;
using Physics;
using Visuals;
using GameElements;
using System.IO;
using Sound;
using UIElements;

public class MyGame : Game
{
    bool _stepped = false;
    public bool _paused = false;
    int _startSceneNumber = 1;

    Canvas _lineContainer = null;

    public void DrawLine(Vec2 start, Vec2 end)
    {
        _lineContainer.graphics.DrawLine(Pens.White, start.x, start.y, end.x, end.y);
    }

    public MyGame() : base(1440, 900, false, false, pPixelArt:true)
    {
        _lineContainer = new Canvas(width, height);
        AddChild(_lineContainer);

        targetFps = 60;

        LoadScene(_startSceneNumber);

        PrintInfo();


        ColliderManager manager = ColliderManager.main;
        
        ColliderLoader loader = new ColliderLoader();
        Pivot empty = new Pivot();
        AddChild(empty);

        /*
        MultiSegmentCollider collider = new MultiSegmentCollider(empty);
        collider.AddSegment(new Vec2(0, 0), new Vec2(100, 0));
        collider.AddSegment(new Vec2(100, 0), new Vec2(200, 200));
        collider.AddSegment(new Vec2(100, 100), new Vec2(0, 100));
        collider.AddSegment(new Vec2(0, 100), new Vec2(0, 0));

        loader.AddCollider("test", collider);
        loader.SaveFile("test.txt");
        /**/

        /*
        loader.loadFile("test.txt");
        MultiSegmentCollider col = loader.GetCollider("test", empty);
        Console.WriteLine(col);
        col.AddToManager(manager);
        
        MultiSegmentVisual vis = new MultiSegmentVisual(col);
        empty.AddChild(vis);

        empty.SetXY(200, 200);
        //empty.rotation = 45;

        //add ball moving to the collider
        Bullet ball = new Bullet(new Vec2(220, 500), 10, new Vec2(0, -10));
        AddChild(ball);
        /**/

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

    void LoadScene(int sceneNumber)
    {
        _startSceneNumber = sceneNumber;
        
        foreach (GameObject child in GetChildren())
        {
                child.Destroy();
        }

        _lineContainer = new Canvas(width, height);
        AddChild(_lineContainer);

        ColliderManager.main.ClearSolidColliders();
        ColliderManager.main.ClearTriggerCollider();

        /*
        // boundary:
        addPropperLine(new Vec2(width - 20, height - 20), new Vec2(20, height - 20));   //bottom
        addPropperLine(new Vec2(20, height - 20), new Vec2(20, 20)); //left
        addPropperLine(new Vec2(20, 20), new Vec2(width - 20, 20)); //top
        addPropperLine(new Vec2(width - 20, 20), new Vec2(width - 20, height - 20));  //right
        */
        
        switch(_startSceneNumber)
        {
            case 0:
                //add the image and collider creator
                ImageSelector sel = new ImageSelector();
                AddChild(sel);
                ColliderCreator col = new ColliderCreator(sel);

                //save button
                Button save = new Button(50, 50, Color.White, Color.Green);
                AddChild(save);
                save.SetXY(0, 0);
                save.SetText("Save");
                save.OnClick += () =>
                {
                    col.Save();
                    LoadScene(2);
                };
                break;
            case 1:
                Button loadButton = new Button(200, 100, Color.White, Color.Red, 5);
                AddChild(loadButton);
                loadButton.SetXY(width / 2, height / 2);
                loadButton.SetText("load collider file");
                loadButton.OnClick += () =>
                {
                    ColliderLoader loader = ColliderLoader.main;
                    System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
                    dialog.Filter = "Collider files(*.txt;*.json)|*.txt;*.json|All files (*.*)|*.*";
                    dialog.InitialDirectory = Directory.GetCurrentDirectory();
                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        loader.loadFile(dialog.FileName);
                    }
                    LoadScene(2);
                };
                break;
            case 2:
                ColliderListPreview preview = new ColliderListPreview(ColliderLoader.main);
                AddChild(preview);
                preview.SetXY(210, 0);
                preview.CreatePreview(width - 220, width / 10, width / 6);

                Button createNewButton = new Button(200, 100, Color.White, Color.Red, 5);
                AddChild(createNewButton);
                createNewButton.SetXY(0, 0);
                createNewButton.SetText("create new collider");
                createNewButton.OnClick += () =>
                {
                    LoadScene(0);
                };

                Button saveButton = new Button(200, 100, Color.White, Color.Red, 5);
                AddChild(saveButton);
                saveButton.SetXY(0, 110);
                saveButton.SetText("save collider file");
                saveButton.OnClick += () =>
                {
                    ColliderLoader.main.Save();
                };

                Button loadNew = new Button(200, 100, Color.White, Color.Red, 5);
                AddChild(loadNew);
                loadNew.SetXY(0, 230);
                loadNew.SetText("load collider file");
                loadNew.OnClick += () =>
                {
                    LoadScene(1);
                };                

                break;
            case 3:
                break;
            case 4:
                break;
            default:
                break;
        }
        
    }

    /****************************************************************************************/

    void PrintInfo()
    {
        Console.WriteLine("Hold spacebar to slow down the frame rate.");
        Console.WriteLine("Press S to toggle stepped mode.");
        Console.WriteLine("Press P to toggle pause.");
        Console.WriteLine("Press D to draw debug lines.");
        Console.WriteLine("Press C to clear all debug lines.");
        Console.WriteLine("Press R to reset scene, and numbers to load different scenes.");
    }

    void HandleInput()
    {
        targetFps = Input.GetKey(Key.SPACE) ? 5 : 60;
        if (Input.GetKeyDown(Key.UP))
        {
            Mover.gravity = new Vec2(0, -0.5f);
        }
        if (Input.GetKeyDown(Key.RIGHT))
        {
            Mover.gravity = new Vec2(0.5f, 0);
        }
        if (Input.GetKeyDown(Key.DOWN))
        {
            Mover.gravity = new Vec2(0, 0.5f);
        }
        if (Input.GetKeyDown(Key.LEFT))
        {
            Mover.gravity = new Vec2(-0.5f, 0);
        }
        if (Input.GetKeyDown(Key.BACKSPACE))
        {
            Mover.gravity = new Vec2(0, 0);
        }
        if (Input.GetKeyDown(Key.S))
        {
            _stepped ^= true;
        }
        if (Input.GetKeyDown(Key.D))
        {
            Mover.drawDebugLine ^= true;
        }
        if (Input.GetKeyDown(Key.P))
        {
            _paused ^= true;
        }
        if (Input.GetKeyDown(Key.C))
        {
            _lineContainer.graphics.Clear(Color.Black);
        }
        if (Input.GetKeyDown(Key.R))
        {
            LoadScene(_startSceneNumber);
        }
        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown(48 + i))
            {
                LoadScene(i);
            }
        }
    }

    void Update()
    {
        HandleInput();
    }

    [STAThread]
    static void Main()
    {
        new MyGame().Start();
    }
}