using ColossalFramework;
using ICities;
using System.Diagnostics;
using System.Reflection;
using System;
using System.Linq;
using GenericGameBridge.Service;
using System.Collections.Generic;

namespace DirectConnectRoads.Util {

    public static class Extensions {
        public static void CopyProperties(object target, object origin) {
            FieldInfo[] fields = origin.GetType().GetFields();
            foreach (FieldInfo fieldInfo in fields) {
                //Extensions.Log($"Copying field:<{fieldInfo.Name}> ...>");
                object value = fieldInfo.GetValue(origin);
                string strValue = value?.ToString() ?? "null";
                //Extensions.Log($"Got field value:<{strValue}> ...>");
                fieldInfo.SetValue(target, value);
                //Extensions.Log($"Copied field:<{fieldInfo.Name}> value:<{strValue}>");
            }
        }

        public static void CopyProperties<T>(object target, object origin) {
            Assert(target is T, "target is T");
            Assert(origin is T, "origin is T");
            FieldInfo[] fields = typeof(T).GetFields();
            foreach (FieldInfo fieldInfo in fields) {
                //Extensions.Log($"Copying field:<{fieldInfo.Name}> ...>");
                object value = fieldInfo.GetValue(origin);
                //string strValue = value?.ToString() ?? "null";
                //Extensions.Log($"Got field value:<{strValue}> ...>");
                fieldInfo.SetValue(target, value);
                //Extensions.Log($"Copied field:<{fieldInfo.Name}> value:<{strValue}>");
            }
        }

        public static T Max<T>()
            where T : Enum =>
           System.Enum.GetValues(typeof(T)).Cast<T>().Max();

        public static void SetBit(this ref byte b, int idx) => b |= (byte)(1 << idx);
        public static void ClearBit(this ref byte b, int idx) => b &= ((byte)~(1 << idx));
        public static bool GetBit(this byte b, int idx) => (b & (byte)(1 << idx)) != 0;
        public static void SetBit(this ref byte b, int idx, bool value) {
            if (value)
                b.SetBit(idx);
            else
                b.ClearBit(idx);
        }

        internal static AppMode currentMode => SimulationManager.instance.m_ManagersWrapper.loading.currentMode;
        internal static bool CheckGameMode(AppMode mode) => CheckGameMode(new[] { mode });
        internal static bool CheckGameMode(AppMode[] modes) {
            try {
                foreach (var mode in modes) {
                    if (currentMode == mode)
                        return true;
                }
            } catch { }
            return false;
        }
        internal static bool InGame => CheckGameMode(AppMode.Game);
        internal static bool InAssetEditor => CheckGameMode(AppMode.AssetEditor);

        internal static void Log(string m, bool u=false) {
            Util.Log.Info(m);
            if (u) UnityEngine.Debug.Log(m);
        }
        
        internal static void Assert(bool con, string m="") {
            if (!con) throw new System.Exception("Assertion failed: " + m);
        }

        internal static string BIG(string m) {
            string mul(string s, int i) {
                string ret_ = "";
                while (i-- > 0) ret_ += s;
                return ret_;
            }
            m = "  " + m + "  ";
            int n = 120;
            string stars1 = mul("*", n);
            string stars2 = mul("*", (n - m.Length) / 2);
            string ret = stars1 + "\n" + stars2 + m + stars2 + "\n" + stars1;
            return ret;
        }

        internal static string GetPrettyFunctionName(MethodInfo m) {
            string s = m.Name;
            string[] ss = s.Split(new[] { "g__", "|" }, System.StringSplitOptions.RemoveEmptyEntries);
            if (ss.Length == 3)
                return ss[1];
            return s;
        }

        public static INetService netService => TrafficManager.Constants.ServiceFactory.NetService;

        /// <summary>
        /// Creates and string of all items with enumerable inpute as {item1, item2, item3}
        /// null argument returns "Null"
        /// </summary>
        internal static string ToSTR<T>(this IEnumerable<T> enumerable) {
            if (enumerable == null)
                return "Null";
            string ret = "{ ";
            foreach (T item in enumerable) {
                ret += $"{item}, ";
            }
            ret.Remove(ret.Length - 2, 2);
            ret += " }";
            return ret;
        }

        internal static string ToSTR(this List<LanePos> laneList) =>
            (from lanePos in laneList select lanePos.laneId).ToSTR();

    }
}
