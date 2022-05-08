using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Aero
{
    /*
     *      Formulas:
     * ---------------------
     *      v = v0 + a*t
     * 
     *      a = f/m
     * 
     *      v = v0 + (f/m) * t
     *      
     *      
     *      
     *   AOA 30:
     *    
     *   Lift : 421
     *   Drag : 262
     *    
     *    
     * 
     */

    class Jumper
    {
        public Vector2 V, FG, FD, FL, Fres, Pos;

        public float VelocityAngle = 0, AoA, time, mass;

        float gravityConst = 9.81f;

        float v0x,v0y, fx, fy, t, m;

        static float LiftC = 421f / 900f;
        static float DragC = 262f / 900f;



        public string Init(float mass_ ,float time_,float StartVel)
        {
            mass = mass_;
            time = time_;

            v0x = MathF.Cos(VelocityAngle) * StartVel;     // Cosine function is in radians :(
            v0y = -(MathF.Sin(VelocityAngle) * StartVel);

            //Position
            Pos = new Vector2(20f, 20f);

            //Gravity Force
            FG = new Vector2(0f, mass * (-9.81f));      

            //Starting Velocity
            V = new Vector2();

            V.X =  MathF.Cos(VelocityAngle) * StartVel;     // Cosine function is in radians :(
            V.Y = -(MathF.Sin(VelocityAngle) * StartVel);

            //Lift Forces
            FL = new Vector2();

            FL.X = MathF.Sin(VelocityAngle) * (LiftC * V.LengthSquared());    // Mutliply with V^2            
            FL.Y = MathF.Cos(VelocityAngle) * (LiftC * V.LengthSquared());
            

            //Drag Forces
            FD = new Vector2();

            FD.X = -(MathF.Cos(VelocityAngle) * (DragC * V.LengthSquared()));   // Multipy with V^2
            FD.Y =  MathF.Sin(VelocityAngle) * (DragC * V.LengthSquared());

            return "Pos:" + Pos.ToString() + "\nGravity" + FG.ToString() + "\nlift" + FL.ToString() + "\nDrag" + FD.ToString() + "\nVel" + V.ToString();
        }

        public Vector2 Timestep()
        {


            Fres = FG + FD + FL;

            V.Y = v0y + (Fres.Y / mass) * time;
            v0y = V.Y;

            V.X = v0x + (Fres.X / mass) * time;
            v0x = V.X;


            Pos.X += V.X*time;      // m/s * s = m
            Pos.Y -= V.Y*time;                          // minus because its an easy fix


            VelocityAngle = MathF.Atan(V.Y / V.X);

            //Lift Forces

            FL.X = MathF.Sin(VelocityAngle) * (LiftC * V.LengthSquared());    // Mutliply with V^2            
            FL.Y = MathF.Cos(VelocityAngle) * (LiftC * V.LengthSquared());


            //Drag Forces

            FD.X = -MathF.Cos(VelocityAngle) * (DragC * V.LengthSquared());   // Multipy with V^2
            FD.Y = MathF.Sin(VelocityAngle) * (DragC * V.LengthSquared());

            float vel = V.Length();

            return Pos;
        }
    }    
}
