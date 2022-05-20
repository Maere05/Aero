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
     *   AOA 30:
     *    
     *   Lift : 421
     *   Drag : 262
     *    
     *    
     */

    class Jumper
    {
        public Vector2 V, FG, FD, FL, Fres, Pos;

        public float VelocityAngle = 0, AoA, time, mass, wind;

        float v0x, v0y;

        public static float LiftC = 421f / 900f;
        public static float DragC = 262f / 900f;


        public string Init(float mass_ ,float time_,float StartVel, float Alpha, float Wind)
        {
            InterpolateAoA(Alpha);

            mass = mass_;
            time = time_;
            wind = Wind;

            v0x = MathF.Cos(VelocityAngle) * StartVel;     // Cosine function is in radians :(
            v0y = -(MathF.Sin(VelocityAngle) * StartVel);

            //Position
            Pos = Vector2.Zero;

            //Gravity 
            FG.X = 0;
            FG.Y = mass * -9.81f;

            //Starting Velocity

            V.X =  MathF.Cos(VelocityAngle) * StartVel;     // Cosine function is in radians :(
            V.Y = -(MathF.Sin(VelocityAngle) * StartVel);

            //Lift Forces
            FL = Vector2.Normalize(new Vector2(-V.Y, V.X)) * LiftC * MathF.Pow(V.Length() + wind,2);

            //Drag Forces
            FD = Vector2.Normalize(new Vector2(-V.X, -V.Y)) * DragC * MathF.Pow(V.Length() + wind,2);

            return "Pos:" + Pos.ToString() + "\nGravity" + FG.ToString() + "\nlift" + FL.ToString() + "\nDrag" + FD.ToString() + "\nVel" + V.ToString();
        }

        public Vector2 Timestep()
        {
            VelocityAngle = MathF.Atan(V.Y / V.X);

            //Lift Forces

            FL = Vector2.Normalize(new Vector2(-V.Y, V.X)) * LiftC * MathF.Pow(V.Length() + wind,2);

            //Drag Forces

            FD = Vector2.Normalize(new Vector2(-V.X, -V.Y)) * DragC * MathF.Pow(V.Length() + wind,2);

            Fres = FG + FD + FL;
            V.Y = v0y + (Fres.Y / mass) * time;
            v0y = V.Y;

            V.X = v0x + (Fres.X / mass) * time;
            v0x = V.X;

            Pos.X += V.X*time;      // m/s * s = m
            Pos.Y -= V.Y*time;                          // minus because its an easy fix

            return Pos;
        }

        public void InterpolateAoA(float Alpha)
        {
            float t;
            if (Alpha <= 10f)
            {
                LiftC = 130f / 900f;
                DragC = 53f / 900f;
            }
            else if (Alpha <= 25f)
            {
                t = (Alpha - 10f) / 15f;

                LiftC = Vector2.Lerp(new Vector2(130f, 53f), new Vector2(342f, 186f), t).X / 900f;
                DragC = Vector2.Lerp(new Vector2(130f, 53f), new Vector2(342f, 186f), t).Y / 900f;
                

            }
            else if (Alpha <= 30f)
            {
                t = (Alpha - 25f) / 5f;

                LiftC = Vector2.Lerp(new Vector2(342f, 186f), new Vector2(421f, 262f), t).X / 900f;
                DragC = Vector2.Lerp(new Vector2(342f, 186f), new Vector2(421f, 262f), t).Y / 900f;

            }
            else if (Alpha <= 35f)
            {
                t = (Alpha - 30f) / 5f;

                LiftC = Vector2.Lerp(new Vector2(421f, 262f), new Vector2(470f, 337f), t).X / 900f;
                DragC = Vector2.Lerp(new Vector2(421f, 262f), new Vector2(470f, 337f), t).Y / 900f;

            }
            else if (Alpha <= 40f)
            {
                t = (Alpha - 35f) / 5f;

                LiftC = Vector2.Lerp(new Vector2(470f, 337f), new Vector2(502f, 414f), t).X / 900f;
                DragC = Vector2.Lerp(new Vector2(470f, 337f), new Vector2(502f, 414f), t).Y / 900f;

            }
            else
            {
                t = (Alpha - 40f) / 20f;

                LiftC = Vector2.Lerp(new Vector2(502f, 414f), new Vector2(370f, 999f), t).X / 900f;
                DragC = Vector2.Lerp(new Vector2(502f, 414f), new Vector2(370f, 999f), t).Y / 900f;

            }
                                          
        }

    }   
    
}
