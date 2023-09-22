using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using PLANSA.Core;
using PLANSA.ViewModel.Pages;

namespace PLANSA.Services
{
    public class DataSaveLoad
    {
        #region Paths
        public static string JsonPathPlans = $"{Environment.CurrentDirectory}\\Data\\UserData\\plansData.json";
        public static string JsonPathSettings = $"{Environment.CurrentDirectory}\\Setiings\\settingsData.json";
        public static string JsonPathCurrentData = $"{Environment.CurrentDirectory}\\Data\\CurrentData\\currentData.json";        
        #endregion

        #region Saves
        public static void Serialize(object o)
        {
            if (MainViewModelPage.Plans != null)
            {
                if (o.GetType() == MainViewModelPage.Plans.GetType())
                {
                    SaveDatas(JsonPathPlans, o);
                }
            }

            //if (SettingsViewModel.settingsObj != null)
            //{
            //    if (o.GetType() == SettingsViewModel.settingsObj.GetType())
            //    {
            //        SaveDatas(JsonPathSettings, o);
            //    }
            //}

            if (MainViewModelPage.CurrentDatas != null)
            {
                if (o.GetType() == MainViewModelPage.CurrentDatas.GetType())
                {
                    SaveDatas(JsonPathCurrentData, o);
                }
            }

            //if (CreatePageViewModel.AdvancedPlans != null)
            //{
            //    if (o.GetType() == CreatePageViewModel.AdvancedPlans.GetType())
            //    {
            //        SaveDatas(JsonPathAdvancedPlansData, o);
            //    }
            //}
        }

        public static void SaveDatas(string path, object o)
        {
            if (path != null)
            {
                string dir = Path.GetDirectoryName(path);
                if (!Directory.Exists(dir))
                {
                    _ = Directory.CreateDirectory(dir);
                }

                using (StreamWriter file = File.CreateText(path))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Formatting = Formatting.Indented;
                    serializer.Serialize(file, o);
                }
            }
        }
        #endregion

        #region Validation
        public static bool IsValidJson(string stringValue)
        {
            if (File.Exists(stringValue))
            {
                var value = File.ReadAllText(stringValue).Trim();
                if ((value.StartsWith("{") && value.EndsWith("}")) ||
                    (value.StartsWith("[") && value.EndsWith("]")))
                {
                    try
                    {
                        JToken obj = JToken.Parse(value);
                        return true;
                    }
                    catch (JsonReaderException)
                    {
                        return false;
                    }
                }
            }
            return false;
        }
        #endregion

        #region Loader
        public static ObservableCollectionEX<T> LoadData<T>(string path)
        {
            if (!IsValidJson(path))
            {
                ObservableCollectionEX<T> objects = new ObservableCollectionEX<T>();
                return objects;
            }

            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<ObservableCollectionEX<T>>(json);
        }
        #endregion
    }
}
