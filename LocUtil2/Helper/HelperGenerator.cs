using System.CodeDom;
using System.Collections;
using System.Reflection;
using System.Resources;

using Rsdn.LocUtil.Model;

namespace Rsdn.LocUtil.Helper
{
	/// <summary>
	/// Генератор хелперов для работы с ресурсами.
	/// </summary>
	public static class HelperGenerator
	{
		private const string _resNameFieldPostfix = "ResourceName";
		private const string _rmPropertyName = "ResourceManager";
		private const string _rmFieldName = "_resourceManager";

		/// <summary>
		/// Генерировать хелпер.
		/// </summary>
		public static CodeCompileUnit Generate(RootCategory cat, string ns,
			bool isInternal)
		{
			var ccu = new CodeCompileUnit();
			var cn = new CodeNamespace(ns);
			ccu.Namespaces.Add(cn);
			GenerateCategory(cn.Types, cat, null, isInternal);
			return ccu;
		}

		private static void GenerateCategory(IList memberList, Category cat,
			string rmType, bool isInternal)
		{
			var rc = cat as RootCategory;
			var ctd = 
				new CodeTypeDeclaration(rc == null ? cat.ShortName : rc.TreeName)
				{IsPartial = true};
			if (isInternal)
			{
				ctd.TypeAttributes = TypeAttributes.NestedAssembly;
				// для внутренних типов должен остаться public
				isInternal = false;
			}
			ctd.IsClass = true;
			memberList.Add(ctd);

			if (rmType == null)
			{
				var rmField =
					new CodeMemberField(typeof (ResourceManager),_rmFieldName)
					{
						Attributes = MemberAttributes.Public | MemberAttributes.Static | MemberAttributes.Final,
						InitExpression = new CodePrimitiveExpression(null)
					};
				ctd.Members.Add(rmField);

				var rmProp = 
					new CodeMemberProperty
					{
						Name = _rmPropertyName,
						Type = new CodeTypeReference(typeof (ResourceManager)),
						Attributes = (MemberAttributes.Public
							| MemberAttributes.Static | MemberAttributes.Final)
					};
				var cmrs = new CodeMethodReturnStatement(
					new CodeFieldReferenceExpression(null, _rmFieldName));
				rmProp.GetStatements.Add(cmrs);
					rmProp.SetStatements.Add(
						new CodeAssignStatement(
							new CodeFieldReferenceExpression(null, _rmFieldName),
							new CodeVariableReferenceExpression("value")));
				ctd.Members.Add(rmProp);
				rmType = rc.TreeName;
			}

			foreach (ResourceItem ri in cat.ResourceItems)
			{
				var cmf = 
					new CodeMemberField(typeof (string), ri.ShortName + _resNameFieldPostfix)
					{
						Attributes = (MemberAttributes.Const | MemberAttributes.Public),
						InitExpression = new CodePrimitiveExpression(ri.Name)
					};
				ctd.Members.Add(cmf);

				var cmp =
					new CodeMemberProperty
					{
						Name = ri.ShortName,
						Type = new CodeTypeReference(typeof (string)),
						Attributes = (MemberAttributes.Static | MemberAttributes.Public)
					};
				var cmrs = new CodeMethodReturnStatement(
					new CodeMethodInvokeExpression(
						new CodePropertyReferenceExpression(
							new CodeTypeReferenceExpression(rmType), _rmPropertyName), "GetString",
						new CodeFieldReferenceExpression(null, ri.ShortName + _resNameFieldPostfix)
					));
				cmp.GetStatements.Add(cmrs);
				ctd.Members.Add(cmp);
			}
			foreach (Category c in cat.Categories)
				GenerateCategory(ctd.Members, c, rmType, isInternal);
		}
	}
}
