using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;
using TanksArmageddon.Core.Editor;

namespace TanksArmageddon.Core.PrefabControl.Editor
{
    internal static class PrefabNameGenerator
    {
        public const string EnumName = "PrefabName";

        public static void Generate(Dictionary<int, string> enumNames)
        {
            CodeCompileUnit compileUnit = new();
            CodeNamespace codeNamespace = new(typeof(PrefabStorage).Namespace);

            CodeTypeDeclaration enumDeclaration = new(EnumName)
            {
                IsEnum = true,
                TypeAttributes = TypeAttributes.Public
            };

            foreach (var enumName in enumNames.Values)
            {
                enumDeclaration.Members.Add(new CodeMemberField()
                {
                    Name = enumName
                });
            }

            codeNamespace.Types.Add(enumDeclaration);
            compileUnit.Namespaces.Add(codeNamespace);

            FileGenerationTools.SaveFile(compileUnit, EnumName, false);
        }
    }
}
