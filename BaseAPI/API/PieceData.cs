using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BaseAPI.API
{
    public class PieceData
    {
        public Piece Piece;
        public GameObject Prefab;
        public Int3 ExtraCells;
        public bool IsCorridor;


        /// <param name="piece">Piece Prefab</param>
        /// <param name="gameObject">Path to AssetBundle</param>
        public PieceData(Piece piece, GameObject gameObject) : this(piece, gameObject, Int3.zero)
        {}
        /// <param name="extraCells">Each Cell takes up a 5m by 3.5m by 5m space define more cells if needed</param>
        public PieceData(Piece piece, GameObject gameObject, Int3 extraCells) : this(piece, gameObject, extraCells, false)
        {}

        /// <param name="piece">Piece Prefab</param>
        /// <param name="gameObject">Path to AssetBundle</param>
        /// <param name="extraCells">Each Cell takes up a 5m by 3.5m by 5m space define more cells if needed</param>
        /// <param name="isCorridor">If this piece is a corridor define it here</param>
        public PieceData(Piece piece, GameObject gameObject, Int3 extraCells, bool isCorridor)
        {
            Piece = piece;
            Prefab = gameObject;
            ExtraCells = extraCells;
            IsCorridor = isCorridor;
        }
    }
}
