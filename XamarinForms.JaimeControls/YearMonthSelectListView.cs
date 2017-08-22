using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinForms.JaimeControls
{

    /// <summary>
    /// Year/Month Select ListView
    /// Author : Jaime Morais 
    /// 
    /// Usage :
    /// 
    /// XAML 
    ///         <customrenderers:YearMonthSelectListView
    ///              MinYear="1998" 
    ///              MaxYear="2017"
    ///              YearMonthSelected="YearMonthSelectListView_YearMonthSelected"/>
    ///              
    /// Code behind (or use your preferred event to command behavior for mvvm) :
    /// 
    ///         private void YearMonthSelectListView_YearMonthSelected(object sender, SelectedItemChangedEventArgs e)
    ///         {
    ///             string yearMonthSelected = e.SelectedItem.ToString();
    ///             this.DisplayAlert("Hello!", "Year/Month Selected : " + yearMonthSelected, "OK");
    ///         }
    /// 
    /// </summary>
    public class YearMonthSelectListView : ListView
    {
        public static readonly BindableProperty MinYearProperty =
            BindableProperty.Create(nameof(MinYearProperty),
                typeof(int),
                typeof(YearMonthSelectListView),
                2000,
                propertyChanged: MinYearPropertyChanged);

        public static readonly BindableProperty MaxYearProperty =
            BindableProperty.Create(nameof(MaxYearProperty),
                typeof(int),
                typeof(YearMonthSelectListView),
                DateTime.Today.Year,
                propertyChanged: MaxYearPropertyChanged);


        public int MinYear
        {
            get { return (int)GetValue(MinYearProperty); }
            set { SetValue(MinYearProperty, value); }
        }

        public int MaxYear
        {
            get { return (int)GetValue(MaxYearProperty); }
            set { SetValue(MaxYearProperty, value); }
        }

        private static void MinYearPropertyChanged(BindableObject bindable, object oldVal, object newVal)
        {
            ((YearMonthSelectListView)bindable).MinYear = int.Parse(newVal.ToString());
        }

        private static void MaxYearPropertyChanged(BindableObject bindable, object oldVal, object newVal)
        {
            ((YearMonthSelectListView)bindable).MaxYear = int.Parse(newVal.ToString());
        }

        public YearMonthSelectListView()
        {
            ItemSelected += YearMonthSelectListView_ItemSelected;

            LoadYearMonthList(null);
        }

        private void YearMonthSelectListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            DateTime yearOut;
            bool isValidYear = DateTime.TryParseExact(e.SelectedItem.ToString(), "yyyy", null, System.Globalization.DateTimeStyles.None, out yearOut);

            if (isValidYear)
                LoadYearMonthList(int.Parse(e.SelectedItem.ToString()));
            else
                YearMonthSelected(sender, e);
        }


        public event EventHandler YearMonthSelected;


        private void LoadYearMonthList(int? selectedYear)
        {
            IList<string> yearMonthList = new List<string>();

            yearMonthList.Clear();

            int currentSelectedYear = DateTime.Today.Year;

            if (selectedYear != null)
                currentSelectedYear = selectedYear.Value;


            // Adds later years to the one defined in property MaxYear
            for (int yearX = MaxYear; yearX > currentSelectedYear; yearX--)
            {
                yearMonthList.Add(yearX.ToString());
            }

            // Adds all months of the current selected year
            for (int monthX = 12; monthX >= 1; monthX--)
            {
                yearMonthList.Add(currentSelectedYear + "/" + GetMonthName(monthX));
            }

            // Adds the previous years to the one defined in property MinYear
            for (int yearX = currentSelectedYear - 1; yearX >= MinYear; yearX--)
            {
                yearMonthList.Add(yearX.ToString());
            }

            this.ItemsSource = yearMonthList;
        }

        private static string GetMonthName(int month)
        {
            string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month).ToLower();
            return char.ToUpper(monthName[0]) + monthName.Substring(1);
        }
    }
}
