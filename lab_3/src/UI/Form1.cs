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
        public int AutoInsertCount { get; set; } = 100_0;

        private readonly List<TelephoneStation> _telephoneStationList = new List<TelephoneStation>() { };
        private readonly List<TelephoneStation> _telephoneStationList2 = new List<TelephoneStation>() { };

        private readonly TelephoneStation[] _telephoneStationsArray;

        public Form1()
        {
            _telephoneStationsArray = new TelephoneStation[AutoInsertCount];
            InitializeComponent();
            deleteButtonObj.Enabled = false;
            deleteButtonObj.Visible = false;
        }

        private void testingButton_Click(object sender, EventArgs e)
        {
            try
            {
                var res = GetBenchmarkResults();
                var uiObjects = new List<List<Label>>()
                {
                    new() {arrayAddLable, listAddLable},
                    new() {arrayCInsertLable, listCInsertLable},
                    new() {arrayRInsertLable, listRInsertLable},
                    new() {arrayDeleteLable, listDeleteLable}
                };
                foreach (var coll in uiObjects.Select((value, index) => new {value, collRes = res[index]}))
                {
                    foreach (var obj in coll.value.Select((value, index) => new {value, result = coll.collRes[index]}))
                    {
                        obj.value.Text = obj.result;
                    }
                }

                ShowList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Что-то пошло не так...", ex.ToString());
            }
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            try
            {
                createButtonObj.Enabled = false;
                deleteButtonObj.Enabled = true;
                CreateItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Что-то пошло не так...", ex.ToString());
            }
        }


        private void deleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                createButtonObj.Enabled = true;
                deleteButtonObj.Enabled = false;
                DeleteItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Что-то пошло не так...", ex.ToString());
            }
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
            for (var i = 0; AutoInsertCount > i; i++)
            {
                var newTStation = new TelephoneStation();
                _telephoneStationsArray[i] = newTStation;
            }

            startTime1.Stop();

            var startTime2 = System.Diagnostics.Stopwatch.StartNew();
            for (var i = 0; AutoInsertCount > i; i++)
            {
                var newTStation = new TelephoneStation();
                _telephoneStationList.Add(newTStation);
            }

            startTime2.Stop();

            for (var i = 0; AutoInsertCount > i; i++)
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
            for (var i = AutoInsertCount - 1; 0 <= i; i--)
            {
                _telephoneStationsArray[i] = null;
            }

            startTime1.Stop();

            var startTime2 = System.Diagnostics.Stopwatch.StartNew();
            for (var i = AutoInsertCount - 1; 0 <= i; i--)
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
            for (var i = AutoInsertCount - 1; 0 <= i; i--)
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
            for (var i = AutoInsertCount - 1; 0 <= i; i--)
            {
                randomIndexes.Add(rand.Next(0, AutoInsertCount - 1));
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

        private void ShowList()
        {
            Random random = new Random();
            for (var i = 0; AutoInsertCount > i; i++)
            {
                var newTStation = new TelephoneStation(random);
                _telephoneStationList2.Add(newTStation);
            }
            foreach (var t in _telephoneStationList2.Take(100))
            {
                showAllObjects.Items.Add(t.ToString());
            }
        }

        private void showAllObjects_Click(object sender, EventArgs e)
        {
            // Console.WriteLine(); 
            MessageBox.Show(this, (sender as ListView).SelectedItems[0].Text, "Текущий объект");
        }
    }
}