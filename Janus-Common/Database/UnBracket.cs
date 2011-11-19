namespace Rsdn.Janus
{
	public static class UnBracket
	{
		public static string ParseUnBracket(string vl)
		{
			var curp = 0;
			//
			const string lit = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefjhijklmnopqrstuvwxyz1234567890";
			const string bop = "()";
			// чистые операции
			const string cop = "+-";
			// грязные операции
			const string dop = "*/";
			while (true)
			{
				if (vl.Substring(curp, 1) == "(")
				{
					var varr = new int[4];
					varr[0] = 0;
					varr[1] = 0;
					var bd = 0;
					for (var i = curp + 1; i < vl.Length; i++)
					{
						if (vl.Substring(i, 1) == ")" && bd == 0)
						{
							varr[0] = curp;
							varr[1] = i;
							break;
						}

						if (vl.Substring(i, 1) == "(")
							bd++;
						else if (vl.Substring(i, 1) == ")")
							bd--;
					}
					if (varr[0] < varr[1])
					{
						var i = varr[0] - 1;
						while (i >= 0)
						{
							if (vl[i] != ' ')
							{
								if (lit.IndexOf(vl[i]) != -1)
								{
									varr[0] = -1;
									varr[1] = -1;
								}
								break;
							}
							i--;
						}
						if (varr[0] < varr[1])
						{
							var df = false;
							if (vl.Substring(varr[0] + 1, varr[1] - varr[0] - 1).IndexOfAny(cop.ToCharArray()) != -1 ||
								vl.Substring(varr[0] + 1, varr[1] - varr[0] - 1).IndexOfAny(dop.ToCharArray()) != -1)
							{
								for (var j = varr[0] - 1; j >= 0; j--)
								{
									if (dop.Contains(vl.Substring(j, 1)))
										df = true;

									if (cop.Contains(vl.Substring(j, 1)))
										break;
								}
								for (var j = varr[1] + 1; j < vl.Length; j++)
								{
									if (dop.Contains(vl.Substring(j, 1)))
										df = true;

									if (cop.Contains(vl.Substring(j, 1)) || bop.Contains(vl.Substring(j, 1)))
										break;
								}
							}
							if (!df)
							{
								vl = vl.Remove(varr[1], 1);
								vl = vl.Remove(varr[0], 1);
								curp -= (varr[0] <= curp ? 1 : 0) + (varr[1] <= curp ? 1 : 0);
							}
						}
					}
				}
				curp += 1;
				if (curp >= vl.Length)
					break;
			}
			return vl;
		}
	}
}