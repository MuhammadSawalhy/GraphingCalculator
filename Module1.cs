using System;
using Microsoft.VisualBasic;
using Graphing;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
namespace Graphing
{
    static class Module1

    {
        public static List<Tab> TabArray = new List<Tab>();
        public static Tab TargettedTab;
        private readonly static string Quot = Strings.Mid("\"", 1, 1);

        public static void Set_setting(string name, object value)
        {
            Properties.Settings.Default[name] = value;
        }
        public static void Set_setting(string[] name, object[] value)
        {
            if (name.Length == value.Length && name.Length > 0)
            {
                for (int i = 0; i <= name.Length - 1; i++)
                    Properties.Settings.Default[name[i]] = value[i];
            }
        }
        public static string ShowInput(string message,string customvalue)
        {
            InputForm form = new InputForm();
            form.ShowDialog();
            return form.Value;
        }

        //public static string[] Build_control_text(Control container_)
        //{
        //    string str = "";
        //    foreach (Control control_ in container_.Controls)
        //    {
        //        if (control_ is TabControl || control_ is Panel || control_ is GroupBox)
        //        {
        //            if (control_ is TabPage)
        //                str += "§" + container_.name + "." + control_.Name + "Ï" + control_.Text + "Ï" + control_.Font.Name + "," + control_.Font.Size;
        //            string[] str1 = Build_control_text(control_);
        //            for (int i = 0; i <= str1.Length - 1; i++)
        //            {
        //                string[] str2 = str1[i].Split("Ï");
        //                str += "§" + container_.name + "." + str2[0] + "Ï" + str2[1] + "Ï" + str2[2];
        //            }
        //        }
        //        else if (control_.Text != "" && !IsNumeric(control_.Text))
        //            str += "§" + container_.Name + "." + control_.Name + "Ï" + control_.Text + "Ï" + control_.Font.Name + "," + control_.Font.Size;
        //    }
        //    if (str != "")
        //        return str.Substring(1, str.Length - 1).Split('§');
        //    return new string[] { };
        //}
        //public static void BuildXmlLangFile(Form form_)
        //{
        //    string[] tex = new string[3];
        //    foreach (Control control_ in form_.Controls)
        //    {
        //        if (control_ is TabControl || control_ is Panel || control_ is GroupBox)
        //        {
        //            string[] str1 = Build_control_text(control_);
        //            for (int i = 0; i <= str1.Length - 1; i++)
        //            {
        //                string[] str2 = str1[i].Split("Ï");
        //                tex[0] += str2[0] + ",";
        //                tex[1] += "<" + str2[0] + ">" + str2[1] + "</" + str2[0] + ">";
        //                tex[2] += "<" + str2[0] + ">" + str2[2] + "</" + str2[0] + ">";
        //            }
        //        }
        //        else if (control_.Text != "" && !IsNumeric(control_.Text) && !control_ is ComboBox)
        //        {
        //            tex[0] += control_.Name + ",";
        //            tex[1] += "<" + control_.Name + ">" + control_.Text + "</" + control_.Name + ">";
        //            tex[2] += "<" + control_.Name + ">" + control_.Font.Name + "," + control_.Font.Size + "</" + control_.Name + ">";
        //        }
        //    }
        //    string quot = Strings.Mid("\"", 1, 1);
        //    string string_ = "<?xml version=" + quot + "1.0" + quot + " encoding=" + quot + "utf-8" + quot + "?>";
        //    string_ += "<language>";
        //    string_ += "<lang>" + "<" + tex[0].Split(",")(0) + ">" + Strings.Mid(tex[0], 1, tex[0].Length - 1) + "</" + tex[0].Split(",")(0) + ">" + "</lang>";
        //    string_ += "<lang>" + tex[1] + "</lang>";
        //    string_ += "<lang>" + tex[2] + "</lang>";
        //    string_ += "</language>";
        //    Clipboard.SetText(string_);
        //}
        //public static void _text(Windows.Forms.Form form_)
        //{
        //    string[] objects;
        //    DataSet_lang = new DataSet();
        //    switch (MathPackage.Main.Lang)
        //    {
        //        case 0:
        //            {
        //                DataSet_lang.ReadXml(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\GraphMaker\Languages\" + form_.Name + "_lang_ar.xml");
        //                break;
        //            }

        //        case 1:
        //            {
        //                DataSet_lang.ReadXml(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\GraphMaker\Languages\" + form_.Name + "_lang_en.xml");
        //                break;
        //            }
        //    }
        //    objects = DataSet_lang.Tables(0).Rows(0).Item(0).split(",");
        //    Windows.Forms.Control contr;
        //    for (int i = 0; i <= objects.Length - 1; i++)
        //    {
        //        string[] str = objects[i].Split(".");
        //        contr = form_.Controls(str[0]);
        //        for (int ii = 1; ii <= str.Length - 1; ii++)
        //            contr = contr.Controls(str[ii]);
        //        contr.Text = DataSet_lang.Tables(0).Rows(1).Item(objects[i]);
        //    }
        //}
        //public static void _font(Windows.Forms.Form form_)
        //{
        //    string[] objects;
        //    DataSet_lang = new DataSet();
        //    switch (MathPackage.Main.Lang)
        //    {
        //        case 0:
        //            {
        //                DataSet_lang.ReadXml(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\GraphMaker\Languages\" + form_.Name + "_lang_ar.xml");
        //                break;
        //            }

        //        case 1:
        //            {
        //                DataSet_lang.ReadXml(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\GraphMaker\Languages\" + form_.Name + "_lang_en.xml");
        //                break;
        //            }
        //    }
        //    objects = DataSet_lang.Tables(0).Rows(0).Item(0).split(",");
        //    Windows.Forms.Control contr;
        //    for (int i = 0; i <= objects.Length - 1; i++)
        //    {
        //        string[] str = objects[i].Split(".");
        //        contr = form_.Controls(str[0]);
        //        for (int ii = 1; ii <= str.Length - 1; ii++)

        //            contr = contr.Controls(str[ii]);

        //        string[] item = DataSet_lang.Tables(0).Rows(2).Item(objects[i]).split(",");
        //        contr.Font = new System.Drawing.Font(item[0], Conversion.Val(item[1]));
        //    }
        //}



    }

}