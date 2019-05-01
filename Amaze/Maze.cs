using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Amaze
{
	public class Maze
	{
		public Maze(int rowCount, int columnCount)
		{
			m_HorizontalWalls = new bool[rowCount, columnCount - 1];
			m_VerticalWalls = new bool[rowCount - 1, columnCount];
			Clear();
		}

		readonly bool[,] m_HorizontalWalls;
		readonly bool[,] m_VerticalWalls;

		public int RowCount => m_HorizontalWalls.GetLength(0);
		public int ColumnCount => m_VerticalWalls.GetLength(1);

		public bool HasLeftWall(int row, int column) => column <= 0 ? true : m_HorizontalWalls[row, column - 1];
		public bool HasRightWall(int row, int column) => column >= ColumnCount - 1 ? true : m_HorizontalWalls[row, column];
		public bool HasTopWall(int row, int column) => row <= 0 ? true : m_VerticalWalls[row - 1, column];
		public bool HasBottomWall(int row, int column) => row >= RowCount - 1 ? true : m_VerticalWalls[row, column];
		public void SetLeftWall(int row, int column, bool value) => m_HorizontalWalls[row, column - 1] = value;
		public void SetRightWall(int row, int column, bool value) => m_HorizontalWalls[row, column] = value;
		public void SetTopWall(int row, int column, bool value) => m_VerticalWalls[row - 1, column] = value;
		public void SetBottomWall(int row, int column, bool value) => m_VerticalWalls[row, column] = value;

		public void Clear()
		{
			foreach (var (i, j) in m_HorizontalWalls.Index())
				m_HorizontalWalls[i, j] = true;
			foreach (var (i, j) in m_VerticalWalls.Index())
				m_VerticalWalls[i, j] = true;
		}
		public void Initialize(Action onReassigned)
		{
			var rng = new MersenneTwister();

			// Initialize cluster numbers for all rooms
			var clusterNumbers = new int[RowCount, ColumnCount];

			void UpdateClusterNumber(int row, int column, bool horizontal)
			{
				// The wall that we broke separates two room of two different clusters
				// So, we merge the clusters by simply replacing cluster numbers
				var nextClusterNumber = horizontal ? clusterNumbers[row, column + 1] : clusterNumbers[row + 1, column];
				var oldClusterNumber = Math.Max(clusterNumbers[row, column], nextClusterNumber);
				var newClusterNumber = Math.Min(clusterNumbers[row, column], nextClusterNumber);
				for (var i = 0; i < clusterNumbers.GetLength(0); i++)
				{
					for (var j = 0; j < clusterNumbers.GetLength(1); j++)
					{
						if (clusterNumbers[i, j] == oldClusterNumber)
							clusterNumbers[i, j] = newClusterNumber;
					}
				}
			}

			for (var i = 0; i < RowCount; i++)
			{
				for (var j = 0; j < ColumnCount; j++)
					clusterNumbers[i, j] = i * clusterNumbers.GetLength(1) + j;
			}
			for (var i = 0; i < RowCount; i++)
			{
				for (var j = 0; j < ColumnCount; j++)
				{
					if (j + 1 < ColumnCount && !m_HorizontalWalls[i, j] && clusterNumbers[i, j] != clusterNumbers[i, j + 1])
						UpdateClusterNumber(i, j, true);
					if (i + 1 < RowCount && !m_VerticalWalls[i, j] && clusterNumbers[i, j] != clusterNumbers[i + 1, j])
						UpdateClusterNumber(i, j, false);
				}
			}

			// Initialize breakable walls
			var breakableWalls = new List<(int Row, int Column, bool Horizontal)>();
			for (var i = 0; i < RowCount; i++)
			{
				for (var j = 0; j < ColumnCount; j++)
				{
					if (j + 1 < ColumnCount && m_HorizontalWalls[i, j] && clusterNumbers[i, j] != clusterNumbers[i, j + 1])
						breakableWalls.Add((i, j, true));
					if (i + 1 < RowCount && m_VerticalWalls[i, j] && clusterNumbers[i, j] != clusterNumbers[i + 1, j])
						breakableWalls.Add((i, j, false));
				}
			}

			while (breakableWalls.Count > 0)
			{
				// Select walls from breakable walls stochastically
				var (row, column, horizontal) = breakableWalls[rng.Next(breakableWalls.Count)];

				// Break selected wall
				(horizontal ? m_HorizontalWalls : m_VerticalWalls)[row, column] = false;

				// Re-assign cluster numbers for all rooms
				UpdateClusterNumber(row, column, horizontal);

				// Update breakable walls
				// Remove walls separating two rooms of same cluster
				breakableWalls.RemoveAll(x => clusterNumbers[x.Row, x.Column] == (x.Horizontal ? clusterNumbers[x.Row, x.Column + 1] : clusterNumbers[x.Row + 1, x.Column]));

				onReassigned();
			}
		}
		public void Draw(Graphics graphics, int cellSize)
		{
			graphics.DrawLine(Pens.Black, 0, 0, cellSize * ColumnCount, 0);
			graphics.DrawLine(Pens.Black, 0, 0, 0, cellSize * RowCount);
			for (var i = 0; i < RowCount; i++)
			{
				for (var j = 0; j < ColumnCount; j++)
				{
					if (HasRightWall(i, j))
						graphics.DrawLine(Pens.Black, (j + 1) * cellSize, i * cellSize, (j + 1) * cellSize, (i + 1) * cellSize);
					if (HasBottomWall(i, j))
						graphics.DrawLine(Pens.Black, j * cellSize, (i + 1) * cellSize, (j + 1) * cellSize, (i + 1) * cellSize);
				}
			}
		}
	}

	public class MersenneTwister : Random
	{
		public MersenneTwister() : this((uint)Environment.TickCount) { }

		public MersenneTwister(uint seed)
		{
			m_StateVector = new uint[624];
			m_StateVector[0] = seed;
			for (m_StateVectorIndex = 1; m_StateVectorIndex < m_StateVector.Length; m_StateVectorIndex++)
			{
				m_StateVector[m_StateVectorIndex] = (uint)(1812433253U * (m_StateVector[m_StateVectorIndex - 1] ^ (m_StateVector[m_StateVectorIndex - 1] >> 30)) + m_StateVectorIndex);
				// See Knuth TAOCP Vol2. 3rd Ed. P.106 for multiplier.
				// In the previous versions, MSBs of the seed affect only MSBs of the array mt[].
				// 2002/01/09 modified by Makoto Matsumoto
			}
		}

		// Period parameters
		const int M = 397;
		const uint UpperMask = 0x80000000U; // most significant w-r bits
		const uint LowerMask = 0x7fffffffU; // least significant r bits
		static readonly uint[] mag01 = new[] { 0x0U, 0x9908b0dfU }; // mag01[x] = x * MATRIX_A  for x=0,1

		readonly uint[] m_StateVector;
		int m_StateVectorIndex;

		// generates a random number on [0,0xffffffff]-interval
		public uint NextUInt32()
		{
			uint y;
			if (m_StateVectorIndex >= m_StateVector.Length)
			{
				// generate N words at one time
				int i;
				for (i = 0; i < m_StateVector.Length - M; i++)
				{
					y = (m_StateVector[i] & UpperMask) | (m_StateVector[i + 1] & LowerMask);
					m_StateVector[i] = m_StateVector[i + M] ^ (y >> 1) ^ mag01[y & 0x1UL];
				}
				for (; i < m_StateVector.Length - 1; i++)
				{
					y = (m_StateVector[i] & UpperMask) | (m_StateVector[i + 1] & LowerMask);
					m_StateVector[i] = m_StateVector[i + (M - m_StateVector.Length)] ^ (y >> 1) ^ mag01[y & 0x1UL];
				}
				y = (m_StateVector[m_StateVector.Length - 1] & UpperMask) | (m_StateVector[0] & LowerMask);
				m_StateVector[m_StateVector.Length - 1] = m_StateVector[M - 1] ^ (y >> 1) ^ mag01[y & 0x1UL];
				m_StateVectorIndex = 0;
			}
			y = m_StateVector[m_StateVectorIndex++];
			// Tempering
			y ^= y >> 11;
			y ^= (y << 7) & 0x9d2c5680U;
			y ^= (y << 15) & 0xefc60000U;
			y ^= y >> 18;
			return y;
		}

		// generates a random number on [0,0x7fffffff]-interval
		public override int Next() => (int)(NextUInt32() >> 1);

		public override int Next(int maxValue) => (int)(NextUInt32() * maxValue / 4294967296);

		public override int Next(int minValue, int maxValue) => (int)(NextUInt32() * ((long)maxValue - minValue) / 4294967296 + minValue);

		// generates a random number on [0,1]-real-interval
		public double NextDoubleClosed() => NextUInt32() * (1.0 / 4294967295.0);  // divided by 2^32-1

		// generates a random number on [0,1)-real-interval
		public override double NextDouble() => NextUInt32() * (1.0 / 4294967296.0);  // divided by 2^32

		// generates a random number on (0,1)-real-interval
		public double NextDoubleOpen() => (NextUInt32() + 0.5) * (1.0 / 4294967296.0);  // divided by 2^32

		// generates a random number on [0,1) with 53-bit resolution
		public double NextDouble53()
		{
			var a = NextUInt32() >> 5;
			var b = NextUInt32() >> 6;
			return (a * 67108864.0 + b) * (1.0 / 9007199254740992.0);
		}

		public override void NextBytes(byte[] buffer)
		{
			var j = 0;
			uint sample = 0;
			for (var i = 0; i < buffer.Length; i++)
			{
				if (j <= 0)
				{
					sample = NextUInt32();
					j = 4;
				}
				buffer[i] = (byte)(sample & 0xff);
				sample >>= 8;
				j--;
			}
		}

		protected override double Sample() => NextDouble();
	}
}
