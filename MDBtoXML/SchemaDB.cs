using System;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using ADOX;
using ADODB;
using System.Reflection;
using System.Text;
using System.Xml;

namespace RSDN.Janus
{
	#region Управление схемой БД
	public class ManageSchemaDB : IDisposable
	{
		private XmlSerializer m_serializer;
		private jSchemaDB m_schema;
		private CatalogClass m_catalogADOX;
		private bool m_isSchema;

		public bool IsSchema
		{
			get{ return m_isSchema;}
		}

		public int CurrVersion
		{
			get{ return m_schema.version;}
		}

		public ManageSchemaDB()
		{
			m_serializer = new XmlSerializer(typeof(jSchemaDB));
			m_schema = new jSchemaDB();
			m_catalogADOX = new CatalogClass();
			m_isSchema = false;
		}

		public void Dispose()
		{
			System.Runtime.InteropServices.Marshal.ReleaseComObject(m_catalogADOX);
		}

		//Загружаем только версию XML
		public static int LoadOnlyVersionRes( string pathXML)
		{
			int iOut = -1;
			using (TextReader reader = new StreamReader(
					   Assembly.GetExecutingAssembly().GetManifestResourceStream(pathXML)))
			{
				XmlDocument doc = new XmlDocument();
				doc.Load( reader);
				XmlAttributeCollection attrColl = doc.DocumentElement.Attributes;
				iOut = Convert.ToInt32( attrColl["version"].Value);		
			}
			return iOut;
		}

		//Загружаем только версию XML
		public static int LoadOnlyVersion( string pathXML)
		{
			int iOut = -1;
			using (TextReader reader = new StreamReader(pathXML))
			{
				XmlDocument doc = new XmlDocument();
				doc.Load( reader);
				XmlAttributeCollection attrColl = doc.DocumentElement.Attributes;
				iOut = Convert.ToInt32( attrColl["version"].Value);
			}
			return iOut;
		}

		//Загружаем схему из XML
		public void LoadFromXML( string pathXML)
		{
			using (TextReader reader = new StreamReader(pathXML))
			{
				m_schema = (jSchemaDB)m_serializer.Deserialize(reader);
				m_isSchema = true;
			}
		}

		//Загрузка схемы из ресурса
		public void LoadFromXMLRes( string filename)
		{
			using (TextReader reader = new StreamReader(
			  Assembly.GetExecutingAssembly().GetManifestResourceStream(filename)))
			{
				m_schema = (jSchemaDB)m_serializer.Deserialize(reader);
				m_isSchema = true;
			}
		}

		//Записываем схему в XML
		public void SaveToXML( string pathXML)
		{
			//version
			m_schema.version++;

			using (TextWriter writer = new StreamWriter(pathXML))
			{
				m_serializer.Serialize(writer, m_schema);
			}
		}

		//Заполняем m_schema из БД
		public void FillFromADOX( string pathDB, bool withData)
		{
			ConnectionClass conn = null;
			try
			{
				conn = new ConnectionClass();
				conn.Open("Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+pathDB,"","",0);
				m_catalogADOX.ActiveConnection = conn;

				m_schema.tables = new jtables[m_catalogADOX.Tables.Count];
				for(int i = 0; i < m_catalogADOX.Tables.Count; i++)
				{
					if (m_catalogADOX.Tables[i].Name.Substring(0,4) != "MSys")
					{
						jtables table = new jtables();
						table.name = m_catalogADOX.Tables[i].Name;
						table.columns = new jcolumns[m_catalogADOX.Tables[i].Columns.Count];

						for(int j = 0; j < m_catalogADOX.Tables[i].Columns.Count; j++)
						{
							jcolumns column = new jcolumns();
							column.name = m_catalogADOX.Tables[i].Columns[j].Name;
							column.type = m_catalogADOX.Tables[i].Columns[j].Type;
							column.precision = m_catalogADOX.Tables[i].Columns[j].Precision;
							column.definedSize = m_catalogADOX.Tables[i].Columns[j].DefinedSize;
							column.autoincrement = (bool)m_catalogADOX.Tables[i].Columns[j].Properties["Autoincrement"].Value;
							column.nullable = (bool)m_catalogADOX.Tables[i].Columns[j].Properties["Nullable"].Value;
							column.fixedLength = (bool)m_catalogADOX.Tables[i].Columns[j].Properties["Fixed Length"].Value;
							//tab.columns.SetValue(col, j);
							table.columns[j] = column;
						}

						ArrayList arrIndex = new ArrayList();

						//index
						table.indexs = new jindexs[m_catalogADOX.Tables[i].Indexes.Count];
						for(int j = 0; j < m_catalogADOX.Tables[i].Indexes.Count; j++)
						{
							if (m_catalogADOX.Tables[i].Indexes[j].Name != "PrimaryKey")
							{
								string nameIndex = m_catalogADOX.Tables[i].Indexes[j].Columns[0].Name;
								if ( arrIndex.Contains(nameIndex))
									continue;

								jindexs index = new jindexs();
								//index.name = m_catalogADOX.Tables[i].Indexes[j].Name;//глюки
								index.name = m_catalogADOX.Tables[i].Indexes[j].Columns[0].Name;

								index.clustered = m_catalogADOX.Tables[i].Indexes[j].Clustered;
								index.primaryKey = m_catalogADOX.Tables[i].Indexes[j].PrimaryKey;
								index.unique = m_catalogADOX.Tables[i].Indexes[j].Unique;
								index.indexNulls = m_catalogADOX.Tables[i].Indexes[j].IndexNulls;
								//tab.indexs.SetValue(ind, j);
								table.indexs[j] = index;

								arrIndex.Add( index.name);
							}
						}

						//keys
						table.keys = new jkeys[m_catalogADOX.Tables[i].Keys.Count];
						for(int j = 0; j < m_catalogADOX.Tables[i].Keys.Count; j++)
						{
							if (m_catalogADOX.Tables[i].Keys[j].Name == "PrimaryKey")
							{
								jkeys key = new jkeys();
								key.name = m_catalogADOX.Tables[i].Keys[j].Name;
								key.column = m_catalogADOX.Tables[i].Keys[j].Columns[0].Name;
								key.type = m_catalogADOX.Tables[i].Keys[j].Type;
								table.keys[j] = key;
							}
						}


						//data
						string tableName = m_catalogADOX.Tables[i].Name;
						if (withData && tableName == "vars")
						{
					
							RecordsetClass rs = new RecordsetClass();
							try
							{
								//int adCmdText = 1;
								int adCmdTable = 2;
								rs.Open( tableName, conn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, adCmdTable);

								if (rs.RecordCount != 0)
								{
									table.rows = new jrows[rs.RecordCount];
									int rc = 0;
									rs.MoveFirst();
									while (!rs.EOF)
									{
										jrows row = new jrows();

										row.jcolvalues = new jcolvalue[rs.Fields.Count];

										for (int c = 0; c < rs.Fields.Count; c++)
										{
											jcolvalue colvalue = new jcolvalue();
											colvalue.name = rs.Fields[c].Name;
											colvalue.colvalue = Convert.ToString(rs.Fields[c].Value);

											row.jcolvalues[c] = colvalue;
										}
								
										table.rows[rc] = row;
										rc++;
										rs.MoveNext();
									}
								}
							}
							finally
							{
								rs.Close();
							}
							//version
							try
							{
								int adCmdText = 1;
								rs.Open( "SELECT varvalue FROM vars WHERE name='VersionDB'", conn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, adCmdText);
								if (rs.RecordCount != 0)
								{
									rs.MoveFirst();
									while (!rs.EOF)
									{
										m_schema.version = Convert.ToInt32(rs.Fields[0].Value);
										rs.MoveNext();
									}
								}
							}
							finally
							{
								rs.Close();
							}
						}

						//m_schema.tables.SetValue(table, i);
						m_schema.tables[i] = table;
					}
				}
				m_isSchema = true;
			}
			finally
			{
				if (conn != null)
					conn.Close();
				m_catalogADOX.ActiveConnection = null;
			}
		}

		//Создаем новую пустую БД из схемы
		public void CreateDB( string pathDB)
		{
			if (!m_isSchema)
				return;

			if (File.Exists(pathDB))
			{
				File.Copy( pathDB, pathDB+".old", true);
				File.Delete( pathDB);
			}

			string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+pathDB;
			try
			{
				m_catalogADOX.Create(connStr);

				for ( int i = 0; i < m_schema.tables.Length; i++ )
				{
					TableClass tableADOX = new TableClass();
					tableADOX.Name = m_schema.tables[i].name;
					tableADOX.ParentCatalog = m_catalogADOX;

					jcolumns[] cols = m_schema.tables[i].columns;
					for ( int j = 0; j < cols.Length; j++ )
					{
						ColumnClass columnADOX = new ColumnClass();

						columnADOX.ParentCatalog = m_catalogADOX;

						columnADOX.Name = cols[j].name;
						columnADOX.Type = cols[j].type;
						columnADOX.DefinedSize = cols[j].definedSize;
						columnADOX.Precision = cols[j].precision;

						columnADOX.Properties["Autoincrement"].Value = (object)cols[j].autoincrement;
						columnADOX.Properties["Nullable"].Value = (object)cols[j].nullable;
						columnADOX.Properties["Fixed Length"].Value = (object)cols[j].fixedLength;

						tableADOX.Columns.Append(columnADOX, cols[j].type, cols[j].definedSize);
					}

					m_catalogADOX.Tables.Append(tableADOX);

					//index
					jindexs[] ind = m_schema.tables[i].indexs;
					if (ind != null)
					{
						for ( int j = 0; j < ind.Length; j++ )
						{
							IndexClass indexADOX = new IndexClass();
							indexADOX.Name = ind[j].name;
							indexADOX.Clustered = ind[j].clustered;
							indexADOX.IndexNulls = ind[j].indexNulls;
							indexADOX.PrimaryKey = ind[j].primaryKey;
							indexADOX.Unique = ind[j].unique;

							m_catalogADOX.Tables[m_schema.tables[i].name].Indexes.Append( ind[j].name, ind[j].name);
						}
					}

					//key
					jkeys[] key = m_schema.tables[i].keys;
					if (key != null)
					{
						for ( int j = 0; j < key.Length; j++ )
						{
							KeyClass keyADOX = new KeyClass();
							keyADOX.Name = key[j].name;
							keyADOX.Type = key[j].type;
							//keyADOX.Columns = key[j].column;
							//keyADOX.Columns.Append(key[j].column, ADOX.DataTypeEnum.adInteger, 0);
							//ColumnClass columnADOX = new ColumnClass();
							//columnADOX.Name = key[j].column; 

							m_catalogADOX.Tables[m_schema.tables[i].name].Keys.Append(
								key[j].name, key[j].type, m_catalogADOX.Tables[m_schema.tables[i].name].Columns[key[j].column], "", "");

							//Без этой записи на win2000 выдавала глюк!!! Первая строка в vars отказывалась запичыватся
							m_catalogADOX.Tables[m_schema.tables[i].name].Keys.Refresh();
						}
					}
				}
			}
			finally
			{
				//if (conn != null)
				//	conn.Close();
				//m_catalogADOX.ActiveConnection = null;
			}

			//Data и 
			ConnectionClass conn = null;
			try
			{
				conn = new ConnectionClass();
				conn.Open(connStr,"","",0);

				for ( int i = 0; i < m_schema.tables.Length; i++ )
				{

					jrows[] rows = m_schema.tables[i].rows;
					if (rows != null && m_schema.tables[i].name=="vars")
						FillDataRestruct( conn, m_schema.tables[i].name, rows);
				}
			}
			finally
			{
				if (conn != null)
					conn.Close();
			}
		}

		//Реструктуризация БД по схеме
		public bool RestructDB( string pathDB)
		{
			if (!m_isSchema)
				return false;
			
			File.Copy( pathDB, pathDB+".old", true);

			bool isRestructed = false;

			ConnectionClass conn = null;
			try
			{
				conn = new ConnectionClass();
				conn.Open("Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+pathDB,"","",0);
				m_catalogADOX.ActiveConnection = conn;

				for ( int i = 0; i < m_schema.tables.Length; ++i )
				{
					string tableName;
					try
					{
						tableName = m_catalogADOX.Tables[m_schema.tables[i].name].Name;
					}
					catch
					{
						AddTableADOX( m_schema.tables[i].name);
						tableName = m_schema.tables[i].name;
						isRestructed = true;
					}

					jcolumns[] cols = m_schema.tables[i].columns;
					for ( int j = 0; j < cols.Length; ++j )
					{
						string columnName = cols[j].name;
						ADOX.DataTypeEnum columnType = cols[j].type;
						int columnPrecision = cols[j].precision;
						int columnDefSize = cols[j].definedSize;
						
						try
						{
							string temp = m_catalogADOX.Tables[tableName].Columns[columnName].Name;
						}
						catch
						{
							ColumnClass columnADOX = new ColumnClass();

							columnADOX.ParentCatalog = m_catalogADOX;

							columnADOX.Name = columnName;
							columnADOX.Type = columnType;
							columnADOX.DefinedSize = columnDefSize;
							columnADOX.Precision = columnPrecision;
							columnADOX.Properties["Autoincrement"].Value = (object)cols[j].autoincrement;
							columnADOX.Properties["Nullable"].Value = (object)cols[j].nullable;
							columnADOX.Properties["Fixed Length"].Value = (object)cols[j].fixedLength;

							AddColumnADOX( tableName, columnADOX, columnType, columnDefSize);
							isRestructed = true;
						}

						ADOX.DataTypeEnum temp_type = m_catalogADOX.Tables[tableName].Columns[columnName].Type;
						int temp_precision = m_catalogADOX.Tables[tableName].Columns[columnName].Precision;
						int temp_definedSize = m_catalogADOX.Tables[tableName].Columns[columnName].DefinedSize;

						if (columnType != temp_type || columnPrecision != temp_precision || columnDefSize != temp_definedSize
							|| cols[j].autoincrement != (bool)m_catalogADOX.Tables[tableName].Columns[columnName].Properties["Autoincrement"].Value 
							|| cols[j].nullable != (bool)m_catalogADOX.Tables[tableName].Columns[columnName].Properties["Nullable"].Value 
							|| cols[j].fixedLength != (bool)m_catalogADOX.Tables[tableName].Columns[columnName].Properties["Fixed Length"].Value
							)
						{
							ALTERColumnADOX( conn, tableName, columnName, columnType, columnPrecision, columnDefSize, cols[j]);
							isRestructed = true;
						}
					}

					//index
					jindexs[] ind = m_schema.tables[i].indexs;
					if (ind != null)
					{
						for ( int j = 0; j < ind.Length; j++ )
						{
							try
							{
								string name = m_catalogADOX.Tables[m_schema.tables[i].name].Indexes[ind[j].name].Name;
							}
							catch
							{
								IndexClass indexADOX = new IndexClass();
								
								indexADOX.Name = ind[j].name;
								indexADOX.Clustered = ind[j].clustered;
								indexADOX.IndexNulls = ind[j].indexNulls;
								indexADOX.PrimaryKey = ind[j].primaryKey;
								indexADOX.Unique = ind[j].unique;
								indexADOX.Columns.Append(ind[j].name, ADOX.DataTypeEnum.adInteger, 0);
								

								//m_catalogADOX.Tables[m_schema.tables[i].name].Indexes.Refresh();
								try
								{
									m_catalogADOX.Tables[m_schema.tables[i].name].Indexes.Append( ind[j].name, ind[j].name);
								}
								catch{}
								//catch (System.Runtime.InteropServices.COMException e)
								//{
									//System.Windows.Forms.MessageBox.Show(e.Message);
								//}
								isRestructed = true;
							}
						}
					}

					//keys
					jkeys[] key = m_schema.tables[i].keys;
					if (key != null)
					{
						for ( int j = 0; j < key.Length; j++ )
						{
							try
							{
								string name = m_catalogADOX.Tables[m_schema.tables[i].name].Keys[key[j].name].Name;
							}
							catch
							{

								KeyClass keyADOX = new KeyClass();
								keyADOX.Name = key[j].name;
								keyADOX.Type = key[j].type;

								m_catalogADOX.Tables[m_schema.tables[i].name].Keys.Append(
									key[j].name, key[j].type, m_catalogADOX.Tables[m_schema.tables[i].name].Columns[key[j].column], "", "");
									//key[j].name, key[j].type, key[j].column, "", "");

								//Без этой записи на win2000 выдавала глюк!!! Первая строка в vars отказывалась записыватся
								m_catalogADOX.Tables[m_schema.tables[i].name].Keys.Refresh();
							}
						}
					}

					//Data
					jrows[] rows = m_schema.tables[i].rows;
					if (rows != null && m_schema.tables[i].name=="vars")
						FillDataRestruct( conn, m_schema.tables[i].name, rows);
				}
			}
			finally
			{
				if (conn != null)
					conn.Close();
				//System.Runtime.InteropServices.Marshal.ReleaseComObject(m_catalogADOX);
				//m_catalogADOX = new CatalogClass();
			}
			return isRestructed;
		}

		//ВНИМАНИЕ!!!: Сделано только для таблицы vars!
		private void FillDataRestruct( ConnectionClass conn, string tableName, jrows[] rows)
		{

			ArrayList al = new ArrayList();
			RecordsetClass rs = new RecordsetClass();
			try
			{
				int adCmdTable = 2;
				rs.Open( tableName, conn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, adCmdTable);
				if (rs.RecordCount != 0)
				{
					rs.MoveFirst();
					while (!rs.EOF)
					{
						al.Add(rs.Fields[0].Value);
						rs.MoveNext();
					}
				}
			}
			finally
			{
				rs.Close();
			}

			int adCmdText = 1;
			object recCount = new object();

			for ( int i = 0; i < rows.Length; ++i )
			{
				jcolvalue[] cols = rows[i].jcolvalues;
				StringBuilder sbCol = new StringBuilder();
				StringBuilder sbVal = new StringBuilder();

				bool insert = true;
				for ( int j = 0; j < cols.Length; ++j )
				{
					if (cols[j].name == "name" && al.Contains(cols[j].colvalue))
					{
						insert = false;
						break;
					}
					sbCol.Append(cols[j].name);
					sbVal.Append('"'+cols[j].colvalue+'"');
					if (j != (cols.Length-1))
					{
						sbCol.Append(",");
						sbVal.Append(",");
					}
				}
				if (insert)
				{
					string strSQL = String.Format("INSERT INTO {0} ({1}) VALUES({2})", tableName, sbCol.ToString(), sbVal.ToString());
					//System.Windows.Forms.MessageBox.Show(strSQL);
					conn.Execute( strSQL, out recCount, adCmdText);
				}
			}
		}

		private void AddTableADOX( string tableName)
		{
			TableClass tableADOX = new TableClass();
			tableADOX.Name = tableName;
			m_catalogADOX.Tables.Append(tableADOX);
		}	

		private void AddColumnADOX( string tableName, ColumnClass columnClass, ADOX.DataTypeEnum columnType, int columnDefSize)
		{
			m_catalogADOX.Tables[tableName].Columns.Append(columnClass, columnType, columnDefSize);
		}

		private void ALTERColumnADOX( ConnectionClass conn, string tableName, string columnName, ADOX.DataTypeEnum columnType, int columnPrecision, int columnDefSize, jcolumns cols)
		{
			//НЕ РАБОТАЕТ!!! ADOX c Jet 4.0 непозволяет изменять параметры колонок
			//m_catalogADOX.Tables[tableName].Columns[columnName].Precision = col_prec;
			//m_catalogADOX.Tables[tableName].Columns.Refresh();

			//string strSQL = "ALTER TABLE "+tableName+" ALTER COLUMN "+columnName+" XXX";

			try
			{
				string tempColumnName = String.Format("x{0}",columnName);
				m_catalogADOX.Tables[tableName].Columns[columnName].Name = tempColumnName;
				m_catalogADOX.Tables[tableName].Columns.Refresh();

				ColumnClass columnADOX = new ColumnClass();

				columnADOX.ParentCatalog = m_catalogADOX;

				columnADOX.Name = columnName;
				columnADOX.Type = columnType;
				columnADOX.DefinedSize = columnDefSize;
				columnADOX.Precision = columnPrecision;
				columnADOX.Properties["Autoincrement"].Value = (object)cols.autoincrement;
				columnADOX.Properties["Nullable"].Value = (object)cols.nullable;
				columnADOX.Properties["Fixed Length"].Value = (object)cols.fixedLength;

				AddColumnADOX( tableName, columnADOX, columnType, columnDefSize);

				string strSQL = String.Format("UPDATE {0} SET {1} = {2}",tableName, columnName,tempColumnName);

				int adCmdText = 1;
				object recCount = new object();
				conn.Execute( strSQL, out recCount, adCmdText);

				//проверка не является ли поле ключевым
				//m_catalogADOX.Tables[tableName].Keys.Refresh();
				for (int i = 0; i < m_catalogADOX.Tables[tableName].Keys.Count; i++ )
				{
					ADOX.Key keyADOX = m_catalogADOX.Tables[tableName].Keys[i];
					for (int j = 0; j < keyADOX.Columns.Count; j++ )
					{
						if ( tempColumnName == keyADOX.Columns[j].Name)
						{
							m_catalogADOX.Tables[tableName].Keys.Delete( keyADOX.Name);
							m_catalogADOX.Tables[tableName].Keys.Refresh();
						}
					}
				}
				
				m_catalogADOX.Tables[tableName].Columns.Delete(tempColumnName);
				m_catalogADOX.Tables[tableName].Columns.Refresh();
			}
			catch(Exception ex)
			{
				System.Windows.Forms.MessageBox.Show(ex.Message);
			}
		}

	}
	#endregion Управление схемой БД

	#region Классы для сериализации схемы БД в XML

	/// <remarks/>
	[System.Xml.Serialization.XmlRootAttribute("janus-database", Namespace="", IsNullable=false)]
	public class jSchemaDB 
	{ 
		/// <remarks/>
		[System.Xml.Serialization.XmlAttribute()]
		public System.Int32 version;

		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute("table")]
		public jtables[] tables;
	}

	/// <remarks/>
	public class jtables 
	{
		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute("colum")]
		public jcolumns[] columns;

		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute("index")]
		public jindexs[] indexs;

		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute("key")]
		public jkeys[] keys;

		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute("row")]
		public jrows[] rows;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string name;
	}

	/// <remarks/>
	public class jcolumns 
	{
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string name;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public ADOX.DataTypeEnum type;
		//public string type;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public System.Int32 precision;
		//public string precision;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public System.Int32 definedSize;

//		/// <remarks/>
//		[System.Xml.Serialization.XmlAttributeAttribute()]
//		//public ADOX.ColumnAttributesEnum Attributes;
//		public string Attributes;
	
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool autoincrement;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool nullable;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool fixedLength;
	}

	/// <remarks/>
	public class jindexs 
	{
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string name;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool clustered;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public ADOX.AllowNullsEnum indexNulls;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool primaryKey;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool unique;
	}

	/// <remarks/>
	public class jkeys 
	{
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string name;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string column;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public ADOX.KeyTypeEnum type;
	}

	/// <remarks/>
	public class jrows 
	{
		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute("col")]
		public jcolvalue[] jcolvalues;  
	}

	/// <remarks/>
	public class jcolvalue 
	{
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string name;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string colvalue;
		//public object colvalue;
	}
	#endregion
}