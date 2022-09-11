using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct MyVector {
    public float x;
    public float y;

    public float magnitude => Mathf.Sqrt(x * x + y * y);
    public MyVector normalized {
        get {
            if (magnitude <= 0.0001f) {
                return new MyVector(0, 0);
            }
            return new MyVector(x / magnitude, y / magnitude);
        }
    }
    public MyVector(float x, float y) {
        this.x = x;
        this.y = y;
    }
    public void Normalize() {
        float tolerance = 0.0001f;
        if (magnitude <= tolerance) {
            x = 0; y = 0;
            return;
        }
        x /= magnitude; y /= magnitude;
    }
    public void Draw(Color color) {
        Debug.DrawLine(Vector3.zero, new Vector3(x, y), color);
    }
    public void Draw(MyVector newOrigin, Color color) {
        Debug.DrawLine(new Vector3(newOrigin.x, newOrigin.y), new Vector3(newOrigin.x + x, newOrigin.y + y), color);
    }
    public static MyVector operator +(MyVector a, MyVector b) {  //Suma
        return new MyVector(a.x + b.x, a.y + b.y);
    }
    public static MyVector operator -(MyVector a, MyVector b) {  //Resta
        return new MyVector(a.x - b.x, a.y - b.y);
    }
    public static MyVector operator *(MyVector a, float b) {     //Producto1
        return new MyVector(a.x * b, a.y * b);
    }
    public static MyVector operator *(float b, MyVector a) {     //Producto1
        return new MyVector(a.x * b, a.y * b);
    }
    public static MyVector operator /(MyVector a, float b) {     //Producto4
        return new MyVector(a.x / b, a.y / b);
    }
}
