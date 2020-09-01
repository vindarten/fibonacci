using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
	public interface IFibonacciContext
	{
		FibonacciData GetCurrent(int calc_id);
		FibonacciData CalculateNext(FibonacciData data);
	}
}
