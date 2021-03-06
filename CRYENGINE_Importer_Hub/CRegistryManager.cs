﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRYENGINE_ImportHub
{
    class CRegistryManager
    {
        const string FULL_KEY_PATH = @"HKEY_CURRENT_USER\Software\CRYENGINE_ImportHub\Settings";
        const string SHORT_KEY_PATH = @"Software\CRYENGINE_ImportHub\Settings";

        public string m_customPathInput { get; set; }
        public static string[] m_customSlots = new string[7];

        public CRegistryManager()
        {
            #if DEBUG
            Framework.Log("Init registry manager");
            #endif

            //Test if the settings are stored in the registry
            RegistryKey testKey = Registry.CurrentUser.OpenSubKey(SHORT_KEY_PATH);
            if (testKey != null)
            {
                GetAllValues();
            }
            else
            {
                CreateKeys();
            }
        }

        private void GetAllValues()
        {
            #if DEBUG
            Framework.Log("Get settings values from registry");
            #endif

            m_customPathInput = (string)Registry.GetValue(FULL_KEY_PATH, "CustomPathInput", null);

            for (int i = 1; i <= 6; i++)
            {
                m_customSlots[i] = (string)Registry.GetValue(FULL_KEY_PATH, "CustomSlot" + i, null);
            }
        }

        private void CreateKeys()
        {
            Framework.Log("Registry keys not found: beginning creation");

            Registry.SetValue(FULL_KEY_PATH, "CustomPathInput", "");

            Registry.SetValue(FULL_KEY_PATH, "CustomSlot1", "SDK Folder~[SDK_FOLDER]~");
            Registry.SetValue(FULL_KEY_PATH, "CustomSlot2", @"Sandbox Editor (x64)~[SDK_FOLDER]bin64\Editor.exe~");
            Registry.SetValue(FULL_KEY_PATH, "CustomSlot3", "CRYENGINE Documentation~http://docs.cryengine.com/~");
            Registry.SetValue(FULL_KEY_PATH, "CustomSlot4", "");
            Registry.SetValue(FULL_KEY_PATH, "CustomSlot5", "");
            Registry.SetValue(FULL_KEY_PATH, "CustomSlot6", "");

            Framework.Log("Registry keys created succefully");

            GetAllValues();
        }

        public static void SaveAllSettings(string CustomPathInput)
        {
            Framework.Log("Save settings on the registry");

            Registry.SetValue(FULL_KEY_PATH, "CustomPathInput", CustomPathInput);

            Framework.Log("Settings saved");
        }

        public static void SaveCustomSlot(int id, string title, string path, string args)
        {
            if (title != null && title != "")
                Registry.SetValue(FULL_KEY_PATH, "CustomSlot" + id, title + "~" + path + "~" + args);
            else
                Registry.SetValue(FULL_KEY_PATH, "CustomSlot" + id, "");

            Framework.Log("Custom link " + title + " (" + id.ToString() + ") saved");
        }

        public static string[] GetCustomSlots()
        {
            return m_customSlots;
        }
    }
}
