using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Windows.Forms;

namespace lab_2
{
    public partial class Form1 : Form
    {
        private List<TelephoneStation> _telephoneStations;
        private int? _currentStationIndex;
        public Form1(List<TelephoneStation> telephoneStationsList)
        {
            InitializeComponent();
            _telephoneStations = telephoneStationsList;
            _telephoneStations.Select(AddStation);

        }
            

        private void SelectStation_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (sender is ListBox stationsListBox)
            {
                IEnumerable<string> createParams;
                _currentStationIndex = stationsListBox.SelectedIndex;
                (createParams = _telephoneStations[(int) _currentStationIndex]
                    .ParamsAsStrings())
                    .Take(listBox2.Items.Count)
                    .Zip(Enumerable.Range(0, listBox2.Items.Count), (tStationParam, index) => 
                        listBox2.Items[index] = tStationParam.ToString())
                    .ToList();
                createParams.Skip(listBox2.Items.Count).Select(x => listBox2.Items.Add(x));
                for (var i =  listBox2.Items.Count - 1; i >= createParams.Count(); i--)
                {
                    listBox2.Items.RemoveAt(i);
                }
                
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            // throw new System.NotImplementedException();
        }

        private void createStationButton_Click(object sender, EventArgs e)
        {
            AddStation();
        }
        
        private void deleteStationButton_Click(object sender, EventArgs e)
        {
            if (_currentStationIndex != null)
            {
                RemoveStation((int)_currentStationIndex);
            }
        }

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            // throw new System.NotImplementedException();
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {
            // throw new System.NotImplementedException();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // throw new System.NotImplementedException();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // throw new System.NotImplementedException();
        }

        private void tableLayoutPanel5_Paint(object sender, PaintEventArgs e)
        {
            // throw new System.NotImplementedException();
        }

        private bool AddStation(TelephoneStation newStation = null)
        {
            _telephoneStations.Add(newStation ??= new TelephoneStation());
            listBox1.Items.Add(newStation);
            return false;
        }

        private void RemoveStation(int index)
        {
            _telephoneStations.RemoveAt(index);
            listBox1.Items.RemoveAt(index);
        }
        private void RemoveStation(TelephoneStation currentStation)
        {
            _telephoneStations.Remove(currentStation);
            listBox1.Items.Remove(currentStation);
        }
    }
}