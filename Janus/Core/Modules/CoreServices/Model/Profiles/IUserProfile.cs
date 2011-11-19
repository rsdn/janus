using System;
using System.Xml;

namespace Rsdn.Janus
{
	public interface IUserProfile
	{
		/// <summary>
		/// Директорий профиля.
		/// </summary>
		string ConfigPath
		{get;}

		/// <summary>
		/// Имя драйвера БД.
		/// </summary>
		string DbDriverName
		{get;}

		/// <summary>
		/// Настройки драйвера БД.
		/// </summary>
		XmlNode DbDriverOptions
		{get;}

		/// <summary>
		/// Описание профиля.
		/// </summary>
		string Description
		{get;}

		/// <summary>
		/// Имя профиля.
		/// </summary>
		string Name
		{get;}
	}
}
