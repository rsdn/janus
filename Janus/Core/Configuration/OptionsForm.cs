using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.PropertyGridInternal;

using Rsdn.Shortcuts;

namespace Rsdn.Janus
{
	/// <summary>
	/// Summary description for OptionsForm.
	/// </summary>
	public partial class OptionsForm : Form
	{
		private ChangeActionType _action;
		private IDialogContainer _dialogContainer;

		public OptionsForm()
		{
			InitializeComponent();
			CustomInitializeComponent();

			StyleConfig.StyleChange += OnStyleChange;
			OnStyleChange(null, StyleChangeEventArgs.AllStyle);
		}

		public ChangeActionType ActionType
		{
			get { return _action; }
		}

		private static void EliminateDesignerBug(PropertyGrid pg)
		{
			pg.HelpVisible = true;
			pg.ToolbarVisible = true;
		}

		private static DisplayNameWrapper GetWrapper(object obj)
		{
			var wrapper = new DisplayNameWrapper(obj);
			wrapper.CheckAttribute += CheckAttribute;

			return wrapper;
		}

		private void CustomInitializeComponent()
		{
			_savedConfig = StyleConfig.GetClone();

			EliminateDesignerBug(_appPropertyGrid);
			_appPropertyGrid.SelectedObject = GetWrapper(Config.GetClone());

			// HACK: rameel: Надоело, что PropertyGrid проматывает страницу в конец.
			_appPropertyGrid.SelectedGridItem = _appPropertyGrid
				.SelectedGridItem.Parent.Parent.GridItems[0];

			_dialogContainer = ApplicationManager.Instance.MainForm
				.ShortcutManager.GetDesignDialog();
			_hotKeysTab.Controls.Add(_dialogContainer.GetDialog());

			EliminateDesignerBug(_stylePropertyGrid);
			_stylePropertyGrid.SelectedObject = GetWrapper(Config.Instance.StyleConfig);

			// Удаляем стандартную закладку и кладем две наши, одна символизирует 
			// стандартные настройки, другая - расширенные. Реальное управление тем,
			// что показывать находится в this.StylePropertyGrid_PropertyTabChanged
			_stylePropertyGrid.PropertyTabs.RemoveTabType(
				typeof (PropertiesTab));

			_stylePropertyGrid.PropertyTabs.AddTabType(
				typeof (NormalStyleTab), PropertyTabScope.Static);
			_stylePropertyGrid.PropertyTabs.AddTabType(
				typeof (DetailStyleTab), PropertyTabScope.Static);
		}

		private void OnStyleChange(object sender, StyleChangeEventArgs a)
		{
			_appPropertyGrid.BackColor = Config.Instance.StyleConfig.OptionsGridBack;
			_appPropertyGrid.ViewBackColor = Config.Instance.StyleConfig.OptionsGridViewBack;
			_appPropertyGrid.LineColor = Config.Instance.StyleConfig.OptionsGridLine;
			_appPropertyGrid.ViewForeColor = Config.Instance.StyleConfig.OptionsGridText;

			_stylePropertyGrid.BackColor = Config.Instance.StyleConfig.OptionsGridBack;
			_stylePropertyGrid.ViewBackColor = Config.Instance.StyleConfig.OptionsGridViewBack;
			_stylePropertyGrid.LineColor = Config.Instance.StyleConfig.OptionsGridLine;
			_stylePropertyGrid.ViewForeColor = Config.Instance.StyleConfig.OptionsGridText;
		}

		private void StrongAction(ChangeActionType actionType)
		{
			_action |= actionType;
		}

		private void AcceptChanges()
		{
			Config.NewConfig(
				(Config)((DisplayNameWrapper)_appPropertyGrid.SelectedObject).WrappedInstance);
			_dialogContainer.AcceptChanges();
		}



		private void _okButton_Click(object sender, EventArgs e)
		{
			AcceptChanges();
		}

		private void _cancelButton_Click(object sender, EventArgs e)
		{
			StyleConfig.NewStyleConfig(_savedConfig);
		}

		private void _saveButton_Click(object sender, EventArgs e)
		{
			Config.Save();
		}

		private void _saveSchemeButton_Click(object sender, EventArgs e)
		{
			using (var fd = new SaveFileDialog())
			{
				fd.Filter = "Style scheme file (*.xml)|*.xml";
				if (fd.ShowDialog(this) == DialogResult.OK)
					StyleConfig.Save(fd.FileName);
			}
		}

		private void _loadSchemeButton_Click(object sender, EventArgs e)
		{
			using (var fd = new OpenFileDialog())
			{
				fd.Filter = "Style scheme file (*.xml)|*.xml";

				if (fd.ShowDialog(this) == DialogResult.OK)
				{
					StyleConfig.Load(fd.FileName);
					_stylePropertyGrid.SelectedObject =
						GetWrapper(Config.Instance.StyleConfig);
				}
			}
		}

		private void _appPropertyGrid_PropertyValueChanged(object s,
			PropertyValueChangedEventArgs e)
		{
			var t = typeof (ChangePropertyAttribute);
			var attr = (ChangePropertyAttribute)e.ChangedItem.PropertyDescriptor.Attributes[t];
			var act = ChangeActionType.NoAction;

			if (attr != null)
				act = attr.ActionType;

			StrongAction(act);
		}

		private static void CheckAttribute(object sender, CheckAttributeEventArgs e)
		{
			if ((typeof (DescriptionAttribute).IsAssignableFrom(e.Attribute.GetType()) &&
				!(typeof (JanusDescriptionAttribute).IsAssignableFrom(e.Attribute.GetType()))) ||
					(typeof (CategoryAttribute).IsAssignableFrom(e.Attribute.GetType()) &&
						!(typeof (JanusCategoryAttribute).IsAssignableFrom(e.Attribute.GetType()))) ||
							(typeof (DisplayNameAttribute).IsAssignableFrom(e.Attribute.GetType()) &&
								!(typeof (JanusDisplayNameAttribute).IsAssignableFrom(e.Attribute.GetType()))))
			{
				e.Checked = false;
#if DEBUG
				string insteadAttr = e.Attribute.GetType().Name;

				if (insteadAttr.StartsWith("Loc"))
					insteadAttr = insteadAttr.Substring(3);
				
				//VladD2: Мля, нет слов! Что поиздеватьс над людми захотелось?
				// Диалог выдается со скоростью пулемета. Если ошибка критична,
				// то кидайте исключение, а не диалоги. 
				Console.WriteLine(string.Format(
									"Attribute '{0}' is not allowed on '{1}.{2}'. Use Janus{3} instead.",
									e.Attribute.GetType().FullName,
									e.PropertyDescriptor.ComponentType.FullName,
									e.PropertyDescriptor.Name, insteadAttr));
#endif
			}
		}
	}
}