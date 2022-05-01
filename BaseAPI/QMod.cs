using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BaseAPI.Patching;
using HarmonyLib;
using QModManager.API.ModLoading;

namespace BaseAPI
{
    [QModCore]
    public static class QMod
    {
        internal static Harmony HarmonyInstance;

        [QModPostPatch]
        public static void PostPatch()
        {
            HarmonyInstance = new Harmony("BaseAPI");

            IEnumerable<Type> patchTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.BaseType == typeof(IPatch));

            foreach (Type patchType in patchTypes)
            {
                IPatch patch = (IPatch)Activator.CreateInstance(patchType);
                patch.Patch();
            }
        }
    }
}
