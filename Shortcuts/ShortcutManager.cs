using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml;

namespace Rsdn.Shortcuts
{
	public class ShortcutManager : Component, ISupportInitialize, IMessageFilter
	{
		public const int WM_CHAR = 0x0102;
		public const int WM_KEYDOWN = 0x0100;
		public const int WM_KEYUP = 0x0101;
		public const int WM_SYSCHAR = 262;
		public const int WM_SYSDEADCHAR = 263;
		public const int WM_SYSKEYDOWN = 260;
		public const int WM_SYSKEYUP = 261;

		private readonly Dictionary<Type, CustomShortcut> _shortcutMap =
			new Dictionary<Type, CustomShortcut>();

		public ShortcutCollection _collection;
		private DesignShortcuts _design;

		public ShortcutManager()
		{
			Application.AddMessageFilter(this);
		}

		#region ISupportInitialize
		public void BeginInit()
		{}

		public void EndInit()
		{
			TreeToHashtable(Nodes);
		}

		private void TreeToHashtable(ShortcutCollection node)
		{
			foreach (CustomShortcut cs in node)
			{
				_shortcutMap.Add(cs.OwnerType, cs);
				TreeToHashtable(cs.Nodes);
			}
		}
		#endregion

		#region Property
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ShortcutCollection Nodes
		{
			get
			{
				if (_collection == null)
					_collection = new ShortcutCollection(null);
				return _collection;
			}
		}

		[Browsable(true)]
		public Form MainForm { get; set; }
		#endregion

		#region Save
		private bool SaveCore(Stream stream)
		{
			var serializer = new ShortcutSerializer(this);
			return serializer.SaveToStream(stream);
		}

		public byte[] SaveToArray()
		{
			var ms = new MemoryStream();
			SaveToStream(ms);

			return ms.GetBuffer();
		}

		public bool SaveToXml(string path)
		{
			using (var file = File.OpenWrite(path))
				return SaveCore(file);
		}

		public XmlNode SaveToXmlNodePreset()
		{
			var ms = new MemoryStream();
			var serializer = new ShortcutSerializer(this);

			serializer.SaveForPreset(ms);

			var xdoc = new XmlDocument();
			ms.Seek(0, SeekOrigin.Begin);
			xdoc.Load(ms);

			return xdoc.DocumentElement;
		}

		public XmlNode SaveToXmlNode()
		{
			var ms = new MemoryStream();
			var xdoc = new XmlDocument();

			SaveCore(ms);

			ms.Seek(0, SeekOrigin.Begin);
			xdoc.Load(ms);

			return xdoc.DocumentElement;
		}

		public bool SaveToStream(Stream stream)
		{
			return SaveCore(stream);
		}
		#endregion

		#region Load
		public void LoadFromArray(byte[] buffer)
		{
			using (var ms = new MemoryStream(buffer))
				LoadFromStream(ms);
		}

		public void LoadFromXml(string path)
		{
			try
			{
				using (var file = File.OpenRead(path))
					LoadCore(file);
			}
			catch
			{}
		}

		public void LoadFromXmlNode(XmlNode node)
		{
			try
			{
				var xdoc = new XmlDocument();
				xdoc.AppendChild(xdoc.ImportNode(node, true));
				var ms = new MemoryStream();
				xdoc.Save(ms);
				ms.Seek(0, SeekOrigin.Begin);
				LoadCore(ms);
			}
			catch
			{}
		}

		public void LoadFromStream(Stream stream)
		{
			LoadCore(stream);
		}

		private void LoadCore(Stream stream)
		{
			var serializer = new ShortcutSerializer(this);
			serializer.LoadFromStream(stream);
		}
		#endregion

		#region Dialog design shortcuts
		public void ShowDesignMdi()
		{
			if (MainForm != null)
				foreach (var frm in MainForm.MdiChildren)
					if (frm is DesignShortcuts)
					{
						frm.Activate();
						return;
					}

			_design = new DesignShortcuts(this) {MdiParent = MainForm};
			_design.Show();
		}

		public void ShowDesignModal(Control parent)
		{
			_design = new DesignShortcuts(this) {Parent = parent};
			_design.ShowDialog();
		}

		public void ShowDesignModal()
		{
			ShowDesignModal(null);
		}

		public IDialogContainer GetDesignDialog()
		{
			return new DesignShortcuts(this);
		}
		#endregion

		#region IMessageFilter Members
		public bool PreFilterMessage(ref Message msg)
		{
			switch (msg.Msg)
			{
				case WM_SYSKEYDOWN: // Обработка шорткатов "Alt + xxx"
				case WM_KEYDOWN: // Обработка обычных шорткатов
					var ctrl = GetControlFromHandle(msg.HWnd);
					var keyData = (Keys)(int)msg.WParam | Control.ModifierKeys;

#if false //DEBUG
					Console.WriteLine("msg.Msg: {0}, keyData: {1}", msg.Msg, keyData);
#endif

					var shortcutNode = FindShortcutNode(ctrl);

					if (shortcutNode != null)
						if (shortcutNode.ProcessMessageKey(ctrl, keyData, this))
							return true;

					break;
			}

			return false;
		}
		#endregion

		protected CustomShortcut FindShortcutNode(Control control)
		{
			var target = FindControlHelper
				.FindControl(control, _shortcutMap);

			return target != null
				? _shortcutMap[target.GetType()]
				: null;
		}

		[DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
		public static extern IntPtr GetParent(IntPtr hWnd);

		private static Control GetControlFromHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
				return null;

			return Control.FromHandle(handle)
				?? GetControlFromHandle(GetParent(handle));
		}

		#region Presets
		private readonly PresetCollection _presets = new PresetCollection();

		[Browsable(false)]
		public PresetCollection Presets
		{
			get { return _presets; }
		}

		public void SaveCurrentAsPreset(string name)
		{
			//Preset p = new Preset(name,SaveToXmlNode(Nodes));

			Presets.AddPreset(new Preset(name, SaveToXmlNodePreset()));
		}

		public void LoadPreset(string name)
		{
			var serializer = new ShortcutSerializer(this);
			serializer.LoadNodes(Presets[name].Nodes);

			//LoadNodes(Presets[name].Nodes);
		}
		#endregion
	}
}