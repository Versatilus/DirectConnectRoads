using HarmonyLib;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;

namespace DirectConnectRoads.Patches.NetNodePatches {
    using Util;

    [HarmonyPatch()]
    public static class RenderInstance {
        static void Log(string m) => Extensions.Log("NetNode_RenderInstance Transpiler: " + m);

        // RenderInstance(RenderManager.CameraInfo cameraInfo, ushort nodeID, NetInfo info, int iter, Flags flags, ref uint instanceIndex, ref RenderManager.Instance data)
        static MethodInfo Target => typeof(global::NetNode).GetMethod("RenderInstance", BindingFlags.NonPublic | BindingFlags.Instance);
        static MethodBase TargetMethod() {
            var ret = Target;
            Extensions.Assert(ret != null, "did not manage to find original function to patch");
            Log("aquired method " + ret);
            return ret;
        }

        //static bool Prefix(ushort nodeID){}
        public static IEnumerable<CodeInstruction> Transpiler(ILGenerator il, IEnumerable<CodeInstruction> instructions) {
            try {
                var codes = TranspilerUtils.ToCodeList(instructions);
                CheckMedianCommons.ApplyCheckMedian(codes, Target, occurance:1);

                Log("successfully patched NetNode.RenderInstance");
                return codes;
            }catch(Exception e) {
                Log(e + "\n" + Environment.StackTrace);
                throw e;
            }
        }
    } // end class
} // end name space