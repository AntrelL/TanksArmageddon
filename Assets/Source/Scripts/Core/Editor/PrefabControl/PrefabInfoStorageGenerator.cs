using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;
using TanksArmageddon.Core.Editor;
using UnityEngine;

namespace TanksArmageddon.Core.PrefabControl.Editor
{
    internal static class PrefabInfoStorageGenerator
    {
        private const string StorageClassName = "PrefabInfoStorage";
        private const string PathsToPrefabsDictionaryName = "_pathsToPrefabs";
        private const string PrefabInstancesDictionaryName = "_prefabInstances";

        public static void Generate(Dictionary<int, string> enumNames, List<string> pathsToPrefabs)
        {
            CodeCompileUnit compileUnit = new();
            CodeNamespace codeNamespace = new(typeof(PrefabStorage).Namespace);

            codeNamespace.Imports.AddRange(new CodeNamespaceImport[] {
                new CodeNamespaceImport("System.Collections.Generic"),
                new CodeNamespaceImport("UnityEngine")
            });

            CodeTypeDeclaration storageClassDeclaration = new(StorageClassName);
            storageClassDeclaration.TypeAttributes = TypeAttributes.NestedAssembly;

            string enumName = PrefabNameGenerator.EnumName;

            AddHiddenDictionaryFieldToDeclaration(
                PrefabInstancesDictionaryName, enumName, nameof(GameObject), storageClassDeclaration);

            AddHiddenDictionaryFieldToDeclaration(
                PathsToPrefabsDictionaryName, enumName, nameof(String).ToLower(), storageClassDeclaration);

            AddStaticConstructor(enumName, enumNames, pathsToPrefabs, storageClassDeclaration);

            AddGetPrefabMethod(enumName, storageClassDeclaration);
            AddLoadPrefabsMethod(storageClassDeclaration);

            codeNamespace.Types.Add(storageClassDeclaration);
            compileUnit.Namespaces.Add(codeNamespace);

            FileGenerationTools.SaveFile(compileUnit, StorageClassName, true);
        }

        private static void AddLoadPrefabsMethod(CodeTypeDeclaration declaration)
        {
            CodeMemberMethod loadPrefabsMethod = new CodeMemberMethod
            {
                Name = "LoadPrefabs",
                ReturnType = new CodeTypeReference(typeof(void)),
                Attributes = MemberAttributes.Public | MemberAttributes.Static
            };

            loadPrefabsMethod.Statements.Add(new CodeSnippetStatement(
            "\t\t\t" + @"foreach (var pathToPrefab in " + PathsToPrefabsDictionaryName + @")
            {
                " + PrefabInstancesDictionaryName +
                @"[pathToPrefab.Key] = Resources.Load<GameObject>(pathToPrefab.Value);
            }"));

            declaration.Members.Add(loadPrefabsMethod);
        }

        private static void AddGetPrefabMethod(string enumName, CodeTypeDeclaration declaration)
        {
            CodeMemberMethod getPrefabMethod = new CodeMemberMethod
            {
                Name = "GetPrefab",
                ReturnType = new CodeTypeReference(nameof(GameObject)),
                Attributes = MemberAttributes.Public | MemberAttributes.Static
            };

            string parameterName = "prefabName";

            getPrefabMethod.Parameters.Add(new CodeParameterDeclarationExpression(enumName, parameterName));
            getPrefabMethod.Statements.Add(new CodeMethodReturnStatement(
                new CodeIndexerExpression(
                    new CodeVariableReferenceExpression(PrefabInstancesDictionaryName),
                    new CodeVariableReferenceExpression(parameterName)
                )
            ));

            declaration.Members.Add(getPrefabMethod);
        }

        private static void AddStaticConstructor(
            string enumName, 
            Dictionary<int, string> enumNames, 
            List<string> pathsToPrefabs, 
            CodeTypeDeclaration declaration)
        {
            CodeTypeConstructor staticConstructor = new CodeTypeConstructor();

            for (int i = 0; i < enumNames.Count; i++)
            {
                var addStatement = new CodeMethodInvokeExpression(
                    new CodeVariableReferenceExpression(PathsToPrefabsDictionaryName),
                    "Add",
                    new CodeExpression[]
                    {
                        new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(enumName), enumNames[i]),
                        new CodePrimitiveExpression(pathsToPrefabs[i])
                    }
                );

                staticConstructor.Statements.Add(addStatement);
            }

            declaration.Members.Add(staticConstructor);
        }

        private static void AddHiddenDictionaryFieldToDeclaration(
            string name, 
            string firstType, 
            string secondType, 
            CodeTypeDeclaration declaration)
        {
            CodeMemberField dictionaryField = CreateDictionaryField(
                name, firstType, secondType, MemberAttributes.Private | MemberAttributes.Static);

            declaration.Members.Add(dictionaryField);
        }

        private static CodeMemberField CreateDictionaryField(
            string name, 
            string firstType, 
            string secondType, 
            MemberAttributes memberAttributes)
        {
            string dictionaryType = $"Dictionary<{firstType}, {secondType}>";

            return new(dictionaryType, name)
            {
                Attributes = memberAttributes,
                InitExpression = new CodeObjectCreateExpression(dictionaryType)
            };
        }
    }
}
