using System;
using GXPEngine;
using TiledMapParser;
using Physics;

public class Player : Pivot
{

    //objects
    TiledObject obj;
    private Scene parentScene;

    //camera target when following the player
    public Pivot cameraTarget;
    AnimationSprite animation;
    SingleMover _mover;

    public Player() : base() { ReadVariables(); }

    public Player(TiledObject obj) : base()
    {
        this.obj = obj;
        ReadVariables();
        animation = new AnimationSprite(obj.GetStringProperty("Sprite"), obj.GetIntProperty("columns"), obj.GetIntProperty("rows"));
        AddChild(animation);
        animation.SetOrigin(animation.width / 2, animation.height / 2);
    }

    public Player(String filename, int cols, int rows, TiledObject obj) : base()
    {
        this.obj = obj;
        ReadVariables();
    }

    /// <summary>
    /// setup the visual for the player,
    /// visual is seperate for collision reasons
    /// </summary>
    private void createAnimation()
    {
        
    }

    /// <summary>
    /// Initializes the sprite for the player and the cameraTarget
    /// </summary>
    /// <param name="parentScene">The scene this player should callback for change of camera target</param>
    public void initialize(Scene parentScene)
    {
        this.parentScene = parentScene;

        //setup the camera
        this.SetScaleXY(1, 1);
        Pivot lookTarget = new Pivot();
        AddChild(lookTarget);
        lookTarget.SetXY(0, 0);
        lookTarget.SetScaleXY(1f, 1f);
        parentScene.setLookTarget(lookTarget);
        parentScene.jumpToTarget();

        _mover = new SingleMover();
        parent.AddChild(_mover);
        _mover.AddChild(this);
        _mover.AddChild(animation);
        SetXY(0, 0);
        _mover.SetCollider(new Ball(_mover, _mover.position, 10));
    }

    /// <summary>
    /// Try to read the player variables from the tiled object, use the fallback variables if not present
    /// </summary>
    private void ReadVariables()
    {

    }

    public void Update()
    {
        _mover.Step();
        HandleInput();
        PlayerAnimation();
        UpdateUI();
        Gizmos.DrawCross(_mover.x, _mover.y, 10, parentScene);
    }

    /// <summary>
    /// Handle the player input
    /// </summary>
    private void HandleInput()
    {
        if (Input.GetKey(Key.A))
        {
            animation.rotation -= 5;
        }
        if (Input.GetKey(Key.D))
        {
            animation.rotation += 5;
        }
    }

    /// <summary>
    /// moves the player and updates all the visuals accordingly
    /// </summary>
    private void PlayerAnimation()
    {

    }

    /// <summary>
    /// Updates the Energy bar at the top of the screen
    /// </summary>
    private void UpdateUI()
    {
        
    }
}