using System;
using System.Collections.Generic;
using System.Text;

namespace FirstApp
{
	interface IConfig
	{
		int calc_num { get; }
		string target_address { get; }
		int target_port { get; }
	}
}
