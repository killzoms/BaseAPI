using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using BaseAPI.API;
using HarmonyLib;

namespace BaseAPI.Patching
{
    internal class BaseRegisterPiecesAsyncPatch : IPatch
    {
        public IEnumerable<CodeInstruction> TranspilerPatch(IEnumerable<CodeInstruction> instructionEnumerable)
        {
            List<CodeInstruction> instructions = instructionEnumerable.ToList();
            bool startedArray = false;
            int lastArrayIndex = 0;
            for (int i = 0; i < instructions.Count; i++)
            {
                bool keepInstruction = true;
                if (instructions[i + 1].opcode == OpCodes.Newarr && (Type)instructions[i + 1].operand == typeof(Base.PieceDef))
                {
                    int arrayCount = (int)instructions[i].operand;
                    lastArrayIndex = arrayCount;
                    if (BaseRegistrar.pieceDataByName.Count > 0)
                    {
                        arrayCount += BaseRegistrar.pieceDataByName.Count;
                    }

                    instructions[i].operand = arrayCount;
                    startedArray = true;
                }

                if (instructions[i+1].opcode == OpCodes.Newarr && (Type)instructions[i+1].operand == typeof(Base.PieceData))
                {
                    int arrayCount = (int)instructions[i].operand;
                    lastArrayIndex = arrayCount;
                    if (BaseRegistrar.pieceDataByName.Count > 0)
                    {
                        arrayCount += BaseRegistrar.pieceDataByName.Count;
                    }

                    instructions[i].operand = arrayCount;
                    startedArray = true;
                }

                if (instructions[i].opcode == OpCodes.Stfld && startedArray)
                {
                    startedArray = false;
                    PieceData[] pieces = BaseRegistrar.pieceDataByName.Values.ToArray();
                    for (int j = 0; j < BaseRegistrar.pieceDataByName.Count; j++)
                    {
                        int arrayIndex = j + lastArrayIndex + 1;
                        PieceData pieceData = pieces[i];
                        yield return new CodeInstruction(OpCodes.Dup);
                        yield return new CodeInstruction(OpCodes.Ldc_I4_S, arrayIndex-1);
                        yield return new CodeInstruction(OpCodes.Ldc_I4_S, arrayIndex); // pieceId
                        yield return new CodeInstruction(OpCodes.Ldstr, pieceData.Piece.Name);
                        yield return new CodeInstruction(OpCodes.Ldc_I4, pieceData.ExtraCells.x);
                        yield return new CodeInstruction(OpCodes.Ldc_I4, pieceData.ExtraCells.y);
                        yield return new CodeInstruction(OpCodes.Ldc_I4, pieceData.ExtraCells.z);
                        yield return new CodeInstruction(OpCodes.Newobj,
                            typeof(Int3).GetConstructor(BindingFlags.Public, null, CallingConventions.Any,
                                new Type[] { typeof(int), typeof(int), typeof(int) }, null));
                        yield return new CodeInstruction(OpCodes.Newobj,
                            typeof(Base.PieceData).GetConstructor(BindingFlags.Public, null, CallingConventions.Any,
                                new Type[] { typeof(Base.Piece), typeof(string), typeof(Int3) }, null));
                        yield return new CodeInstruction(OpCodes.Stelem, typeof(Base.PieceData));
                    }
                }

                if (keepInstruction)
                {
                    yield return instructions[i];
                }
            }
        }

        public void Patch()
        {
            QMod.HarmonyInstance.Patch(GetMethod(), null, null,
                new HarmonyMethod(this.GetType().GetMethod(nameof(TranspilerPatch))));
        }


        private static Type GetLoadAsyncEnumerableMethod()
        {
            Type[] nestedTypes = typeof(Base).GetNestedTypes(BindingFlags.NonPublic | BindingFlags.Static);
            Type targetEnumeratorClass = null;

            foreach (Type type in nestedTypes)
            {
                if (type.FullName?.Contains("RegisterPiecesAsync") == true)
                {
                    targetEnumeratorClass = type;
                }
            }

            return targetEnumeratorClass;
        }

        private static MethodInfo GetMethod()
        {
            MethodInfo method = GetLoadAsyncEnumerableMethod().GetMethod("MoveNext", BindingFlags.NonPublic | BindingFlags.Instance);

            return method;
        }
    }
}
