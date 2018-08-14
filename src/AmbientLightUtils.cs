﻿using System;
using System.Reflection;
using UnityEngine;

namespace AmbientLights
{
    class AmbientLightUtils
    {
        public static int hour_now;
        public static int minute_now;

        public static int GetCurrentTimeFormatted()
        {
            return (hour_now * 100) + minute_now;
        }

        public static Vector3 StringToVector3(string sVector)
        {
            // Remove the parentheses
            if (sVector.StartsWith("(") && sVector.EndsWith(")"))
            {
                sVector = sVector.Substring(1, sVector.Length - 2);
            }

            // split the items
            string[] sArray = sVector.Split(',');

            // store as a Vector3
            Vector3 result = new Vector3(
                float.Parse(sArray[0]),
                float.Parse(sArray[1]),
                float.Parse(sArray[2]));

            return result;
        }

        public static Color32 ParseColor32(String col) {
            string[] strings = col.Split(',');

            Color32 output = new Color32((byte)float.Parse(strings[0]), (byte)float.Parse(strings[1]), (byte)float.Parse(strings[2]), 255);

            return output;
        }

        public static void RegisterCommands()
        {
            uConsole.RegisterCommand("light_int", new uConsole.DebugCommand(() =>
            {
                float intensity = uConsole.GetFloat();
                AmbientLightControl.light_override = true;
                AmbientLightControl.SetLightsIntensity(intensity);
            }));

            uConsole.RegisterCommand("lightset_int", new uConsole.DebugCommand(() =>
            {
                string set = uConsole.GetString();
                float intensity = uConsole.GetFloat();

                AmbientLightControl.light_override = true;
                AmbientLightControl.SetLightsIntensity(intensity, set);
            }));

            uConsole.RegisterCommand("light_range", new uConsole.DebugCommand(() =>
            {
                float range = uConsole.GetFloat();

                AmbientLightControl.light_override = true;
                AmbientLightControl.SetLightsRange(range);
            }));

            uConsole.RegisterCommand("lightset_range", new uConsole.DebugCommand(() =>
            {
                string set = uConsole.GetString();
                float range = uConsole.GetFloat();

                AmbientLightControl.light_override = true;
                AmbientLightControl.SetLightsRange(range, set);
            }));

            uConsole.RegisterCommand("light_color", new uConsole.DebugCommand(() =>
            {
                string colorR = uConsole.GetString();
                string colorG = uConsole.GetString();
                string colorB = uConsole.GetString();

                Color32 color = AmbientLightUtils.ParseColor32(colorR + "," + colorG + "," + colorB);

                AmbientLightControl.light_override = true;
                AmbientLightControl.SetLightsColor(color);
            }));

            uConsole.RegisterCommand("lightset_color", new uConsole.DebugCommand(() =>
            {
                string set = uConsole.GetString();

                string colorR = uConsole.GetString();
                string colorG = uConsole.GetString();
                string colorB = uConsole.GetString();

                Color32 color = AmbientLightUtils.ParseColor32(colorR + "," + colorG + "," + colorB);

                AmbientLightControl.light_override = true;
                AmbientLightControl.SetLightsColor(color, set);
            }));

            uConsole.RegisterCommand("light_shadow", new uConsole.DebugCommand(() =>
            {
                string shadow = uConsole.GetString();

                AmbientLightControl.light_override = true;
                AmbientLightControl.SetLightsShadow(shadow);
            }));

            uConsole.RegisterCommand("lightset_shadow", new uConsole.DebugCommand(() =>
            {
                string set = uConsole.GetString();
                string shadow = uConsole.GetString();

                AmbientLightControl.light_override = true;
                AmbientLightControl.SetLightsShadow(shadow, set);
            }));

            uConsole.RegisterCommand("light_on", new uConsole.DebugCommand(() =>
            {
                AmbientLightControl.light_override = false;
                AmbientLightControl.MaybeUpdateLightsToPeriod(true);
            }));

            uConsole.RegisterCommand("light_off", new uConsole.DebugCommand(() =>
            {
                AmbientLightControl.light_override = true;
                AmbientLightControl.SetLightsIntensity(0f);
            }));

            uConsole.RegisterCommand("debug_config", new uConsole.DebugCommand(() =>
            {
                Debug.Log(Utils.SerializeObject(AmbientLightControl.config));
                Debug.Log(Utils.SerializeObject(AmbientLightControl.global_periods_config));
            }));

            uConsole.RegisterCommand("p", new uConsole.DebugCommand(() =>
            {
                GetPoint();
            }));
        }

        public static void GetPoint()
        {
            vp_FPSCamera cam = GameManager.GetVpFPSPlayer().FPSCamera;
            RaycastHit raycastHit = DoRayCast(cam.transform.position, cam.transform.forward);
            Debug.Log(raycastHit.point);
        }

        public static RaycastHit DoRayCast(Vector3 start, Vector3 direction)
        {
            RaycastHit result;
            Physics.Raycast(start, direction, out result, float.PositiveInfinity);
            return result;
        }

        public static object GetPrivateFieldObject(object inst, string name)
        {
            FieldInfo field = inst.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic);
            if (field == null)
            {
                return null;
            }
            return field.GetValue(inst);
        }

        public static bool GetPrivateFieldBool(object inst, string name)
        {
            return (GetPrivateFieldObject(inst, name) as bool?) ?? false;
        }

        public static float GetPrivateFieldFloat(object inst, string name)
        {
            object privateFieldObject = GetPrivateFieldObject(inst, name);
            float? num = privateFieldObject as float?;
            if (num == null)
            {
                return 0f;
            }
            return num.GetValueOrDefault();
        }

        public static void SetPrivateFieldObject(object inst, string name, object value)
        {
            FieldInfo field = inst.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic);
            if (field != null)
            {
                field.SetValue(inst, value);
            }
        }

        public static void SetPrivateFieldObject(object inst, string name, object value, Type type)
        {
            FieldInfo field = inst.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic);
            if (field != null && field.FieldType == type)
            {
                field.SetValue(inst, value);
            }
        }

        public static void SetPrivateFieldFloat(object inst, string name, float value)
        {
            SetPrivateFieldObject(inst, name, value, typeof(float));
        }

        public static object InvokePrivateMethod(object inst, string name, params object[] arguments)
        {
            MethodInfo method = inst.GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic);
            if (method != null)
            {
                return method.Invoke(inst, arguments);
            }
            return null;
        }
    }
}
