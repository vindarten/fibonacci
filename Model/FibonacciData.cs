using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Model
{
	public class FibonacciData
	{
		public long Value { get; set; }
		public int CalculationId { get; set; }

		public FibonacciData()
		{ }

		public FibonacciData(long value, int calculation_id)
		{
			this.Value = value;
			this.CalculationId = calculation_id;
		}

		public StringContent ToStringContent()
		{
			string jsonString = $"{{\"value\":{Value},\"calculationid\":{CalculationId}}}";
			return new StringContent(jsonString, Encoding.UTF8, "application/json");
		}
	}
}
