﻿using MahApps.Metro;
using MahApps.Metro.Controls.Dialogs;
using OfflineServer.Servers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace OfflineServer.Data.Settings
{
    public class AppSettings : ObservableObject
    {
        public UISettings uiSettings { get; set; }
        public AppSettings()
        {
            uiSettings = new UISettings();
        }

        public void saveInstance()
        {
            File.WriteAllText(DataEx.xml_Settings, this.SerializeObject(), Encoding.UTF8);
        }

        public class UISettings : ObservableObject
        {
            private static Persona activePersonaProxy { get { return Access.CurrentSession.ActivePersona; } }
            public class Style : ObservableObject
            {
                private Accent accent;
                private AppTheme theme;

                public Boolean haveSeenCustomAccentSampleMessage = false;
                public Boolean haveSeenCustomThemeSampleMessage = false;

                [XmlIgnore]
                public List<String> list_Accents { get; set; } = new List<String>();
                [XmlIgnore]
                public List<String> list_Themes { get; set; } = new List<String>();
                public String Accent
                {
                    get
                    {
                        return accent.Name;
                    }
                    set
                    {
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            if (Access.mainWindow != null && value == "CustomAccentSample" && !haveSeenCustomAccentSampleMessage)
                            {
                                Access.mainWindow.informUser("Sample custom accent", "Did you know that you can add or create infinite amounts of custom accents? Well, now you do! Go to " + DataEx.dir_Accents + " and get creative!");
                                haveSeenCustomAccentSampleMessage = true;
                            }
                            accent = ThemeManager.GetAccent(value);
                            if (theme != null) applyNewStyle();
                            RaisePropertyChangedEvent("Accent");
                        }
                    }
                }
                public String Theme
                {
                    get
                    {
                        return theme.Name;
                    }
                    set
                    {
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            if (Access.mainWindow != null && value == "CustomThemeSample" && !haveSeenCustomThemeSampleMessage)
                            {
                                Access.mainWindow.informUser("Sample custom theme", "Just like the accents, you can add or create infinite amounts of custom themes as well! Go to " + DataEx.dir_Themes + " and go crazy with it!");
                                haveSeenCustomThemeSampleMessage = true;
                            }
                            theme = ThemeManager.GetAppTheme(value);
                            if (accent != null) applyNewStyle();
                            Application.Current.Resources["ThemeImageColor"] = new SolidColorBrush(value == "BaseDark" ? Color.FromArgb(255, 80, 80, 80) : Color.FromArgb(255, 0, 0, 0));
                            RaisePropertyChangedEvent("Theme");
                        }
                    }
                }

                public void doDefault()
                {
                    accent = ThemeManager.GetAccent("Steel");
                    theme = ThemeManager.GetAppTheme("BaseDark");
                    RaisePropertyChangedEvent("Accent");
                    RaisePropertyChangedEvent("Theme");
                    applyNewStyle();
                }

                public void applyNewStyle()
                {
                    ThemeManager.ChangeAppStyle(Application.Current, accent, theme);
                    if (Access.dataAccess != null) Access.dataAccess.appSettings.saveInstance();
                }

                public Style()
                {
                    foreach (String accentXaml in Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, DataEx.dir_Accents), "*.xaml", SearchOption.TopDirectoryOnly))
                    {
                        String accentName = accentXaml.Replace(Path.Combine(Environment.CurrentDirectory, DataEx.dir_Accents), String.Empty).Replace(".xaml", String.Empty);

                        ThemeManager.AddAccent(accentName, new Uri(accentXaml, UriKind.Absolute));
                        list_Accents.Add(accentName);
                    }

                    foreach (String themeXaml in Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, DataEx.dir_Themes), "*.xaml", SearchOption.TopDirectoryOnly))
                    {
                        String themeName = themeXaml.Replace(Path.Combine(Environment.CurrentDirectory, DataEx.dir_Themes), String.Empty).Replace(".xaml", String.Empty);

                        ThemeManager.AddAppTheme(themeName, new Uri(themeXaml, UriKind.Absolute));
                        list_Themes.Add(themeName);
                    }

                    doDefault();
                }
            }

            public class Language : ObservableObject
            {
                #region ViewModel
                private String persona;
                private String achievementTreasureHunt;
                private String achievementJumpDistance;
                private String personaInfo;
                private String name;
                private String motto;
                private String level;
                private String cash;
                private String detailedPersonaInfo;
                private String personaList;
                private String levelXP;
                private String levelToolTip;
                private String cashToolTip;
                private String boostToolTip;
                private String amountOfCars;
                private String garageToolTip;
                private String timePlayedToolTip;
                private String garage;
                private String baseCarIdDefinition;
                private String noBaseCarIdDefinition;
                private String addACar;
                private String addACarText;
                private String removeCar;
                private String removeCarLastCarError;
                private String removeCarNoSelectedCarError;
                private String loadXElement;
                private String settings;
                private String uISettings;
                private String accent;
                private String theme;
                private String displayLanguage;
                private String select;
                private String cancel;

                public String Persona
                {
                    get
                    {
                        return persona;
                    }
                    set
                    {
                        if (persona != value)
                        {
                            persona = value;
                            RaisePropertyChangedEvent("Persona");
                        }
                    }
                }
                public String AchievementTreasureHunt
                {
                    get
                    {
                        if (activePersonaProxy != null)
                            return String.Format(achievementTreasureHunt, "dummy1", "dummy2");
                        return String.Empty;
                    }
                    set
                    {
                        if (achievementTreasureHunt != value)
                        {
                            achievementTreasureHunt = value;
                            RaisePropertyChangedEvent("AchievementTreasureHunt");
                        }
                    }
                }
                public String AchievementJumpDistance
                {
                    get
                    {
                        if (activePersonaProxy != null)
                            return String.Format(achievementJumpDistance, "dummy1", "dummy2");
                        return String.Empty;
                    }
                    set
                    {
                        if (achievementJumpDistance != value)
                        {
                            achievementJumpDistance = value;
                            RaisePropertyChangedEvent("AchievementJumpDistance");
                        }
                    }
                }
                public String PersonaInfo
                {
                    get
                    {
                        return personaInfo;
                    }
                    set
                    {
                        if (personaInfo != value)
                        {
                            personaInfo = value;
                            RaisePropertyChangedEvent("PersonaInfo");
                        }
                    }
                }
                public String Name
                {
                    get
                    {
                        return name;
                    }
                    set
                    {
                        if (name != value)
                        {
                            name = value;
                            RaisePropertyChangedEvent("Name");
                        }
                    }
                }
                public String Motto
                {
                    get
                    {
                        return motto;
                    }
                    set
                    {
                        if (motto != value)
                        {
                            motto = value;
                            RaisePropertyChangedEvent("Motto");
                        }
                    }
                }
                public String Level
                {
                    get
                    {
                        return level;
                    }
                    set
                    {
                        if (level != value)
                        {
                            level = value;
                            RaisePropertyChangedEvent("Level");
                        }
                    }
                }
                public String Cash
                {
                    get
                    {
                        return cash;
                    }
                    set
                    {
                        if (cash != value)
                        {
                            cash = value;
                            RaisePropertyChangedEvent("Cash");
                        }
                    }
                }
                public String DetailedPersonaInfo
                {
                    get
                    {
                        return detailedPersonaInfo;
                    }
                    set
                    {
                        if (detailedPersonaInfo != value)
                        {
                            detailedPersonaInfo = value;
                            RaisePropertyChangedEvent("DetailedPersonaInfo");
                        }
                    }
                }
                public String PersonaList
                {
                    get
                    {
                        return personaList;
                    }
                    set
                    {
                        if (personaList != value)
                        {
                            personaList = value;
                            RaisePropertyChangedEvent("PersonaList");
                        }
                    }
                }
                public String LevelXP
                {
                    get
                    {
                        if (activePersonaProxy != null)
                            return String.Format(levelXP, "CALCULATE FROM LEVEL REPS");
                        return String.Empty;
                    }
                    set
                    {
                        if (levelXP != value)
                        {
                            levelXP = value;
                            RaisePropertyChangedEvent("LevelXP");
                        }
                    }
                }
                public String LevelToolTip
                {
                    get
                    {
                        if (activePersonaProxy != null)
                            return String.Format(levelToolTip, activePersonaProxy.Name, activePersonaProxy.Level);
                        return String.Empty;
                    }
                    set
                    {
                        if (levelToolTip != value)
                        {
                            levelToolTip = value;
                            RaisePropertyChangedEvent("LevelToolTip");
                        }
                    }
                }
                public String CashToolTip
                {
                    get
                    {
                        if (activePersonaProxy != null)
                            return String.Format(cashToolTip, activePersonaProxy.Name, activePersonaProxy.Cash);
                        return String.Empty;
                    }
                    set
                    {
                        if (cashToolTip != value)
                        {
                            cashToolTip = value;
                            RaisePropertyChangedEvent("CashToolTip");
                        }
                    }
                }
                public String BoostToolTip
                {
                    get
                    {
                        if (activePersonaProxy != null)
                            return String.Format(boostToolTip, activePersonaProxy.Name, activePersonaProxy.Boost);
                        return String.Empty;
                    }
                    set
                    {
                        if (boostToolTip != value)
                        {
                            boostToolTip = value;
                            RaisePropertyChangedEvent("BoostToolTip");
                        }
                    }
                }
                public String AmountOfCars
                {
                    get
                    {
                        return amountOfCars;
                    }
                    set
                    {
                        if (amountOfCars != value)
                        {
                            amountOfCars = value;
                            RaisePropertyChangedEvent("AmountOfCars");
                        }
                    }
                }
                public String GarageToolTip
                {
                    get
                    {
                        if (activePersonaProxy != null)
                            return String.Format(garageToolTip, activePersonaProxy.Name, activePersonaProxy.Cars.Count);
                        return String.Empty;
                    }
                    set
                    {
                        if (garageToolTip != value)
                        {
                            garageToolTip = value;
                            RaisePropertyChangedEvent("GarageToolTip");
                        }
                    }
                }
                public String TimePlayedToolTip
                {
                    get
                    {
                        if (activePersonaProxy != null)
                            return String.Format(timePlayedToolTip, activePersonaProxy.Name, "dummy1");
                        return String.Empty;
                    }
                    set
                    {
                        if (timePlayedToolTip != value)
                        {
                            timePlayedToolTip = value;
                            RaisePropertyChangedEvent("TimePlayedToolTip");
                        }
                    }
                }
                public String Garage
                {
                    get
                    {
                        return garage;
                    }
                    set
                    {
                        if (garage != value)
                        {
                            garage = value;
                            RaisePropertyChangedEvent("Garage");
                        }
                    }
                }
                public String BaseCarIdDefinition
                {
                    get
                    {
                        return baseCarIdDefinition;
                    }
                    set
                    {
                        if (baseCarIdDefinition != value)
                        {
                            baseCarIdDefinition = value;
                            RaisePropertyChangedEvent("BaseCarIdDefinition");
                        }
                    }
                }
                public String NoBaseCarIdDefinition
                {
                    get
                    {
                        return noBaseCarIdDefinition;
                    }
                    set
                    {
                        if (noBaseCarIdDefinition != value)
                        {
                            noBaseCarIdDefinition = value;
                            RaisePropertyChangedEvent("NoBaseCarIdDefinition");
                        }
                    }
                }
                public String AddACar
                {
                    get
                    {
                        return addACar;
                    }
                    set
                    {
                        if (addACar != value)
                        {
                            addACar = value;
                            RaisePropertyChangedEvent("AddACar");
                        }
                    }
                }
                public String AddACarText
                {
                    get
                    {
                        return addACarText;
                    }
                    set
                    {
                        if (addACarText != value)
                        {
                            addACarText = value;
                            RaisePropertyChangedEvent("AddACarText");
                        }
                    }
                }
                public String RemoveCar
                {
                    get
                    {
                        return removeCar;
                    }
                    set
                    {
                        if (removeCar != value)
                        {
                            removeCar = value;
                            RaisePropertyChangedEvent("RemoveCar");
                        }
                    }
                }
                public String RemoveCarLastCarError
                {
                    get
                    {
                        return removeCarLastCarError;
                    }
                    set
                    {
                        if (removeCarLastCarError != value)
                        {
                            removeCarLastCarError = value;
                            RaisePropertyChangedEvent("RemoveCarLastCarError");
                        }
                    }
                }
                public String RemoveCarNoSelectedCarError
                {
                    get
                    {
                        return removeCarNoSelectedCarError;
                    }
                    set
                    {
                        if (removeCarNoSelectedCarError != value)
                        {
                            removeCarNoSelectedCarError = value;
                            RaisePropertyChangedEvent("RemoveCarNoSelectedCarError");
                        }
                    }
                }
                public String LoadXElement
                {
                    get
                    {
                        return loadXElement;
                    }
                    set
                    {
                        if (loadXElement != value)
                        {
                            loadXElement = value;
                            RaisePropertyChangedEvent("LoadXElement");
                        }
                    }
                }
                public String Settings
                {
                    get
                    {
                        return settings;
                    }
                    set
                    {
                        if (settings != value)
                        {
                            settings = value;
                            RaisePropertyChangedEvent("Settings");
                        }
                    }
                }
                public String UISettings
                {
                    get
                    {
                        return uISettings;
                    }
                    set
                    {
                        if (uISettings != value)
                        {
                            uISettings = value;
                            RaisePropertyChangedEvent("UISettings");
                        }
                    }
                }
                public String Accent
                {
                    get
                    {
                        return accent;
                    }
                    set
                    {
                        if (accent != value)
                        {
                            accent = value;
                            RaisePropertyChangedEvent("Accent");
                        }
                    }
                }
                public String Theme
                {
                    get
                    {
                        return theme;
                    }
                    set
                    {
                        if (theme != value)
                        {
                            theme = value;
                            RaisePropertyChangedEvent("Theme");
                        }
                    }
                }
                public String DisplayLanguage
                {
                    get
                    {
                        return displayLanguage;
                    }
                    set
                    {
                        if (displayLanguage != value)
                        {
                            displayLanguage = value;
                            RaisePropertyChangedEvent("DisplayLanguage");
                        }
                    }
                }
                public String Select
                {
                    get
                    {
                        return select;
                    }
                    set
                    {
                        if (select != value)
                        {
                            select = value;
                            RaisePropertyChangedEvent("Select");
                        }
                    }
                }
                public String Cancel
                {
                    get
                    {
                        return cancel;
                    }
                    set
                    {
                        if (cancel != value)
                        {
                            cancel = value;
                            RaisePropertyChangedEvent("Cancel");
                        }
                    }
                }
                #endregion

                [XmlIgnore]
                public List<String> list_Languages { get; set; } = new List<String>();
                public static Language loadFromLanguageFile(String languageName)
                {
                    return DataEx.getInstanceFromXml<Language>(languageName + ".xml");
                }
                public Language()
                {
                    foreach (String languageFile in Directory.GetFiles(DataEx.dir_Languages, "*.xml", SearchOption.TopDirectoryOnly))
                    {
                        list_Languages.Add(languageFile.Replace(DataEx.dir_Languages, String.Empty).Replace(".xml", String.Empty));
                    }
                }
            }

            public Style style { get; set; }
            [XmlIgnore]
            public Language language { get; set; }
            private String languageFile;
            public String LanguageFile
            {
                get { return languageFile; }
                set
                {
                    if (languageFile != value && !String.IsNullOrWhiteSpace(value)) {
                        languageFile = value;
                        language = Language.loadFromLanguageFile(languageFile);
                        if (language == null)
                        {
                            languageFile = "English";
                            language = Language.loadFromLanguageFile("English");
                        }
                        if (Access.dataAccess != null) Access.dataAccess.appSettings.saveInstance();
                        RaisePropertyChangedEvent("language");
                        RaisePropertyChangedEvent("LanguageFile");
                    }
                }
            }

            public UISettings()
            {
                style = new Style();
                LanguageFile = "English";
            }
        }
    }
}