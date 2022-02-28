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
        private List<TelephoneStation> _TelephoneStations;
        private int? _CurrentStationIndex;
        public Form1(List<TelephoneStation> telephoneStationsList)
        {
            InitializeComponent();
            _TelephoneStations = telephoneStationsList;
            _TelephoneStations.Select(AddStation);

        }
            

        private void SelectStation_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (sender is ListBox stationsListBox)
            {
                _CurrentStationIndex = stationsListBox.SelectedIndex;
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
            if (_CurrentStationIndex != null)
            {
                RemoveStation((int)_CurrentStationIndex);
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
            _TelephoneStations.Add(newStation ??= new TelephoneStation());
            listBox1.Items.Add(newStation);
            return false;
        }

        private void RemoveStation(int index)
        {
            _TelephoneStations.RemoveAt(index);
            listBox1.Items.RemoveAt(index);
        }
        private void RemoveStation(TelephoneStation currentStation)
        {
            _TelephoneStations.Remove(currentStation);
            listBox1.Items.Remove(currentStation);
        }
    }
}