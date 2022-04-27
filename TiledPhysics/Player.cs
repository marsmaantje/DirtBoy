﻿using System;
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
    EasyDraw collider;
    float health;
    float maxHealth;
    float maxHealthRadius;
    float minHealthRadius;

    float radius
    {
        get => Mathf.Map(health, 0, maxHealth, minHealthRadius, maxHealthRadius);
    }

    float health01
    {
        get => health / maxHealth;
    }

    public Player() : base() { ReadVariables(); }

    public Player(TiledObject obj) : base()
    {
        this.obj = obj;
        ReadVariables();
        createAnimation();
    }

    public Player(String filename, int cols, int rows, TiledObject obj) : base()
    {
        this.obj = obj;
        ReadVariables();
    }

    /// <summary>
    /// Try to read the player variables from the tiled object, use the fallback variables if not present
    /// </summary>
    void ReadVariables()
    {
        maxHealth = obj.GetIntProperty("maxHealth", 10);
        maxHealthRadius = obj.GetFloatProperty("maxHealthRadius", 10);
        minHealthRadius = obj.GetFloatProperty("minHealthRadius", 1);
        health = maxHealth;
    }

    /// <summary>
    /// setup the visual for the player,
    /// visual is seperate for collision reasons
    /// </summary>
    private void createAnimation()
    {
        animation = new AnimationSprite(obj.GetStringProperty("Sprite"), obj.GetIntProperty("columns"), obj.GetIntProperty("rows"));
        AddChild(animation);
        animation.SetOrigin(animation.width / 2, animation.height / 2);
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

        //setup the physics
        _mover = new SingleMover();
        parent.AddChild(_mover);
        _mover.position = position;
        _mover.AddChild(this);
        SetXY(0, 0);
        _mover.SetCollider(new Ball(_mover, _mover.position, radius));
        Console.WriteLine(parent);
        collider = new EasyDraw(10, 10);
    }


    public void Update()
    {
        health -= 0.01f + 0.01f * _mover.Velocity.Length() / 10f;
        animation.scale = health01;
        if (_mover.collider is Ball ball)
        {
            ball.radius = radius;
            collider.width = (int)radius; //problematic?
            collider.height = (int)radius;

        }
        _mover.Step();
        HandleInput();
        PlayerAnimation();
        UpdateUI();
        //Gizmos.DrawCross(_mover.x, _mover.y, 10, parentScene);
    }

    /// <summary>
    /// Handle the player input
    /// </summary>
    private void HandleInput()
    {
        if (health < 1)
        {
            health = maxHealth;
        }
        if (Input.GetKey(Key.A))
        {
            animation.rotation -= 5;
            _mover.Velocity += new Vec2(-1f,0);
        }
        if (Input.GetKey(Key.D))
        {
            animation.rotation += 5;
            _mover.Velocity += new Vec2(1f, 0);
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