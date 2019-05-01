using System.Collections.Generic;
using System.Linq;

namespace Amaze
{
	static class Utils
	{
		public static IEnumerable<(int Index, T Value)> Enumerate<T>(this IEnumerable<T> source) => source.Select((x, i) => (i, x));

		public static IEnumerable<((int, int) Index, T Value)> Enumerate<T>(this T[,] source)
		{
			for (var i = 0; i < source.GetLength(0); i++)
			{
				for (var j = 0; j < source.GetLength(1); j++)
					yield return ((i, j), source[i, j]);
			}
		}

		public static IEnumerable<int> Index<T>(this IEnumerable<T> source) => source.Select((x, i) => i);

		public static IEnumerable<(int, int)> Index<T>(this T[,] source)
		{
			for (var i = 0; i < source.GetLength(0); i++)
			{
				for (var j = 0; j < source.GetLength(1); j++)
					yield return (i, j);
			}
		}
	}
}
