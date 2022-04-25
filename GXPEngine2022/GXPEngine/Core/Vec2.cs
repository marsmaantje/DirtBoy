using System;
using GXPEngine; // Allows using Mathf functions

public struct Vec2
{
    public float x;
    public float y;


    public Vec2(float pX = 0, float pY = 0)
    {
        x = pX;
        y = pY;
    }

    public Vec2(GXPEngine.Core.Vector2 vec)
    {
        x = vec.x;
        y = vec.y;
    }

    /// <summary>
    /// Makes the Vector a unit vecotr
    /// </summary>
    public void Normalize() => this /= (Length() > 0) ? Length() : 1;

    /// <summary>
    /// Creates a new Vector and normalizes it without modifying the original
    /// </summary>
    /// <returns>new normalized vector</returns>
    public Vec2 Normalized()
    {
        Vec2 returnVec = this;
        returnVec.Normalize();
        return returnVec;
    }

    /// <summary>
    /// Sets the X and Y components
    /// </summary>
    /// <param name="x">X component of the vector</param>
    /// <param name="y">Y component of the vector</param>
    public void SetXY(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    /// <summary>
    /// Sets the X and Y components according to the angle and length given, 0 degrees is x forward
    /// </summary>
    /// <param name="angle">angle in radians</param>
    /// <param name="length">length of the vector</param>
    public void setAngleLength(float angle, float length)
    {
        SetXY(length * Mathf.Cos(angle), length * Mathf.Sin(angle));
    }

    /// <summary>
    /// Rotate the vector by an angle
    /// </summary>
    /// <param name="radians">the angle in radians to rotate the vector by</param>
    public void Rotate(float radians, Vec2 pivot = new Vec2())
    {
        this -= pivot;
        this.SetXY(x * Mathf.Cos(radians) - y * Mathf.Sin(radians), x * Mathf.Sin(radians) + y * Mathf.Cos(radians));
        this += pivot;
    }

    /// <summary>
    /// Get the distance between two vectors
    /// </summary>
    /// <param name="a">first vecor</param>
    /// <param name="b">second vector</param>
    /// <returns>Distance between two given vectors</returns>
    public static float Distance(Vec2 a, Vec2 b)
    {
        Vec2 diff = a - b;
        return diff.Length();
    }

    /// <summary>
    /// Gets the Length of the Vector
    /// </summary>
    /// <returns>Length of the Vector</returns>
    public float Length() => Mathf.Sqrt((this.x * this.x) + (this.y * this.y));

    /// <summary>
    /// Gets and sets the angle of the Vector in radians
    /// </summary>
    float Angle
    {
        get => Mathf.Atan2(y, x);
        set
        {
            SetXY(Length(), 0);
            Rotate(value);
        }
    }

    /// <summary>
    /// Rotates the vector by 90 degrees and normalizes it
    /// </summary>
    /// <returns>Returns the normalized normalVector of the given vector</returns>
    public Vec2 Normal()
    {
        return new Vec2(-y, x).Normalized();
    }

    /// <summary>
    /// returns the Dot product between this and the given vector
    /// </summary>
    /// <param name="other">other Vector</param>
    /// <returns>length of dot product</returns>
    public float Dot(Vec2 other)
    {
        return x * other.x + y * other.y;
    }

    /// <summary>
    /// calculates the projection of the current vector onto the other
    /// </summary>
    /// <param name="other">vector to project on</param>
    /// <returns>projected vector</returns>
    public Vec2 Project(Vec2 other)
    {
        Vec2 normalized = other.Normalized();
        return this.Dot(normalized) * normalized;
    }

    /// <summary>
    /// Reflects the vector along the given normal
    /// </summary>
    /// <param name="normal">normal to reflect against</param>
    /// <returns>the reflected vector</returns>
    public Vec2 Reflect(Vec2 normal, float bounciness = 1)
    {
        normal.Normalize(); //just to be sure that the normal is normalized
        this = this - ((1 + bounciness) * (this.Dot(normal) * normal));
        
        return this;
    }

    /// <summary>
    /// Clamps the current vector between a and b
    /// </summary>
    /// <param name="a">First vector to clamp between</param>
    /// <param name="b">Second vector to clamp between</param>
    /// <returns>Clamped version of the curretn vector</returns>
    public Vec2 Clamp(Vec2 a, Vec2 b)
    {
        float minX = a.x < b.x ? a.x : b.x;
        float maxX = a.x > b.x ? a.x : b.x;
        float minY = a.y < b.y ? a.y : b.y;
        float maxY = a.y > b.y ? a.y : b.y;
        return new Vec2(Mathf.Clamp(x, minX, maxX), Mathf.Clamp(y, minY, maxY));
    }

    

    public float GetAngleDegrees() => Rad2Deg(Angle);
    public float GetAngleRadians() => Angle;
    public void SetAngleDegrees(float Degrees) => Angle = Deg2Rad(Degrees);
    public void SetAngleRadians(float Radians) => Angle = Radians;
    public void RotateDegrees(float Degrees) => Rotate(Deg2Rad(Degrees));
    public void RotateRadians(float Radians) => Rotate(Radians);
    public void RotateAroundDegrees(float Degrees, Vec2 Pivot) => Rotate(Deg2Rad(Degrees), Pivot);
    public void RotateAroundRadians(float Radians, Vec2 Pivot) => Rotate(Radians, Pivot);
    


    // Operators
    public static Vec2 operator +(Vec2 a, Vec2 b) => new Vec2(a.x + b.x, a.y + b.y);
    public static Vec2 operator -(Vec2 a, Vec2 b) => new Vec2(a.x - b.x, a.y - b.y);
    public static Vec2 operator -(Vec2 a) => new Vec2(-a.x, -a.y);
    public static Vec2 operator -(Vec2 a, float b) => new Vec2(a.x - b, a.y - b);
    public static Vec2 operator -(float a, Vec2 b) => new Vec2(a - b.x, a - b.y);
    public static Vec2 operator *(Vec2 a, Vec2 b) => new Vec2(a.x * b.x, a.y * b.y);
    public static Vec2 operator *(Vec2 a, float b) => new Vec2(a.x * b, a.y * b);
    public static Vec2 operator *(float b, Vec2 a) => new Vec2(a.x * b, a.y * b);
    public static Vec2 operator /(Vec2 a, Vec2 b) => new Vec2(a.x / b.x, a.y / b.y);
    public static Vec2 operator /(Vec2 a, float b) => new Vec2(a.x / b, a.y / b);
    public static bool operator ==(Vec2 a, Vec2 b) => (a.x == b.x && a.y == b.y);
    public static bool operator !=(Vec2 a, Vec2 b) => (a.x != b.x || a.y != b.y);

    public override string ToString()
    {
        return String.Format("({0},{1})", x, y);
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    /// <summary>
    /// Returns a duplicate of this struct
    /// </summary>
    /// <returns></returns>
    public Vec2 Copy()
    {
        return new Vec2(x, y);
    }

    /// <summary>
    /// Calculates the vector from the first to the second input
    /// </summary>
    /// <param name="from">Start point</param>
    /// <param name="to">End point</param>
    /// <returns>Vector going from start to end point</returns>
    public static Vec2 FromTo(Vec2 from, Vec2 to)
    {
        return to.Copy() - from;
    }

    /// <summary>
    /// Returns the length of the Z-axis given Vector a and b are on the XY plane
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>The length of the Z-axis given Vector a and b are on the XY plane</returns>
    public static float Cross(Vec2 a, Vec2 b)
    {
        return (a.x * b.y) - (a.y * b.x);
    }

    public static float Deg2Rad(float Degrees) => Degrees * Mathf.DegToRad;
    public static float Rad2Deg(float Radians) => Radians * Mathf.RadToDeg;
    public static Vec2 GetUnitVectorDeg(float Degrees)
    {
        Vec2 returnVec = new Vec2(1, 0);
        returnVec.Rotate(Deg2Rad(Degrees));
        return returnVec;
    }
    
    public static Vec2 GetUnitVectorRad(float Radians)
    {
        Vec2 returnVec = new Vec2(1, 0);
        returnVec.Rotate(Radians);
        return returnVec;
    }

    public static Vec2 RandomUnitVector()
    {
        Vec2 ReturnVec = new Vec2(1, 0);
        ReturnVec.Rotate(Utils.Random(0f ,2f) * Mathf.PI);
        return ReturnVec;
    }

    public static float PointToLineDistance(Vec2 point, Vec2 line)
    {
        Vec2 projected = point.Project(line).Clamp(new Vec2(0, 0), line);

        return Vec2.Distance(point, projected);
    }

    public static void DoTests()
    {
        Console.WriteLine("Testing Vec2 implementation:");

        //length
        Vec2 x = new Vec2(-3, 4);
        Console.WriteLine("Length check (expected 5): " + (x.Length() == 5f) + " " + x.Length());

        //normalize
        x.SetXY(-3, 4);
        x.Normalize();
        Console.WriteLine("Normalize check (expected (-0.6, 0.8)): " + x);
        x *= 3;
        Console.WriteLine("Normalized check (expected (-0.6, 0.8)): " + x.Normalized());



        // +
        Console.WriteLine("+ check (expected(3,2)): " + ((new Vec2(2, 3) + new Vec2(1, -1)) == new Vec2(3, 2)));

        // -
        Console.WriteLine("- check (expected(3,2)): " + ((new Vec2(2, 3) - new Vec2(-1, 1)) == new Vec2(3, 2)));

        // *
        Console.WriteLine("* check (expected(3,2)): " + ((new Vec2(-6, 1) * new Vec2(-0.5f, 2)) == new Vec2(3, 2)));

        // /
        Console.WriteLine("/ check (expected(3,2)): " + ((new Vec2(-6, 1) / new Vec2(-2f, 0.5f)) == new Vec2(3, 2)));

        // test GetUnitVector and Angle degrees
        Vec2 v1 = Vec2.GetUnitVectorDeg(90);
        Console.WriteLine("GetUnitVectorDegrees check (expected(0,1))" + v1);
        Console.WriteLine("GetAngleDegrees check (expected 90) " + v1.GetAngleDegrees());
        v1.SetAngleDegrees(180);
        Console.WriteLine("SetAngleDegrees check (expected (-1,0)) " + v1);
        v1.RotateAroundDegrees(180, new Vec2(1, 0));
        Console.WriteLine("RotateAroundDegrees check (expected (3,0)) " + v1);

        // test GetUnitVector and Angle radians
        v1 = Vec2.GetUnitVectorRad(Mathf.PI / 2);
        Console.WriteLine("GetUnitVectorDegrees check (expected(0,1))" + v1);
        Console.WriteLine("GetAngleDegrees check (expected " + Mathf.PI / 2 + ") " + v1.GetAngleRadians());
        v1.SetAngleRadians(Mathf.PI);
        Console.WriteLine("SetAngleDegrees check (expected (-1,0)) " + v1);
        v1.RotateAroundRadians(Mathf.PI, new Vec2(1, 0));
        Console.WriteLine("RotateAroundRadians check (expected (3,0)) " + v1);

        Console.WriteLine("Deg2Rad check (expected " + Mathf.PI + ") " + Vec2.Deg2Rad(180));
        Console.WriteLine("Deg2Rad check (expected " + 180 + ") " + Vec2.Rad2Deg(Mathf.PI));

    }
}

