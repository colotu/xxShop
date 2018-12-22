/**
* TaskTimers.cs
*
* 功 能： N/A
* 类 名： TaskTimers
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/8/21 21:28:54   Ben    初版
*
* Copyright (c) 2012-2013 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;

namespace YSWL.TimerTask.Model
{
	/// <summary>
	/// TaskTimers:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class TaskTimer
	{
		public TaskTimer()
		{}
		#region Model
		private int _id;
		private string _executetype;
		private bool _issingle= true;
		private decimal _interval;
		private DateTime _executetime;
		private int _executenumber=0;
		private string _param1;
		private string _param2;
		private string _param3;
		private string _param4;
		private string _param5;
		private string _param6;
		private string _param7;
		private string _param8;
		private string _param9;
		private string _param10;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 执行类
		/// </summary>
		public string ExecuteType
		{
			set{ _executetype=value;}
			get{return _executetype;}
		}
		/// <summary>
		/// 只执行一次
		/// </summary>
		public bool IsSingle
		{
			set{ _issingle=value;}
			get{return _issingle;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal Interval
		{
			set{ _interval=value;}
			get{return _interval;}
		}
		/// <summary>
		/// 执行时间
		/// </summary>
		public DateTime ExecuteTime
		{
			set{ _executetime=value;}
			get{return _executetime;}
		}
		/// <summary>
		/// 执行次数
		/// </summary>
		public int ExecuteNumber
		{
			set{ _executenumber=value;}
			get{return _executenumber;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Param1
		{
			set{ _param1=value;}
			get{return _param1;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Param2
		{
			set{ _param2=value;}
			get{return _param2;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Param3
		{
			set{ _param3=value;}
			get{return _param3;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Param4
		{
			set{ _param4=value;}
			get{return _param4;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Param5
		{
			set{ _param5=value;}
			get{return _param5;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Param6
		{
			set{ _param6=value;}
			get{return _param6;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Param7
		{
			set{ _param7=value;}
			get{return _param7;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Param8
		{
			set{ _param8=value;}
			get{return _param8;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Param9
		{
			set{ _param9=value;}
			get{return _param9;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Param10
		{
			set{ _param10=value;}
			get{return _param10;}
		}
		#endregion Model

	}
}

