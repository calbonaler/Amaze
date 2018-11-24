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

		bool[,] m_HorizontalWalls;
		bool[,] m_VerticalWalls;
		int[,] m_ClusterNumbers;

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
			if (m_ClusterNumbers == null)
				m_ClusterNumbers = new int[RowCount, ColumnCount];
			var roomCount = 0;
			foreach (var (i, j) in m_ClusterNumbers.Index())
				m_ClusterNumbers[i, j] = roomCount++;

			while (true)
			{
				// Re-assign cluster numbers for all rooms
				while (true)
				{
					bool reassigned = false;
					for (int i = 0; i < RowCount; i++)
					{
						for (int j = 0; j < ColumnCount; j++)
						{
							if (j + 1 < ColumnCount && !m_HorizontalWalls[i, j] && m_ClusterNumbers[i, j] != m_ClusterNumbers[i, j + 1])
							{
								m_ClusterNumbers[i, j] = m_ClusterNumbers[i, j + 1] = Math.Min(m_ClusterNumbers[i, j], m_ClusterNumbers[i, j + 1]);
								reassigned = true;
							}
							if (i + 1 < RowCount && !m_VerticalWalls[i, j] && m_ClusterNumbers[i, j] != m_ClusterNumbers[i + 1, j])
							{
								m_ClusterNumbers[i, j] = m_ClusterNumbers[i + 1, j] = Math.Min(m_ClusterNumbers[i, j], m_ClusterNumbers[i + 1, j]);
								reassigned = true;
							}
						}
					}
					if (!reassigned) break;
				}
				onReassigned();

				// Stop wall breaking if all the cluster numbers are the same
				if (m_ClusterNumbers.Cast<int>().Zip(m_ClusterNumbers.Cast<int>().Skip(1), (x, y) => x == y).All(x => x))
					break;

				// Break walls stochastically
				bool[,] allWalls;
				Func<int, int, int> nextRoomClusterNumber;
				if (rng.Next(2) == 0)
				{
					allWalls = m_HorizontalWalls;
					nextRoomClusterNumber = (i, j) => m_ClusterNumbers[i, j + 1];
				}
				else
				{
					allWalls = m_VerticalWalls;
					nextRoomClusterNumber = (i, j) => m_ClusterNumbers[i + 1, j];
				}
				List<(int, int)> breakableWalls = new List<(int Row, int Column)>();
				for (int i = 0; i < allWalls.GetLength(0); i++)
				{
					for (int j = 0; j < allWalls.GetLength(1); j++)
					{
						if (!allWalls[i, j] || m_ClusterNumbers[i, j] == nextRoomClusterNumber(i, j)) continue;
						breakableWalls.Add((i, j));
					}
				}
				if (breakableWalls.Count > 0)
				{
					var (i, j) = breakableWalls[rng.Next(breakableWalls.Count)];
					allWalls[i, j] = false;
				}
			}
		}
		public void Draw(Graphics graphics, int cellSize)
		{
			graphics.DrawLine(Pens.Black, 0, 0, cellSize * ColumnCount, 0);
			graphics.DrawLine(Pens.Black, 0, 0, 0, cellSize * RowCount);
			using (Font font = new Font("Meiryo", 9))
			using (StringFormat sf = new StringFormat())
			{
				sf.Alignment = StringAlignment.Center;
				sf.LineAlignment = StringAlignment.Center;
				for (int i = 0; i < RowCount; i++)
				{
					for (int j = 0; j < ColumnCount; j++)
					{
						if (HasRightWall(i, j))
							graphics.DrawLine(Pens.Black, (j + 1) * cellSize, i * cellSize, (j + 1) * cellSize, (i + 1) * cellSize);
						if (HasBottomWall(i, j))
							graphics.DrawLine(Pens.Black, j * cellSize, (i + 1) * cellSize, (j + 1) * cellSize, (i + 1) * cellSize);
						if (m_ClusterNumbers != null)
							graphics.DrawString(m_ClusterNumbers[i, j].ToString(), font, Brushes.Black, new RectangleF(j * cellSize, i * cellSize, cellSize, cellSize), sf);
					}
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

		uint[] m_StateVector;
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
			y ^= (y >> 11);
			y ^= (y << 7) & 0x9d2c5680U;
			y ^= (y << 15) & 0xefc60000U;
			y ^= (y >> 18);
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
			int j = 0;
			uint sample = 0;
			for (int i = 0; i < buffer.Length; i++)
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
