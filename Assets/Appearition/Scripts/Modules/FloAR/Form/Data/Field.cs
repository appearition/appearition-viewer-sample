// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: Field.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Appearition.Form
{
    [System.Serializable]
    public class Field
    {
        public string FieldKey;
        public long FieldType;
        public string FieldLabel;
        public string Instructions;
        public int Ordinal;
        public bool IsComposite;
        public int OrdinalComposite;
        public int CompositeRecursion;
        public List<string> ListItems;

        static Dictionary<long, string> _fieldTypeNameDictionary = new Dictionary<long, string> {
            {401, "Field Radio"},
            {402, "Checkbox"},
            {403, "Textbox Single Line"},
            {404, "Textbox Multi Line"},
            {405, "SectionTitle"},
            {406, "Take Photo"},
            {407, "Read Documents"},
            {408, "Date Selection"},
            {409, "Choose from List"},
            {411, "Barcode"},
            {415, "Time Selection"},
            {416, "Date and Time Selection"},
            {417, "Verify Asset"},
            {418, "Asset Check In/Out"}
        };

        /// <summary>
        /// Fetches the name of a field type from a given id.
        /// </summary>
        /// <param name="fieldType"></param>
        /// <returns></returns>
        public static string GetFieldTypeName(long fieldType)
        {
            if (_fieldTypeNameDictionary.ContainsKey(fieldType))
                return _fieldTypeNameDictionary[fieldType];
            return "";
        }
    }
}