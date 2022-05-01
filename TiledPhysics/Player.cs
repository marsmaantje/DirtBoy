using System;
using GXPEngine;
using TiledMapParser;
using Physics;
using Objects;

public class Player : Pivot
{
    #region variables
    //objects
    TiledObject obj;
    private Scene parentScene;

    //camera target when following the player
    public Pivot cameraTarget;
    AnimationSprite animation;
    SingleMover _mover;
    EasyDraw collider;
    public float health;
    float maxHealth, minHealthRadius, maxHealthRadius, minSpeed, maxSpeed, minMass, maxMass, minJumpHeight, maxJumpHeight, animationWidth, cameraSize;
    Vec2[] prevPositions = new Vec2[2];
    int prevPositionIndex = 0;
    Vec2[] prevVelocities = new Vec2[5];
    int prevVelocityIndex = 0;
    #endregion

    #region Getters/Setters
    public float radius
    {
        get => Mathf.Map(health01, 0, 1, minHealthRadius, maxHealthRadius);
    }

    float health01
    {
        get => Mathf.Pow(health / maxHealth, 2);
    }
    #endregion

    #region construction
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
        minHealthRadius = obj.GetFloatProperty("minHealthRadius", 1);
        maxHealthRadius = obj.GetFloatProperty("maxHealthRadius", 10);
        minSpeed = obj.GetFloatProperty("minSpeed", 1);
        maxSpeed = obj.GetFloatProperty("maxSpeed", 10);
        minMass = obj.GetFloatProperty("minMass", 1);
        maxMass = obj.GetFloatProperty("maxMass", 10);
        minJumpHeight = obj.GetFloatProperty("minJumpHeight", 1);
        maxJumpHeight = obj.GetFloatProperty("maxJumpHeight", 10);
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
        animationWidth = animation.width;
    }

    /// <summary>
    /// Initializes the sprite for the player and the cameraTarget
    /// </summary>
    /// <param name="parentScene">The scene this player should callback for change of camera target</param>
    public void initialize(Scene parentScene)
    {
        this.parentScene = parentScene;

        //setup the camera
        this.SetScaleXY(1f, 1f);
        Pivot lookTarget = new Pivot();
        AddChild(lookTarget);
        lookTarget.SetXY(0, 0);
        lookTarget.SetScaleXY(0.4f, 0.4f);
        parentScene.setLookTarget(lookTarget);
        parentScene.jumpToTarget();
        cameraTarget = lookTarget;
        cameraSize = 1f;

        //setup the physics
        _mover = new SingleMover();
        parent.AddChild(_mover);
        _mover.position = position;
        _mover.AddChild(this);
        SetXY(0, 0);
        _mover.SetCollider(new Ball(_mover, _mover.position, radius));
        collider = new EasyDraw(animation.width, animation.height);
        animation.AddChild(collider);
        collider.SetOrigin(animation.width / 2, animation.height / 2);
        collider.SetXY(0, 0);
        _mover.Bounciness = 0.01f;

    }
    #endregion

    #region udpate
    public void Update()
    {
        //update prevPosition
        prevPositions[prevPositionIndex] = _mover.position;
        prevPositionIndex = (prevPositionIndex + 1) % prevPositions.Length;

        //update prevVelocity
        prevVelocities[prevVelocityIndex] = _mover.Velocity;
        prevVelocityIndex = (prevVelocityIndex + 1) % prevVelocities.Length;

        //update radius
        float pRadius = radius;
        UpdateHealth();
        float radiusDelta = radius - pRadius;

        if (_mover.collider is Ball ball)
        {
            ball.radius = radius;
            collider.width = (int)radius;
            collider.height = (int)radius;
        }

        //update mass
        _mover.Mass = Mathf.Map(health01, 0, 1, minMass, maxMass);
        if (_mover.lastCollision != null)
            _mover.collider.position += (_mover.lastCollision.normal * radiusDelta);


        _mover.Step();
        HandleInput();
        PlayerAnimation();
        UpdateUI();
    }

    /// <summary>
    /// Update all health related things of the player
    /// </summary>
    private void UpdateHealth()
    {
        if (_mover.lastCollision != null && _mover.lastCollision.other.owner is ColliderObject other)
        {
            switch (other._collisionType)
            {
                case CollisionType.CONCRETE: health -= _mover.Velocity.Length() / (30 * maxSpeed); break;
                case CollisionType.DIRT: health += _mover.Velocity.Length() / (30 * maxSpeed); break;
                case CollisionType.GRASS: break;
                case CollisionType.NULL: break;
            }
        }
        health = Mathf.Clamp(health, 0, maxHealth);
    }

    /// <summary>
    /// Handle the player input
    /// </summary>
    private void HandleInput()
    {
        Vec2 desiredVelocity = new Vec2(
            ((Input.GetKey(Key.D) ? 1 : 0) - (Input.GetKey(Key.A) ? 1 : 0)) * maxSpeed,
            0);
        Vec2 deltaVelocity = (desiredVelocity - _mover.Velocity) * new Vec2(1,0);
        if (_mover.lastCollision != null)
        {
            _mover.ApplyForce(deltaVelocity * _mover.lastCollision.normal.Normal());
            if (Input.GetKey(Key.W) && _mover.lastCollision.normal.y < -0.3f)
            {
                _mover.Velocity += _mover.lastCollision.normal * Mathf.Map(health01, 0, 1, minJumpHeight, maxJumpHeight);
            }
        }
        else
        {
            _mover.ApplyForce(deltaVelocity * 0.2f * new Vec2(1, 0));
        }

        if (Input.scrolled)
        {
            cameraSize = cameraSize + Input.scrollWheelValue * -0.1f * cameraSize;
            cameraTarget.scale = cameraSize;
        }
    }

    /// <summary>
    /// moves the player and updates all the visuals accordingly
    /// </summary>
    private void PlayerAnimation()
    {
        float targetWidth = radius * 2;
        float scale = targetWidth / animationWidth;
        animation.scale = scale;
        cameraTarget.scale = scale * cameraSize;

        Vec2 a = new Vec2();
        foreach (Vec2 b in prevPositions)
            a += b;
        a /= prevPositions.Length;
        a -= _mover.position;
        animation.position = a;

        a = new Vec2();
        foreach (Vec2 b in prevVelocities)
            a += b;
        a /= prevVelocities.Length;
        animation.rotation = a.x * 2;

        parentScene.setLookTarget(cameraTarget);
        foreach (GameObject other in collider.GetCollisions())
        {
            if (other is CameraTrigger trigger)
            {
                parentScene.setLookTarget(trigger.target);
            }
        }
    }

    /// <summary>
    /// Updates the Energy bar at the top of the screen
    /// </summary>
    private void UpdateUI()
    {

    }
    #endregion
}