using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace BaseAPI.Patching
{
    internal class BaseProcessActiveLoadPiecePatch : IPatch
    {
        public void Prefix(Base.PieceData pieceData)
        {
            if (BaseRegistrar.pieceDataByName.ContainsKey(pieceData.name))
            {
                
            }
        }

        public void Patch()
        {
            QMod.HarmonyInstance.Patch(
                typeof(Base).GetMethod(nameof(Base.ProcessActiveLoadPiece),
                    BindingFlags.Static | BindingFlags.NonPublic),
                new HarmonyMethod(
                    typeof(BaseProcessActiveLoadPiecePatch).GetMethod(nameof(BaseProcessActiveLoadPiecePatch.Prefix))));
        }
    }
}
