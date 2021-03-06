﻿using ICSharpCode.SharpZipLib.Zip;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using static AddonManager.Addon;

namespace AddonManager
{
    internal static class AddonEx
    {
        internal static String dir_HttpServerCatalogs;
        internal static String dir_HttpServerBaskets;
        internal static String dir_Accents;
        internal static String dir_Themes;
        internal static String dir_Languages;
        internal static String dir_MemoryPatches;
        internal static String dir_Logs;

        internal static dynamic readAddonProperty(this String filePath, dynamic[] typeDef)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                try
                {
                    int bytesToRead = typeDef[0] != AddonProperty.AddonFile ? typeDef[2] * 4 : (int)fileStream.Length - typeDef[1];
                    int bytesRead = 0;
                    int count = 0;
                    byte[] buffer = new byte[bytesToRead];
                    fileStream.Position = typeDef[1];

                    do
                    {
                        bytesRead += count;
                        bytesToRead -= count;
                        count = fileStream.Read(buffer, bytesRead, bytesToRead);
                    } while (count > 0);
                    if (typeDef[0] != AddonProperty.AddonFile)
                        return new UTF8Encoding(false, false).GetString(buffer).TrimEnd('\0');
                    return buffer;
                }
                catch (Exception ex)
                {
                    Debug.Print("Something isn't quite right...");
                    Debug.Print(ex.ToString());
                    return null;
                }
            }
        }
        internal static void saveAddonProperty(this String filePath, dynamic[] typeDef, String strValue, Boolean resetFile = false)
        {
            using (FileStream fileStream = new FileStream(filePath, resetFile ? FileMode.Create : FileMode.Open, FileAccess.Write, FileShare.None))
            {
                byte[] value = new UTF8Encoding(false, false).GetBytes(strValue.Substring(0, Math.Min(strValue.Length, typeDef[2] - 1)) + '\0');
                fileStream.Position = typeDef[1];
                fileStream.Write(value, 0, value.Length);
            }
        }

        internal static String extractAddonFile(this String addonPath)
        {
            string tempDir = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(addonPath));
            if (Directory.Exists(tempDir)) Directory.Delete(tempDir, true);
            Directory.CreateDirectory(tempDir);

            string tempFile = Path.GetTempFileName();
            File.WriteAllBytes(tempFile, readAddonProperty(addonPath, addonFileDef));

            new FastZip().ExtractZip(tempFile, tempDir, null);
            if (File.Exists(tempFile)) File.Delete(tempFile);

            return tempDir;
        }
        internal static Boolean saveAddonFile(this String filePath, String addonType, params String[][] lists)
        {
            retry:
            string tempDir = Path.Combine(Path.GetTempPath(), "addonmanager");
            string zipFile = Path.Combine(Path.GetTempPath(), "addon.zip");
            if (Directory.Exists(tempDir)) Directory.Delete(tempDir, true);
            if (File.Exists(zipFile)) File.Delete(zipFile);
            Directory.CreateDirectory(tempDir);

            try
            {
                switch (addonType)
                {
                    case AddonType.catalogWithBaskets:
                        {
                            string[] products = lists[0];
                            string[] categories = lists[1];
                            string[] baskets = lists[2];

                            var productsTemp = Directory.CreateDirectory(Path.Combine(tempDir, dir_HttpServerCatalogs,
                                Path.GetFileNameWithoutExtension(filePath), "Products"));
                            var categoriesTemp = Directory.CreateDirectory(Path.Combine(tempDir, dir_HttpServerCatalogs,
                                Path.GetFileNameWithoutExtension(filePath), "Categories"));
                            var basketsTemp = Directory.CreateDirectory(Path.Combine(tempDir, dir_HttpServerBaskets,
                                Path.GetFileNameWithoutExtension(filePath)));

                            foreach (string product in products)
                                File.Copy(AddonProject.Catalog.getFileLocation(product), Path.Combine(productsTemp.FullName,
                                    AddonProject.Catalog.getListBoxEntryText(product)), true);
                            foreach (string category in categories)
                                File.Copy(AddonProject.Catalog.getFileLocation(category), Path.Combine(categoriesTemp.FullName,
                                    AddonProject.Catalog.getListBoxEntryText(category)), true);
                            foreach (string basket in baskets)
                                File.Copy(AddonProject.Catalog.getFileLocation(basket), Path.Combine(basketsTemp.FullName,
                                    AddonProject.Catalog.getListBoxEntryText(basket)), true);
                        }
                        break;
                    case AddonType.accent:
                        {
                            string accentName = Path.GetFileNameWithoutExtension(filePath);
                            string accentXaml = lists[0][0];
                            var accentsTemp = Directory.CreateDirectory(Path.Combine(tempDir, dir_Accents));
                            string tempAccentFile = accentsTemp.FullName + accentName + ".xaml";

                            using (FileStream fileStream = new FileStream(tempAccentFile, FileMode.Create, FileAccess.Write, FileShare.None))
                            {
                                byte[] value = new UTF8Encoding(false, false).GetBytes(accentXaml);
                                fileStream.Write(value, 0, value.Length);
                            }
                        }
                        break;
                    case AddonType.theme:
                        {
                            string themeName = Path.GetFileNameWithoutExtension(filePath);
                            string themeXaml = lists[0][0];
                            var themesTemp = Directory.CreateDirectory(Path.Combine(tempDir, dir_Themes));
                            string tempThemeFile = themesTemp.FullName + themeName + ".xaml";

                            using (FileStream fileStream = new FileStream(tempThemeFile, FileMode.Create, FileAccess.Write, FileShare.None))
                            {
                                byte[] value = new UTF8Encoding(false, false).GetBytes(themeXaml);
                                fileStream.Write(value, 0, value.Length);
                            }
                        }
                        break;
                    case AddonType.language:
                        {
                            string languageName = Path.GetFileNameWithoutExtension(filePath);
                            string languageXml = lists[0][0];
                            var languagesTemp = Directory.CreateDirectory(Path.Combine(tempDir, dir_Languages));
                            string tempLanguageFile = languagesTemp.FullName + languageName + ".xml";

                            using (FileStream fileStream = new FileStream(tempLanguageFile, FileMode.Create, FileAccess.Write, FileShare.None))
                            {
                                byte[] value = new UTF8Encoding(false, false).GetBytes(languageXml);
                                fileStream.Write(value, 0, value.Length);
                            }
                        }
                        break;
                    case AddonType.memoryPatch:
                        // not yet implemented
                        break;
                }
            }
            catch (DirectoryNotFoundException dnfEx)
            {
                DialogResult userResponse = MessageBox.Show("AddonManager couldn't access the following directory:\r\n'" + dnfEx.getPath() + "'.",
                    "Beep boop, I done goofed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (userResponse == DialogResult.Retry)
                    goto retry;

                if (Directory.Exists(tempDir)) Directory.Delete(tempDir, true);
                if (File.Exists(zipFile)) File.Delete(zipFile);
                return false;
            }
            catch (FileNotFoundException fnfEx)
            {
                DialogResult userResponse = MessageBox.Show("AddonManager couldn't access the following file:\r\n'" + fnfEx.FileName + "'.",
                    "Beep boop, I done goofed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (userResponse == DialogResult.Retry)
                    goto retry;

                if (Directory.Exists(tempDir)) Directory.Delete(tempDir, true);
                if (File.Exists(zipFile)) File.Delete(zipFile);
                return false;
            }

            FastZip fz = new FastZip();
            fz.CreateEmptyDirectories = true;
            fz.CreateZip(zipFile, tempDir, true, null);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Write, FileShare.None))
            {
                byte[] addonFileBytes = File.ReadAllBytes(zipFile);
                fileStream.Position = Addon.addonFileDef[1];
                fileStream.Write(addonFileBytes, 0, addonFileBytes.Length);
            }

            if (Directory.Exists(tempDir)) Directory.Delete(tempDir, true);
            if (File.Exists(zipFile)) File.Delete(zipFile);
            return true;
        }

        internal static Boolean installAddon(this String addonPath)
        {
            string extractedAddonDir = extractAddonFile(addonPath);

            try
            {
                foreach (string directory in Directory.GetDirectories(extractedAddonDir, "*", System.IO.SearchOption.TopDirectoryOnly))
                {
                    FileSystem.MoveDirectory(Path.GetDirectoryName(directory),
                        Path.GetDirectoryName(Application.ExecutablePath),
                        UIOption.AllDialogs, UICancelOption.ThrowException);
                }
                if (Directory.Exists(extractedAddonDir)) Directory.Delete(extractedAddonDir, true);
                return true;
            }
            catch (Exception)
            {
                if (Directory.Exists(extractedAddonDir)) Directory.Delete(extractedAddonDir, true);
                return false;
            }
        }
    }
}