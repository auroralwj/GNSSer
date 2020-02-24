using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Office.Interop.Word;

namespace Geo.Utils
{
    /// <summary>
    /// Word组件的很多参数强制使用ref，许多常数无法直接使用，必须用下面的变量传递
    /// </summary>
    public class WordVariable
    {
        /// <summary>
        /// WdDocumentType.wdTypeDocument
        /// </summary>
        public static object WdDocumentType_wdTypeDocument = WdDocumentType.wdTypeDocument;
        /// <summary>
        /// WdPageNumberAlignment.wdAlignPageNumberCenter
        /// </summary>
        public static object WdPageNumberAlignment_wdAlignPageNumberCenter = WdPageNumberAlignment.wdAlignPageNumberCenter;
        /// <summary>
        /// WdUnits.wdLine
        /// </summary>
        public static object WdUnits_wdLine = WdUnits.wdLine;
        /// <summary>
        /// WdUnits.wdCharacter
        /// </summary>
        public static object WdUnits_wdCharacter = WdUnits.wdCharacter;
        /// <summary>
        /// WdBreakType.wdSectionBreakNextPage
        /// </summary>
        public static object WdBreakType_wdSectionBreakNextPage = WdBreakType.wdSectionBreakNextPage;
        /// <summary>
        /// WdRecoveryType.wdPasteDefault
        /// </summary>
        public static object WdRecoveryType_wdPasteDefault = WdRecoveryType.wdPasteDefault;
        /// <summary>
        /// WdSaveOptions.wdDoNotSaveChanges
        /// </summary>
        public static object WdSaveOptions_wdDoNotSaveChanges = WdSaveOptions.wdDoNotSaveChanges;
        /// <summary>
        /// WdReplace.wdReplaceAll
        /// </summary>
        public static object WdReplace_wdReplaceAll = WdReplace.wdReplaceAll;
    }
}