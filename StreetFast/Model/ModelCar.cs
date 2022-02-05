using System;
using System.Collections.Generic;
using System.Text;
using StreetLourd.Model.Schema;

namespace StreetLourd.Model
{
    class ModelCar
    {
        public Car Data { get; set; }

        public string Company { get => this.Data.Company; }
        public string Name { get => this.Data.Name; }
        public string Classe { get => this.Data.Classe; }
        public string Score { get => this.Data.Score.ToString(); }
        public string Count { get => this.Data.Runs.Count.ToString() + " Essais"; }

        private int BestTimeIndex = -1;
        private int LastTimeIndex = -1;

        public ModelCar(Car data = null)
        {
            if (data == null)
                data = new Car();
            this.Data = data;
        }

        public ModelCar(string Company, string Name, string Classe, int Score)
        {
            this.Data = new Car();
            this.Data.Company = Company;
            this.Data.Name = Name;
            this.Data.Classe = Classe;
            this.Data.Score = Score;
        }

        public void ChangeCar(string Company, string Name, string Classe, int Score)
        {
            this.Data.Company = Company;
            this.Data.Name = Name;
            this.Data.Classe = Classe;
            this.Data.Score = Score;
        }

        public double BestTimeNum()
        {
            if (this.Data.Runs.Count == 0)
                return 0;
            if (this.BestTimeIndex < 0)
                this.setBestTimeIndex();
            return this.Data.Runs[this.BestTimeIndex].Time;
        }

        public string BestTime()
        {
            if (this.Data.Runs.Count == 0)
                return "";
            if (this.BestTimeIndex < 0)
                this.setBestTimeIndex();

            double BT = this.Data.Runs[this.BestTimeIndex].Time;
            string Sec = (BT % 60).ToString();
            if (Sec.Split(",")[0].Length == 1)
                Sec = "0" + Sec;
            if (Sec.Length > 6)
                Sec = Sec.Remove(6);
            string Format = ((int)BT / 60).ToString() + ":" + Sec;
            return Format;
        }
        public string BestTimeDate()
        {
            if (this.Data.Runs.Count == 0)
                return "";
            if (this.BestTimeIndex < 0)
                this.setBestTimeIndex();

            DateTime BTD = this.Data.Runs[this.BestTimeIndex].Date;
            return BTD.ToShortDateString();
        }

        private void setBestTimeIndex()
        {
            int newIndex = -1;
            for(int i = 0; i < this.Data.Runs.Count; i++)
            {
                if (newIndex >= 0)
                {
                    if (this.Data.Runs[i].Time < this.Data.Runs[newIndex].Time)
                        newIndex = i;
                }
                else
                    newIndex = i;
            }
            this.BestTimeIndex = newIndex;
        }

        public int NewRun(string Time)
        {
            double NewTime = 0;
            string[] Explode = Time.Split(":");
            int Min;
            double Sec;
            if (Explode.Length != 2)
                return 0;
            if (Explode[1].Length != 6)
                return 0;
            try
            {
                Min = int.Parse(Explode[0]);
                Sec = double.Parse(Explode[1].Replace(".", ","));
            }
            catch(Exception e)
            {
                return 0;
            }
            if (Sec >= 60)
                return 0;

            NewTime = (Min * 60) + Sec;
            Run NewRun = new Run();
            NewRun.Time = NewTime;

            int returner = 1;
            if(this.Data.Runs.Count > 0)
            {
                returner = 2;
                foreach(Run run in this.Data.Runs)
                {
                    if (run.Time < NewTime)
                        returner = 1;
                }
            }

            this.Data.Runs.Add(NewRun);

            return returner;
        }

        public string LastTime()
        {
            if (this.Data.Runs.Count == 0)
                return "";
            if (this.LastTimeIndex < 0)
                this.setLastTimeIndex();

            double BT = this.Data.Runs[this.LastTimeIndex].Time;
            string Sec = (BT % 60).ToString();
            if (Sec.Split(",")[0].Length == 1)
                Sec = "0" + Sec;
            if (Sec.Length > 6)
                Sec = Sec.Remove(6);
            string Format = ((int)BT / 60).ToString() + ":" + Sec;
            return Format;
        }

        public string LastTimeDate()
        {
            if (this.Data.Runs.Count == 0)
                return "";
            if (this.LastTimeIndex < 0)
                this.setLastTimeIndex();

            DateTime BTD = this.Data.Runs[this.LastTimeIndex].Date;
            return BTD.ToShortDateString();
        }

        public long LastTimeTicks()
        {
            if (this.Data.Runs.Count == 0)
                return 0;
            if (this.LastTimeIndex < 0)
                this.setLastTimeIndex();
            return this.Data.Runs[this.LastTimeIndex].Date.Ticks;
        }

        public void setLastTimeIndex()
        {
            int index = -1;
            foreach (Run run in this.Data.Runs)
            {
                if(index < 0)
                    index = this.Data.Runs.IndexOf(run);
                else if (run.Date.Ticks > this.Data.Runs[index].Date.Ticks)
                    index = this.Data.Runs.IndexOf(run);
            }
            this.LastTimeIndex = index;
        }
    }
}
