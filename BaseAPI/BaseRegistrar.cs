using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseAPI.API;

namespace BaseAPI
{
    public static class BaseRegistrar
    {
        internal static readonly Dictionary<string, PieceData> pieceDataByName = new Dictionary<string, PieceData>();

        public static bool TryRegisterPiece(PieceData pieceData)
        {
            if (pieceDataByName.ContainsKey(pieceData.Piece.Name))
            {
                return false;
            }



            pieceDataByName.Add(pieceData.Piece.Name, pieceData);

            return true;
        }
    }
}
