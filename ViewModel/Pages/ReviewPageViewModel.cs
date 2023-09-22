using PLANSA.Core;
using PLANSA.Model;
using PLANSA.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLANSA.ViewModel.Pages
{
    internal class ReviewPageViewModel
    {
        #region Collections
        public static ObservableCollectionEX<PlanModel>? Plans { get; set; }
        #endregion

        public ReviewPageViewModel()
        {
            StartPage();
        }

        #region Methods
        #region StartOperations
        public void StartPage()
        {
            if (MainViewModelPage.NullChecker(Plans))
            {
                Plans = new();
                try
                {
                    Plans = DataSaveLoad.LoadData<PlanModel>(DataSaveLoad.JsonPathPlans);
                }
                catch (Exception ex)
                {

                    Debug.WriteLine("Неудалось загрузить файлы коллекции Plans.\nКод ошибки: " + ex.Message);
                }
            }
            else
            {
                try
                {
                    Plans = DataSaveLoad.LoadData<PlanModel>(DataSaveLoad.JsonPathPlans);
                }
                catch (Exception ex)
                {

                    Debug.WriteLine("Неудалось загрузить файлы коллекции Plans.\nКод ошибки: " + ex.Message);
                }
            }
        }
        #endregion        
        #endregion
    }
}
