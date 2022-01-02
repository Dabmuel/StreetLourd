using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Text;
using StreetLourd.View;

namespace StreetLourd.Controller
{
    class ControllerMain
    {
        private MainWindow viewMain { get; set; }
        private ViewAddMap viewAddMap { get; set; }
        private Model.SchemaFile schemaFile { get; set; } = new Model.SchemaFile();
        private List<ControllerMap> Maps { get; set; } = new List<ControllerMap>();

        public ControllerMain(MainWindow viewMain)
        {
            this.viewMain = viewMain;
            this.viewMain.BtAddMap.Click += this.OpenAddWindow;
            this.viewMain.BtStatCircuit.Click += this.OpenStatWindow;
            this.viewMain.BtStatCross.Click += this.OpenStatWindow;
            this.viewMain.BtStatRally.Click += this.OpenStatWindow;

            List<Model.Schema.Map> List = this.schemaFile.ReadAll();
            foreach(Model.Schema.Map map in List)
            {
                this.AddControllerMap(new ControllerMap(this, map));
            }
        }

        private void AddControllerMap(ControllerMap controllerMap)
        {
            this.Maps.Add(controllerMap);
            Frame frame = new Frame();
            frame.Content = controllerMap.mapButton;
            this.viewMain.List.Items.Add(frame);
        }

        public void OpenAddWindow(object a, object b)
        {
            if (this.viewAddMap != null)
                if (this.viewAddMap.IsLoaded)
                    return;
            this.viewAddMap = new ViewAddMap();
            this.viewAddMap.BtAdd.Click += this.AddMap;
            this.viewAddMap.TxName.KeyDown += (a, e) =>
            {
                if (e.Key == System.Windows.Input.Key.Return)
                    this.AddMap(null, null);
            };
            this.viewAddMap.TxKm.KeyDown += (a, e) =>
            {
                if (e.Key == System.Windows.Input.Key.Return)
                    this.AddMap(null, null);
            };
            this.viewAddMap.Visibility = System.Windows.Visibility.Visible;
        }

        public void AddMapPage(ViewMap viewMap, ControllerMap controllerMap)
        {
            Frame frame = new Frame();
            frame.Content = viewMap;
            TabItem tabItem = new TabItem();
            tabItem.Header = controllerMap.GetMap.Name;
            tabItem.Content = frame;
            this.viewMain.Tab.Items.Add(tabItem);
            this.viewMain.Tab.SelectedItem = tabItem;
        }

        public void AddStatPage(ViewStat viewStat, string Type)
        {
            Frame frame = new Frame();
            frame.Content = viewStat;
            TabItem tabItem = new TabItem();
            tabItem.Header = "Rapport " + Type;
            tabItem.Content = frame;
            this.viewMain.Tab.Items.Add(tabItem);
            this.viewMain.Tab.SelectedItem = tabItem;
        }

        public void SetPage(object Page)
        {
            foreach (TabItem tabItem in this.viewMain.Tab.Items)
            {
                if (this.viewMain.Tab.Items.IndexOf(tabItem) == 0)
                    continue;
                if (((Frame)tabItem.Content).Content == Page)
                {
                    this.viewMain.Tab.SelectedItem = tabItem;
                    break;
                }
            }
        }

        public void ClosePage(object Page)
        {
            foreach(TabItem tabItem in this.viewMain.Tab.Items)
            {
                if (this.viewMain.Tab.Items.IndexOf(tabItem) == 0)
                    continue;
                if(((Frame)tabItem.Content).Content == Page)
                {
                    this.viewMain.Tab.Items.Remove(tabItem);
                    break;
                }
            }
        }

        public void OpenStatWindow(object a, object b)
        {
            Button bt = (Button)a;
            string Type = bt.Content.ToString().Split(" ")[1];
            if (Type == "Cross-C")
                Type = "Cross-Country";

            foreach (TabItem tabItem in this.viewMain.Tab.Items)
            {
                if (tabItem.Header.ToString() == "Rapport " + Type)
                {
                    try
                    {
                        ViewStat uhoh = ((ViewStat)((Frame)tabItem.Content).Content);
                        this.viewMain.Tab.SelectedItem = tabItem;
                        return;
                    }
                    catch(InvalidCastException e)
                    {
                        continue;
                    }
                }
            }

            List<StreetLourd.Model.ModelMap> MapsList= new List<StreetLourd.Model.ModelMap>();
            foreach(ControllerMap CtMap in this.Maps)
            {
                if(CtMap.Type == Type && CtMap.Checked)
                    MapsList.Add(CtMap.GetMap);
            }
            new ControllerStat(Type, MapsList, this);
        }

        public void AddMap(object a, object b)
        {
            float Km;
            try
            {
                Km = float.Parse(this.viewAddMap.TxKm.Text.Replace(".", ","));
            }
            catch(Exception e)
            {
                Km = 0;
            }
            ControllerMap controllerMap = new ControllerMap(this, this.viewAddMap.TxName.Text, this.viewAddMap.CbType.Text, Km);
            this.AddControllerMap(controllerMap);
            this.viewAddMap.Close();
        }

        public void DeleteControllerMap(ControllerMap controllerMap)
        {
            this.Maps.Remove(controllerMap);
            Frame thisOne = null;
            foreach (object frame in this.viewMain.List.Items)
            {
                if (((ViewMapButton)((Frame)frame).Content) == controllerMap.mapButton)
                    thisOne = (Frame)frame;
            }
            if (thisOne != null)
                this.viewMain.List.Items.Remove(thisOne);
        }
    }
}
