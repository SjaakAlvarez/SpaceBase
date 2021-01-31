using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;

namespace SpaceBaseMono
{
    public delegate object ValidatorDelegate(string readValue);
    public delegate string SaveFormatDelegate(object readValue);

    public class GameSettings
    {
        struct setting
        {
            public object fileName;
            public string name;

            public object actualValue;
            public object defaultValue;

            public ValidatorDelegate validate;
            public SaveFormatDelegate saveFormat;
        }

        //const string settingsDirectory = "Settings";
        const string configSuffix = ".ini";

        static Dictionary<string, setting> settings =
            new Dictionary<string, setting>();
        public static string SettingsDirectory() { return Environment.CurrentDirectory  + Path.DirectorySeparatorChar; }

        public static void registerSetting(string file, string name, object defaultValue, ValidatorDelegate validator, SaveFormatDelegate saveFormat)
        {
            INIFile f;
            setting v = new setting();

            if (!Directory.Exists(SettingsDirectory()))
                Directory.CreateDirectory(SettingsDirectory());

            string[] taxonomy = name.Split(".".ToCharArray(), 2);

            //Debug.Assert(taxonomy.Length == 2, "Taxonomy must include at least one dot character [.]");

            v.name = name;
            v.fileName = file;

            v.validate = validator;
            v.saveFormat = saveFormat;

            v.defaultValue = defaultValue;

            f = new INIFile(SettingsDirectory() + v.fileName + configSuffix);
            v.actualValue = validator(f.ReadValue(taxonomy[0], taxonomy[1], saveFormat(defaultValue)));

            settings.Add(name, v);
        }

        public static void set(string name, Object value)
        {
            setting s = settings[name];
            s.actualValue = value;
            settings[name] = s;
        }

        internal static void saveSettings()
        {
            foreach (setting v in settings.Values)
            {
                Save(v);
            }
        }

        private static void Save(setting v)
        {
            INIFile f = new INIFile(SettingsDirectory() + v.fileName + configSuffix);

            string[] taxonomy = v.name.Split(".".ToCharArray(), 2);

            f.WriteValue(taxonomy[0], taxonomy[1], v.saveFormat(v.actualValue));
        }

        public static string getString(string p)
        {
            return (string)settings[p].actualValue;
        }

        public static Keys getKey(string p)
        {
            Keys key;
            if (Enum.TryParse<Keys>((string)settings[p].actualValue, out key))
                return key;
            else
                return (Keys)Enum.Parse(typeof(Keys),(string)settings[p].defaultValue);
        }

        public static string getStringDef(string p)
        {
            return (string)settings[p].defaultValue;
        }

        public static bool getBool(string p)
        {
            return Boolean.Parse(settings[p].actualValue.ToString());
            //return (bool)settings[p].actualValue;
        }

        public static bool getBoolDef(string p)
        {
            return (bool)settings[p].defaultValue;
        }

        public static int getInt(string p)
        {
            return (int)settings[p].actualValue;
        }

        public static int getIntDef(string p)
        {
            return (int)settings[p].defaultValue;
        }

        public static float getFloat(string p)
        {
            return (float)settings[p].actualValue;
        }

        public static float getFloatDef(string p)
        {
            return (float)settings[p].defaultValue;
        }

        public static string SaveFormatStd(object var)
        {
            return var.ToString();
        }

        public static object ValidateString(string var)
        {
            return var;
        }

        public static object ValidateFloat(string var)
        {
            float result = 0;

            if (!float.TryParse(var, out result))
                return 0;

            return result;
        }

        public static object ValidateBool(string var)
        {
            bool result = true;

            if (!bool.TryParse(var, out result))
                return 0;

            return result;
        }

        public static object ValidateInt(string var)
        {
            int result = 0;

            if (!int.TryParse(var, out result))
                return 0;

            return result;
        }
    }
}
