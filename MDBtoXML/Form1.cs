using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using System.Reflection;
using System.IO;
using ADOX;
using ADODB;
using System.Xml;
using RSDN.Janus;

using System.Runtime.InteropServices;
using System.Text;

namespace MDBtoXML
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tbPathMDB;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tbPathXML;
		private System.Windows.Forms.Button btMDB;
		private System.Windows.Forms.Button btXML;
		private System.Windows.Forms.Button btProcess;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			tbPathMDB.Text = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
			tbPathXML.Text = tbPathMDB.Text;

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.tbPathMDB = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.tbPathXML = new System.Windows.Forms.TextBox();
			this.btMDB = new System.Windows.Forms.Button();
			this.btXML = new System.Windows.Forms.Button();
			this.btProcess = new System.Windows.Forms.Button();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(24, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(200, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Путь к базе данных MDB(Access)";
			// 
			// tbPathMDB
			// 
			this.tbPathMDB.Location = new System.Drawing.Point(24, 40);
			this.tbPathMDB.Name = "tbPathMDB";
			this.tbPathMDB.Size = new System.Drawing.Size(304, 20);
			this.tbPathMDB.TabIndex = 1;
			this.tbPathMDB.Text = "tbPathMDB";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(24, 72);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(264, 16);
			this.label2.TabIndex = 2;
			this.label2.Text = "Каталог для выходного файла XML";
			// 
			// tbPathXML
			// 
			this.tbPathXML.Location = new System.Drawing.Point(24, 96);
			this.tbPathXML.Name = "tbPathXML";
			this.tbPathXML.Size = new System.Drawing.Size(304, 20);
			this.tbPathXML.TabIndex = 3;
			this.tbPathXML.Text = "С:\\";
			// 
			// btMDB
			// 
			this.btMDB.Location = new System.Drawing.Point(336, 40);
			this.btMDB.Name = "btMDB";
			this.btMDB.Size = new System.Drawing.Size(22, 20);
			this.btMDB.TabIndex = 4;
			this.btMDB.Text = "...";
			this.btMDB.Click += new System.EventHandler(this.btMDB_Click);
			// 
			// btXML
			// 
			this.btXML.Location = new System.Drawing.Point(336, 96);
			this.btXML.Name = "btXML";
			this.btXML.Size = new System.Drawing.Size(22, 20);
			this.btXML.TabIndex = 5;
			this.btXML.Text = "...";
			this.btXML.Click += new System.EventHandler(this.btXML_Click);
			// 
			// btProcess
			// 
			this.btProcess.Location = new System.Drawing.Point(24, 136);
			this.btProcess.Name = "btProcess";
			this.btProcess.Size = new System.Drawing.Size(96, 23);
			this.btProcess.TabIndex = 6;
			this.btProcess.Text = "Сформировать";
			this.btProcess.Click += new System.EventHandler(this.btProcess_Click);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(400, 181);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.btProcess,
																		  this.btXML,
																		  this.btMDB,
																		  this.tbPathXML,
																		  this.label2,
																		  this.tbPathMDB,
																		  this.label1});
			this.Name = "Form1";
			this.Text = "MDBtoXML";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) 
		{
			if (args.Length == 2)
			{
//				Form1 fm1 = new Form1();				
//				fm1.tbPathMDB.Text = args[0];
//				fm1.tbPathXML.Text = args[1] + @"\conf_db.xml";
				Process( args[0], args[1]);
			}
			else
				Application.Run(new Form1());
		}

		private void btMDB_Click(object sender, System.EventArgs e)
		{
			openFileDialog1.InitialDirectory = 
				new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName+"\\";
			openFileDialog1.Filter = "MDB files (*.mdb)|*.mdb";
			if(openFileDialog1.ShowDialog() == DialogResult.OK)
				tbPathMDB.Text = openFileDialog1.FileName;
		}

		private void btXML_Click(object sender, System.EventArgs e)
		{
//			openFileDialog1.InitialDirectory = 
//				new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName+"\\";
//			openFileDialog1.Filter = "All files (*.*)|*.*";
//			if(openFileDialog1.ShowDialog() == DialogResult.OK)
//				tbPathXML.Text = openFileDialog1.FileName;

			string strPath = Shell.BrowseForFolder("Директория для xml-файла");
			if (strPath != "") tbPathXML.Text = strPath;
		}

		private void btProcess_Click(object sender, System.EventArgs e)
		{
			Process(tbPathMDB.Text, tbPathXML.Text);
		}

		private static void Process( string pathMDB, string pathXML)
		{
			using (ManageSchemaDB mg = new ManageSchemaDB())
			{
				mg.FillFromADOX( pathMDB,true);
				mg.SaveToXML( pathXML+"\\conf_db.xml");
			}

			MessageBox.Show("Создание файла завершено!");
		}

	
	}


	public class Shell
	{
		[StructLayout(LayoutKind.Sequential)]
			internal struct BROWSEINFO
		{
			public uint hwndOwner;
			public IntPtr pidlRoot;
			[MarshalAs(UnmanagedType.LPTStr, SizeConst=260)]
			public string pszDisplayName;
			public string lpszTitle;
			public uint ulFlags;
			public uint lpfn;
			public int lParam;
			public int iImage;
		}

		[DllImport("Shell32.dll")]
		private static extern uint SHBrowseForFolder(ref BROWSEINFO browseInfo);
		[DllImport("Shell32.dll")]
		private static extern uint SHGetPathFromIDList(uint pidList, StringBuilder pszPath);
		[DllImport("ole32.dll")]
		private static extern void CoTaskMemFree(uint hMem);

		static public string BrowseForFolder(string title)
		{
			BROWSEINFO browseInfo = new BROWSEINFO();
			browseInfo.hwndOwner = 0;
			browseInfo.lpszTitle = title;
			browseInfo.ulFlags = 1;

			uint lpIDList = SHBrowseForFolder(ref browseInfo);
			if (lpIDList != 0)
			{
				StringBuilder path = new StringBuilder(260);
				SHGetPathFromIDList(lpIDList, path);

				CoTaskMemFree(lpIDList);
				return  path.ToString().TrimEnd(new char[]{'\\'});
			}
			return "";
		}
	}

}
