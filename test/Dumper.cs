using System;
using System.Collections;
using System.Reflection;
using System.Text;
using Google.Protobuf;
using UnityEngine;

public class Dumper
{
    private static readonly StringBuilder _text = new StringBuilder("", 1024);

    private static void AppendIndent(int num)
    {
        _text.Append(' ', num);
    }

    private static void DoDump(object obj)
    {
        if (obj == null)
        {
            _text.Append("null");
            _text.Append(",");
            return;
        }

        Type t = obj.GetType();

        //repeat field
        if (obj is IList)
        {
            /*
            _text.Append(t.FullName);
            _text.Append(",");
            AppendIndent(1);
            */

            _text.Append("[");
            IList list = obj as IList;
            foreach (object v in list)
            {
                DoDump(v);
            }
            _text.Append("]");
        }
        else if (t.IsValueType || obj is string || obj is ByteString)
        {
            _text.Append(obj);
            _text.Append(",");
            AppendIndent(1);
        }
        else if (t.IsArray)
        {
            var a = (Array)obj;
            _text.Append("[");
            for (int i = 0; i < a.Length; i++)
            {
                _text.Append(i);
                _text.Append("=");
                DoDump(a.GetValue(i));
            }
            _text.Append("]");
        }
        else if (t.IsClass)
        {
            _text.Append(string.Format("<{0}>", t.Name));
            _text.Append("{");
            var fields = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (fields.Length > 0)
            {
                foreach (var info in fields)
                {
                    _text.Append(info.Name);
                    _text.Append("=");
                    var value = info.GetGetMethod().Invoke(obj, null);
                    DoDump(value);
                }
            }
            _text.Append("}");
        }
        else
        {
            Debug.LogWarning("unsupport type: " + t.FullName);
            _text.Append(obj);
            _text.Append(",");
            AppendIndent(1);
        }
    }

    public static string DumpAsString(object obj, string hint = "")
    {
        _text.Remove(0, _text.Length);
        _text.Append(hint);
        DoDump(obj);
        return _text.ToString();
    }

    public static void Dump(object obj, string hint = "")
    {
        //#if UNITY_EDITOR
        Debug.Log(DumpAsString(obj, hint));
        //#endif
    }
}



