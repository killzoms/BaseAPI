using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BaseAPI.API;
using HarmonyLib;
using UnityEngine;

namespace BaseAPI.Patching
{
    internal class BaseProcessActiveLoadPiecePatch : IPatch
    {
        public void Prefix(Base.PieceData pieceData, Base.PieceDef[] ___pieces)
        {
            if (BaseRegistrar.pieceDataByName.ContainsKey(pieceData.name))
            {
                PieceData piece = BaseRegistrar.pieceDataByName[pieceData.name];
                piece.Prefab.SetActive(value: false);
                ___pieces[(int)pieceData.piece] = new Base.PieceDef(piece.Prefab, pieceData.extraCells, Quaternion.identity);
            }
        }

        public void Patch()
        {
            QMod.HarmonyInstance.Patch(
                typeof(Base).GetMethod(nameof(Base.ProcessActiveLoadPiece),
                    BindingFlags.Static | BindingFlags.NonPublic),
                new HarmonyMethod(
                    typeof(BaseProcessActiveLoadPiecePatch).GetMethod(nameof(Prefix))));
        }
    }
}
