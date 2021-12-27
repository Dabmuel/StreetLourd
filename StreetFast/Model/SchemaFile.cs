using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using StreetLourd.Model.Schema;

namespace StreetLourd.Model
{
    class SchemaFile
    {
        public void Write(Map schema)
        {
            string JSON = JsonSerializer.Serialize(schema);
            if (!Directory.Exists("Save\\"))
                Directory.CreateDirectory("Save\\");
            File.WriteAllText("Save\\" + schema.FileName , JSON);
        }

        public void Archive(Map schema)
        {
            string JSON = JsonSerializer.Serialize(schema);
            if (!Directory.Exists("Save\\"))
                Directory.CreateDirectory("Save\\");
            if (!Directory.Exists("Archive\\"))
                Directory.CreateDirectory("Archive\\");
            if (File.Exists("Save\\" + schema.FileName))
                this.Delete(schema.FileName);
            File.WriteAllText("Archive\\" + schema.FileName, JSON);
        }

        public void Delete(string Filename)
        {
            File.Delete("Save\\" + Filename);
        }

        public Map Read(string name)
        {
            if (!Directory.Exists("Save\\"))
                Directory.CreateDirectory("Save\\");
            string Content = File.ReadAllText("Save\\" + name);
            try
            {
                return JsonSerializer.Deserialize<Map>(Content);
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public List<Map> ReadAll()
        {
            List<Map> Returner = new List<Map>();

            if (!Directory.Exists("Save\\"))
                Directory.CreateDirectory("Save\\");
            string[] files = Directory.GetFiles("Save\\");

            foreach(string file in files)
            {
                Map schema = this.Read(file.Split("\\")[file.Split("\\").Length -1]);
                if (schema != null)
                    Returner.Add(schema);
            }

            return Returner;
        }
    }
}
