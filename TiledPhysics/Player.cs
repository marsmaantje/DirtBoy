using System;
using GXPEngine;
using TiledMapParser;
using Physics;
using Objects;

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
    public float health;
    float maxHealth, minHealthRadius, maxHealthRadius, maxSpeed, minMass, maxMass;

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
        maxSpeed = obj.GetFloatProperty("maxSpeed", 10);
        minMass = obj.GetFloatProperty("minMass", 1);
        maxMass = obj.GetFloatProperty("maxMass", 10);
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
        this.SetScaleXY(1f, 1f);
        Pivot lookTarget = new Pivot();
        AddChild(lookTarget);
        lookTarget.SetXY(0, 0);
        lookTarget.SetScaleXY(0.4f, 0.4f);
        parentScene.setLookTarget(lookTarget);
        parentScene.jumpToTarget();
        cameraTarget = lookTarget;

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
        _mover.Bounciness = 0.5f;
    }

    public void Update()
    {
        float pRadius = radius;
        UpdateHealth();
        float radiusDelta = radius - pRadius;

        if (_mover.collider is Ball ball)
        {
            ball.radius = radius;
            collider.width = (int)radius;
            collider.height = (int)radius;
        }

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
                case CollisionType.CONCRETE: health -= _mover.Velocity.Length() / (50 * maxSpeed); break;
                case CollisionType.DIRT: health += maxSpeed / (50 * _mover.Velocity.Length()); break;
                case CollisionType.GRASS: break;
                case CollisionType.NULL: break;
            }
        }
        health = Mathf.Clamp(health, 0, maxHealth);
        /*
        if (health < 1)
        {
            health = maxHealth;
        }
        */
    }

    /// <summary>
    /// Handle the player input
    /// </summary>
    private void HandleInput()
    {
        Vec2 desiredVelocity = new Vec2(
            ((Input.GetKey(Key.D) ? 1 : 0) - (Input.GetKey(Key.A) ? 1 : 0)) * maxSpeed,
            0);
        Vec2 deltaVelocity = desiredVelocity - _mover.Velocity;
        if (_mover.lastCollision != null)
        {
            //_mover.Accelaration = (deltaVelocity) * 0.1f * _mover.lastCollision.normal.Normal();
            _mover.ApplyForce(deltaVelocity * _mover.lastCollision.normal.Normal());
            if (Input.GetKey(Key.W))
            {
                //_mover.Velocity += _mover.lastCollision.normal * maxSpeed;
                _mover.ApplyForce(new Vec2(0, -maxSpeed) * maxMass);
            }
        }
        else
        {
            //_mover.ApplyForce(deltaVelocity * 0.5f * new Vec2(1, 0));
            _mover.Accelaration = (desiredVelocity - _mover.Velocity) * 0.05f * new Vec2(1, 0);
        }
    }

    /// <summary>
    /// moves the player and updates all the visuals accordingly
    /// </summary>
    private void PlayerAnimation()
    {
        animation.scale = Mathf.Map(health01, 0, 1, 0.1f, 1f);
        animation.rotation = _mover.Velocity.x * 2;
        animation.position -= (animation.position - _mover.Velocity) * 0.01f;
        cameraTarget.scale = animation.scale * 0.5f;

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
}