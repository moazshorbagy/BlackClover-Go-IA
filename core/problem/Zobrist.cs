using System;

namespace BlackClover
{
    class ZobristHasher
    {
        private static ZobristHasher _hasher;
        static readonly int BOARD_SIZE = 19;
        static readonly int NUM_PIECES = 2;
        private UInt64[,,] zobristTable;

        private ZobristHasher()
        {
            zobristTable = new UInt64[BOARD_SIZE, BOARD_SIZE, NUM_PIECES];
            Random random = new Random();
            var buffer = new byte[sizeof(Int64)];
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    for (int k = 0; k < NUM_PIECES; k++)
                    {
                        random.NextBytes(buffer);
                        zobristTable[i, j, k] = (UInt64)BitConverter.ToInt64(buffer, 0);
                    }
                }
            }
        }
        /// <summary>
        /// A singleton for the ZobristHasher
        /// </summary>
        /// <returns> returns the only isntance of the ZobristHasher</returns>
        public static ZobristHasher GetInstance()
        {
            if (_hasher == null)
            {
                _hasher = new ZobristHasher();
            }
            return _hasher;
        }

        private int MapPiece(char piece)
        {
            return piece == 'W' ? 1 : (piece == 'B' ? 0 : - 1);
        }

        /// <summary>
        /// This function computes the hash where the board 
        /// is a 2d array of <int> 
        /// <summary>
        public UInt64 ComputeHash(char[,] board)
        {
            UInt64 hash = 0;
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                   int piece = MapPiece(board[i, j]);
                    
                    if (piece != -1)
                    {
                        hash ^= zobristTable[i, j, piece];
                    }
                }
            }
            return hash;
        }
    }
}
