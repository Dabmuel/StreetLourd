using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using StreetLourd.View;
using StreetLourd.Model;

namespace StreetLourd.Controller
{
    class ControllerMap
    {
        public ViewMapButton mapButton { get; set; } = new ViewMapButton();
        public string Type { get => this.Map.Type; }
        public bool Checked { get => this.mapButton.Check.IsChecked.Value; }

        private bool ResearchOnly { get => this.viewMap.ChBxOnlyResearch.IsChecked.Value; set => this.viewMap.ChBxOnlyResearch.IsChecked = value; }

        private ViewMap viewMap { get; set; }
        private ViewAddCar viewAddCar { get; set; }
        private ViewAddRun viewAddRun { get; set; }
        private ViewSure viewSure { get; set; }
        private ViewAddMap viewChangeMap { get; set; }

        private ModelMap Map { get; set; }
        public ModelMap GetMap { get => this.Map; }
        private ControllerMain controllerMain { get; set; }

        public ControllerMap(ControllerMain controllerMain, string Name, string Type, float Km = 0)
        {
            this.Map = new ModelMap(Name, Type, Km);
            this.init(controllerMain);
        }

        public ControllerMap(ControllerMain controllerMain, Model.Schema.Map SchemaMap)
        {
            this.Map = new ModelMap(SchemaMap);
            this.init(controllerMain);
        }

        private void init(ControllerMain controllerMain)
        {
            this.controllerMain = controllerMain;
            this.initMapButton();
            this.mapButton.Bt.Click += this.OpenWindow;
        }

        private void initMapButton()
        {
            List<Model.Schema.Car> TotalCarList = this.Map.Cars("Toutes");
            int TotalRunCount = 0;
            foreach (Model.Schema.Car Car in TotalCarList)
                TotalRunCount += Car.Runs.Count;

            this.mapButton.Width = 690;
            this.mapButton.Check.IsChecked = true;
            this.mapButton.Bt.Content = this.Map.Name;
            this.mapButton.TxKm.Text = this.Map.DistanceTx;
            this.mapButton.TxType.Text = this.Map.Type;
            this.mapButton.TxType.Foreground = StreetLourdColor.RaceColor(this.Map.Type);
            this.mapButton.TxTries.Text = TotalRunCount.ToString() + " enregistrements";
        }

        public void OpenWindow(object a, object b)
        {
            if (this.viewMap != null)
            {
                this.controllerMain.SetPage(viewMap);
                return;
            }
            this.viewMap = new ViewMap();
            this.viewMap.Title = this.Map.Name;
            this.viewMap.TxName.Text = this.Map.Name;
            this.viewMap.TxType.Text = this.Map.Type;
            this.viewMap.TxKm.Text = this.Map.DistanceTx;
            this.viewMap.ImgBg.ImageSource = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/StreetLourd;component/view/" + this.Map.Type.ToLower() + ".jpg"));
            this.viewMap.Visibility = System.Windows.Visibility.Visible;

            this.viewMap.BtCloseMap.Click += this.CloseWindow;
            this.viewMap.BtAddCar.Click += this.OpenAddCarWindow;
            this.viewMap.BtAddRun.Click += this.OpenAddRunWindow;
            this.viewMap.BtDeletCar.Click += this.OpenCarSureWindow;
            this.viewMap.BtChangeMap.Click += this.OpenChangeMapWindow;
            this.viewMap.BtDeleteMap.Click += this.OpenMapSureWindow;
            this.viewMap.BtChangeCar.Click += this.OpenChangeCarWindow;
            this.viewMap.CbFilter.SelectionChanged += this.RefreshCar;
            this.viewMap.CbSort.SelectionChanged += this.RefreshCar;
            this.viewMap.TxBxResearch.TextChanged += this.RefreshCar;
            this.viewMap.ChBxOnlyResearch.Click += this.RefreshCar;

            this.controllerMain.AddMapPage(viewMap, this);
            this.RefreshCar();
        }

        public void CloseWindow(object a = null, object b = null)
        {
            this.controllerMain.ClosePage(viewMap);
            this.viewMap = null;
        }

        public void OpenAddCarWindow(object a, object b)
        {
            if (this.viewAddCar != null)
                if (this.viewAddCar.IsLoaded)
                    return;
            this.viewAddCar = new ViewAddCar();
            this.viewAddCar.BtAdd.Click += this.AddCar;
            this.viewAddCar.Visibility = System.Windows.Visibility.Visible;
        }

        public void AddCar(object a, object b)
        {
            if (this.viewAddCar.TxCompany.Text == "" || this.viewAddCar.TxName.Text == "")
                return;
            int Score;
            try
            {
                Score = int.Parse(this.viewAddCar.TxScore.Text);
            }
            catch(Exception e)
            {
                return;
            }
            if (Score > 999 || Score < 100)
                return;
            string Classe;
            if (Score > 998)
                Classe = "X";
            else if (Score > 900)
                Classe = "S2";
            else if (Score > 800)
                Classe = "S1";
            else if (Score > 700)
                Classe = "A";
            else if (Score > 600)
                Classe = "B";
            else if (Score > 500)
                Classe = "C";
            else
                Classe = "D";

            int newCarId = this.Map.AddCar(this.viewAddCar.TxCompany.Text, this.viewAddCar.TxName.Text, Classe, Score);
            this.viewAddCar.Close();
            this.RefreshCar();
            this.OpenAddRunWindow(newCarId);
        }

        public void RefreshCar(object a, object b)
        {
            this.RefreshCar();
        }
        private void RefreshCar()
        {
            this.viewMap.List.Items.Clear();

            string Filter = ((ComboBoxItem)this.viewMap.CbFilter.SelectedItem).Content.ToString();
            string Sort = ((ComboBoxItem)this.viewMap.CbSort.SelectedItem).Content.ToString();
            string ResearchQuery = this.viewMap.TxBxResearch.Text;

            List<Model.Schema.Car> CarList = this.Map.Cars(Filter, Sort);
            List<Model.Schema.Car> TotalCarList = this.Map.Cars("Toutes");
            int TotalRunCount = 0;
            foreach (Model.Schema.Car Car in TotalCarList)
                TotalRunCount += Car.Runs.Count;

            this.viewMap.TxShowedCars.Text = CarList.Count + " voitures affichées";
            this.viewMap.TxTotalCars.Text = TotalCarList.Count + " voitures";
            this.viewMap.TxTotalRun.Text = TotalRunCount + " tours";
            this.viewMap.TxTotalTime.Text = this.Map.TotalTime(TotalCarList);

            this.mapButton.TxTries.Text = TotalRunCount.ToString() + " enregistrements";

            foreach (Model.Schema.Car Car in CarList)
            {
                ModelCar modelCar = new ModelCar(Car);

                if (this.ResearchOnly)
                    if (!IsRightCar(modelCar, ResearchQuery))
                        continue;

                ViewRun viewRun = new ViewRun();
                Frame frame = new Frame();

                frame.HorizontalAlignment = HorizontalAlignment.Left;
                frame.VerticalAlignment = VerticalAlignment.Top;
                frame.Margin = new Thickness(0, 0, 0, 0);

                viewRun.TxName.Text = modelCar.Company + " " + modelCar.Name;
                viewRun.TxClasse.Text = " " + modelCar.Classe + " " + modelCar.Score + " ";
                viewRun.TxClasse.Background = StreetLourdColor.ClassColor(modelCar.Classe);
                viewRun.TxNb.Text = modelCar.Count;
                viewRun.TxId.Text = this.Map.Cars().FindIndex(x => x == Car).ToString(); ;
                if(viewMap.IsLoaded)
                    viewRun.Width = this.viewMap.List.ActualWidth - 30;
                else
                    viewRun.Width = this.viewMap.List.Width - 30;
                viewRun.Height = 48;

                if (!this.ResearchOnly)
                    if (IsRightCar(modelCar, ResearchQuery))
                        viewRun.Background = StreetLourdColor.ResearchColor;

                switch (Sort)
                {
                    case "Dernier essais":
                        viewRun.TxDate.Text = modelCar.LastTimeDate();
                        viewRun.TxTime.Text = modelCar.LastTime();
                        break;
                    default:
                        viewRun.TxDate.Text = modelCar.BestTimeDate();
                        viewRun.TxTime.Text = modelCar.BestTime();
                        break;
                }

                frame.Content = viewRun;
                this.viewMap.List.Items.Add(frame);
            }
        }

        private bool IsRightCar(ModelCar Car, string Query)
        {
            if (Query.Replace(" ", "").Length == 0)
                return false;
            string[] queries = Query.ToUpper().Split(" ");
            foreach(string query in queries)
            {
                if (Car.Company.ToUpper().Contains(query))
                    return true;
                if (Car.Name.ToUpper().Contains(query))
                    return true;
            }
            return false;
        }

        private void OpenAddRunWindow(int Id)
        {
            if (this.viewAddRun != null)
                if (this.viewAddRun.IsLoaded)
                    return; 
            ModelCar Car = new ModelCar(this.Map.Cars()[Id]);
            this.viewAddRun = new ViewAddRun();
            this.viewAddRun.TxId.Text = Id.ToString();
            this.viewAddRun.TxMap.Text = this.Map.Name;
            this.viewAddRun.TxCar.Text = Car.Company + " " + Car.Name;
            this.viewAddRun.BtAdd.Click += this.AddRun;
            this.viewAddRun.Visibility = Visibility.Visible;
        }

        public void OpenAddRunWindow(object a, object b)
        {
            if (this.viewAddRun != null)
                if (this.viewAddRun.IsLoaded)
                    return;
            if (this.viewMap.List.SelectedItem == null)
                return;
            int Id = int.Parse(((ViewRun)((Frame)this.viewMap.List.SelectedItem).Content).TxId.Text);
            ModelCar Car = new ModelCar(this.Map.Cars()[Id]);
            this.viewAddRun = new ViewAddRun();
            this.viewAddRun.TxId.Text = Id.ToString();
            this.viewAddRun.TxMap.Text = this.Map.Name;
            this.viewAddRun.TxCar.Text = Car.Company + " " + Car.Name;
            this.viewAddRun.BtAdd.Click += this.AddRun;
            this.viewAddRun.Visibility = Visibility.Visible;
        }

        public void AddRun(object a, object b)
        {
            int Id = int.Parse(this.viewAddRun.TxId.Text);
            ModelCar Car = new ModelCar(this.Map.Cars()[Id]);
            if (Car.NewRun(this.viewAddRun.TxTime.Text))
            {
                this.viewAddRun.Close();
                this.RefreshCar();
                this.Map.Save();
            }
        }

        public void OpenCarSureWindow(object a, object b)
        {
            if (this.viewSure != null)
                if (this.viewSure.IsLoaded)
                    return;
            if (this.viewMap.List.SelectedItem == null)
                return;
            int Id = int.Parse(((ViewRun)((Frame)this.viewMap.List.SelectedItem).Content).TxId.Text);
            this.viewSure = new ViewSure();
            this.viewSure.Data.Text = Id.ToString();
            this.viewSure.BtYes.Click += this.DeleteCar;
            this.viewSure.Visibility = Visibility.Visible;
        }

        public void DeleteCar(object a, object b)
        {
            int Id = int.Parse(this.viewSure.Data.Text);
            this.Map.RemoveCar(Id);
            this.viewSure.Close();
            this.RefreshCar();
        }

        public void OpenChangeMapWindow(object a, object b)
        {
            if (this.viewChangeMap != null)
                if (this.viewChangeMap.IsLoaded)
                    return;
            this.viewChangeMap = new ViewAddMap();
            this.viewChangeMap.Title = "Modifier la course";
            this.viewChangeMap.BtAdd.Content = "Modifier";
            this.viewChangeMap.TxName.Text = this.Map.Name;
            this.viewChangeMap.TxKm.Text = this.Map.Distance.ToString();
            foreach(ComboBoxItem item in this.viewChangeMap.CbType.Items)
            {
                if ((string)item.Content == this.Map.Type)
                    this.viewChangeMap.CbType.SelectedItem = item;
            }
            this.viewChangeMap.BtAdd.Click += this.ChangeMap;
            this.viewChangeMap.Visibility = Visibility.Visible;
        }

        public void ChangeMap(object a, object b)
        {
            float Km;
            try
            {
                Km = float.Parse(this.viewChangeMap.TxKm.Text.Replace(".", ","));
            }
            catch (Exception e)
            {
                Km = 0;
            }
            this.Map.ChangeMap(this.viewChangeMap.TxName.Text, this.viewChangeMap.CbType.Text, Km);
            this.viewChangeMap.Close();
            this.viewMap.Title = this.Map.Name;
            this.viewMap.TxName.Text = this.Map.Name;
            this.viewMap.TxType.Text = this.Map.Type;
            this.viewMap.TxKm.Text = this.Map.DistanceTx;
            this.viewMap.ImgBg.ImageSource = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/StreetLourd;component/view/" + this.Map.Type.ToLower() + ".jpg"));
            this.initMapButton();
        }

        public void OpenMapSureWindow(object a, object b)
        {
            if (this.viewSure != null)
                if (this.viewSure.IsLoaded)
                    return;
            this.viewSure = new ViewSure();
            this.viewSure.BtYes.Click += this.DeleteMap;
            this.viewSure.Visibility = Visibility.Visible;
        }

        public void DeleteMap(object a, object b)
        {
            this.viewSure.Close();
            if (this.viewChangeMap != null)
                if (this.viewChangeMap.IsLoaded)
                    viewChangeMap.Close();
            if (this.viewAddRun != null)
                if (this.viewAddRun.IsLoaded)
                    viewAddRun.Close();
            if (this.viewAddCar != null)
                if (this.viewAddCar.IsLoaded)
                    viewAddCar.Close();
            this.controllerMain.DeleteControllerMap(this);
            this.Map.DeleteMap();
            this.CloseWindow();
        }

        public void OpenChangeCarWindow(object a, object b)
        {
            if (this.viewAddCar != null)
                if (this.viewAddCar.IsLoaded)
                    return;
            if (this.viewMap.List.SelectedItem == null)
                return;
            int Id = int.Parse(((ViewRun)((Frame)this.viewMap.List.SelectedItem).Content).TxId.Text);
            ModelCar Car = new ModelCar(this.Map.Cars()[Id]);
            this.viewAddCar = new ViewAddCar();
            this.viewAddCar.Title = "Modifier une voiture";
            this.viewAddCar.BtAdd.Content = "Modifier";
            this.viewAddCar.TxId.Text = Id.ToString();
            this.viewAddCar.TxCompany.Text = Car.Company;
            this.viewAddCar.TxName.Text = Car.Name;
            this.viewAddCar.TxScore.Text = Car.Score;
            this.viewAddCar.BtAdd.Click += this.ChangeCar;
            this.viewAddCar.Visibility = System.Windows.Visibility.Visible;
        }

        public void ChangeCar(object a, object b)
        {
            if (this.viewAddCar.TxCompany.Text == "" || this.viewAddCar.TxName.Text == "")
                return;
            int Score;
            try
            {
                Score = int.Parse(this.viewAddCar.TxScore.Text);
            }
            catch (Exception e)
            {
                return;
            }
            if (Score > 999 || Score < 100)
                return;
            string Classe;
            if (Score > 998)
                Classe = "X";
            else if (Score > 900)
                Classe = "S2";
            else if (Score > 800)
                Classe = "S1";
            else if (Score > 700)
                Classe = "A";
            else if (Score > 600)
                Classe = "B";
            else if (Score > 500)
                Classe = "C";
            else
                Classe = "D";
            int Id = int.Parse(this.viewAddCar.TxId.Text);
            ModelCar Car = new ModelCar(this.Map.Cars()[Id]);
            Car.ChangeCar(this.viewAddCar.TxCompany.Text, this.viewAddCar.TxName.Text, Classe, Score);
            this.viewAddCar.Close();
            this.RefreshCar();
            this.Map.Save();
        }
    }
}
