using System;
using System.Collections.Generic;
using System.Text;
using StreetLourd.Model.Schema;

namespace StreetLourd.Model
{
    class ModelMap
    {
        private Map Data { get; set; }
        private SchemaFile schemaFile { get; set; } = new SchemaFile();

        public string Name { get => this.Data.Name; }
        public string Type { get => this.Data.Type; }
        public string DistanceTx { get
            {
                if (this.Data.Km > 0)
                    return this.Data.Km.ToString() + " km";
                return "";
            }
        }

        public float Distance { get => this.Data.Km; }

        public ModelMap(Map data = null)
        {
            if (data == null)
                data = new Map();
            this.Data = data;
            this.schemaFile.Write(this.Data);
        }

        public ModelMap(string Name, string Type, float Km)
        {
            this.Data = new Map();
            this.Data.FileName = Name + ".json";
            this.Data.Name = Name;
            this.Data.Type = Type;
            this.Data.Km = Km;
            this.schemaFile.Write(this.Data);
        }
        public List<Car> Cars(string Filter = "", string Sort = "")
        {
            List<Car> Returner = new List<Car>();

            switch (Filter)
            {
                case "X":
                case "S2":
                case "S1":
                case "A":
                case "B":
                case "C":
                case "D":
                    Returner = this.ClassCars(Filter);
                    break;
                case "Supprimées":
                    foreach (Car car in this.Data.Cars)
                    {
                        if (!car.Enabled)
                            Returner.Add(car);
                    }
                    break;
                case "Toutes":
                    foreach (Car car in this.Data.Cars)
                    {
                        if (car.Enabled)
                            Returner.Add(car);
                    }
                    break;
                default:
                    Returner = this.Data.Cars;
                    break;
            }

            switch (Sort)
            {
                case "Temps":
                    Dictionary<Car, double> TimePreCalculate = new Dictionary<Car, double>();
                    foreach(Car x in Returner)
                    {
                        TimePreCalculate.Add(x, new ModelCar(x).BestTimeNum());
                    }
                    Returner.Sort((x, y) => TimePreCalculate[x].CompareTo(TimePreCalculate[y]));
                    break;
                case "Marque":
                    Returner.Sort((x, y) => x.Company.CompareTo(y.Company));
                    break;
                case "Nombre d'essais":
                    Returner.Sort((y, x) => x.Runs.Count.CompareTo(y.Runs.Count));
                    break;
                case "Dernier essais":
                    Dictionary<Car, long> TryPreCalculate = new Dictionary<Car, long>();
                    foreach (Car x in Returner)
                    {
                        TryPreCalculate.Add(x, new ModelCar(x).LastTimeTicks());
                    }
                    Returner.Sort((y, x) => TryPreCalculate[x].CompareTo(TryPreCalculate[y]));
                    break;
                default:
                    break;
            }
            return Returner;
        }

        public int AddCar(Car SchemaCar)
        {
            this.Data.Cars.Add(SchemaCar);
            this.schemaFile.Write(this.Data);
            return this.Data.Cars.IndexOf(SchemaCar);
        }
        public int AddCar(string Company, string Name, string Classe, int Score)
        {
            ModelCar car = new ModelCar(Company, Name, Classe, Score);
            this.Data.Cars.Add(car.Data);
            this.schemaFile.Write(this.Data);
            return this.Data.Cars.IndexOf(car.Data);
        }

        public void RemoveCar(int id)
        {
            this.Data.Cars[id].Enabled = false;
            this.schemaFile.Write(this.Data);
        }

        public void ChangeMap(string Name, string Type, float Km)
        {
            this.Data.Name = Name;
            this.Data.Type = Type;
            this.Data.Km = Km;
            if (this.Data.FileName != Name + ".json")
                this.schemaFile.Delete(this.Data.FileName);
            this.Data.FileName = Name + ".json";
            this.schemaFile.Write(this.Data);
        }

        private List<Car> ClassCars(string Class)
        {
            List<Car> Returner = new List<Car>();
            foreach (Car car in this.Data.Cars)
            {
                if (car.Classe == Class && car.Enabled)
                    Returner.Add(car);
            }
            return Returner;
        }

        public void DeleteMap()
        {
            schemaFile.Archive(Data);
        }

        public void Save()
        {
            this.schemaFile.Write(this.Data);
        }

        public string TotalTime(List<Car> List = null)
        {
            double TotalTime = 0;
            if (List == null)
                List = this.Cars("Toutes");
            foreach (Car car in List)
                foreach (Run run in car.Runs)
                    TotalTime += run.Time;

            string Hours = ((int)(TotalTime / 3600)).ToString();

            string Minutes = ((int)((TotalTime % 3600)) / 60).ToString();
            if (Minutes.Length < 2)
                Minutes = "0" + Minutes;

            string Seconds = ((int)(TotalTime % 60)).ToString();
            if (Seconds.Length < 2)
                Seconds = "0" + Seconds;

            return Hours + ":" + Minutes + ":" + Seconds;
        }
    }
}
