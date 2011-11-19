using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

using Rsdn.LocUtil.Model.Design;

namespace Rsdn.LocUtil
{
	/// <summary>
	/// Корневой контейнер.
	/// </summary>
	public class RootAppContainer : Container, IEditContext
	{
		private readonly IWin32Window _dialogsOwner;
		private CultureInfo[] _availableCultures = new CultureInfo[0];

		/// <summary>
		/// Инициализирует экземпляр.
		/// </summary>
		/// <param name="dialogsOwner"></param>
		public RootAppContainer(IWin32Window dialogsOwner)
		{
			_dialogsOwner = dialogsOwner;
		}

		/// <summary>
		/// Получить или установить доступные для редактирования культуры.
		/// </summary>
		public CultureInfo[] AvailableCultures
		{
			get { return _availableCultures; }
			set { _availableCultures = value; }
		}

		/// <summary>
		/// Получить сервис.
		/// </summary>
		protected override object GetService(Type service)
		{
			if (service == typeof (IEditContext))
				return this;
			else
				return base.GetService(service);
		}

		CultureInfo[] IEditContext.AvailableCultures
		{
			get { return _availableCultures; }
			set { _availableCultures = value; }
		}

		IWin32Window IEditContext.DialogsOwner
		{
			get { return _dialogsOwner; }
		}
	}
}
