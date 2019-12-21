using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    class Diary
    {
        private bool[,] _dates; // one dimension array that present 2 dimension matrix;
        const int _month = 12, _days = 31;

        /// <summary>
        /// default c'tor
        /// </summary>
        public Diary()
        {
            _dates = new bool[_month, _days];//Create an Calendar annual
        }

        public void GetHospitality() //selection Invitation 
        {
            Console.Write("enter month: ");
            int month = Convert.ToInt32(Console.ReadLine());
            Console.Write("enter day: ");
            int day = Convert.ToInt32(Console.ReadLine());
            Console.Write("enter number of vacation days: ");
            int countOfDays = Convert.ToInt32(Console.ReadLine());

            if (countOfDays >= 365 || AddHospitality(day - 1, month - 1, countOfDays - 1))
                Console.WriteLine("\nApplication Approved");
            else
                Console.WriteLine("\nApplication Denied");
        }

        public bool AddHospitality(int day, int month, int countOfNights) //Invitation Check
        {
            bool[,] d = _dates;
            if (d[month, day] == false || (d[month, day] == true && d[month, day + 1] == false))
            {
                for (int i = 0; i < countOfNights; i++, day++)
                {
                    if (day == _days) // check if we in end of month
                    {
                        // past to next month
                        day = 0;
                        month++;
                        if (month == _month) // check if we in end of year
                            month = 0;
                    }
                    // check if exists busy day 
                    if (d[month, day] == true && (i != 0 || i == countOfNights - 1))
                        return false;
                    else
                        d[month, day] = true;
                }

                _dates = d; // update the data structer
                return true;
            }

            return false;
        }

        public void PrintBusyDays()
        {
            Console.Write(ToString());
        }

        public override string ToString()
        {
            string toString = "";
            bool count = false;
            for (int month = 0; month < _month; month++) //run through months
            {
                for (int day = 0; day < _days; day++)   //run through days
                {
                    if (_dates[month, day] == true && count == false) //if yesterday not Busy and today Busy
                    {
                        count = true;
                        toString += (day + 1) + "." + (month + 1) + " - ";
                    }

                    if (_dates[month, day] == false && count == true) //if today not Busy and yesterday Busy 
                    {
                        count = false;
                        if (day == 0) // Hosting end on the last day of month
                            toString += _days + "." + month + "\n";
                        else
                            toString += _days + "." + (month + 1) + "\n";
                    }

                    if (month + 1 == _month && day + 1 == _days && _dates[month, day] == true)// last day of year
                    {
                        count = false;
                        toString += _days + "." + _month + "\n";
                    }
                }
            }
            return toString;
        }


        public void PrintCountOfTaken()// amount of days busy 
        {
            double count = 0;//count the day
            for (int i = 0; i < _month; i++)//run about month
                for (int j = 0; j < _days; j++)//run about days
                    if (_dates[i, j])
                        count++;

            Console.WriteLine("number of taken days: {0}", count);
            Console.WriteLine("Percent of taken days: {0}", count / 365.0);
        }

        public int GetCountOfTaken()// amount of days busy 
        {
            int count = 0;//count the day
            for (int i = 0; i < _month; i++)//run about month
                for (int j = 0; j < _days; j++)//run about days
                    if (_dates[i, j])
                        count++;

            return count;
        }

        public double GetCountOfTakennPercent()// Percent of days busy 
        {
            double count = 0;//count the day
            for (int i = 0; i < _month; i++)//run about month
                for (int j = 0; j < _days; j++)//run about days
                    if (_dates[i, j])
                        count++;

            return count / 365.0;
        }
    }
}