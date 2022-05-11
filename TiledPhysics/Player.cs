using System;
using GXPEngine;
using TiledMapParser;
using Physics;
using Objects;
using Particles;

public class Player : Pivot
{
    #region variables
    //objects
    TiledObject obj;
    private Scene parentScene;
    ParticleEmitter emitter;
    float emitRate = 1f;
    //camera target when following the player
    public Pivot cameraTarget;
    AnimationSprite animation;
    SingleMover _mover;
    EasyDraw _collider;
    public float health;
    bool shooting;
    float maxHealth, minHealthRadius, maxHealthRadius, minSpeed, maxSpeed, minMass, maxMass, minJumpHeight, maxJumpHeight, animationWidth, cameraSize;
    Vec2[] prevPositions = new Vec2[2];
    int prevPositionIndex = 0;
    Vec2[] prevVelocities = new Vec2[5];
    int prevVelocityIndex = 0;
    int frameOffset = 0;
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
    bool isOnGround
    {
        get => _mover.lastCollision != null && _mover.lastCollision.normal.y < 0f;
    }
    CollisionType groundType
    {
        get
        {
            if (_mover.lastCollision != null && _mover.lastCollision.other.owner is ColliderObject other)
                return other._collisionType;
            return CollisionType.NULL;
        }
    }

    public Mover Mover
    {
        get => _mover;
    }

    public GXPEngine.Core.Collider Collider
    {
        get => _collider.collider;
    }

    #endregion

    #region construction
    public Player() : base() { ReadVariables(); }

    public Player(TiledObject obj) : base()
    {
        this.obj = obj;
        ReadVariables();
        createAnimation();
        emitter = new ParticleEmitter("sprites/empty.png");
    }

    public Player(String filename, int cols, int rows, TiledObject obj) : base()
    {
        this.obj = obj;
        ReadVariables();
        emitter = new ParticleEmitter("sprites/empty.png");
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
        cameraSize = 4f;

        //setup the physics
        _mover = new SingleMover();
        parent.AddChild(_mover);
        _mover.position = position;
        _mover.AddChild(this);
        SetXY(0, 0);
        _mover.SetCollider(new Ball(_mover, _mover.position, radius));
        _collider = new EasyDraw(animation.width, animation.height);
        animation.AddChild(_collider);
        _collider.SetOrigin(animation.width / 2, animation.height / 2);
        _collider.SetXY(0, 0);
        _mover.Bounciness = 0.01f;
        _mover.AirFriction = 0.01f;
        _mover.Friction = 0.01f;
        //setting particle parent
        emitter.particleSpace = parentScene;

        PlayerAnimation();
        parentScene.jumpToTarget();
    }
    #endregion

    #region Update
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
            _collider.width = (int)radius;
            _collider.height = (int)radius;
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
        switch (groundType)
        {
            case CollisionType.TREE: health -= 0.01f + _mover.Velocity.Length() / 50; break;
            case CollisionType.DIRT: health += 0.01f + _mover.Velocity.Length() / 50; break;
            case CollisionType.GRASS: break;
            case CollisionType.NULL: break;
        }
        health = Mathf.Clamp(health, 0, maxHealth);
    }

    /// <summary>
    /// Handle the player input
    /// </summary>
    private void HandleInput()
    {
        //debug log
        if (Input.GetKeyDown(Key.J))
            Console.WriteLine("Velocity: {0}, Actual Velocity: {1}", _mover.Velocity, _mover.position - prevPositions[prevPositionIndex]);

        //movement
        Vec2 desiredVelocity = new Vec2(
            ((Input.GetKey(Key.D) ? 1 : 0) - (Input.GetKey(Key.A) ? 1 : 0)) * Mathf.Map(health01, 0, 1, minSpeed, maxSpeed),
            0);
        Vec2 deltaVelocity = (desiredVelocity - _mover.Velocity);
        if (_mover.lastCollision != null)
        {
            _mover.ApplyForce(deltaVelocity * _mover.lastCollision.normal.Normal());
            if (Input.GetKey(Key.W) && _mover.lastCollision.normal.y < -0.3f)
            {
                _mover.Velocity += new Vec2(0,-1) * Mathf.Map(health01, 0, 1, minJumpHeight, maxJumpHeight);
            }
        }
        else
        {
            _mover.ApplyForce(deltaVelocity * 0.2f * new Vec2(1, 0));
        }

        //zoom
        if (Input.scrolled)
        {
            cameraSize = cameraSize + Input.scrollWheelValue * -0.1f * cameraSize;
            cameraTarget.scale = cameraSize;
        }

        //shooting
        if (Input.GetMouseButtonDown(0) && health > maxHealth / 3)
        {//if click, shoot
            if (GlobalVariables.shooting)
            {
                Vec2 relativeMouseDirection = new Vec2(Input.mouseX, Input.mouseY) - this.TransformPoint(new Vec2());
                relativeMouseDirection /= parentScene.scale;
                //Gizmos.DrawArrow(0, 0, relativeMouseDirection.x, relativeMouseDirection.y, 0.1f, this);
                Shoot(relativeMouseDirection, 30);
                health -= maxHealth / 3.5f;
            }
        }
    }

    void Shoot(Vec2 direction, float strength)
    {
        direction.Normalize();
        Vec2 speed = direction * strength;
        Shot shot = new Shot("sprites/empty.png", 1, 1, 2000);
        parentScene.AddChild(shot);
        shot.position = _mover.position + direction * radius;
        shot.initialize(parentScene);
        shot.Mover.Velocity = speed;

    }

    /// <summary>
    /// moves the player and updates all the visuals accordingly
    /// </summary>
    private void PlayerAnimation()
    {
        //update scale
        float targetWidth = radius * 2;
        float scale = targetWidth / animationWidth;
        animation.scale = scale;
        cameraTarget.scale = scale * cameraSize;

        //smooth position
        Vec2 a = new Vec2();
        foreach (Vec2 b in prevPositions)
            a += b;
        a /= prevPositions.Length;
        a -= _mover.position;
        animation.position = a + new Vec2(0, Mathf.Map(health01, 0, 1, 5, 15));

        //smooth rotation
        a = new Vec2();
        foreach (Vec2 b in prevVelocities)
            a += b;
        a /= prevVelocities.Length;
        animation.rotation = a.x * 2;

        //camera control
        parentScene.setLookTarget(cameraTarget);
        foreach (GameObject other in _collider.GetCollisions())
        {
            if (other is CameraTrigger trigger)
            {
                parentScene.setLookTarget(trigger.target);
            }
        }

        //particles
        /*switch (groundType)
        {
            case CollisionType.CONCRETE: emitter.sprite = "sprites/concreteParticle.png"; break;
            case CollisionType.DIRT: emitter.sprite = "sprites/dirtParticle.png"; break;
            case CollisionType.GRASS: emitter.sprite = "sprites/grassParticle.png"; break;
            case CollisionType.NULL: break;
        }
        
        emitter.rate = isOnGround ? emitRate : 0;*/
        
        animation.Animate(1 / 60f * 8);

        if (_mover.Velocity.Length() > 0.1f) //we are moving
        {
            if (_mover.Velocity.x > 0)
                animation.Mirror(false, false);
            else
                animation.Mirror(true, false);

            if (isOnGround)
                animation.SetCycle(8 + frameOffset, 2);
            else
            {
                if (_mover.Velocity.y < 0) //we are moving up
                {
                    animation.SetCycle(4 + frameOffset, 2);
                }
                else //we are moving down
                {
                    animation.SetCycle(6 + frameOffset, 2);
                }
            }
        }
        else
        {
            animation.SetCycle(0 + frameOffset, 2);
        }

        if(isOnGround)
            frameOffset = (Time.time / 1000f) % 1 < 0.5f ? 2 : 0;
        else
            frameOffset = 0;
    }

    /// <summary>
    /// Updates the Energy bar at the top of the screen
    /// </summary>
    private void UpdateUI()
    {

    }
    #endregion
}