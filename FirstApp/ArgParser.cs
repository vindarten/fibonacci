using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FirstApp
{
	class ArgParser : IConfig
	{
		public int calc_num { get; private set; } = 10;
		public string target_address { get; private set; } = "localhost";
		public int target_port { get; private set; } = 5000;

		public ArgParser(string[] args)
		{
			Dictionary<string, string> dict = args.Select(a => a.Split(new[] { '=' }, 2))
				.GroupBy(a => a[0], a => a.Length == 2 ? a[1] : null)
				.ToDictionary(g => g.Key, g => g.FirstOrDefault());

			calc_num = Parse(dict, "calc_num", calc_num);
			target_address = Parse(dict, "target_address", target_address);
			target_port = Parse(dict, "target_port", target_port);

			CheckValues();
		}

		private static int Parse(Dictionary<string, string> dict, string param_name, int default_value)
		{
			string string_value;
			if (!dict.TryGetValue(param_name, out string_value))
			{
				return default_value;
			}

			int int_value;
			if (!Int32.TryParse(string_value, out int_value))
			{
				throw new InvalidValueException(param_name);
			}
			return int_value;
		}

		private static string Parse(Dictionary<string, string> dict, string param_name, string default_value)
		{
			return dict.TryGetValue(param_name, out string string_value) ? string_value : default_value;
		}

		private void CheckValues()
		{
			if (calc_num < 1)
			{
				throw new InvalidValueException("calc_num");
			}
			if (target_port < 1)
			{
				throw new InvalidValueException("target_port");
			}
		}
	}

	class InvalidValueException : Exception
	{
		public InvalidValueException(string param_name) : base("Invalid value of '" + param_name + "'")
		{ }
	}
}
