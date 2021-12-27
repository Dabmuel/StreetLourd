using System;
using System.Collections.Generic;
using System.Text;
using StreetLourd.Model.Schema;

namespace StreetLourd.Model
{
    class ModelMapStat
    {
        private List<CarStat> Cars { get; set; } = new List<CarStat>();
        public List<CarStat> GetCars { get => this.Cars; }

        public ModelMapStat(ModelMap mapModel, string Filter = "Toutes")
        {
            List<Car> carList = mapModel.Cars(Filter);
            Dictionary<Car, double> carBTimes = new Dictionary<Car, double>();
            double BTime = 0;
            foreach (Car car in carList)
            {
                double time = new ModelCar(car).BestTimeNum();
                if(time > 0)
                    carBTimes.Add(car, time);
            }
            foreach(KeyValuePair<Car, double> time in carBTimes)
            {
                if (BTime == 0)
                    BTime = time.Value;
                else if(BTime > time.Value)
                    BTime = time.Value;
            }
            foreach(KeyValuePair<Car, double> car in carBTimes)
            {
                CarStat tmp = new CarStat();
                tmp.Company = car.Key.Company;
                tmp.Name = car.Key.Name;
                tmp.Classe = car.Key.Classe;
                tmp.Score = car.Key.Score;
                tmp.Stat = (int)((1 / (Math.Exp((10 / BTime) * (car.Value - BTime)))) * 100);
                //tmp.Stat = (int)((2 - (car.Value/BTime)) * 100);
                //tmp.Stat = (int)((BTime / car.Value) * 100);
                tmp.RunNb = car.Key.Runs.Count;
                this.Cars.Add(tmp);
            }
        }
    }
}
