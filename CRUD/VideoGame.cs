using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD
{
    [Flags]
    public enum Type { Software = 0, SinglePlayer = 1, MultiPlayer = 2, Coop = 4, MMO = 8, Action = 16, Aventure = 32, RolePlayGame = 64, Management = 128, Strategy = 256, Simulation = 512, Race = 1024 };
    public struct VideoGame
    {
        private static int lastId = 0;
        private int _id;
        private int _year;
        private string _name;
        private Type _types;
        private string _studio;

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }
        public int Year
        {
            get { return _year; }
            set { _year = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public Type Types
        {
            get { return _types; }
            set { _types = value; }
        }
        public string Studio
        {
            get { return _studio; }
            set { _studio = value; }
        }

        public VideoGame(string name, string studio, int types, int year)
        {
            ID = AutoGenerateId();
            Year = year;
            Name = name;
            Types = (Type)types;
            Studio = studio;

        }
        public override string ToString()
        {
            return $"| {ID} | {Year} | ";
        }
        private static int AutoGenerateId()
        {
            return Interlocked.Increment(ref lastId);
        }
    }
}
