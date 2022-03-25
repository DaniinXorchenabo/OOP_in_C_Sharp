using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace lab_3
{
    public partial class Form1 : Form
    {
        public int autoInsertCount
        {
            get => _autoInsertCount;
            set => _autoInsertCount = value;
        }

        private List<TelephoneStation> _telephoneStationList = new List<TelephoneStation>() { };
        private TelephoneStation[] _telephoneStationsArray;
        private int _autoInsertCount = 100_0000;

        public Form1()
        {
            _telephoneStationsArray = new TelephoneStation[autoInsertCount];
            InitializeComponent();
            deleteButtonObj.Enabled = false;
            deleteButtonObj.Visible = false;
        }

        private void testingButton_Click(object sender, EventArgs e)
        {
            var res = GetBenchmarkResults();
            var uiObjects = new List<List<Label>>()
            {
                new List<Label>(){arrayAddLable, listAddLable},
                new List<Label>(){arrayCInsertLable, listCInsertLable},
                new List<Label>(){arrayRInsertLable, listRInsertLable},
                new List<Label>(){arrayDeleteLable, listDeleteLable}
                
            };
            foreach (var coll in uiObjects.Select((value, index) => new {value, index}))
            {
                foreach (var obj in coll.value.Select((value, index) => new {value, index}))
                {
                    obj.value.Text = res[coll.index][obj.index];
                }
            }

        }

        private void createButton_Click(object sender, EventArgs e)
        {
            createButtonObj.Enabled = false;
            deleteButtonObj.Enabled = true;
            CreateItems();
        }


        private void deleteButton_Click(object sender, EventArgs e)
        {
            createButtonObj.Enabled = true;
            deleteButtonObj.Enabled = false;
            DeleteItems();
        }

        private List<List<string>> GetBenchmarkResults()
        {
            return new List<List<string>>()
            {
                CreateItems(), ConsistentlySelectItems(),
                RandomlySelectItems(), DeleteItems()
            };
        }

        private List<string> CreateItems()
        {
            var startTime1 = System.Diagnostics.Stopwatch.StartNew();
            for (var i = 0; autoInsertCount > i; i++)
            {
                var newTStation = new TelephoneStation();
                _telephoneStationsArray[i] = newTStation;
            }

            startTime1.Stop();

            var startTime2 = System.Diagnostics.Stopwatch.StartNew();
            for (var i = 0; autoInsertCount > i; i++)
            {
                var newTStation = new TelephoneStation();
                _telephoneStationList.Add(newTStation);
            }

            startTime2.Stop();

            for (var i = 0; autoInsertCount > i; i++)
            {
                var newTStation = _telephoneStationList[i];
                _telephoneStationsArray[i] = newTStation;
            }

            var resultTime1 = startTime1.Elapsed;
            var resultTime2 = startTime2.Elapsed;
            string elapsedTime1 = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
                resultTime1.Hours,
                resultTime1.Minutes,
                resultTime1.Seconds,
                resultTime1.Milliseconds);
            string elapsedTime2 = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
                resultTime2.Hours,
                resultTime2.Minutes,
                resultTime2.Seconds,
                resultTime2.Milliseconds);

            return new List<string> {elapsedTime1, elapsedTime2};
        }

        private List<string> DeleteItems()
        {
            var startTime1 = System.Diagnostics.Stopwatch.StartNew();
            for (var i = autoInsertCount - 1; 0 <= i; i--)
            {
                _telephoneStationsArray[i] = null;
            }

            startTime1.Stop();

            var startTime2 = System.Diagnostics.Stopwatch.StartNew();
            for (var i = autoInsertCount - 1; 0 <= i; i--)
            {
                _telephoneStationList.RemoveAt(i);
            }


            startTime2.Stop();

            var resultTime1 = startTime1.Elapsed;
            var resultTime2 = startTime2.Elapsed;
            string elapsedTime1 = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
                resultTime1.Hours,
                resultTime1.Minutes,
                resultTime1.Seconds,
                resultTime1.Milliseconds);
            string elapsedTime2 = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
                resultTime2.Hours,
                resultTime2.Minutes,
                resultTime2.Seconds,
                resultTime2.Milliseconds);

            return new List<string> {elapsedTime1, elapsedTime2};
        }

        private List<string> ConsistentlySelectItems()
        {
            var startTime1 = System.Diagnostics.Stopwatch.StartNew();
            for (var i = autoInsertCount - 1; 0 <= i; i--)
            {
                var el = _telephoneStationsArray[i];
            }

            startTime1.Stop();

            var startTime2 = System.Diagnostics.Stopwatch.StartNew();
            foreach (var tStation in _telephoneStationList)
            {
                var el = tStation;
            }

            startTime2.Stop();

            var resultTime1 = startTime1.Elapsed;
            var resultTime2 = startTime2.Elapsed;
            string elapsedTime1 = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
                resultTime1.Hours,
                resultTime1.Minutes,
                resultTime1.Seconds,
                resultTime1.Milliseconds);
            string elapsedTime2 = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
                resultTime2.Hours,
                resultTime2.Minutes,
                resultTime2.Seconds,
                resultTime2.Milliseconds);

            return new List<string> {elapsedTime1, elapsedTime2};
        }

        private List<string> RandomlySelectItems()
        {
            var rand = new Random();
            var randomIndexes = new List<int>() { };
            for (var i = autoInsertCount - 1; 0 <= i; i--)
            {
                randomIndexes.Add(rand.Next(0, autoInsertCount - 1));
            }

            var startTime1 = System.Diagnostics.Stopwatch.StartNew();

            foreach (var i in randomIndexes)
            {
                var el = _telephoneStationsArray[i];
            }

            startTime1.Stop();

            var startTime2 = System.Diagnostics.Stopwatch.StartNew();
            foreach (var i in randomIndexes)
            {
                var el = _telephoneStationList[i];
            }

            startTime2.Stop();

            var resultTime1 = startTime1.Elapsed;
            var resultTime2 = startTime2.Elapsed;
            string elapsedTime1 = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
                resultTime1.Hours,
                resultTime1.Minutes,
                resultTime1.Seconds,
                resultTime1.Milliseconds);
            string elapsedTime2 = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
                resultTime2.Hours,
                resultTime2.Minutes,
                resultTime2.Seconds,
                resultTime2.Milliseconds);

            return new List<string> {elapsedTime1, elapsedTime2};
        }

        private void Form1_Resize(object sender, System.EventArgs e)
        {
            //     Control control = (Control)sender;
            //
            //     // Ensure the Form remains square (Height = Width).
            //     if(control.Size.Height < 185 && control.Size.Width < 470)
            //     {
            //         Console.WriteLine($"{control.Size.Width}, {control.Size.Height}");
            //         control.Size = new Size(control.Size.Width, control.Size.Width);
            //         FormBorderStyle = FormBorderStyle.Fixed3D;
            //     }
            //     else
            //     {
            //         FormBorderStyle = FormBorderStyle.Sizable;
            //     }
        }
        
    }
}