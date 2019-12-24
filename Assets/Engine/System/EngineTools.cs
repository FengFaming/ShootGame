/*
 * Creator:ffm
 * Desc:框架内置工具
 * Time:2019/12/23 17:08:12
* */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Engine
{
	public class EngineTools : Singleton<EngineTools>
	{
		/// <summary>
		/// 检查身份证号码是否符合
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public bool CheckIDCard(string id)
		{
			if (id.Length == 18)
			{
				return CheckIDCard18(id);
			}

			return false;
		}

		/// <summary>
		/// 提起身份证上的性别
		///  1：男；-1：女；0：身份证有误
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public int CheckIDCardSex(string id)
		{
			if (CheckIDCard(id))
			{
				if (id.Length == 18)
				{
					int sex = int.Parse(id.Substring(16, 1));
					return sex % 2 == 0 ? -1 : 1;
				}
			}

			return 0;
		}

		private bool CheckIDCard18(string id)
		{
			long n = 0;
			if (long.TryParse(id.Remove(17), out n) == false ||
				n < Math.Pow(10, 16) ||
				long.TryParse(id.Replace('x', '0').Replace('X', '0'), out n) == false)
			{
				///里面包含非数字或者首位不大于零
				return false;
			}

			string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
			if (address.IndexOf(id.Remove(2)) == -1)
			{
				///省份验证不通过
				return false;
			}

			string birth = id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
			DateTime time = new DateTime();
			if (DateTime.TryParse(birth, out time) == false)
			{
				///生日验证不通过
				return false;
			}

			string[] arrvarifycode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
			string[] wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
			char[] ai = id.Remove(17).ToCharArray();
			int sum = 0;
			for (int index = 0; index < 17; index++)
			{
				sum += int.Parse(wi[index]) * int.Parse(ai[index].ToString());
			}

			int y = -1;
			Math.DivRem(sum, 11, out y);
			if (arrvarifycode[y] != id.Substring(17, 1).ToLower())
			{
				///校验码不正确
				return false;
			}

			///身份证符合GB1643-1999标准
			return true;
		}
	}
}
