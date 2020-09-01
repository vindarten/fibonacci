using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Model
{
	public class FibonacciContext : IFibonacciContext
	{
		private ConcurrentDictionary<int, long> calculations = new ConcurrentDictionary<int, long>();

		public FibonacciData GetCurrent(int calc_id)
		{
			if(calculations.TryGetValue(calc_id, out long value))
			{
				return new FibonacciData(value, calc_id);
			}
			return new FibonacciData(0, 0);
		}

		public FibonacciData CalculateNext(FibonacciData data)
		{
			long next_value = calculations.AddOrUpdate(data.CalculationId, 1 + data.Value, (key, old_value) => old_value + data.Value);
			return new FibonacciData(next_value, data.CalculationId);
		}
	}
}
