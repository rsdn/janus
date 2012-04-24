using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Windows.Forms;

namespace Rsdn.Janus
{
	/// <summary>
	/// Summary description for AboutJanusForm.
	/// </summary>
	public partial class AboutJanusForm : Form
	{

		#region Declarations

        private readonly IServiceProvider _provider;

		private const string _resourcePrefix = "Rsdn.Janus.Core.UI.";

		private bool        _isShowing = true;
		private bool        _useEffect = true;
	
		#endregion

		#region Constructor(s)

		public AboutJanusForm(IServiceProvider provider)
		{
			_provider = provider;
			InitializeComponent();

			CustomInitializeComponent();
		}

		#endregion

		#region Private methods

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (_useEffect)
			{
				Opacity = 0.0;
				Activate();
				Refresh();
				_fadeTimer.Start();
			}
		}

		private void CustomInitializeComponent()
		{
			_webBrowser.BackColor = Color.Black;
			_versionLabel.Text    = ApplicationInfo.NameWithVersion;

			if (!Environment.OSVersion.IsModernOS())
				_useEffect = false;

			_asmButton.Checked = true;
			BuildAssemblyPage();
		}

		/*private void _webBrowser_BeforeNavigate(object sender, BeforeNavigateEventArgs e)
		{
			e.Cancel = true;
			ApplicationManager.Instance.BrowserManager.ShowUrl(e.Url);
		}*/

		private void AboutJanusForm_Closing(object sender, CancelEventArgs e)
		{
			if (_useEffect)
			{
				Owner.Activate();
				_isShowing = false;
				_fadeTimer.Start();
			}
		}

		private void FadeTimerTick(object sender, EventArgs e)
		{
			if (_isShowing)
			{
				var d = 1000.0/_fadeTimer.Interval/100.0;
				if (Opacity + d >= 1.0)
				{
					Opacity = 1.0;
					_fadeTimer.Stop();
				}
				else
				{
					Opacity = Opacity + d;
				}
			}
			else
			{
				var d = 1000.0/_fadeTimer.Interval/100.0;
				if (Opacity - d <= 0.0)
				{
					Opacity = 0.0;
					_fadeTimer.Stop();
				}
				else
				{
					Opacity = Opacity - d;
				}
			}
		}

		#endregion

		#region  Заполнение assembly info

		private class AssemblyComparer : IComparer
		{
			public int Compare(object asm1, object asm2)
			{
				var result = ((Assembly) asm1).GetName().Name
					.CompareTo(((Assembly) asm2).GetName().Name);

				return result;
			}
		}

		private enum AssemblyListColumn
		{
			Name,
			Ver,
			Loc,
			Key
		}

		private const string _tableRow =
			"<tr style='background-color: {0}'><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>";

		private const string _tableHeader =
			"<td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td>";

		private static int GetStartPosition(string assemblyStr, char stopChar, int countStop)
		{
			var result = 0;
			countStop--;

			if (countStop != 0)
				for (result = 0; result < assemblyStr.Length; result++)
				{
					if (assemblyStr[result] == stopChar)
						countStop--;

					if (countStop == 0)
					{
						result++;
						break;
					}
				}
			
			return result;
		}

		private static string GetSubString(string assemblyName, int stringNum)
		{
			var result  = string.Empty;
			var canBldStr = stringNum == 1;

			for (
				var i = GetStartPosition(assemblyName, ',', stringNum);
				i < assemblyName.Length;
				i++
				)
			{
				if (assemblyName[i] == ',')
					break;
				
				if (canBldStr)
					result += assemblyName[i];
				
				if (assemblyName[i] == '=')
					canBldStr = true;
			}

			return result;
		}

		private static string GetInfoString(string assemblyName, AssemblyListColumn column)
		{
			switch (column)
			{
				case AssemblyListColumn.Key:  return GetSubString(assemblyName, 4);
				case AssemblyListColumn.Loc:  return GetSubString(assemblyName, 3);
				case AssemblyListColumn.Ver:  return GetSubString(assemblyName, 2);
				case AssemblyListColumn.Name: return GetSubString(assemblyName, 1);
				default:
					return GetSubString(assemblyName, 1);
			}
		}

		private void BuildAssemblyPage()
		{
			var sb = new StringBuilder();
			var columnsCount = 0;

			var asmArray = AppDomain.CurrentDomain.GetAssemblies();
			Array.Sort(asmArray, new AssemblyComparer());

			foreach (var asm in asmArray)
				if (!asm.IsDynamic && asm.Location != string.Empty)
				{
					sb.AppendFormat(
						_tableRow,
						columnsCount%2 == 0 ? "#F4FFF4" : "#E4FFF4",
						GetInfoString(asm.FullName, AssemblyListColumn.Name),
						GetInfoString(asm.FullName, AssemblyListColumn.Ver),
						GetInfoString(asm.FullName, AssemblyListColumn.Loc),
						GetInfoString(asm.FullName, AssemblyListColumn.Key));

					columnsCount++;
				}

			_webBrowser.DocumentText = string.Format(GetTemplate(), 
				SR.About.AssemblyInfo,
				string.Format(_tableHeader,
					SR.About.AIFileName,
					SR.Version,
					SR.About.Locale,
					SR.About.AIKey),
				sb);
		}

		private void ButtonAssemblyInfoClick(object sender, EventArgs e)
		{
			_webBrowser.DocumentText = String.Empty;
			BuildAssemblyPage();
		}

		#endregion

		#region  Заполнение component info

		private const string _componentRow =
			@"<tr style='background-color: {0}'>
				<td><a href='{1}' target='_blank'>{2}</a></td>
				<td><a href='{3}' target='_blank'>{4}</a></td>
			</tr>";

		private const string _componentHeader = "<th>{0}</th><th>{1}</th>";

		// Should we move it to resources or something?
		//
		private readonly string[] _components = new[]
			{
				"Business Logic Toolkit for .NET", "http://www.bltoolkit.net/",
				"MIT", "http://bltoolkit.net/license.htm",

				"DockPanel Suite", "http://sourceforge.net/projects/dockpanelsuite/",
				"MIT", "http://dockpanelsuite.svn.sourceforge.net/viewvc/*checkout*/dockpanelsuite/trunk/license.txt",

				"Lusene.Net", "http://incubator.apache.org/projects/lucene.net.html",
				"ASF", "http://www.apache.org/licenses/LICENSE-2.0.txt",

				"Scintilla", "http://www.scintilla.org/",
				"MIT", "http://scintilla.sourceforge.net/License.txt"
			};

		private void BuildComponentsPage()
		{
			var sb = new StringBuilder();

			for (var i = 0; i < _components.Length / 4; ++i)
			{
				sb.AppendFormat(_componentRow, i%2 == 0 ? "#F4FFF4" : "#E4FFF4",
					_components[i * 4 + 1],_components[i * 4 + 0],
					_components[i * 4 + 3],_components[i * 4 + 2]);
			}

			_webBrowser.DocumentText = string.Format(GetTemplate(), 
				SR.About.ComponentInfo,
				string.Format(_componentHeader,
					SR.Component,
					SR.License),
				sb);
		}

		private void ButtonShowComponentsClick(object sender, EventArgs e)
		{
			_webBrowser.DocumentText = String.Empty;
			BuildComponentsPage();
		}

		#endregion

		#region Заполнение разработчиков

		private const string _devTableRow =
			@"<tr style='background-color: {0}'>
				<td><a href='janus://user-info/{1}'>{2}</a></td>
				<td align='center'>{3}</td>
				<td><a href='mailto:{4}'>{4}</a></td>
				<td><a target='_blank' href='{5}</a></td>
			</tr>";

		private static string GetTemplate()
		{
			var htmlStream = Assembly.GetExecutingAssembly()
				.GetRequiredResourceStream(_resourcePrefix + "AboutMainPage.html");

			using (var sr = new StreamReader(htmlStream))
				return sr.ReadToEnd();
		}

		private void BuildDevelopersPage()
		{
			var sb = new StringBuilder();
			var columnsCount = 0;
			var userIds =
				new[]
				{
					12284, 2962, 15309, 5743, 5161, 28760, 19717, 73,
					3242, 6921, 343, 2668, 7107, 1938, 3655, 3655,
					10596, 16874, 1, 20712, 21120, 4964, 8546, 11709,
					5623, 9088, 33647, 61389, 44901, 507, 52960
				};
			using (var db = _provider.CreateDBContext())
			{
				var users =
					db
						.Users(u => userIds.Contains(u.ID))
						.OrderBy(u => u.Name)
						.Select(
							u =>
								new
								{
									u.ID,
									Name = u.DisplayName(),
									u.HomePage,
									u.UserClass
								});
				foreach (var user in users)
				{
					columnsCount++;
					var userHomePage =
						string.Format(
							user.HomePage.StartsWith("http://")
								? "{0}'>{1}"
								: "http://{0}'>{1}",
							user.HomePage,
							user.HomePage);

					sb.AppendFormat(
						_devTableRow,
						columnsCount % 2 == 0 ? "#F4FFF4" : "#E4FFF4",
						user.ID,
						user.Name,
						JanusFormatMessage.FormatUserClass(user.UserClass, true),
						"",
						userHomePage);
				}
			}

			_webBrowser.DocumentText = string.Format(
				GetTemplate(),
				SR.About.DevelopersList,
				string.Format(_tableHeader,
					SR.UserInfoName,
					SR.UserInfoStatus,
					"e-mail",
					SR.UserInfoHomePage),
				sb);
		}

		private void ButtonShowDevelopersClick(object sender, EventArgs e)
		{
			_webBrowser.DocumentText = string.Empty;
			BuildDevelopersPage();
		}

		#endregion
	}
}
