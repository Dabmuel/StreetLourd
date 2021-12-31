using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using StreetLourd.View;
using StreetLourd.Model;
using StreetLourd.Model.Schema;

namespace StreetLourd.Controller
{
    class ControllerStat
    {
        private ViewStat viewStat { get; set; } = new ViewStat();
        private List<ModelMap> MapsList { get; set; }
        private List<CarStat> StatList { get; set; } = new List<CarStat>();
        private string Type { get; set; }
        private bool ResearchOnly { get => this.viewStat.ChBxOnlyResearch.IsChecked.Value; set => this.viewStat.ChBxOnlyResearch.IsChecked = value; }

        public ControllerStat(string Type, List<ModelMap> MapsList)
        {
            this.Type = Type;
            this.MapsList = MapsList;
            this.initView();

            this.viewStat.Activated += this.RefreshViewList;
            this.viewStat.CbFilter.SelectionChanged += this.NewFilter;
            this.viewStat.ChBxOnlyResearch.Click += this.RefreshViewList;
            this.viewStat.TxBxResearch.TextChanged += this.RefreshViewList;

            this.RefreshList("Toutes");
        }

        private void initView()
        {
            this.viewStat.TxName.Text = "Rapport " + this.Type;
            foreach(ModelMap map in this.MapsList)
            {
                TextBlock txBlock = new TextBlock();
                txBlock.Text = map.Name;
                this.viewStat.MapList.Items.Add(txBlock);
            }
            this.viewStat.Visibility = System.Windows.Visibility.Visible;
        }

        public void NewFilter(object a, object b)
        {
            string Filter = ((ComboBoxItem)this.viewStat.CbFilter.SelectedItem).Content.ToString();
            this.RefreshList(Filter);
            this.RefreshViewList();
        }

        private void RefreshList(string Filter)
        {
            List<ModelMapStat> MapsStatList = new List<ModelMapStat>();
            this.StatList.Clear();

            foreach (ModelMap map in this.MapsList)
            {
                MapsStatList.Add(new ModelMapStat(map, Filter));
            }

            if(MapsStatList.Count > 0)
            {
                foreach(CarStat carStat in MapsStatList[0].GetCars)
                {
                    this.StatList.Add(carStat);
                }
            }

            for(int i = 1; i < MapsStatList.Count; i++)
            {
                foreach (CarStat carStat in MapsStatList[i].GetCars)
                {
                    CarStat uhoh = this.StatList.Find(x => (x.Company == carStat.Company && x.Name == carStat.Name && x.Score == carStat.Score));
                    if(uhoh != null)
                    {
                        this.CarStatMerge(uhoh, carStat);
                    }
                    else
                    {
                        this.StatList.Add(carStat);
                    }
                }
            }

            this.StatList.Sort((y, x) => x.Stat.CompareTo(y.Stat));
        }

        public void RefreshViewList(object a, object b)
        {
            this.RefreshViewList();
        }

        private void RefreshViewList()
        {
            this.viewStat.List.Items.Clear();
            string ResearchQuery = this.viewStat.TxBxResearch.Text;

            foreach (CarStat Car in StatList)
            {
                if (this.ResearchOnly)
                    if (!IsRightCar(Car, ResearchQuery))
                        continue;

                ViewRun viewRun = new ViewRun();
                Frame frame = new Frame();

                frame.HorizontalAlignment = HorizontalAlignment.Left;
                frame.VerticalAlignment = VerticalAlignment.Top;
                frame.Margin = new Thickness(0, 0, 0, 0);

                viewRun.TxName.Text = Car.Company + " " + Car.Name;
                viewRun.TxClasse.Text = " " + Car.Classe + " " + Car.Score + " ";
                viewRun.TxClasse.Background = StreetLourdColor.ClassColor(Car.Classe);
                viewRun.TxNb.Text = Car.MapNb.ToString() + " courses";
                viewRun.Width = this.viewStat.List.ActualWidth - 30;
                viewRun.Height = 48;
                viewRun.TxDate.Text = Car.RunNb.ToString() + " tours";
                viewRun.TxTime.Text = Car.Stat.ToString();

                if (!this.ResearchOnly)
                    if (IsRightCar(Car, ResearchQuery))
                        viewRun.Background = StreetLourdColor.ResearchColor;

                frame.Content = viewRun;
                this.viewStat.List.Items.Add(frame);
            }
        }

        private void CarStatMerge(CarStat InList, CarStat ToAdd)
        {
            InList.RunNb += ToAdd.RunNb;
            InList.Stat = ((InList.Stat * InList.MapNb) + ToAdd.Stat) / (InList.MapNb + 1);
            InList.MapNb++;
        }

        private bool IsRightCar(CarStat Car, string Query)
        {
            if (Query.Replace(" ", "").Length == 0)
                return false;
            string[] queries = Query.ToUpper().Split(" ");
            foreach (string query in queries)
            {
                if (Car.Company.ToUpper().Contains(query))
                    return true;
                if (Car.Name.ToUpper().Contains(query))
                    return true;
            }
            return false;
        }
    }
}
